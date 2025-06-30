using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Basket.Data.Processors;

public class OutboxProcessor
(IServiceProvider serviceProvider, IBus bus, ILogger<OutboxProcessor> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                
                var outboxMessages = await dbContext.OutboxMessages
                    .Where(m => m.ProcessedOn == null)
                    .ToListAsync(stoppingToken);

                if (outboxMessages.Count == 0)
                {
                    logger.LogInformation("No outbox messages to process.");
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); // Adjust the delay as needed
                    continue;
                }

                foreach (var message in outboxMessages)
                {
                    var eventType = Type.GetType(message.Type);
                    if (eventType == null)
                    {
                        logger.LogWarning("Could not resolve type: {Type}", message.Type);
                        continue;
                    }
                    var eventMessage = JsonSerializer.Deserialize(message.Content, eventType);
                    if (eventMessage == null)
                    {
                        logger.LogWarning("Could not deserialize message: {Content}", message.Content);
                        continue;
                    }

                    await bus.Publish(eventMessage, stoppingToken);

                    message.ProcessedOn = DateTime.UtcNow;

                    logger.LogInformation("Successfully processed outbox message with ID: {Id}", message.Id);
                }

                await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing outbox messages");
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); // Adjust the delay as needed
        }
    }
}

using MassTransit;
using Shared.Messaging.Events;

namespace Ordering.EventHandlers;

public class BasketCheckoutEventHandler
    (OrderService service, ILogger<BasketCheckoutIntegrationEvent> logger)
    : IConsumer<BasketCheckoutIntegrationEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutIntegrationEvent> context)
    {
        logger.LogInformation("Basket checkout event received: {UserName}, {TotalPrice}",
            context.Message.UserName, context.Message.TotalPrice);

        // create order
        var order = new Order
        {
            UserName = context.Message.UserName,
            TotalPrice = context.Message.TotalPrice,
            FirstName = context.Message.FirstName,
            LastName = context.Message.LastName,
            EmailAddress = context.Message.EmailAddress,
            AddressLine = context.Message.AddressLine
        };

        await service.CreateOrderAsync(order);
    }
}

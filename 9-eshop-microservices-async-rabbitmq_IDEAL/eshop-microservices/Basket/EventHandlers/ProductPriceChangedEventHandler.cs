using MassTransit;
using Shared.Messaging.Events;

namespace Basket.EventHandlers;

public class ProductPriceChangedEventHandler
    (BasketService service, ILogger<ProductPriceChangedEventHandler> logger)
    : IConsumer<ProductPriceChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
        logger.LogInformation("Product price changed event received: {ProductId}, {Price}",
            context.Message.ProductId, context.Message.Price);

        // find products on basket and update price
        await service.UpdateBasketItemProductPrices
            (context.Message.ProductId, context.Message.Price);
    }
}

using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Messaging.Events;
using System.Text.Json;

namespace Basket.Services;

public class BasketService(IDistributedCache cache, 
                CatalogApiClient catalogApiClient, 
                IBus bus)
{
    public async Task<ShoppingCart?> GetBasket(string userName)
    {
        var basket = await cache.GetStringAsync(userName);
        return string.IsNullOrEmpty(basket) ? null :
            JsonSerializer.Deserialize<ShoppingCart>(basket);
    }

    public async Task UpdateBasket(ShoppingCart basket)
    {
        // Before update(Add/remove Item) into SC, we should call Catalog ms GetProductById method
        // Get latest product information and set Price and ProductName when adding item into SC
        foreach (var item in basket.Items)
        {
            var product = await catalogApiClient.GetProductById(item.ProductId);
            item.Price = product.Price;
            item.ProductName = product.Name;
        }

        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));
    }

    public async Task CheckoutBasket(BasketCheckout basketCheckout)
    {
        // Get existing basket with total price
        // Set totalprice on basketcheckout event message
        // Send basket checkout event to rabbitmq using masstransit
        // Delete the basket

        // Get existing basket with total price
        var shoppingCart = await GetBasket(basketCheckout.UserName);
        if (shoppingCart is null)
        {
            throw new InvalidOperationException($"Shopping cart for user '{basketCheckout.UserName}' not found.");
        }

        // Set total price on basket checkout event message
        basketCheckout.TotalPrice = shoppingCart.TotalPrice;

        // Send basket checkout event to rabbitmq using masstransit
        var integrationEvent = new BasketCheckoutIntegrationEvent
        {
            UserName = basketCheckout.UserName,
            TotalPrice = shoppingCart.TotalPrice,
            FirstName = basketCheckout.FirstName,
            LastName = basketCheckout.LastName,
            EmailAddress = basketCheckout.EmailAddress,
            AddressLine = basketCheckout.AddressLine
        };
        // Publish checkout basket event and create order
        await bus.Publish(integrationEvent);

        // Delete the basket
        await DeleteBasket(basketCheckout.UserName);
    }

    public async Task DeleteBasket(string userName)
    {
        await cache.RemoveAsync(userName);
    }

    internal async Task UpdateBasketItemProductPrices(int productId, decimal price)
    {
        // IDistributedCache not supported list of keys function
        // https://github.com/dotnet/runtime/issues/36402

        var basket = await GetBasket("swn");

        if (basket == null) return;

        var item = basket!.Items.FirstOrDefault(x => x.ProductId == productId);
        if (item != null)
        {
            item.Price = price;
            await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));
        }
    }
}

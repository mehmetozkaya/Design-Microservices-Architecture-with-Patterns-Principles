using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Services;

public class BasketService(IDistributedCache cache, 
                CatalogApiClient catalogApiClient, 
                OrderingApiClient orderingApiClient)
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
        // get existing basket with total price
        // Set totalprice on basketcheckout event message
        // send basket checkout event to rabbitmq using masstransit
        // delete the basket

        // get existing basket with total price
        var shoppingCart = await GetBasket(basketCheckout.UserName);
        if (shoppingCart is null)
        {
            throw new InvalidOperationException($"Shopping cart for user '{basketCheckout.UserName}' not found.");
        }

        // Set total price on basket checkout event message
        basketCheckout.TotalPrice = shoppingCart.TotalPrice;

        // send basket checkout event to rabbitmq using masstransit
        // TODO: publish checkout basket event and create order        
        // WORKAROUND: Added Ordering project api reference and inject and sync call Ordering
        var order = new OrderDto
        {
            UserName = basketCheckout.UserName,
            TotalPrice = basketCheckout.TotalPrice,
            FirstName = basketCheckout.FirstName,
            LastName = basketCheckout.LastName,
            EmailAddress = basketCheckout.EmailAddress,
            AddressLine = basketCheckout.AddressLine
        };
        await orderingApiClient.CreateOrder(order);

        // delete the basket
        await DeleteBasket(basketCheckout.UserName);        
    }

    public async Task DeleteBasket(string userName)
    {
        await cache.RemoveAsync(userName);
    }   
}

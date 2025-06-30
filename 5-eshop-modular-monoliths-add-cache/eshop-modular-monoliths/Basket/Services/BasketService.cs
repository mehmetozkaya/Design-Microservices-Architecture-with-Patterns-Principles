using Basket.Data;
using Basket.Models;
using Microsoft.EntityFrameworkCore;
using Ordering.Services;

namespace Basket.Services;
public class BasketService(BasketDbContext dbContext, OrderService orderService)
{
    public async Task<ShoppingCart?> GetBasket(string userName)
    {
        var shoppingCart = await dbContext.ShoppingCarts
                .Include(cart => cart.Items)
                .FirstOrDefaultAsync(cart => cart.UserName == userName);

        return shoppingCart;
    }

    public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
    {
        var existingCart = await GetBasket(shoppingCart.UserName);
        if (existingCart is null)
        {
            dbContext.ShoppingCarts.Add(shoppingCart);
        }
        else
        {
            existingCart.Items = shoppingCart.Items;
            dbContext.ShoppingCarts.Update(existingCart);
        }
        await dbContext.SaveChangesAsync();
        return shoppingCart;
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
        // WORKAROUND: Added Ordering project reference and inject and method call OrderService
        var order = new Ordering.Models.Order
        {
            UserName = basketCheckout.UserName,
            TotalPrice = basketCheckout.TotalPrice,
            FirstName = basketCheckout.FirstName,
            LastName = basketCheckout.LastName,
            EmailAddress = basketCheckout.EmailAddress,
            AddressLine = basketCheckout.AddressLine
        };
        await orderService.CreateOrderAsync(order);

        // delete the basket
        dbContext.ShoppingCarts.Remove(shoppingCart);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteBasket(string userName)
    {
        var shoppingCart = await GetBasket(userName);
        if (shoppingCart is not null)
        {
            dbContext.ShoppingCarts.Remove(shoppingCart);
            await dbContext.SaveChangesAsync();
        }
    }
}

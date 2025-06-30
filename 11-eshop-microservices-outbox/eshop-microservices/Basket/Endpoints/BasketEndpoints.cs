namespace Basket.Endpoints;

public static class BasketEndpoints
{
    public static void MapBasketEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("basket");

        // GET by userName
        group.MapGet("/{userName}", async (string userName, BasketService service) =>
        {
            var shoppingCart = await service.GetBasket(userName);

            if (shoppingCart is null)
            {
                return Results.NotFound($"Shopping cart for user '{userName}' not found.");
            }

            return Results.Ok(shoppingCart);
        })
        .WithName("GetBasket")
        .Produces<ShoppingCart>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // POST (Upsert)
        group.MapPost("/", async (ShoppingCart shoppingCart, BasketService service) =>
        {
            await service.UpdateBasket(shoppingCart);
            return Results.Created("GetBasket", shoppingCart);
        })
        .WithName("UpdateBasket")
        .Produces<ShoppingCart>(StatusCodes.Status201Created);

        // POST Checkout
        group.MapPost("/checkout", async (BasketCheckout basketCheckout, BasketService service) =>
        {
            await service.CheckoutBasket(basketCheckout);
            return Results.NoContent();
        })
        .WithName("CheckoutBasket")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest);

        // DELETE
        group.MapDelete("/{userName}", async (string userName, BasketService service) =>
        {
            await service.DeleteBasket(userName);
            return Results.NoContent();
        })
        .WithName("DeleteBasket")
        .Produces(StatusCodes.Status204NoContent);        
    }
}

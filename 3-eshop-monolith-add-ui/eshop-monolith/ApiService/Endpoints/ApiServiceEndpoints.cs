using ApiService.Data;
using ApiService.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiService.Endpoints;

public static class ApiServiceEndpoints
{
    public static void MapApiServiceEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/apiservice");

        ////////// Product Endpoints

        // GET all products
        group.MapGet("/products", async (EShopDbContext dbContext) =>
        {
            var products = await dbContext.Products.ToListAsync();
            return Results.Ok(products);
        })
        .WithName("GetAllProducts")
        .Produces<List<Product>>(StatusCodes.Status200OK);

        // GET product by id
        group.MapGet("/products/{id}", async (int id, EShopDbContext dbContext) =>
        {
            var product = await dbContext.Products.FindAsync(id);
            return product is not null ? Results.Ok(product) : Results.NotFound();
        })
        .WithName("GetProductById")
        .Produces<Product>(StatusCodes.Status200OK);

        // POST product
        app.MapPost("/products", async (Product product, EShopDbContext dbContext) =>
        {
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();
            return Results.Created($"/products/{product.Id}", product);
        })
        .WithName("CreateProduct")
        .Produces<List<Product>>(StatusCodes.Status201Created);

        // PUT product
        app.MapPut("/products/{id}", async (int id, Product updatedProduct, EShopDbContext dbContext) =>
        {
            var product = await dbContext.Products.FindAsync(id);
            if (product is null) return Results.NotFound();
            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.ImageUrl = updatedProduct.ImageUrl;
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("UpdateProduct")
        .Produces<List<Product>>(StatusCodes.Status204NoContent);

        // DELETE product
        app.MapDelete("/products/{id}", async (int id, EShopDbContext dbContext) =>
        {
            var product = await dbContext.Products.FindAsync(id);
            if (product is null) return Results.NotFound();
            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("DeleteProduct")
        .Produces<List<Product>>(StatusCodes.Status204NoContent);


        ////////// ShoppingCart Endpoints

        // GET Basket by userName
        group.MapGet("/basket/{userName}", async (string userName, EShopDbContext dbContext) =>
        {
            var shoppingCart = await dbContext.ShoppingCarts
                .Include(cart => cart.Items)
                .FirstOrDefaultAsync(cart => cart.UserName == userName);

            if (shoppingCart is null)
            {
                return Results.NotFound($"Shopping cart for user '{userName}' not found.");
            }

            return Results.Ok(shoppingCart);
        })
        .WithName("GetBasketByUserName")
        .Produces<ShoppingCart>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // POST (Upsert) Basket
        group.MapPost("/basket", async (ShoppingCart shoppingCart, EShopDbContext dbContext) =>
        {
            var existingCart = await dbContext.ShoppingCarts
                .Include(cart => cart.Items)
                .FirstOrDefaultAsync(cart => cart.UserName == shoppingCart.UserName);

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
            return Results.Ok(shoppingCart);
        })
        .WithName("UpdateBasket")
        .Produces<ShoppingCart>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // POST Checkout Basket
        group.MapPost("/basket/checkout", async (Order checkoutOrder, EShopDbContext dbContext) =>
        {
            var shoppingCart = await dbContext.ShoppingCarts
                .Include(cart => cart.Items)
                .FirstOrDefaultAsync(cart => cart.UserName == checkoutOrder.UserName);

            if (shoppingCart is null)
            {
                return Results.NotFound($"Shopping cart for user '{checkoutOrder.UserName}' not found.");
            }
            var order = new Order
            {
                UserName = checkoutOrder.UserName,
                TotalPrice = checkoutOrder.TotalPrice,
                FirstName = checkoutOrder.FirstName,
                LastName = checkoutOrder.LastName,
                EmailAddress = checkoutOrder.EmailAddress,
                AddressLine = checkoutOrder.AddressLine
            };

            dbContext.Orders.Add(order);
            dbContext.ShoppingCarts.Remove(shoppingCart);
            await dbContext.SaveChangesAsync();

            return Results.Created($"/orders/{order.Id}", order);
        })
        .WithName("CheckoutBasket")
        .Produces<Order>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status404NotFound);

        // DELETE Basket by userName
        group.MapDelete("/basket/{userName}", async (string userName, EShopDbContext dbContext) =>
        {
            var shoppingCart = await dbContext.ShoppingCarts
                .Include(cart => cart.Items)
                .FirstOrDefaultAsync(cart => cart.UserName == userName);

            if (shoppingCart is null)
            {
                return Results.NotFound($"Shopping cart for user '{userName}' not found.");
            }

            dbContext.ShoppingCarts.Remove(shoppingCart);
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("DeleteBasket")
        .Produces<ShoppingCart>(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);


        ////////// Order Endpoints

        // GET All Orders
        group.MapGet("/orders", async (EShopDbContext dbContext) =>
        {
            var orders = await dbContext.Orders.ToListAsync();
            return Results.Ok(orders);
        })
        .WithName("GetAllOrders")
        .Produces<List<Order>>(StatusCodes.Status200OK);

        // GET Order by userName
        group.MapGet("/orders/{userName}", async (string userName, EShopDbContext dbContext) =>
        {
            var orders = await dbContext.Orders
                .Where(o => o.UserName == userName)
                .ToListAsync();

            if (orders is null || !orders.Any())
            {
                return Results.NotFound($"No orders found for user '{userName}'.");
            }

            return Results.Ok(orders);
        })
        .WithName("GetOrderByUserName")
        .Produces<List<Order>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}

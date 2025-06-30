using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Ordering.Models;
using Ordering.Services;

namespace Ordering.Endpoints;
public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/orders");

        // GET all
        group.MapGet("/", async (OrderService service) =>
        {
            var orders = await service.GetAllOrdersAsync();
            return Results.Ok(orders);
        })
        .WithName("GetAllOrders")
        .Produces<List<Order>>(StatusCodes.Status200OK);

        // GET Order by userName
        group.MapGet("/{userName}", async (string userName, OrderService service) =>
        {
            var orders = await service.GetOrdersByUserNameAsync(userName);
            if (orders is null || !orders.Any()) return Results.NotFound($"No orders found for user '{userName}'.");
            return Results.Ok(orders);
        })
        .WithName("GetOrderByUserName")
        .Produces<List<Order>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

    }
}

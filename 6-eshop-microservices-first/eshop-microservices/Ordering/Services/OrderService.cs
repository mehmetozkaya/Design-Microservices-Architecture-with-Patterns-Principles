namespace Ordering.Services;

public class OrderService(OrderDbContext dbContext)
{
    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await dbContext.Orders.ToListAsync();
    }    

    public async Task<IEnumerable<Order>> GetOrdersByUserNameAsync(string userName)
    {
        return await dbContext.Orders
            .Where(o => o.UserName == userName)
            .ToListAsync();
    }

    // for basket checkout operation
    public async Task CreateOrderAsync(Order order)
    {
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();
    }
}

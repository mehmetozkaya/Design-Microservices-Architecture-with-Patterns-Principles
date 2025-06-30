using Microsoft.EntityFrameworkCore;
using Ordering.Models;

namespace Ordering.Data;
public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options)
        : base(options) { }

    public DbSet<Order> Orders { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("ordering");

        base.OnModelCreating(builder);
    }
}

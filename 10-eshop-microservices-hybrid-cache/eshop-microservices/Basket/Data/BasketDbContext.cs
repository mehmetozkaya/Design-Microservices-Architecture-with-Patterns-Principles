using Microsoft.EntityFrameworkCore;

namespace Basket.Data;

public class BasketDbContext : DbContext
{
    public BasketDbContext(DbContextOptions<BasketDbContext> options)
        : base(options) { }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; } = default!;
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ShoppingCart>()
        .HasKey(s => s.Id);

        builder.Entity<ShoppingCart>()
            .HasIndex(s => s.UserName)
        .IsUnique();

        builder.Entity<ShoppingCart>()
         .HasMany(s => s.Items)
         .WithOne()
         .HasForeignKey(si => si.ShoppingCartId);
    }
}

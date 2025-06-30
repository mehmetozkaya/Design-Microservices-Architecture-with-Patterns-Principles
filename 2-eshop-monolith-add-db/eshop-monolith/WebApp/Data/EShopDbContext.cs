using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data;

public class EShopDbContext : DbContext
{
    public EShopDbContext(DbContextOptions<EShopDbContext> options) 
        : base(options)
    {
    }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<ShoppingCart> ShoppingCarts { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShoppingCart>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<ShoppingCart>()
            .HasIndex(s => s.UserName)
            .IsUnique();

        modelBuilder.Entity<ShoppingCart>()
         .HasMany(s => s.Items)
         .WithOne()
         .HasForeignKey(si => si.ShoppingCartId);
    }

    public async Task<ShoppingCart> LoadUserBasket()
    {
        // Get Basket If Not Exist Create New Basket with Default Logged In User Name: swn
        var userName = "swn";

        var basket = await ShoppingCarts
                            .Include(s => s.Items)
                            .FirstOrDefaultAsync(sc => sc.UserName == userName);

        if (basket == null)
        {
            basket = new ShoppingCart
            {
                UserName = userName,
                Items = []
            };
        }

        return basket;
    }
}

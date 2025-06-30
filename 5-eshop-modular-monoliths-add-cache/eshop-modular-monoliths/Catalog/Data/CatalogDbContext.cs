using Catalog.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Data;
public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
        : base(options) { }

    public DbSet<Product> Products { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("catalog");
        
        base.OnModelCreating(builder);
    }
}

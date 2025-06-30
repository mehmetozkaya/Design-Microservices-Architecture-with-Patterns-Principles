using Catalog.Data;
using Catalog.Data.Seed;
using Catalog.Endpoints;
using Catalog.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Data;
using Shared.Data.Seed;

namespace Catalog;
public static class CatalogModule
{
    public static WebApplicationBuilder AddCatalogModule(this WebApplicationBuilder builder,
        IConfiguration configuration)
    {
        // Add services to the container.

        builder.AddNpgsqlDbContext<CatalogDbContext>(connectionName: "eshopdb");

        builder.Services.AddScoped<IDataSeeder, CatalogDataSeeder>();

        builder.Services.AddScoped<ProductService>();

        return builder;
    }

    public static WebApplication UseCatalogModule(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        
        app.UseMigration<CatalogDbContext>();

        app.MapProductEndpoints();

        return app;
    }
}

using Basket.Data;
using Basket.Endpoints;
using Basket.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Data;

namespace Basket;
public static class BasketModule
{
    public static WebApplicationBuilder AddBasketModule(this WebApplicationBuilder builder,
        IConfiguration configuration)
    {
        // Add services to the container.

        builder.AddNpgsqlDbContext<BasketDbContext>(connectionName: "eshopdb");

        builder.Services.AddScoped<BasketService>();

        return builder;
    }

    public static WebApplication UseBasketModule(this WebApplication app)
    {
        // Configure the HTTP request pipeline.

        app.UseMigration<BasketDbContext>();

        app.MapBasketEndpoints();

        return app;
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ordering.Data;
using Ordering.Endpoints;
using Ordering.Services;
using Shared.Data;

namespace Ordering;
public static class OrderingModule
{
    public static WebApplicationBuilder AddOrderingModule(this WebApplicationBuilder builder,
        IConfiguration configuration)
    {
        // Add services to the container.

        builder.AddNpgsqlDbContext<OrderDbContext>(connectionName: "eshopdb");        

        builder.Services.AddScoped<OrderService>();

        return builder;
    }

    public static WebApplication UseOrderingModule(this WebApplication app)
    {
        // Configure the HTTP request pipeline.

        app.UseMigration<OrderDbContext>();

        app.MapOrderEndpoints();

        return app;
    }
}

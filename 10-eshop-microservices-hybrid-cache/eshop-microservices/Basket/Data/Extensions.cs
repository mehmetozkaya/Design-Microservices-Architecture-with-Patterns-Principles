using Microsoft.EntityFrameworkCore;

namespace Basket.Data;

public static class Extensions
{
    public static void UseMigration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<BasketDbContext>();

        context.Database.Migrate();        
    }
}

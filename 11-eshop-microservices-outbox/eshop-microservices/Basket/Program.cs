
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<BasketDbContext>(connectionName: "basketdb");

builder.AddRedisDistributedCache(connectionName: "cache");

builder.Services.AddHybridCache(options =>
{    
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromMinutes(5),
        LocalCacheExpiration = TimeSpan.FromMinutes(1),
        Flags = HybridCacheEntryFlags.DisableLocalCache
    };
});

builder.Services.AddScoped<BasketService>();

builder.Services.AddHttpClient<CatalogApiClient>(client =>
{
    client.BaseAddress = new("https+http://catalog");
});

builder.Services.AddMassTransitWithAssemblies(Assembly.GetExecutingAssembly());

builder.Services.AddHostedService<OutboxProcessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.UseMigration();

app.MapBasketEndpoints();

app.Run();

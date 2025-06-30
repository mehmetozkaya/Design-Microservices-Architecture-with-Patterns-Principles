var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.AddRedisDistributedCache(connectionName: "cache");

builder.Services.AddScoped<BasketService>();

builder.Services.AddHttpClient<CatalogApiClient>(client =>
{
    client.BaseAddress = new("https+http://catalog");
});

builder.Services.AddHttpClient<OrderingApiClient>(client =>
{
    client.BaseAddress = new("https+http://ordering");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.MapBasketEndpoints();

app.Run();


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

app.Run();


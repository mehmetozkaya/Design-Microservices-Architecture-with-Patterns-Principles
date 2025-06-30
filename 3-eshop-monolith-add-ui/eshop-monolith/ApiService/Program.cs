using ApiService.Data;
using ApiService.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<EShopDbContext>(connectionName: "eshopdb");

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.UseMigration();

app.MapApiServiceEndpoints();

app.Run();

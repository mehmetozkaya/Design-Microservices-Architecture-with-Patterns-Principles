
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.AddSqlServerDbContext<OrderDbContext>(connectionName: "orderdb");

builder.Services.AddScoped<OrderService>();

builder.Services.AddMassTransitWithAssemblies(Assembly.GetExecutingAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.UseMigration();

app.MapOrderEndpoints();

app.Run();

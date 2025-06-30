
var builder = DistributedApplication.CreateBuilder(args);

// Backing Services
var postgres = builder
        .AddPostgres("postgres")
        .WithPgAdmin(pgAdmin => pgAdmin.WithUrlForEndpoint("http", url => url.DisplayText = "PostgreDB Browser"))
        .WithDataVolume()
        .WithLifetime(ContainerLifetime.Persistent);

var catalogdb = postgres.AddDatabase("catalogdb");

var basketdb = postgres.AddDatabase("basketdb");

var cache = builder
        .AddRedis("cache")
        .WithRedisInsight()
        .WithDataVolume()
        .WithLifetime(ContainerLifetime.Persistent);

var sqlServer = builder
        .AddSqlServer("sqlserver")
        .WithDataVolume()
        .WithLifetime(ContainerLifetime.Persistent);

var orderdb = sqlServer.AddDatabase("orderdb");

var rabbitmq = builder
    .AddRabbitMQ("rabbitmq")
    .WithManagementPlugin()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

// Projects
var catalog = builder
        .AddProject<Projects.Catalog>("catalog")
        .WithReference(catalogdb)
        .WithReference(rabbitmq)
        .WaitFor(catalogdb)
        .WaitFor(rabbitmq);

var basket = builder
        .AddProject<Projects.Basket>("basket").WithReplicas(3)
        .WithReference(basketdb)
        .WithReference(cache)
        .WithReference(catalog)
        .WithReference(rabbitmq)
        .WaitFor(basketdb)
        .WaitFor(cache)
        .WaitFor(catalog)
        .WaitFor(rabbitmq);

var ordering = builder
        .AddProject<Projects.Ordering>("ordering")
        .WithReference(orderdb)
        .WithReference(rabbitmq)
        .WaitFor(orderdb)
        .WaitFor(rabbitmq);

var yarpapigateway = builder
        .AddProject<Projects.YarpApiGateway>("yarpapigateway")
        .WithReference(catalog)
        .WithReference(basket)
        .WithReference(ordering)
        .WaitFor(catalog)
        .WaitFor(basket)
        .WaitFor(ordering);

var webapp = builder
        .AddProject<Projects.WebApp>("webapp")
        .WithExternalHttpEndpoints()
        .WithUrlForEndpoint("https", url => url.DisplayText = "EShop WebApp (HTTPS)")
        .WithUrlForEndpoint("http", url => url.DisplayText = "EShop WebApp (HTTP)")
        .WithReference(yarpapigateway)
        .WaitFor(yarpapigateway);        

builder.Build().Run();

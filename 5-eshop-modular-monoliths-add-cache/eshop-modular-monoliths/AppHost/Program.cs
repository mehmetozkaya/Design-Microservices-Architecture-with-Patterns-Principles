var builder = DistributedApplication.CreateBuilder(args);

// Backing Services
var postgres = builder
        .AddPostgres("postgres")
        .WithPgAdmin(pgAdmin => pgAdmin.WithUrlForEndpoint("http", url => url.DisplayText = "PostgreDB Browser"))
        .WithDataVolume()
        .WithLifetime(ContainerLifetime.Persistent);

var eshopDb = postgres.AddDatabase("eshopdb");

var cache = builder
        .AddRedis("cache")
        .WithRedisInsight()
        .WithDataVolume()
        .WithLifetime(ContainerLifetime.Persistent);

// Projects
var apiService = builder
        .AddProject<Projects.ApiService>("apiservice").WithReplicas(3)
        .WithReference(eshopDb)
        .WaitFor(eshopDb);

var webapp = builder
        .AddProject<Projects.WebApp>("webapp")
        .WithExternalHttpEndpoints()
        .WithUrlForEndpoint("https", url => url.DisplayText = "EShop WebApp (HTTPS)")
        .WithUrlForEndpoint("http", url => url.DisplayText = "EShop WebApp (HTTP)")
        .WithReference(cache)
        .WaitFor(cache)
        .WithReference(apiService)
        .WaitFor(apiService);        

builder.Build().Run();

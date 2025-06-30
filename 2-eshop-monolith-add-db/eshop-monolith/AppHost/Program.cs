var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
        .AddPostgres("postgres")
        .WithPgAdmin(pgAdmin => pgAdmin.WithUrlForEndpoint("http", url => url.DisplayText = "PostgreDB Browser"))
        .WithDataVolume()
        .WithLifetime(ContainerLifetime.Persistent);

var eshopDb = postgres.AddDatabase("eshopdb");

builder
    .AddProject<Projects.WebApp>("webapp")
    .WithUrlForEndpoint("https", url => url.DisplayText = "EShop WebApp (HTTPS)")
    .WithUrlForEndpoint("http", url => url.DisplayText = "EShop WebApp (HTTP)")
    .WithReference(eshopDb)
    .WaitFor(eshopDb);

builder.Build().Run();

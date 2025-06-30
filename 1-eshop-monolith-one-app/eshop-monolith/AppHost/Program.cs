var builder = DistributedApplication.CreateBuilder(args);

builder
    .AddProject<Projects.WebApp>("webapp")
    .WithUrlForEndpoint("https", url => url.DisplayText = "EShop WebApp (HTTPS)")
    .WithUrlForEndpoint("http", url => url.DisplayText = "EShop WebApp (HTTP)");

builder.Build().Run();

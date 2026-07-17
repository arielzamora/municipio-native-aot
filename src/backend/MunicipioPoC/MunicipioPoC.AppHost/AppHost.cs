var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.MunicipioPoC_Api>("api")
    .WithHttpEndpoint(port: 5222, name: "http");

builder.AddProject<Projects.MunicipioPoC_Ingestion>("ingestion")
    .WithReference(api);

builder.Build().Run();

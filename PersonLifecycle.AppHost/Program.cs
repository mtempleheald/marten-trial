var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("postgres-user", secret: true);
var password = builder.AddParameter("postgres-password", secret: true);

var postgres = builder.AddPostgres("postgres", username, password)
    //.WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgWeb(pgWeb =>
    {
        pgWeb.WithHostPort(5050);
    });

// Note that AddDatabase doesn't actually create the database, it just adds it to the builder
// Note also that databaseName needs to match the username above, because AddPostgres does not set POSTGRES_DB env var
var eventStoreDb = postgres.AddDatabase("eventStoreDb", "pguser");

builder.AddProject<Projects.PersonLifecycle_WebApi>("webapi")
    .WithReference(eventStoreDb);

builder.Build().Run();
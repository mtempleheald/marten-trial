using Marten;
using Marten.Events.Projections;
using PersonLifecycle.WebApi;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.AddNpgsqlDataSource("eventStoreDb");
builder.Services.AddMarten(options =>
{
    options.UseSystemTextJsonForSerialization();
    
    options.Projections.Add<PersonProjection>(ProjectionLifecycle.Inline);

    if (builder.Environment.IsDevelopment())
    {
        options.AutoCreateSchemaObjects = AutoCreate.All;
    }
}).UseNpgsqlDataSource();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();

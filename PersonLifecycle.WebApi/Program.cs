using Marten;
using Marten.Events.Daemon.Resiliency;
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
}).UseNpgsqlDataSource()
    .AddSubscriptionWithServices<SubscriberService>(ServiceLifetime.Singleton, o =>
    {
        o.SubscriptionName = "sample";
        o.SubscriptionVersion = 1;
        o.Options.BatchSize = 10;

        //o.FilterIncomingEventsOnStreamType(typeof(PersonBorn));

        // for playing catch-up
        o.IncludeArchivedEvents = true;
        // For replaying from a specific time
        //o.Options.SubscribeFromTime(new DateTimeOffset(2025,4,1,0,0,0, 0.Seconds()));
        // Start from a specific event
        //o.Options.SubscribeFromSequence(100);
        // Start from now on a named DB
        //o.Options.SubscribeFromPresent("eventStoreDb");
    })
    .AddAsyncDaemon(DaemonMode.HotCold);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();

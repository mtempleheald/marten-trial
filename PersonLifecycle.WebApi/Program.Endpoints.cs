using Marten;

namespace PersonLifecycle.WebApi;

public static class EndpointsExtensions
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet("/person", () => "Hello, Person!");

        app.MapPost("/person/born", async (IDocumentStore store, PersonBorn @event) =>
        {
            await using var session = store.LightweightSession();
            session.Events.StartStream<Person>(@event.PersonId, @event);
            await session.SaveChangesAsync();
            return Results.Ok();
        });

        app.MapPost("/person/name-changed", async (IDocumentStore store, NameChanged @event) =>
        {
            await using var session = store.LightweightSession();
            session.Events.Append(@event.PersonId, @event);
            await session.SaveChangesAsync();
            return Results.Ok();
        });

        app.MapPost("/person/address-changed", async (IDocumentStore store, AddressChanged @event) =>
        {
            await using var session = store.LightweightSession();
            session.Events.Append(@event.PersonId, @event);
            await session.SaveChangesAsync();
            return Results.Ok();
        });

        app.MapPost("/person/died", async (IDocumentStore store, PersonDied @event) =>
        {
            await using var session = store.LightweightSession();
            session.Events.Append(@event.PersonId, @event);
            await session.SaveChangesAsync();
            return Results.Ok();
        });

        app.MapGet("/person/{personId}", async (IQuerySession session, Guid personId) =>
        {
            var person = await session.LoadAsync<Person>(personId);
            return person != null ? Results.Ok(person) : Results.NotFound();
        });
    }
}
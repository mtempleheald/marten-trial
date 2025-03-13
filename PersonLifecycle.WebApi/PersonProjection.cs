using Marten.Events.Aggregation;

namespace PersonLifecycle.WebApi;

public sealed record Person(
    Guid Id,
    string? Name,
    string? Address,
    DateTime DateOfBirth,
    DateTime? DateOfDeath
);

public class PersonProjection : SingleStreamProjection<Person>
{
    public static Person Create(PersonBorn @event) => new (
        @event.PersonId, 
        @event.Name,
        null,
        @event.DateOfBirth,
        null
    );

    public static Person Apply(NameChanged @event, Person person) => person with
    {
        Name = @event.NewName
    };

    public static Person Apply(AddressChanged @event, Person person) => person with
    {
        Address = @event.NewAddress
    };

    public static Person Apply(PersonDied @event, Person person) => person with
    {
        DateOfDeath = @event.DateOfDeath
    };
}
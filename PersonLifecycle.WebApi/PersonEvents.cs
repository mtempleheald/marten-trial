namespace PersonLifecycle.WebApi;

public sealed record PersonBorn(Guid PersonId, DateTime DateOfBirth, string Name);

public sealed record NameChanged(Guid PersonId, string NewName);

public sealed record AddressChanged(Guid PersonId, string NewAddress);

public sealed record PersonDied(Guid PersonId, DateTime DateOfDeath);
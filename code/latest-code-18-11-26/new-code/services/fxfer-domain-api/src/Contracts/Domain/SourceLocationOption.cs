namespace Fxfer.DomainApi.Contracts.Domain;

public sealed record SourceLocationOption
{
    public required Guid FtsSourceLocationId { get; init; }
    public required string ConfigName { get; init; }
    public required string Address { get; init; }
}

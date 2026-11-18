namespace Fxfer.Bff.Contracts.Common;

public sealed record SourceLocationOption
{
    public Guid FtsSourceLocationId { get; init; }
    public required string ConfigName { get; init; }
    public required string Address { get; init; }
}


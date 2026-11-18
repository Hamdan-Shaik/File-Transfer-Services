namespace Fxfer.Bff.Contracts.Common;

public sealed record DataListItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
}


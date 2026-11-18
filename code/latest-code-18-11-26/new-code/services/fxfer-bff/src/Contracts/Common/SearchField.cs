namespace Fxfer.Bff.Contracts.Common;

public sealed record SearchField
{
    public required string FieldName { get; init; }
    public required object? FieldValue { get; init; }
}


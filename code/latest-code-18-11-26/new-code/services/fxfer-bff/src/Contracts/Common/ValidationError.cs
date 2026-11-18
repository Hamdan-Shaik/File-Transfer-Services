namespace Fxfer.Bff.Contracts.Common;

public sealed record ValidationError
{
    public required string Field { get; init; }
    public required string Code { get; init; }
    public required string Message { get; init; }
}


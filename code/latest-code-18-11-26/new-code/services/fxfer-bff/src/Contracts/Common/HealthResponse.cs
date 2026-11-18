namespace Fxfer.Bff.Contracts.Common;

public sealed record HealthResponse
{
    public required string Status { get; init; }
    public DateTimeOffset TimeUtc { get; init; }
}


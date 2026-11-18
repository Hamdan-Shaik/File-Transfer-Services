namespace Fxfer.ProjectionWorker.Worker.Processing;

public sealed record ProcessingContext
{
    public required string TenantId { get; init; }
    public required string CorrelationId { get; init; }
    public required string TraceId { get; init; }
    public required Guid EventId { get; init; }
}


namespace Fxfer.ProjectionWorker.Infrastructure.Projections;

public sealed record SourceLocationInvalidatedProjectionPayload
{
    public required string SourceLocationId { get; init; }
    public required string OperatorId { get; init; }
    public List<DestinationProjectionPayload>? Destinations { get; init; }
}

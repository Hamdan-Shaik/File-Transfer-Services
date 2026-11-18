namespace Fxfer.ProjectionWorker.Infrastructure.Projections;

public sealed record GroupInfoProjectionPayload
{
    public required Guid GroupId { get; init; }
    public required int NumberOfRecords { get; init; }
}

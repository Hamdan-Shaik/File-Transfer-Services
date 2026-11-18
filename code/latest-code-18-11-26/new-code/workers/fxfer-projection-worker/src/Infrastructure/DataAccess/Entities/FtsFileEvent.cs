namespace Fxfer.ProjectionWorker.Infrastructure.DataAccess.Entities;

public sealed record FtsFileEvent
{
    public required Guid EventId { get; init; }
    public required Guid FileId { get; init; }
    public required Guid DestinationId { get; init; }
    public required string EventCd { get; init; }
    public string? EventTrackDescription { get; init; }
    public required DateTimeOffset CreatedTs { get; init; }
    public required string OperatorId { get; init; }
    public required DateTimeOffset LastModifiedTs { get; init; }
}


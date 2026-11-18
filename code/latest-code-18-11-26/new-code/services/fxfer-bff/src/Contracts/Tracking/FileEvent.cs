namespace Fxfer.Bff.Contracts.Tracking;

public sealed record FileEvent
{
    public Guid EventId { get; init; }
    public Guid FileId { get; init; }
    public Guid? DestinationId { get; init; }
    public required string EventCode { get; init; }
    public required string EventTrackDescription { get; init; }
    public DateTimeOffset CreatedDate { get; init; }
}


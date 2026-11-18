namespace Fxfer.DomainApi.Contracts.Domain;

/// <summary>
/// <para>Object containing properties of a File Tracking Event.</para>
/// </summary>
public sealed record FileEvent
{
    public required Guid EventId { get; init; }
    public required Guid FileId { get; init; }
    public required Guid DestinationId { get; init; }
    public required string EventCode { get; init; }
    public required string EventTrackDescription { get; init; }
    public DateTimeOffset CreatedDate { get; init; }
}


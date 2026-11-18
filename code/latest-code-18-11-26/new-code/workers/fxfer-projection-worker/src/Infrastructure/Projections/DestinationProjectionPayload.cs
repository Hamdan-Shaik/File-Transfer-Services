namespace Fxfer.ProjectionWorker.Infrastructure.Projections;

public sealed record DestinationProjectionPayload
{
    public required Guid DestinationId { get; init; }
    public required string DestinationAddress { get; init; }
    public required string DestinationProtocol { get; init; }
    public required bool OverwriteAtDestination { get; init; }
    public required bool CompressAtDestination { get; init; }
    public required bool RenameAtDestination { get; init; }
    public required bool DestinationArchive { get; init; }
    public required bool IsValid { get; init; }
    public Guid? FtpServerId { get; init; }
    public required string OperatorId { get; init; }
    public string? RenamePattern { get; init; }
    public string? ContactName { get; init; }
    public string? ContactPhone { get; init; }
    public string? ContactEmail { get; init; }
    public Guid? FteMetadataId { get; init; }
    public string? DestinationType { get; init; }
    public string? ContractName { get; init; }
}

namespace FxferService.Projections.Infrastructure.Messaging.Contracts.Events;

public sealed class FtsDestinationLocationAdded : FtsSourceLocationEventBase
{
    public Guid DestinationId { get; init; }
    public Guid SourceLocationId { get; init; }
    public required string DestinationAddress { get; init; }
    public required string DestinationProtocol { get; init; }
    public Guid FtpServerId { get; init; }
    public string? RenamePattern { get; init; }
    public bool OverWriteAtDestination { get; init; }
    public bool CompressAtDestination { get; init; }
    public bool RenameAtDestination { get; init; }
    public bool DestinationArchive { get; init; }
    public string? ContactName { get; init; }
    public string? ContactPhone { get; init; }
    public string? ContactEmail { get; init; }
    public bool IsValid { get; set; }
    public required string OperatorId { get; init; }
    public Guid? FTEMetadataId { get; init; }
    public string? DestinationType { get; init; }
    public string? ContractName { get; init; }
}


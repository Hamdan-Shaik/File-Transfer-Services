namespace Fxfer.DomainApi.Contracts.Events;

public sealed record FtsDestinationLocationAdded
{
    public Guid DestinationId { get; init; }
    public Guid SourceLocationId { get; init; }
    public string? DestinationAddress { get; init; }
    public string? DestinationProtocol { get; init; }
    public Guid FtpServerId { get; init; }
    public string? RenamePattern { get; init; }
    public bool OverWriteAtDestination { get; init; }
    public bool CompressAtDestination { get; init; }
    public bool RenameAtDestination { get; init; }
    public bool DestinationArchive { get; init; }
    public string? ContactName { get; init; }
    public string? ContactPhone { get; init; }
    public string? ContactEmail { get; init; }
    public bool IsValid { get; init; }
    public DateTime LastModifiedDate { get; init; }
    public DateTime CreationDate { get; init; }
    public string? OperatorId { get; init; }
    public Guid? FTEMetadataId { get; init; }
    public string? DestinationType { get; init; }
    public string? ContractName { get; init; }
    public string? ImportJobName { get; init; }
}

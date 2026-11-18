namespace Fxfer.Bff.Contracts.TransferConfigurations;

public sealed record Destination
{
    public Guid? DestinationId { get; init; }
    public required string DestinationAddress { get; init; }
    public required string DestinationProtocol { get; init; }
    public Guid? FtpServerId { get; init; }
    public string? RenamePattern { get; init; }
    public bool? OverWriteAtDestination { get; init; }
    public bool? CompressAtDestination { get; init; }
    public bool? RenameAtDestination { get; init; }
    public bool? DestinationArchive { get; init; }
    public string? ContactName { get; init; }
    public string? ContactPhone { get; init; }
    public string? ContactEmail { get; init; }
    public bool? IsValid { get; init; }
    public Guid? FteMetadataId { get; init; }
    public string? DestinationType { get; init; }
    public string? ContractName { get; init; }
    public string? ImportJobName { get; init; }
    public string? OperatorId { get; init; }
    public DateTimeOffset CreationDate { get; init; }
    public DateTimeOffset LastModifiedDate { get; init; }
}


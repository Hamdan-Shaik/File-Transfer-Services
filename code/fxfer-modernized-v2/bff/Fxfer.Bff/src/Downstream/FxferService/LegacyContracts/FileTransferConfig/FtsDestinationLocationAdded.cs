namespace FileTransferConfig;

public sealed class FtsDestinationLocationAdded
{
    public bool CompressAtDestination { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactName { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContractName { get; set; }
    public string? DestinationAddress { get; set; }
    public bool DestinationArchive { get; set; }
    public Guid DestinationId { get; set; }
    public string? DestinationProtocol { get; set; }
    public string? DestinationType { get; set; }
    public Guid? FTEMetadataId { get; set; }
    public Guid FtpServerId { get; set; }
    public string? ImportJobName { get; set; }
    public bool IsValid { get; set; }
    public string? OperatorId { get; set; }
    public bool OverWriteAtDestination { get; set; }
    public bool RenameAtDestination { get; set; }
    public string? RenamePattern { get; set; }
}

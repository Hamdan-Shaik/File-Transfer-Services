namespace FileTransferSvcQueryService;

public sealed class FtsDestinationLocationAdded
{
    public Guid DestinationId { get; set; }
    public Guid? FTEMetadataId { get; set; }

    public string? ImportJobName { get; set; }

    public string? ContractName { get; set; }
    public string? DestinationType { get; set; }
    public string? DestinationProtocol { get; set; }
    public Guid FtpServerId { get; set; }
    public string? RenamePattern { get; set; }

    public bool OverWriteAtDestination { get; set; }
    public bool CompressAtDestination { get; set; }
    public bool RenameAtDestination { get; set; }
    public bool DestinationArchive { get; set; }

    public bool IsValid { get; set; }
}
namespace Fxfer.TransferWorker.Contracts.Batch;

public sealed class DestinationModel
{
    public Guid DestinationId { get; set; }

    public string? DestinationAddress { get; set; }

    public string? DestinationProtocolCode { get; set; }

    public bool IsFullyUploaded { get; set; }

    public bool IsOverwriteAtDestination { get; set; }

    public bool IsCompressAtDestination { get; set; }

    public bool IsRenameAtDestination { get; set; }

    public string? RenamePattern { get; set; }

    public bool IsArchive { get; set; }

    public string? EncryptionCode { get; set; }

    public string? EncryptionPublicKey { get; set; }

    public string? EncryptionPrivateKey { get; set; }

    public string? PrivateKeyPassword { get; set; }

    public FtpServerModel? FtpServerDetail { get; set; }

    public Guid? FTEMetadataId { get; set; }

    public string? ContractName { get; set; }

    public string? EmailId { get; set; }

    public string? ContactName { get; set; }

    public string? DestinationFileName { get; set; }
}


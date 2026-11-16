using System;

namespace Fxfer.ExecutionWorker.Application.Contracts.Domain;

public class DestinationModel
{
    public Guid DestinationId { get; set; }

    public string DestinationAddress { get; set; } = null!;

    public string DestinationProtocolCode { get; set; } = null!;

    public bool IsOverwriteAtDestination { get; set; }

    public bool IsCompressAtDestination { get; set; }

    public bool IsRenameAtDestination { get; set; }

    public string RenamePattern { get; set; } = null!;

    public bool IsArchive { get; set; }

    public string EncryptionCode { get; set; } = null!;

    public string EncryptionPublicKey { get; set; } = null!;

    public string EncryptionPrivateKey { get; set; } = null!;

    public string PrivateKeyPassword { get; set; } = null!;

    public FtpServerModel FtpServerDetail { get; set; } = null!;

    public string DestinationFileName { get; set; } = null!;

    public Guid? FTEMetadataId { get; set; }

    public bool IsFullyUploaded { get; set; }

    public string EmailId { get; set; } = string.Empty;

    public string ContactName { get; set; } = string.Empty;

    public string ContractName { get; set; } = null!;
}
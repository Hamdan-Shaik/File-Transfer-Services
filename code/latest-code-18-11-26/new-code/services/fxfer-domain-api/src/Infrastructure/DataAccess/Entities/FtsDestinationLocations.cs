namespace Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

// Legacy entity: HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities.FtsDestinationLocations
// Table: FTS_DESTINATION
// Columns (legacy casing): DESTINATION_ID, FTS_SOURCE_LOCATION_ID, DESTINATION_ADDRESS, DEST_PROTOCOL_CD,
// OVERWRITE_AT_DEST_IND, COMPRESS_AT_DEST_IND, RENAME_AT_DEST_IND, DEST_IS_ARCHIVE_IND, IS_VALID,
// FTP_SERVER_ID, RENAME_PATTERN, CONTACT_NAME, CONTACT_PHONE, CONTACT_EMAIL, OPERATOR_ID,
// CREATION_TS, LAST_MODIFIED_TS, CDE_ENCRYPTION, ENCRYPTION_PUBLICKEY, ENCRYPTION_PRIVATEKEY,
// PRIVATEKEY_PASSWORD, EXTERNAL_FILE_DEFN_ID, DESTINATION_TYPE, CONTRACT_NAME
// Legacy-to-target column naming (fxfer-db-schema):
// CREATION_TS -> created_ts, ENCRYPTION_PUBLICKEY -> encryption_public_key,
// ENCRYPTION_PRIVATEKEY -> encryption_private_key_secret_ref, PRIVATEKEY_PASSWORD -> private_key_password_secret_ref
public sealed record FtsDestinationLocations
{
    public Guid DestinationId { get; init; }
    public Guid FtsSourceLocationId { get; init; }
    public required string DestinationAddress { get; init; }
    public required string DestProtocolCD { get; init; }
    public bool OverwriteAtDest { get; init; }
    public bool CompressAtDest { get; init; }
    public bool RenameAtDest { get; init; }
    public bool DestIsArchive { get; init; }
    public bool IsValid { get; init; }
    public Guid? FtpServerId { get; init; }
    public string? RenamePattern { get; init; }
    public string? ContactName { get; init; }
    public string? ContactPhone { get; init; }
    public string? ContactEmail { get; init; }
    public required string OperatorId { get; init; }
    public DateTimeOffset CreationDate { get; init; }
    public DateTimeOffset LastModifiedDate { get; init; }
    public string? CdeEncryption { get; init; }
    public string? EncryptionPublicKey { get; init; }
    public string? EncryptionPrivateKeySecretRef { get; init; }
    public string? PrivateKeyPasswordSecretRef { get; init; }
    public Guid? FTEMetadataId { get; init; }
    public string? DestinationType { get; init; }
    public string? ContractName { get; init; }

    // Legacy navigation collections (initialized in legacy constructor)
    public ICollection<FtsFileDelivery> FtsFileDeliveryFK { get; init; } = new HashSet<FtsFileDelivery>();
    public ICollection<FtsFileEvent> FtsFileEventFK { get; init; } = new HashSet<FtsFileEvent>();
    public ICollection<FtsFileException> FtsFileExceptionFK { get; init; } = new HashSet<FtsFileException>();
}


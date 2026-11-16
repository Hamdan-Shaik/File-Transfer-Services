using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FxferService.Infrastructure.Persistence.Entities;

[Table("FTS_DESTINATION")]
public partial class FtsDestinationLocations
{
    public FtsDestinationLocations()
    {
        FtsFileDeliveryFK = new HashSet<FtsFileDeliveryEntity>();
        FtsFileEventFK = new HashSet<FtsFileEventEntity>();
        FtsFileExceptionFK = new HashSet<FtsFileExceptionEntity>();
    }

    [Key]
    [Column("DESTINATION_ID")]
    public Guid DestinationId { get; set; }

    [Column("FTS_SOURCE_LOCATION_ID")]
    public Guid FtsSourceLocationId { get; set; }

    [Required]
    [StringLength(248)]
    [Column("DESTINATION_ADDRESS")]
    public string DestinationAddress { get; set; } = null!;

    [Required]
    [StringLength(4)]
    [Column("DEST_PROTOCOL_CD")]
    public string DestProtocolCD { get; set; } = null!;

    [Column("OVERWRITE_AT_DEST_IND")]
    public bool OverwriteAtDest { get; set; }

    [Column("COMPRESS_AT_DEST_IND")]
    public bool CompressAtDest { get; set; }

    [Column("RENAME_AT_DEST_IND")]
    public bool ReNameAtDest { get; set; }

    [Column("DEST_IS_ARCHIVE_IND")]
    public bool DestIsArchive { get; set; }

    [Column("IS_VALID")]
    public bool IsValid { get; set; }

    [Column("FTP_SERVER_ID")]
    public Guid FtpServerId { get; set; }

    [StringLength(50)]
    [Column("RENAME_PATTERN")]
    public string? RenamePattern { get; set; }

    [StringLength(50)]
    [Column("CONTACT_NAME")]
    public string? ContactName { get; set; }

    [StringLength(15)]
    [Column("CONTACT_PHONE")]
    public string? ContactPhone { get; set; }

    [StringLength(150)]
    [Column("CONTACT_EMAIL")]
    public string? ContactEmail { get; set; }

    [Required]
    [StringLength(200)]
    [Column("OPERATOR_ID")]
    public string OperatorId { get; set; } = null!;

    [Column("CREATION_TS")]
    public DateTime CreationDate { get; set; }

    [Column("LAST_MODIFIED_TS")]
    public DateTime LastModifiedDate { get; set; }

    [Column("CDE_ENCRYPTION")]
    [StringLength(8)]
    public string? CdeEncryption { get; set; }

    [Column("ENCRYPTION_PUBLICKEY")]
    public string? EncryptionPublicKey { get; set; }

    [Column("ENCRYPTION_PRIVATEKEY")]
    public string? EncryptionPrivateKey { get; set; }

    [StringLength(25)]
    [Column("PRIVATEKEY_PASSWORD")]
    public string? PrivateKeyPassword { get; set; }

    [Column("EXTERNAL_FILE_DEFN_ID")]
    public Guid? FTEMetadataId { get; set; }

    [StringLength(200)]
    [Column("DESTINATION_TYPE")]
    public string? DestinationType { get; set; }

    [StringLength(200)]
    [Column("CONTRACT_NAME")]
    public string? ContractName { get; set; }

    public virtual ICollection<FtsFileDeliveryEntity> FtsFileDeliveryFK { get; set; }

    public virtual ICollection<FtsFileEventEntity> FtsFileEventFK { get; set; }

    public virtual ICollection<FtsFileExceptionEntity> FtsFileExceptionFK { get; set; }
}

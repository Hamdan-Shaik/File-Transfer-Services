using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FxferService.Infrastructure.Persistence.Entities;

[Table("FTS_INTEGRATION_FILE")]
public class FtsIntegrationFile
{
    [Key]
    [Column("FTS_INTEGRATION_FILE_ID")]
    public Guid FtsIntegrationFileId { get; set; }

    [Required]
    [Column("GROUP_ID")]
    public Guid? GroupId { get; set; }

    [Column("CONFIG_ID")]
    public int? ConfigId { get; set; }

    [Required]
    [Column("FILE_NAME")]
    [StringLength(500)]
    public string FileName { get; set; } = null!;

    [Required]
    [Column("ETF_OUTBOUND_METADATA_DEFN_ID")]
    public Guid ETFOutboundMetadataDefnId { get; set; }

    [Required]
    [Column("IS_PROCESSED")]
    public bool IsProcessed { get; set; }

    [Required]
    [Column("OPERATOR_ID")]
    [StringLength(200)]
    public string OperatorId { get; set; } = null!;

    [Required]
    [Column("IS_VALID")]
    public bool IsValid { get; set; }

    [Required]
    [Column("CREATED_TS")]
    public DateTime CreationDate { get; set; }

    [Required]
    [Column("LAST_MODIFIED_TS")]
    public DateTime LastModifiedDate { get; set; }
}

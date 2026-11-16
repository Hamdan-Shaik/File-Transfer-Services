using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FxferService.Infrastructure.Persistence.Entities;

[Table("FTS_INBOUND_TARGET")]
public class FtsInboundTarget
{
    [Key]
    [Column("FTS_INBOUND_TARGET_ID")]
    public Guid FtsInboundTargetId { get; set; }

    [Required]
    [Column("EXTERNAL_FILE_DEFN_ID")]
    public Guid FTEMetadataId { get; set; }

    [Required]
    [Column("IMPORT_JOB_NAME")]
    [StringLength(200)]
    public string ImportJobName { get; set; } = null!;

    [Required]
    [Column("CONTRACT_NAME")]
    [StringLength(200)]
    public string ContractName { get; set; } = null!;

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

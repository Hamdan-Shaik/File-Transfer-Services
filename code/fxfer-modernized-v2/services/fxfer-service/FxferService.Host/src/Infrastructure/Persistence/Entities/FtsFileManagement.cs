using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FxferService.Infrastructure.Persistence.Entities;

[Table("FTS_FILE_MANAGEMENT")]
public partial class FtsFileManagement
{
    public FtsFileManagement()
    {
        FtsFileDeliveryFK = new HashSet<FtsFileDelivery>();
        FtsFileEventFK = new HashSet<FtsFileEvent>();
        FtsFileExceptionFK = new HashSet<FtsFileException>();
    }

    [Key]
    [Column("FILE_ID")]
    public Guid FileId { get; set; }

    [Column("FTS_SOURCE_LOCATION_ID")]
    public Guid FileSourceLocationId { get; set; }

    [Required]
    [StringLength(255)]
    [NonUnicode]
    [Column("FILE_NAME")]
    public string FileName { get; set; } = null!;

    [Required]
    [StringLength(1)]
    [NonUnicode]
    [Column("FILE_STATUS")]
    public string FileStatus { get; set; } = null!;

    [Column("FILE_STATUS_TS")]
    public DateTime FileStatusTs { get; set; }

    [Column("SAK_FILE_TRACK")]
    public int? SakFileTrack { get; set; }

    [Column("FILE_SIZE")]
    public decimal FileSize { get; set; }

    [StringLength(20)]
    [NonUnicode]
    [Column("CHECKSUM_CD")]
    public string ChecksumCD { get; set; } = null!;

    [Required]
    [StringLength(200)]
    [NonUnicode]
    [Column("OPERATOR_ID")]
    public string OperatorId { get; set; } = null!;

    [Column("LAST_MODIFIED_TS")]
    public DateTime LastModifiedTs { get; set; }

    [Column("CREATED_TS")]
    public DateTime CreatedDate { get; set; }

    public virtual ICollection<FtsFileDelivery> FtsFileDeliveryFK { get; set; }

    public virtual ICollection<FtsFileEvent> FtsFileEventFK { get; set; }

    public virtual ICollection<FtsFileException> FtsFileExceptionFK { get; set; }

    [ForeignKey("FtsSoureceLocationId")]
    public virtual FtsSourceLocation FtsSourceLocationFK { get; set; } = null!;
}

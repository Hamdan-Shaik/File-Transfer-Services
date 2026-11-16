using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FxferService.Infrastructure.Persistence.Entities;

[Table("FTS_FILE_EVENT")]
public partial class FtsFileEvent
{
    public FtsFileEvent()
    {
        FtsFileExceptionFK = new HashSet<FtsFileException>();
    }

    [Key]
    [Column("EVENT_ID")]
    public Guid EventId { get; set; }

    [Column("FILE_ID")]
    public Guid FileId { get; set; }

    [Column("DESTINATION_ID")]
    public Guid DestinationId { get; set; }

    [Required]
    [StringLength(20)]
    [NonUnicode]
    [Column("EVENT_CD")]
    public string EventCD { get; set; } = null!;

    [Column("CREATED_TS")]
    public DateTime CreatedDate { get; set; }

    [Required]
    [StringLength(200)]
    [NonUnicode]
    [Column("OPERATOR_ID")]
    public string OperatorId { get; set; } = null!;

    [Column("LAST_MODIFIED_TS")]
    public DateTime LastModifiedTs { get; set; }

    [StringLength(500)]
    [NonUnicode]
    [Column("EVENT_TRACK_DESCRIPTION")]
    public string EventTrackDescription { get; set; } = null!;

    public virtual FtsDestinationLocations FtsDestinationLocationsFK { get; set; } = null!;

    public virtual FtsFileManagement FtsFileManagementFK { get; set; } = null!;

    public virtual ICollection<FtsFileException> FtsFileExceptionFK { get; set; }
}

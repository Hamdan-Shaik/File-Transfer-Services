using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FxferService.Infrastructure.Persistence.Entities;

[Table("FTS_FILE_EXCEPTION")]
public partial class FtsFileException
{
    [Key]
    [Column("EXCEPTION_ID")]
    public Guid ExceptionId { get; set; }

    [Column("FILE_ID")]
    public Guid FileId { get; set; }

    [Column("DESTINATION_ID")]
    public Guid DestinationId { get; set; }

    [Column("EVENT_ID")]
    public Guid EventId { get; set; }

    [Required]
    [StringLength(2000)]
    [NonUnicode]
    [Column("EXCEPTION_TYPE")]
    public string ExceptionType { get; set; } = null!;

    [Required]
    [StringLength(2000)]
    [NonUnicode]
    [Column("METHOD_NAME")]
    public string MethodName { get; set; } = null!;

    [StringLength(2000)]
    [NonUnicode]
    [Column("MESSAGE")]
    public string Message { get; set; } = null!;

    [StringLength(4000)]
    [NonUnicode]
    [Column("STACK_TRACE")]
    public string StackTrace { get; set; } = null!;

    [Column("CREATED_TS")]
    public DateTime CreatedDate { get; set; }

    [Required]
    [StringLength(200)]
    [NonUnicode]
    [Column("OPERATOR_ID")]
    public string OperatorId { get; set; } = null!;

    [Column("LAST_MODIFIED_TS")]
    public DateTime LastModifiedTs { get; set; }

    public virtual FtsDestinationLocations FtsDestinationLocationsFK { get; set; } = null!;

    public virtual FtsFileEvent FtsFileEventFK { get; set; } = null!;

    public virtual FtsFileManagement FtsFileManagementFK { get; set; } = null!;
}

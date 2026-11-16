using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace FxferService.Infrastructure.Persistence.Entities;

[Table("FTS_NOTIFICATION")]
[ExcludeFromCodeCoverage]
public partial class FtsNotification
{
    [Key]
    [Column("NOTIFICATION_ID")]
    public Guid NotificationId { get; set; }

    [Column("FTS_SOURCE_LOCATION_ID")]
    public Guid FtsSourceLocationId { get; set; }

    [Required]
    [Column("EMAIL_ADDRESS")]
    [StringLength(150)]
    public string EmailAddress { get; set; } = null!;

    [Required]
    [Column("CONTACT_NAME")]
    [StringLength(200)]
    public string ContactName { get; set; } = null!;

    [Required]
    [Column("CONTACT_PHONE")]
    [StringLength(15)]
    public string ContactPhone { get; set; } = null!;

    [Required]
    [Column("NOTIFY_ON_ERROR_IND")]
    public bool NotifyOnErrorInd { get; set; }

    [Required]
    [Column("NOTIFY_ON_SUCCESS_IND")]
    public bool NotifyOnSuccessInd { get; set; }

    [Required]
    [Column("OPERATOR_ID")]
    [StringLength(200)]
    public string OperatorId { get; set; } = null!;

    [Column("CREATED_TS")]
    public DateTime CreatedTs { get; set; }

    [Column("LAST_MODIFIED_TS")]
    public DateTime LastModifiedTs { get; set; }

    public virtual FtsSourceLocation FtsSourceLocationFK { get; set; } = null!;
}

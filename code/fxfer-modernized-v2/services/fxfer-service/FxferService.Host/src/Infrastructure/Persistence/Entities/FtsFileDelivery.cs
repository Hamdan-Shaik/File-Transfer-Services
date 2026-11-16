using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FxferService.Infrastructure.Persistence.Entities;

[Table("FTS_FILE_DELIVERY")]
public partial class FtsFileDelivery
{
    [Key]
    [Column("DELIVERY_ID")]
    public Guid DeliveryId { get; set; }

    [Column("FILE_ID")]
    public Guid FileId { get; set; }

    [Column("DESTINATION_ID")]
    public Guid DestinationId { get; set; }

    [Column("DELIVERY_TS")]
    public DateTime DeliveryTs { get; set; }

    [Required]
    [StringLength(255)]
    [NonUnicode]
    [Column("DESTINATION_FILE_NAME")]
    public string DestinationFileName { get; set; } = null!;

    [Required]
    [StringLength(200)]
    [NonUnicode]
    [Column("OPERATOR_ID")]
    public string OperatorId { get; set; } = null!;

    [Column("LAST_MODIFIED_TS")]
    public DateTime LastModifiedTs { get; set; }

    [Column("CREATED_TS")]
    public DateTime CreatedDate { get; set; }

    public virtual FtsDestinationLocations FtsDestinationLocationsFK { get; set; } = null!;

    public virtual FtsFileManagement FtsFileManagementFK { get; set; } = null!;
}

using System.Diagnostics.CodeAnalysis;

namespace Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

// Legacy entity: HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities.FtsNotification
// Table: FTS_NOTIFICATION
// Columns (legacy casing): NOTIFICATION_ID, FTS_SOURCE_LOCATION_ID, EMAIL_ADDRESS, CONTACT_NAME,
// CONTACT_PHONE, NOTIFY_ON_ERROR_IND, NOTIFY_ON_SUCCESS_IND, OPERATOR_ID, CREATED_TS, LAST_MODIFIED_TS
// Legacy constraints: EMAIL_ADDRESS Required StringLength(150); CONTACT_NAME Required StringLength(200);
// CONTACT_PHONE Required StringLength(15); OPERATOR_ID Required StringLength(200)
[ExcludeFromCodeCoverage]
public partial class FtsNotification
{
    public Guid NotificationId { get; set; }
    public Guid FtsSourceLocationId { get; set; }
    public required string EmailAddress { get; set; }
    public required string ContactName { get; set; }
    public required string ContactPhone { get; set; }
    public bool NotifyOnErrorInd { get; set; }
    public bool NotifyOnSuccessInd { get; set; }
    public required string OperatorId { get; set; }
    public DateTime CreatedTs { get; set; }
    public DateTime LastModifiedTs { get; set; }

    public virtual FtsSourceLocation? FtsSourceLocationFK { get; set; }
}


namespace Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

// Legacy entity: HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities.FtsFileDelivery
// Table: FTS_FILE_DELIVERY
// Columns (legacy casing): DELIVERY_ID, FILE_ID, DESTINATION_ID, DELIVERY_TS, DESTINATION_FILE_NAME,
// OPERATOR_ID, LAST_MODIFIED_TS, CREATED_TS
// Legacy constraints: DESTINATION_FILE_NAME Required StringLength(255) NonUnicode; OPERATOR_ID Required StringLength(200) NonUnicode
public sealed class FtsFileDelivery
{
    public Guid DeliveryId { get; set; }
    public Guid FileId { get; set; }
    public Guid DestinationId { get; set; }
    public DateTime DeliveryTs { get; set; }
    public required string DestinationFileName { get; set; }
    public required string OperatorId { get; set; }
    public DateTime LastModifiedTs { get; set; }
    public DateTime CreatedDate { get; set; }

    public FtsDestinationLocations? FtsDestinationLocationsFK { get; set; }

    public FtsFileManagement? FtsFileManagementFK { get; set; }
}


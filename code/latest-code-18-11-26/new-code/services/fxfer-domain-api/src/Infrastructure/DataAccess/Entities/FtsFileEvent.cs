namespace Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

// Legacy entity: HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities.FtsFileEvent
// Table: FTS_FILE_EVENT
// Columns (legacy casing): EVENT_ID, FILE_ID, DESTINATION_ID, EVENT_CD, CREATED_TS, OPERATOR_ID,
// LAST_MODIFIED_TS, EVENT_TRACK_DESCRIPTION
// Legacy constraints: EVENT_CD Required StringLength(20) NonUnicode; OPERATOR_ID Required StringLength(200) NonUnicode; EVENT_TRACK_DESCRIPTION StringLength(500) NonUnicode
public sealed class FtsFileEvent
{
    public FtsFileEvent()
    {
        FtsFileExceptionFK = new HashSet<FtsFileException>();
    }

    public Guid EventId { get; set; }
    public Guid FileId { get; set; }
    public Guid DestinationId { get; set; }
    public required string EventCD { get; set; }
    public DateTime CreatedDate { get; set; }
    public required string OperatorId { get; set; }
    public DateTime LastModifiedTs { get; set; }
    public string? EventTrackDescription { get; set; }

    public FtsDestinationLocations? FtsDestinationLocationsFK { get; set; }

    public FtsFileManagement? FtsFileManagementFK { get; set; }

    public ICollection<FtsFileException> FtsFileExceptionFK { get; set; }
}


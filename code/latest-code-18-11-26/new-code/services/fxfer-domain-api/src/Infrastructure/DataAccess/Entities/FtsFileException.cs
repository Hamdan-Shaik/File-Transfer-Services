namespace Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

// Legacy entity: HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities.FtsFileException
// Table: FTS_FILE_EXCEPTION
// Columns (legacy casing): EXCEPTION_ID, FILE_ID, DESTINATION_ID, EVENT_ID, EXCEPTION_TYPE,
// METHOD_NAME, MESSAGE, STACK_TRACE, CREATED_TS, OPERATOR_ID, LAST_MODIFIED_TS
// Legacy constraints: EXCEPTION_TYPE Required StringLength(2000) NonUnicode; METHOD_NAME Required StringLength(2000) NonUnicode;
// MESSAGE StringLength(2000) NonUnicode; STACK_TRACE StringLength(4000) NonUnicode; OPERATOR_ID Required StringLength(200) NonUnicode
public sealed class FtsFileException
{
    public Guid ExceptionId { get; set; }
    public Guid FileId { get; set; }
    public Guid DestinationId { get; set; }
    public Guid EventId { get; set; }
    public required string ExceptionType { get; set; }
    public required string MethodName { get; set; }
    public string? Message { get; set; }
    public string? StackTrace { get; set; }
    public DateTime CreatedDate { get; set; }
    public required string OperatorId { get; set; }
    public DateTime LastModifiedTs { get; set; }

    public FtsDestinationLocations? FtsDestinationLocationsFK { get; set; }

    public FtsFileEvent? FtsFileEventFK { get; set; }

    public FtsFileManagement? FtsFileManagementFK { get; set; }
}


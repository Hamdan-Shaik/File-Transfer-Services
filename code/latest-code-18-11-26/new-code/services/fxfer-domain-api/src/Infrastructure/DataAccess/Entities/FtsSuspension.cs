using System.Diagnostics.CodeAnalysis;

namespace Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

// Legacy entity: HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities.FtsSuspension
// Table: FTS_SUSPENSION
// Columns (legacy casing): SUSPENSION_ID, FTS_SOURCE_LOCATION_ID, ERROR_TEXT, STACK_TRACE, OPERATOR_ID,
// CREATED_TS, LAST_MODIFIED_TS
// Legacy-to-target column naming (fxfer-db-schema): CREATED_TS -> created_ts
[ExcludeFromCodeCoverage]
public sealed class FtsSuspension
{
    public Guid SuspensionId { get; set; }
    public Guid FtsSourceLocationId { get; set; }
    public required string ErrorText { get; set; }
    public required string StackTrace { get; set; }
    public required string OperatorId { get; set; }
    public DateTime CreatedTs { get; set; }
    public DateTime LastModifiedTs { get; set; }

    public FtsSourceLocation? FtsSourceLocationFK { get; set; }
}

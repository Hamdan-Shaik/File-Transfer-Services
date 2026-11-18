namespace Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

// Legacy entity: HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities.FtsFileManagement
// Table: FTS_FILE_MANAGEMENT
// Columns (legacy casing): FILE_ID, FTS_SOURCE_LOCATION_ID, FILE_NAME, FILE_STATUS, FILE_STATUS_TS,
// SAK_FILE_TRACK, FILE_SIZE, CHECKSUM_CD, OPERATOR_ID, LAST_MODIFIED_TS, CREATED_TS, GROUP_ID
// Legacy constraints: FILE_NAME Required StringLength(255) NonUnicode; FILE_STATUS Required StringLength(1) NonUnicode (fixed-length);
// CHECKSUM_CD StringLength(20) NonUnicode; OPERATOR_ID Required StringLength(200) NonUnicode
public sealed class FtsFileManagement
{
    public FtsFileManagement()
    {
        FtsFileDeliveryFK = new HashSet<FtsFileDelivery>();
        FtsFileEventFK = new HashSet<FtsFileEvent>();
        FtsFileExceptionFK = new HashSet<FtsFileException>();
    }

    public Guid FileId { get; set; }
    public Guid FileSourceLocationId { get; set; }
    public required string FileName { get; set; }
    public required string FileStatus { get; set; }
    public DateTime FileStatusTs { get; set; }
    public int? SakFileTrack { get; set; }
    public decimal FileSize { get; set; }
    public string? ChecksumCD { get; set; }
    public required string OperatorId { get; set; }
    public DateTime LastModifiedTs { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid GroupId { get; set; }

    public ICollection<FtsFileDelivery> FtsFileDeliveryFK { get; set; }
    public ICollection<FtsFileEvent> FtsFileEventFK { get; set; }
    public ICollection<FtsFileException> FtsFileExceptionFK { get; set; }

    public FtsSourceLocation? FtsSourceLocationFK { get; set; }
}


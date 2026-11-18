using System.Diagnostics.CodeAnalysis;

using Fxfer.DomainApi.Domain.Entities;

namespace Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

// Legacy entity: HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities.FtsSourceLocation
// Table: FTS_SOURCE_LOCATION
// Columns (legacy casing): FTS_SOURCE_LOCATION_ID, CONFIG_ID, CONFIG_NAME, SOURCE_PROTOCOL_CD, SOURCE_ADDRESS,
// SOURCE_FILE_MASK, FTP_SERVER_ID, SEND_NOTIFICATIONS_IND, OWNER_NAME, MODULE_ID, OWNER_PHONE, OWNER_EMAIL,
// CONFIG_DESCRIPTION, RUN_ON_MONDAY_IND, RUN_ON_TUESDAY_IND, RUN_ON_WED_IND, RUN_ON_THURS_IND, RUN_ON_FRI_IND,
// RUN_ON_SAT_IND, RUN_ON_SUN_IND, RUN_ON_FRST_DAY_OF_MONTH_IND, RUN_ON_LAST_DAY_OF_MONTH_IND,
// RUN_ON_THIS_DAY_OF_MONTH, START_TIME_24HHMM, END_TIME_24HHMM, FILE_DIRECTION, TRADING_PARTNER, IS_VALID,
// IS_SUSPENDED, IS_SUSPENDED_CHANGED_TS, OPERATOR_ID, CREATED_TS, LAST_MODIFIED_TS
// Legacy-to-target column naming (fxfer-db-schema): MODULE_ID -> owner_module, CREATED_TS -> created_ts
[ExcludeFromCodeCoverage]
public sealed class FtsSourceLocation
{
    public FtsSourceLocation()
    {
        FtsFileManagementFK = new HashSet<FtsFileManagement>();
    }

    public Guid FtsSourceLocationId { get; set; }
    public int ConfigId { get; set; }
    public string ConfigName { get; set; } = string.Empty;
    public string SourceProtocolCD { get; set; } = string.Empty;
    public string SourceAddress { get; set; } = string.Empty;
    public string SourceFileMask { get; set; } = string.Empty;
    public Guid? FtpServerId { get; set; }
    public bool SendNotificationsInd { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public Guid OwnerModule { get; set; }
    public string? OwnerPhone { get; set; }
    public string? OwnerEmail { get; set; }
    public string? Description { get; set; }
    public bool RunOnMondayInd { get; set; }
    public bool RunOnTuesdayInd { get; set; }
    public bool RunOnWednesdayInd { get; set; }
    public bool RunOnThursdayInd { get; set; }
    public bool RunOnFridayInd { get; set; }
    public bool RunOnSaturdayInd { get; set; }
    public bool RunOnSundayInd { get; set; }
    public bool RunOnFirstDayOfMonthInd { get; set; }
    public bool RunOnLastDayOfMonthInd { get; set; }
    public short? RunOnThisDayOfMonth { get; set; }
    public string StartTime24HHMM { get; set; } = string.Empty;
    public string EndTime24HHMM { get; set; } = string.Empty;
    public string? FileDirection { get; set; }
    public string? TradingPartner { get; set; }
    public bool IsValid { get; set; }
    public bool IsSuspended { get; set; }
    public DateTimeOffset? IsSuspendedChangedTs { get; set; }
    public string OperatorId { get; set; } = string.Empty;
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset LastModifiedDate { get; set; }

    public FtpServer? FtpServer { get; set; }

    public ICollection<FtsFileManagement> FtsFileManagementFK { get; set; }
}

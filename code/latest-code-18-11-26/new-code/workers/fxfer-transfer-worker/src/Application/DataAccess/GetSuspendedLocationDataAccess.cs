using System.Net;
using System.Net.Http.Json;

using Fxfer.TransferWorker.Application.Constants;
using Fxfer.TransferWorker.Contracts.Batch;
using Fxfer.TransferWorker.Domain.Constants;

namespace Fxfer.TransferWorker.Application.DataAccess;

public sealed class GetSuspendedLocationDataAccess : DataAccessBase<SourceLocationModel?>
{
    private readonly Guid _sourceLocationId;

    public GetSuspendedLocationDataAccess(
        IFxferDomainApiClient domainApiClient,
        IRequestContextAccessor requestContextAccessor,
        Guid sourceLocationId)
        : base(domainApiClient, requestContextAccessor)
    {
        _sourceLocationId = sourceLocationId;
    }

    protected override async Task<SourceLocationModel?> ExecuteQueryAsync(CancellationToken cancellationToken)
    {
        var configuration = await GetTransferConfigurationAsync(DomainApiClient, cancellationToken);
        if (configuration is null)
        {
            return null;
        }

        var entitySourceData = MapSourceLocationEntity(configuration);

        var ftpServer = new FtpServer();
        if (configuration.FtpServerId is Guid ftpServerId && ftpServerId != Guid.Empty)
        {
            ftpServer = await GetFtpServerAsync(DomainApiClient, ftpServerId, cancellationToken) ?? ftpServer;
        }

        var sourceData = GetSourceLocationData(entitySourceData, ftpServer);
        return sourceData;
    }

    private async Task<TransferConfiguration?> GetTransferConfigurationAsync(IFxferDomainApiClient domainApiClient, CancellationToken cancellationToken)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/fxfer-domain/v1/transfer-configurations/{_sourceLocationId}");
        using var response = await domainApiClient.SendAsync(httpRequest, cancellationToken);

        if (response.StatusCode is HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TransferConfiguration>(cancellationToken);
    }

    private static async Task<FtpServer?> GetFtpServerAsync(IFxferDomainApiClient domainApiClient, Guid ftpServerId, CancellationToken cancellationToken)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/fxfer-domain/v1/ftp-servers/{ftpServerId}");
        using var response = await domainApiClient.SendAsync(httpRequest, cancellationToken);

        if (response.StatusCode is HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<FtpServer>(cancellationToken);
    }

    private static FtsSourceLocation MapSourceLocationEntity(TransferConfiguration configuration)
    {
        return new FtsSourceLocation
        {
            FtsSourceLocationId = configuration.FtsSourceLocationId,
            Configid = configuration.ConfigId,
            SourceProtocalCode = configuration.Protocol,
            SourceAddress = configuration.Address,
            SourceFileMask = configuration.Mask,
            SendNotificationsIndicator = configuration.SendNotification ?? false,
            IsValid = configuration.IsActive,
            IsSuspended = configuration.Suspended ?? false,
            IsSuspendedChangedDate = configuration.SuspendedChangeDate?.UtcDateTime,
            FtpServerId = configuration.FtpServerId,
            RunOnMondayIndicator = configuration.RunOnMonday ?? false,
            RunOnTuesdayIndicator = configuration.RunOnTuesday ?? false,
            RunOnWedIndicator = configuration.RunOnWednesday ?? false,
            RunOnThursIndicator = configuration.RunOnThursday ?? false,
            RunOnFriIndicator = configuration.RunOnFriday ?? false,
            RunOnSatIndicator = configuration.RunOnSaturday ?? false,
            RunOnSunIndicator = configuration.RunOnSunday ?? false,
            RunOnFrstDayOfMonthIndicator = configuration.RunOnFirstDay ?? false,
            RunOnLastDayOfMonthIndicator = configuration.RunOnLastDay ?? false,
            RunOnThisDayOfMonth = configuration.RunOnThisDay ?? 0,
            StartTime24HHMM = configuration.StartWindow,
            EndTime24HHMM = configuration.EndWindow,
        };
    }

    private SourceLocationModel GetSourceLocationData(FtsSourceLocation sourceLocData, FtpServer ftpServer)
    {
        SourceLocationModel model = GetSourceLocationModel(sourceLocData);

        UpdateWeeklyScheduleData(ref model, sourceLocData);

        UpdateMonthlyScheduleData(ref model, sourceLocData);

        UpdateScheduleStartTime(ref model, sourceLocData);

        UpdateScheduleEndTime(ref model, sourceLocData);

        UpdateFtpServerData(ref model, ftpServer, sourceLocData.SourceProtocalCode);

        return model;
    }

    private static SourceLocationModel GetSourceLocationModel(FtsSourceLocation entitySourceData)
    {
        SourceLocationModel model = new SourceLocationModel();
        model.FtpServerDetail = new FtpServerModel();
        model.Schedule = new SourceScheduleModel()
        {
            DaysToRun = new List<DayOfWeek>(),
            MonthlyProcessDayToRun = new List<int>()
        };
        model.SourceLocationId = entitySourceData.FtsSourceLocationId;
        model.ConfigurationId = entitySourceData.Configid;
        model.SourceProtocolCode = entitySourceData.SourceProtocalCode;
        model.SourceAddress = entitySourceData.SourceAddress;
        model.FileMask = entitySourceData.SourceFileMask;
        model.SendNotificationsIndicator = entitySourceData.SendNotificationsIndicator;
        model.IsActive = entitySourceData.IsValid;
        model.IsSuspended = entitySourceData.IsSuspended;
        model.SuspendedIndChanged = entitySourceData.IsSuspendedChangedDate.HasValue ? entitySourceData.IsSuspendedChangedDate.Value : DateTime.MinValue;
        model.SuspendedDate = model.SuspendedIndChanged;

        return model;
    }

    private static void UpdateMonthlyScheduleData(ref SourceLocationModel sourceModel, FtsSourceLocation entitySourceData)
    {
        var schedule = sourceModel.Schedule ?? throw new NullReferenceException();
        if (entitySourceData.RunOnFrstDayOfMonthIndicator)
        {
            schedule.MonthlyProcessDayToRun.Add(1);
        }

        if (entitySourceData.RunOnLastDayOfMonthIndicator)
        {
            schedule.MonthlyProcessDayToRun.Add(DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month));
        }

        if (!Convert.ToBoolean(entitySourceData.RunOnThisDayOfMonth))
        {
            schedule.MonthlyProcessDayToRun.Add(Convert.ToInt32(entitySourceData.RunOnThisDayOfMonth));
        }
    }

    private static void UpdateWeeklyScheduleData(ref SourceLocationModel sourceModel, FtsSourceLocation entitySourceData)
    {
        var schedule = sourceModel.Schedule ?? throw new NullReferenceException();
        if (entitySourceData.RunOnMondayIndicator)
        {
            schedule.DaysToRun.Add(DayOfWeek.Monday);
        }

        if (entitySourceData.RunOnTuesdayIndicator)
        {
            schedule.DaysToRun.Add(DayOfWeek.Tuesday);
        }

        if (entitySourceData.RunOnWedIndicator)
        {
            schedule.DaysToRun.Add(DayOfWeek.Wednesday);
        }

        if (entitySourceData.RunOnThursIndicator)
        {
            schedule.DaysToRun.Add(DayOfWeek.Thursday);
        }

        if (entitySourceData.RunOnFriIndicator)
        {
            schedule.DaysToRun.Add(DayOfWeek.Friday);
        }

        if (entitySourceData.RunOnSatIndicator)
        {
            schedule.DaysToRun.Add(DayOfWeek.Saturday);
        }

        if (entitySourceData.RunOnSunIndicator)
        {
            schedule.DaysToRun.Add(DayOfWeek.Sunday);
        }
    }

    private static void UpdateScheduleStartTime(ref SourceLocationModel sourceModel, FtsSourceLocation entitySourceData)
    {
        var schedule = sourceModel.Schedule ?? throw new NullReferenceException();
        string? startTimeData = entitySourceData.StartTime24HHMM;
        int startTimeHours = 0;
        int startTimeMinutes = 0;
        TimeSpan scheduledTime = new TimeSpan(0, 0, 0);
        if (string.IsNullOrEmpty(startTimeData) == false && startTimeData.Length > 3 &&
            int.TryParse(startTimeData.Substring(0, 2), out startTimeHours) && int.TryParse(startTimeData.Substring(2, 2), out startTimeMinutes))
        {
            schedule.StartTime = new TimeSpan(startTimeHours, startTimeMinutes, 0);
        }
        else
        {
            schedule.StartTime = scheduledTime;
        }
    }

    private static void UpdateScheduleEndTime(ref SourceLocationModel sourceModel, FtsSourceLocation entitySourceData)
    {
        var schedule = sourceModel.Schedule ?? throw new NullReferenceException();
        string? endTimeData = entitySourceData.EndTime24HHMM;
        int endTimeHours = 0;
        int endTimeMinutes = 0;
        TimeSpan scheduledTime = new TimeSpan(0, 0, 0);
        if (string.IsNullOrEmpty(endTimeData) == false && endTimeData.Length > 3 &&
            int.TryParse(endTimeData.Substring(0, 2), out endTimeHours) && int.TryParse(endTimeData.Substring(2, 2), out endTimeMinutes))
        {
            schedule.EndTime = new TimeSpan(endTimeHours, endTimeMinutes, 0);
        }
        else
        {
            schedule.EndTime = scheduledTime;
        }
    }

    private static void UpdateFtpServerData(ref SourceLocationModel sourceModel, FtpServer entityFtpServerData, string? protocol)
    {
        var ftpServerDetail = sourceModel.FtpServerDetail ?? throw new NullReferenceException();
        if (string.Compare(protocol, ProtocolCodeConstant.UNC, StringComparison.InvariantCultureIgnoreCase) != 0)
        {
            ftpServerDetail.FtpServerIp = entityFtpServerData.FtpServerIp;
            ftpServerDetail.FtpUser = entityFtpServerData.FtpUser;
            ftpServerDetail.FtpPassword = entityFtpServerData.FtpPassword;
            ftpServerDetail.FtpPort = entityFtpServerData.FtpPort;
            ftpServerDetail.SSHAuth = entityFtpServerData.SSHAuth;
        }
    }

    private sealed record TransferConfiguration
    {
        public Guid FtsSourceLocationId { get; init; }
        public long ConfigId { get; init; }
        public Guid? FtpServerId { get; init; }
        public string? Address { get; init; }
        public string? Mask { get; init; }
        public string? Protocol { get; init; }
        public bool? SendNotification { get; init; }
        public bool IsActive { get; init; }
        public bool? Suspended { get; init; }
        public DateTimeOffset? SuspendedChangeDate { get; init; }

        public bool? RunOnMonday { get; init; }
        public bool? RunOnTuesday { get; init; }
        public bool? RunOnWednesday { get; init; }
        public bool? RunOnThursday { get; init; }
        public bool? RunOnFriday { get; init; }
        public bool? RunOnSaturday { get; init; }
        public bool? RunOnSunday { get; init; }

        public bool? RunOnFirstDay { get; init; }
        public bool? RunOnLastDay { get; init; }
        public int? RunOnThisDay { get; init; }

        public string? StartWindow { get; init; }
        public string? EndWindow { get; init; }
    }

    private sealed class FtsSourceLocation
    {
        public Guid FtsSourceLocationId { get; init; }
        public long Configid { get; init; }
        public string? SourceProtocalCode { get; init; }
        public string? SourceAddress { get; init; }
        public string? SourceFileMask { get; init; }
        public bool SendNotificationsIndicator { get; init; }
        public bool IsValid { get; init; }
        public bool IsSuspended { get; init; }
        public DateTime? IsSuspendedChangedDate { get; init; }

        public Guid? FtpServerId { get; init; }

        public bool RunOnMondayIndicator { get; init; }
        public bool RunOnTuesdayIndicator { get; init; }
        public bool RunOnWedIndicator { get; init; }
        public bool RunOnThursIndicator { get; init; }
        public bool RunOnFriIndicator { get; init; }
        public bool RunOnSatIndicator { get; init; }
        public bool RunOnSunIndicator { get; init; }

        public bool RunOnFrstDayOfMonthIndicator { get; init; }
        public bool RunOnLastDayOfMonthIndicator { get; init; }
        public int RunOnThisDayOfMonth { get; init; }

        public string? StartTime24HHMM { get; init; }
        public string? EndTime24HHMM { get; init; }
    }

    private sealed record FtpServer
    {
        public string? FtpServerIp { get; init; }
        public string? FtpUser { get; init; }
        public string? FtpPassword { get; init; }

        public int FtpPort { get; init; }

        public bool SSHAuth { get; init; }
    }
}


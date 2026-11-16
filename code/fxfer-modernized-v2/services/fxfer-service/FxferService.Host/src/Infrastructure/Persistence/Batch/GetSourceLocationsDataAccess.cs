using FxferService.Application.Persistence.Batch;
using FxferService.Domain.Utilities;
using FxferService.Infrastructure.Persistence.Batch.Contracts;
using FxferService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FxferService.Infrastructure.Persistence.Batch;

public sealed class GetSourceLocationsDataAccess : DataAccessBase<List<SourceLocationModel>>
{
    protected override List<SourceLocationModel> ExecuteQuery(IDbContextBase dbcontext)
    {
        var results = dbcontext.Set<FtsSourceLocationEntity>().AsNoTracking().ToList();
        List<SourceLocationModel> sourceLocationList = new();
        foreach (var data in results)
        {
            var ftpServer = new FtpServer();
            if (data.FtpServerId != null)
            {
                var loaded = dbcontext.Set<FtpServer>().AsNoTracking().FirstOrDefault(x => x.FtpServerId == data.FtpServerId);
                if (loaded != null)
                {
                    ftpServer = loaded;
                }
            }

            SourceLocationModel sourceData = GetSourceLocationData(data, ftpServer);
            sourceLocationList.Add(sourceData);
        }

        return sourceLocationList;
    }

    private SourceLocationModel GetSourceLocationData(FtsSourceLocationEntity sourceLocData, FtpServer ftpServer)
    {
        SourceLocationModel model = GetSourceLocationModel(sourceLocData);

        UpdateWeeklyScheduleData(ref model, sourceLocData);

        UpdateMonthlyScheduleData(ref model, sourceLocData);

        UpdateScheduleStartTime(ref model, sourceLocData);

        UpdateScheduleEndTime(ref model, sourceLocData);

        UpdateFtpServerData(ref model, ftpServer, sourceLocData.SourceProtocolCode);

        return model;
    }

    [MaintainabilityDetails(ApprovedIndex = 55, ReviewedOnSprint = 94, ReviewerName = "Josue Chavez")]
    private SourceLocationModel GetSourceLocationModel(FtsSourceLocationEntity entitySourceData)
    {
        SourceLocationModel model = new();
        model.FtpServerDetail = new FxferService.Domain.Contracts.Domain.FtpServerModel();
        model.Schedule = new SourceScheduleModel
        {
            DaysToRun = new List<DayOfWeek>(),
            MonthlyProcessDayToRun = new List<int>(),
        };

        model.SourceLocationId = entitySourceData.FtsSourceLocationId;
        model.ConfigurationId = entitySourceData.ConfigId;
        model.SourceProtocolCode = entitySourceData.SourceProtocolCode;
        model.SourceAddress = entitySourceData.SourceAddress;
        model.FileMask = entitySourceData.SourceFileMask;
        model.OwnerEmail = entitySourceData.OwnerEmail;
        model.OwnerName = entitySourceData.OwnerName;
        model.SendNotificationsIndicator = entitySourceData.SendNotifications;
        model.IsActive = entitySourceData.IsValid;
        model.IsSuspended = entitySourceData.IsSuspended;
        model.SuspendedIndChanged = entitySourceData.SuspendedChangedAt.HasValue
            ? entitySourceData.SuspendedChangedAt.Value.UtcDateTime
            : DateTime.MinValue;
        model.ConfigurationId = entitySourceData.ConfigId;
        model.ConfigurationName = entitySourceData.ConfigName;
        model.ConfigurationDescription = entitySourceData.ConfigDescription;
        model.FileDirection = entitySourceData.FileDirection;

        return model;
    }

    private void UpdateMonthlyScheduleData(ref SourceLocationModel sourceModel, FtsSourceLocationEntity entitySourceData)
    {
        if (entitySourceData.RunOnFirstDayOfMonth)
        {
            sourceModel.Schedule!.MonthlyProcessDayToRun.Add(1);
        }

        if (entitySourceData.RunOnLastDayOfMonth)
        {
            sourceModel.Schedule!.MonthlyProcessDayToRun.Add(DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month));
        }

        if (!Convert.ToBoolean(entitySourceData.RunOnThisDayOfMonth))
        {
            sourceModel.Schedule!.MonthlyProcessDayToRun.Add(Convert.ToInt32(entitySourceData.RunOnThisDayOfMonth));
        }
    }

    [MaintainabilityDetails(ApprovedIndex = 57, ReviewedOnSprint = 94, ReviewerName = "Josue Chavez")]
    private void UpdateWeeklyScheduleData(ref SourceLocationModel sourceModel, FtsSourceLocationEntity entitySourceData)
    {
        if (entitySourceData.RunOnMonday)
        {
            sourceModel.Schedule!.DaysToRun.Add(DayOfWeek.Monday);
        }

        if (entitySourceData.RunOnTuesday)
        {
            sourceModel.Schedule!.DaysToRun.Add(DayOfWeek.Tuesday);
        }

        if (entitySourceData.RunOnWednesday)
        {
            sourceModel.Schedule!.DaysToRun.Add(DayOfWeek.Wednesday);
        }

        if (entitySourceData.RunOnThursday)
        {
            sourceModel.Schedule!.DaysToRun.Add(DayOfWeek.Thursday);
        }

        if (entitySourceData.RunOnFriday)
        {
            sourceModel.Schedule!.DaysToRun.Add(DayOfWeek.Friday);
        }

        if (entitySourceData.RunOnSaturday)
        {
            sourceModel.Schedule!.DaysToRun.Add(DayOfWeek.Saturday);
        }

        if (entitySourceData.RunOnSunday)
        {
            sourceModel.Schedule!.DaysToRun.Add(DayOfWeek.Sunday);
        }
    }

    private void UpdateScheduleStartTime(ref SourceLocationModel sourceModel, FtsSourceLocationEntity entitySourceData)
    {
        string startTimeData = entitySourceData.StartTime24hhmm;
        int startTimeHours = 0;
        int startTimeMinutes = 0;
        TimeSpan scheduledTime = new TimeSpan(0, 0, 0);
        if (string.IsNullOrEmpty(startTimeData) == false && startTimeData.Length > 3
            && int.TryParse(startTimeData.Substring(0, 2), out startTimeHours)
            && int.TryParse(startTimeData.Substring(2, 2), out startTimeMinutes))
        {
            sourceModel.Schedule!.StartTime = new TimeSpan(startTimeHours, startTimeMinutes, 0);
        }
        else
        {
            sourceModel.Schedule!.StartTime = scheduledTime;
        }
    }

    private void UpdateScheduleEndTime(ref SourceLocationModel sourceModel, FtsSourceLocationEntity entitySourceData)
    {
        string endTimeData = entitySourceData.EndTime24hhmm;
        int endTimeHours = 0;
        int endTimeMinutes = 0;
        TimeSpan scheduledTime = new TimeSpan(0, 0, 0);
        if (string.IsNullOrEmpty(endTimeData) == false && endTimeData.Length > 3
            && int.TryParse(endTimeData.Substring(0, 2), out endTimeHours)
            && int.TryParse(endTimeData.Substring(2, 2), out endTimeMinutes))
        {
            sourceModel.Schedule!.EndTime = new TimeSpan(endTimeHours, endTimeMinutes, 0);
        }
        else
        {
            sourceModel.Schedule!.EndTime = scheduledTime;
        }
    }

    private void UpdateFtpServerData(ref SourceLocationModel sourceModel, FtpServer entityFtpServerData, string protocol)
    {
        if (string.Compare(protocol, ProtocolCodeConstant.UNC, StringComparison.InvariantCultureIgnoreCase) != 0)
        {
            sourceModel.FtpServerDetail.FtpServerIp = entityFtpServerData.FtpServerIp;
            sourceModel.FtpServerDetail.FtpUser = entityFtpServerData.FtpUser;
            sourceModel.FtpServerDetail.FtpPassword = entityFtpServerData.FtpPassword;
            sourceModel.FtpServerDetail.FtpPort = entityFtpServerData.FtpPort;
            sourceModel.FtpServerDetail.UseSSHAuth = entityFtpServerData.UseSSHAuth;
        }
    }
}

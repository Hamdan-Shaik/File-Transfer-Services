using System;
using System.Collections.Generic;

namespace Fxfer.ExecutionWorker.Application.Contracts.Domain;

public class SourceLocationModel
{
    public SourceLocationModel()
    {
        Destinations = new List<DestinationModel>();
    }

    public Guid SourceLocationId { get; set; }

    public long ConfigurationId { get; set; }

    public int PreviousConfigurationId { get; set; }

    public string ConfigurationName { get; set; } = null!;

    public string ConfigurationDescription { get; set; } = null!;

    public string SourceProtocolCode { get; set; } = null!;

    public string SourceAddress { get; set; } = null!;

    public bool SendNotificationsIndicator { get; set; }

    public int PreviousConfigId { get; set; }

    public FtpServerModel FtpServerDetail { get; set; } = null!;

    public SourceScheduleModel? Schedule { get; set; }

    public List<DestinationModel> Destinations { get; set; } = new();

    public string FileMask { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsSuspended { get; set; }

    public DateTime SuspendedIndChanged { get; set; }

    public DateTime SuspendedDate { get; set; }

    public bool IsScheduledToRun
    {
        get
        {
            bool result = false;

            //if no schedule is set always run the source or if DateTime.Now is in the schedule parameters
            if (Schedule == null || IsNowInScheduledTime)
            {
                result = true;
            }

            return result;
        }
    }

    public int MinsToWaitBeforeRetrySuspendedConfig
    {
        get
        {
            // legacy source read from ConfigurationManager.AppSettings["MinsToWaitBeforeRetrySuspendedConfig"].
            return Convert.ToInt32(Environment.GetEnvironmentVariable(nameof(MinsToWaitBeforeRetrySuspendedConfig))!.ToString()) * -1;
        }
    }

    private bool IsNowInScheduledTime
    {
        get
        {
            if (!IsTimeOfDayBetween(DateTime.Now, Schedule!.StartTime, Schedule.EndTime))
            {
                return false;
            }

            bool result = false;

            if (Schedule.DaysToRun!.Exists(c => c == DateTime.Now.DayOfWeek))
            {
                result = true;
            }
            else if (Schedule.MonthlyProcessDayToRun!.Exists(c => c == DateTime.Now.Day))
            {
                result = true;
            }

            return result;
        }
    }

    private bool IsTimeOfDayBetween(DateTime time, TimeSpan startTime, TimeSpan endTime)
    {
        return time.TimeOfDay >= startTime && time.TimeOfDay <= endTime;
    }

    public string OwnerName { get; set; } = null!;

    public string OwnerEmail { get; set; } = string.Empty;

    public string FileDirection { get; set; } = null!;
}

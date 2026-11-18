namespace Fxfer.TransferWorker.Contracts.Batch;

public sealed class SourceLocationModel
{
    public SourceLocationModel()
    {
        Destinations = new List<DestinationModel>();
    }

    public Guid SourceLocationId { get; set; }

    public long ConfigurationId { get; set; }

    public int PreviousConfigurationId { get; set; }

    public string? SourceProtocolCode { get; set; }

    public string? SourceAddress { get; set; }

    public string? OwnerName { get; set; }

    public string? OwnerEmail { get; set; }

    public string? ConfigurationName { get; set; }

    public string? ConfigurationDescription { get; set; }

    public string? FileDirection { get; set; }

    public bool SendNotificationsIndicator { get; set; }

    public int PreviousConfigId { get; set; }

    public FtpServerModel? FtpServerDetail { get; set; }

    public SourceScheduleModel? Schedule { get; set; }

    public List<DestinationModel> Destinations { get; set; }

    public bool IsActive { get; set; }

    public bool IsSuspended { get; set; }

    public DateTime SuspendedIndChanged { get; set; }

    public string? FileMask { get; set; }

    public DateTime SuspendedDate { get; set; }

    public bool IsScheduledToRun
    {
        get
        {
            bool result = false;

            // if no schedule is set always run the source or if DateTime.Now is in the schedule parameters
            if (this.Schedule == null || this.IsNowInScheduledTime)
            {
                result = true;
            }

            return result;
        }
    }

    public int MinsToWaitBeforeRetrySuspendedConfig { get; set; }

    private bool IsNowInScheduledTime
    {
        get
        {
            if (!this.IsTimeOfDayBetween(DateTime.Now, this.Schedule.StartTime, this.Schedule.EndTime))
            {
                return false;
            }

            bool result = false;

            if (this.Schedule.DaysToRun.Exists(c => c == DateTime.Now.DayOfWeek))
            {
                result = true;
            }
            else if (this.Schedule.MonthlyProcessDayToRun.Exists(c => c == DateTime.Now.Day))
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
}

namespace FxferService.Domain.Contracts.Batch;

/// <summary>
/// <para>Describes a source lcoation that we can retrieve files from.</para>
/// </summary>
public class SourceLocationModel
{
    /// <summary>
    /// <para>Constructor to set default values.</para>
    /// </summary>
    public SourceLocationModel()
    {
        Destinations = new List<DestinationModel>();
    }

    public Guid SourceLocationId { get; set; }

    public long ConfigurationId { get; set; }

    public int PreviousConfigurationId { get; set; }

    public string? SourceProtocolCode { get; set; }

    public string? SourceAddress { get; set; }

    public bool SendNotificationsIndicator { get; set; }

    public int PreviousConfigId { get; set; }

    /// <summary>
    /// <para>Represent the FTP server if that is the type of protocol being used.</para>
    /// </summary>
    public FtpServerModel? FtpServerDetail { get; set; }

    public SourceScheduleModel? Schedule { get; set; }

    public List<DestinationModel> Destinations { get; set; }

    /// <summary>
    /// Indicates whether the source is active or not
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Indicates if the source ahs been suspended because of some type of err
    /// </summary>
    public bool IsSuspended { get; set; }

    /// <summary>
    /// The date/time the location was suspended
    /// </summary>
    public DateTime SuspendedIndChanged { get; set; }

    /// <summary>
    /// The file mask to apply to files at this source
    /// </summary>
    public string? FileMask { get; set; }

    /// <summary>
    /// <para>The date/time the location was suspended.</para>
    /// </summary>
    public DateTime SuspendedDate { get; set; }

    /// <summary>
    /// <para>Indicates that this souce is within the time window.</para>
    /// </summary>
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

    /// <summary>
    /// <para>Length of time to wait before retrying the location.</para>
    /// </summary>
    public int MinsToWaitBeforeRetrySuspendedConfig
    {
        get
        {
            // legacy source read from ConfigurationManager.AppSettings[MinsToWaitBeforeRetrySuspendedConfig].ToString().
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
}


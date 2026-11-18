namespace Fxfer.DomainApi.Contracts.Domain;

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

    public bool SendNotificationsIndicator { get; set; }

    public FtpServerModel? FtpServerDetail { get; set; }

    public SourceScheduleModel? Schedule { get; set; }

    public List<DestinationModel> Destinations { get; set; }

    public bool IsActive { get; set; }

    public bool IsSuspended { get; set; }

    public DateTime SuspendedIndChanged { get; set; }

    public string? FileMask { get; set; }

    public DateTime SuspendedDate { get; set; }

    public string? OwnerEmail { get; set; }

    public string? OwnerName { get; set; }

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

    public string? ConfigurationName { get; set; }

    public string? ConfigurationDescription { get; set; }

    public string? FileDirection { get; set; }

    // Legacy/BAS query-service shape (used by backend-data-access-query-handlers-2).
    public Guid FtsSourceLocationId { get; set; }

    public int ConfigId { get; set; }

    public string? ConfigName { get; set; }

    public string? Description { get; set; }

    public string? FtpServerName { get; set; }

    public Guid? FtpServerId { get; set; }

    public string? Address { get; set; }

    public Guid OwnerSubsystem { get; set; }

    public string? OwnerPhoneNumber { get; set; }

    public string? Protocol { get; set; }

    public string? Mask { get; set; }

    public string? TradingPartner { get; set; }

    public bool RunOnMonday { get; set; }

    public bool RunOnTuesday { get; set; }

    public bool RunOnWednesday { get; set; }

    public bool RunOnThursday { get; set; }

    public bool RunOnFriday { get; set; }

    public bool RunOnSaturday { get; set; }

    public bool RunOnSunday { get; set; }

    public bool RunOnFirstDay { get; set; }

    public bool RunOnLastDay { get; set; }

    public short? RunOnThisDay { get; set; }

    public bool Suspended { get; set; }

    public string? StartWindow { get; set; }

    public string? EndWindow { get; set; }

    public bool SendNotification { get; set; }

    public List<Fxfer.DomainApi.Contracts.Events.FtsDestinationLocationAdded>? DestinationAddress { get; set; }

    private bool IsNowInScheduledTime
    {
        get
        {
            var schedule = Schedule;
            if (schedule is null)
            {
                return false;
            }

            if (!IsTimeOfDayBetween(DateTime.Now, schedule.StartTime, schedule.EndTime))
            {
                return false;
            }

            bool result = false;

            if (schedule.DaysToRun.Exists(c => c == DateTime.Now.DayOfWeek))
            {
                result = true;
            }
            else if (schedule.MonthlyProcessDayToRun.Exists(c => c == DateTime.Now.Day))
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

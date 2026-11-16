namespace FileTransferConfig;

public sealed class FtsSourceLocationAdded
{
    public string? FtpServerId { get; set; }
    public string? Mask { get; set; }
    public string? Protocol { get; set; }
    public string? OwnerPhone { get; set; }
    public string? OwnerModule { get; set; }

    public bool RunOnMonday { get; set; }
    public bool RunOnTuesday { get; set; }
    public bool RunOnWed { get; set; }
    public bool RunOnThrus { get; set; }
    public bool RunOnFri { get; set; }
    public bool RunOnSat { get; set; }
    public bool RunOnSun { get; set; }
    public bool RunOnFrstDayOfMonthInd { get; set; }
    public bool RunOnLastDayOfMonthInd { get; set; }
    public int RunOnThisDayOfMonth { get; set; }

    public string? StartTime { get; set; }
    public string? EndTime { get; set; }

    public bool SendNotifications { get; set; }
    public string? FileDirection { get; set; }
}

namespace FileTransferConfig;

public sealed class FtsSourceLocationAddCommand
{
    public RequestorModel? Requestor { get; set; }
    public string? ConfigName { get; set; }
    public string? Description { get; set; }
    public string? FtpServerId { get; set; }
    public string? Protocol { get; set; }
    public string? Address { get; set; }
    public string? Mask { get; set; }
    public string? FileDirection { get; set; }
    public bool SendNotifications { get; set; }
    public bool IsValid { get; set; }
    public string? OwnerName { get; set; }
    public string? OwnerModule { get; set; }
    public string? OwnerPhone { get; set; }
    public string? OwnerEmail { get; set; }

    public bool RunOnMonday { get; set; }
    public bool RunOnTuesday { get; set; }
    public bool RunOnWed { get; set; }
    public bool RunOnThrus { get; set; }
    public bool RunOnFri { get; set; }
    public bool RunOnSat { get; set; }
    public bool RunOnSun { get; set; }
    public bool RunOnFrstDayOfMonthInd { get; set; }
    public bool RunOnLastDayOfMonthInd { get; set; }
    public byte RunOnThisDayOfMonth { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }

    public string? TradingPartner { get; set; }
    public string? OperatorId { get; set; }
    public FtsDestinationLocationAdded[] DestinationAddress { get; set; } = Array.Empty<FtsDestinationLocationAdded>();
    public bool IsSuspended { get; set; }
    public DateTime? IsSuspendedChangedDate { get; set; }
}

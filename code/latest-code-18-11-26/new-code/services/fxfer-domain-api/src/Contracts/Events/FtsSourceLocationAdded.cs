namespace Fxfer.DomainApi.Contracts.Events;

public record FtsSourceLocationAdded : FtsSourceLocationEvents
{
    public int ConfigId { get; init; }
    public string? ConfigName { get; init; }
    public string? Description { get; init; }
    public string? FtpServerId { get; init; }
    public string? Address { get; init; }
    public string? Mask { get; init; }
    public string? Protocol { get; init; }
    public string? FileDirection { get; init; }
    public string? OwnerEmail { get; init; }
    public string? OwnerName { get; init; }
    public string? OwnerPhone { get; init; }
    public string? OwnerModule { get; init; }
    public string? TradingPartner { get; init; }
    public string? OperatorId { get; init; }
    public bool RunOnMonday { get; init; }
    public bool RunOnTuesday { get; init; }
    public bool RunOnWed { get; init; }
    public bool RunOnThrus { get; init; }
    public bool RunOnFri { get; init; }
    public bool RunOnSat { get; init; }
    public bool RunOnSun { get; init; }
    public bool RunOnFrstDayOfMonthInd { get; init; }
    public bool RunOnLastDayOfMonthInd { get; init; }
    public byte RunOnThisDayOfMonth { get; init; }
    public string? StartTime { get; init; }
    public string? EndTime { get; init; }
    public bool IsValid { get; init; }
    public bool IsSuspended { get; init; }
    public DateTime? IsSuspendedChangedDate { get; init; }
    public bool SendNotifications { get; init; }
    public DateTime LastModifiedDate { get; init; }
    public DateTime CreationDate { get; init; }
    public List<FtsDestinationLocationAdded>? DestinationAddress { get; init; }
}

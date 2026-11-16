namespace FxferService.Projections.Infrastructure.Messaging.Contracts.Events;

public sealed class FtsSourceLocationUpdated : FtsSourceLocationEventBase
{
    public int ConfigId { get; init; }
    public required string ConfigName { get; init; }
    public required string Description { get; init; }
    public required string FtpServerId { get; init; }
    public required string Address { get; init; }
    public required string Mask { get; init; }
    public required string Protocol { get; init; }
    public required string FileDirection { get; init; }
    public required string OwnerEmail { get; init; }
    public required string OwnerName { get; init; }
    public required string OwnerPhone { get; init; }
    public required string OwnerModule { get; init; }
    public required string TradingPartner { get; init; }
    public required string OperatorId { get; init; }
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
    public required string StartTime { get; init; }
    public required string EndTime { get; init; }
    public bool IsValid { get; init; }
    public bool IsSuspended { get; init; }
    public DateTime? IsSuspendedChangedDate { get; init; }
    public bool SendNotifications { get; init; }
    public required List<FtsDestinationLocationAdded> DestinationAddress { get; init; }
}


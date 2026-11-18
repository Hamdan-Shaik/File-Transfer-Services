namespace Fxfer.ProjectionWorker.Infrastructure.Projections;

public sealed record SourceLocationProjectionPayload
{
    public required string SourceLocationId { get; init; }
    public required int ConfigId { get; init; }
    public required string ConfigName { get; init; }
    public string? Description { get; init; }
    public string? FtpServerId { get; init; }
    public required string Protocol { get; init; }
    public required string Address { get; init; }
    public required string Mask { get; init; }
    public string? FileDirection { get; init; }
    public required bool SendNotifications { get; init; }
    public required bool IsValid { get; init; }
    public required bool IsSuspended { get; init; }
    public DateTimeOffset? IsSuspendedChangedDate { get; init; }
    public required string OwnerName { get; init; }
    public required string OwnerModule { get; init; }
    public string? OwnerPhone { get; init; }
    public string? OwnerEmail { get; init; }
    public required bool RunOnMonday { get; init; }
    public required bool RunOnTuesday { get; init; }
    public required bool RunOnWed { get; init; }
    public required bool RunOnThrus { get; init; }
    public required bool RunOnFri { get; init; }
    public required bool RunOnSat { get; init; }
    public required bool RunOnSun { get; init; }
    public required bool RunOnFrstDayOfMonthInd { get; init; }
    public required bool RunOnLastDayOfMonthInd { get; init; }
    public short? RunOnThisDayOfMonth { get; init; }
    public required string StartTime { get; init; }
    public required string EndTime { get; init; }
    public string? TradingPartner { get; init; }
    public required string OperatorId { get; init; }
    public required List<DestinationProjectionPayload> Destinations { get; init; }
}

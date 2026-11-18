namespace Fxfer.ProjectionWorker.Infrastructure.DataAccess.Entities;

public sealed record FtsSourceLocation
{
    public required Guid FtsSourceLocationId { get; init; }
    public required int ConfigId { get; init; }
    public required string ConfigName { get; init; }
    public required string SourceProtocolCd { get; init; }
    public required string SourceAddress { get; init; }
    public required string SourceFileMask { get; init; }
    public Guid? FtpServerId { get; init; }
    public required bool SendNotificationsInd { get; init; }
    public required string OwnerName { get; init; }
    public required Guid OwnerModule { get; init; }
    public string? OwnerPhone { get; init; }
    public string? OwnerEmail { get; init; }
    public string? Description { get; init; }
    public required bool RunOnMondayInd { get; init; }
    public required bool RunOnTuesdayInd { get; init; }
    public required bool RunOnWednesdayInd { get; init; }
    public required bool RunOnThursdayInd { get; init; }
    public required bool RunOnFridayInd { get; init; }
    public required bool RunOnSaturdayInd { get; init; }
    public required bool RunOnSundayInd { get; init; }
    public required bool RunOnFirstDayOfMonthInd { get; init; }
    public required bool RunOnLastDayOfMonthInd { get; init; }
    public short? RunOnThisDayOfMonth { get; init; }
    public required string StartTime24hhmm { get; init; }
    public required string EndTime24hhmm { get; init; }
    public string? FileDirection { get; init; }
    public string? TradingPartner { get; init; }
    public required bool IsValid { get; init; }
    public required bool IsSuspended { get; init; }
    public DateTimeOffset? IsSuspendedChangedTs { get; init; }
    public required string OperatorId { get; init; }
    public required DateTimeOffset CreatedTs { get; init; }
    public required DateTimeOffset LastModifiedTs { get; init; }
}

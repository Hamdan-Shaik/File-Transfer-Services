namespace Fxfer.DomainApi.Contracts.Domain;

public sealed record TransferConfiguration
{
    public required Guid FtsSourceLocationId { get; init; }
    public int ConfigId { get; init; }
    public required string ConfigName { get; init; }
    public string? Description { get; init; }
    public Guid? FtpServerId { get; init; }
    public string? FtpServerName { get; init; }
    public required string Address { get; init; }
    public string? Mask { get; init; }
    public required string Protocol { get; init; }
    public string? FileDirection { get; init; }
    public required string OwnerName { get; init; }
    public required string OwnerEmail { get; init; }
    public Guid OwnerSubsystem { get; init; }
    public required string OwnerPhoneNumber { get; init; }
    public string? TradingPartner { get; init; }
    public required string OperatorId { get; init; }
    public bool RunOnMonday { get; init; }
    public bool RunOnTuesday { get; init; }
    public bool RunOnWednesday { get; init; }
    public bool RunOnThursday { get; init; }
    public bool RunOnFriday { get; init; }
    public bool RunOnSaturday { get; init; }
    public bool RunOnSunday { get; init; }
    public bool RunOnFirstDay { get; init; }
    public bool RunOnLastDay { get; init; }
    public int RunOnThisDay { get; init; }
    public required string StartWindow { get; init; }
    public required string EndWindow { get; init; }
    public bool SendNotification { get; init; }
    public bool IsActive { get; init; }
    public DateTimeOffset ValidChangeDate { get; init; }
    public bool Suspended { get; init; }
    public DateTimeOffset SuspendedChangeDate { get; init; }
    public DateTimeOffset LastModifiedDate { get; init; }
    public DateTimeOffset CreationDate { get; init; }
    public required IReadOnlyList<Destination> Destinations { get; init; }
}

namespace Fxfer.DomainApi.Contracts.Commands;

public sealed record TransferConfigurationUpsertRequest
{
    public required string ConfigName { get; init; }
    public string? Description { get; init; }
    public Guid? FtpServerId { get; init; }
    public required string Address { get; init; }
    public string? Mask { get; init; }
    public required string Protocol { get; init; }
    public string? FileDirection { get; init; }
    public required string OwnerName { get; init; }
    public required string OwnerEmail { get; init; }
    public required string OwnerPhoneNumber { get; init; }
    public required Guid OwnerSubsystem { get; init; }
    public string? TradingPartner { get; init; }
    public bool? SendNotification { get; init; }
    public bool? RunOnMonday { get; init; }
    public bool? RunOnTuesday { get; init; }
    public bool? RunOnWednesday { get; init; }
    public bool? RunOnThursday { get; init; }
    public bool? RunOnFriday { get; init; }
    public bool? RunOnSaturday { get; init; }
    public bool? RunOnSunday { get; init; }
    public bool? RunOnFirstDay { get; init; }
    public bool? RunOnLastDay { get; init; }
    public int? RunOnThisDay { get; init; }
    public required string StartWindow { get; init; }
    public required string EndWindow { get; init; }
    public bool? IsActive { get; init; }
    public bool? Suspended { get; init; }
    public required IReadOnlyList<DestinationUpsert> Destinations { get; init; }
    public required string OperatorId { get; init; }
}


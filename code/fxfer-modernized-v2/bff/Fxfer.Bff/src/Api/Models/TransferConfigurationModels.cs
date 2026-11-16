using System.ComponentModel.DataAnnotations;

namespace Fxfer.Bff.Api.Models;

public sealed record TransferConfigurationSummary(
    Guid TransferConfigurationId,
    int ConfigNumber,
    string Name,
    string ProtocolCode,
    string Address,
    string FileMask,
    bool IsActive,
    bool IsSuspended);

public sealed record OwnerInfo(
    [param: Required] string Name,
    Guid ModuleId,
    [param: Required] string Phone,
    [param: Required] string Email);

public sealed record ScheduleInfo(
    bool RunOnMonday,
    bool RunOnTuesday,
    bool RunOnWednesday,
    bool RunOnThursday,
    bool RunOnFriday,
    bool RunOnSaturday,
    bool RunOnSunday,
    bool RunOnFirstDayOfMonth,
    bool RunOnLastDayOfMonth,
    int RunOnThisDayOfMonth,
    [param: Required] string StartTime24hhmm,
    [param: Required] string EndTime24hhmm);

public sealed record DestinationContact(string? Name, string? Phone, string? Email);

public sealed record DestinationEncryption(
    string? CdeEncryption,
    string? PublicKey,
    string? PrivateKey,
    string? PrivateKeyPassword);

public sealed record Destination(
    Guid DestinationId,
    [param: Required] string Address,
    [param: Required] string ProtocolCode,
    bool OverwriteAtDestination,
    bool CompressAtDestination,
    bool RenameAtDestination,
    bool DestinationIsArchive,
    bool IsActive,
    Guid FtpServerId,
    string? RenamePattern,
    DestinationContact? Contact,
    DestinationEncryption? Encryption,
    Guid? ExternalFileDefinitionId,
    string? DestinationType,
    string? ContractName);

public sealed record DestinationUpsert(
    Guid? DestinationId,
    [param: Required] string Address,
    [param: Required] string ProtocolCode,
    bool OverwriteAtDestination,
    bool CompressAtDestination,
    bool RenameAtDestination,
    bool DestinationIsArchive,
    bool IsActive,
    Guid FtpServerId,
    string? RenamePattern,
    DestinationContact? Contact,
    DestinationEncryption? Encryption,
    Guid? ExternalFileDefinitionId,
    string? DestinationType,
    string? ContractName);

public sealed record TransferConfiguration(
    Guid TransferConfigurationId,
    int ConfigNumber,
    string Name,
    string? Description,
    string ProtocolCode,
    string Address,
    string FileMask,
    Guid? FtpServerId,
    string? FtpServerName,
    bool SendNotifications,
    OwnerInfo Owner,
    ScheduleInfo Schedule,
    string? FileDirection,
    string? TradingPartner,
    bool IsActive,
    bool IsSuspended,
    DateTimeOffset? SuspendedChangedAt,
    Destination[] Destinations);

public sealed record TransferConfigurationUpsert(
    Guid? TransferConfigurationId,
    int ConfigNumber,
    [param: Required] string Name,
    string? Description,
    [param: Required] string ProtocolCode,
    [param: Required] string Address,
    [param: Required] string FileMask,
    Guid? FtpServerId,
    bool SendNotifications,
    OwnerInfo Owner,
    ScheduleInfo Schedule,
    string? FileDirection,
    string? TradingPartner,
    bool IsActive,
    bool IsSuspended,
    DestinationUpsert[] Destinations);

public sealed record TransferConfigurationIdResponse(Guid TransferConfigurationId);
public sealed record TransferConfigurationStateResponse(Guid TransferConfigurationId, bool IsActive);

namespace FxferService.Api.Commands.Models;

public sealed record OwnerInfo(string Name, Guid ModuleId, string Phone, string Email);

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
    string StartTime24hhmm,
    string EndTime24hhmm);

public sealed record TransferConfigurationSummary(
    Guid TransferConfigurationId,
    int ConfigNumber,
    string Name,
    string ProtocolCode,
    string Address,
    string FileMask,
    bool IsActive,
    bool IsSuspended);

public sealed record Destination(
    Guid DestinationId,
    string Address,
    string ProtocolCode,
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
    string Address,
    string ProtocolCode,
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

public sealed record DestinationContact(string? Name, string? Phone, string? Email);

public sealed record DestinationEncryption(string? CdeEncryption, string? PublicKey, string? PrivateKey, string? PrivateKeyPassword);

public sealed record TransferConfiguration(
    Guid TransferConfigurationId,
    int ConfigNumber,
    string Name,
    string? Description,
    string ProtocolCode,
    string Address,
    string FileMask,
    Guid? FtpServerId,
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
    int? ConfigNumber,
    string Name,
    string? Description,
    string ProtocolCode,
    string Address,
    string FileMask,
    Guid? FtpServerId,
    bool SendNotifications,
    OwnerInfo Owner,
    ScheduleInfo Schedule,
    string? FileDirection,
    string? TradingPartner,
    bool IsActive,
    bool IsSuspended,
    DateTimeOffset? SuspendedChangedAt,
    DestinationUpsert[] Destinations);

public sealed record TransferConfigurationIdResponse(Guid TransferConfigurationId);

public sealed record TransferConfigurationStateResponse(Guid TransferConfigurationId, bool IsActive);

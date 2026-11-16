using System.ComponentModel.DataAnnotations;

namespace FxferService.Infrastructure.Persistence.Entities;

public sealed class FtpServerEntity
{
    public Guid FtpServerId { get; set; }
    public string Name { get; set; } = "";
    public string IpAddress { get; set; } = "";
    public string TransferMode { get; set; } = "";
    public string User { get; set; } = "";
    public string Password { get; set; } = "";
    public string Description { get; set; } = "";
    public short Port { get; set; }
    public string? ProxyAddress { get; set; }
    public short? ProxyPort { get; set; }
    public bool SshAuthentication { get; set; }
    public bool IsValid { get; set; }
    [StringLength(200)]
    public string OperatorId { get; set; } = "";
    public DateTimeOffset LastModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public sealed class FtsSourceLocationEntity
{
    public Guid FtsSourceLocationId { get; set; }
    public int ConfigId { get; set; }
    public string ConfigName { get; set; } = "";
    public string SourceProtocolCode { get; set; } = "";
    public string SourceAddress { get; set; } = "";
    public string SourceFileMask { get; set; } = "";
    public Guid? FtpServerId { get; set; }
    public bool SendNotifications { get; set; }
    public string OwnerName { get; set; } = "";
    public Guid ModuleId { get; set; }
    public string OwnerPhone { get; set; } = "";
    public string OwnerEmail { get; set; } = "";
    public string? ConfigDescription { get; set; }
    public bool RunOnMonday { get; set; }
    public bool RunOnTuesday { get; set; }
    public bool RunOnWednesday { get; set; }
    public bool RunOnThursday { get; set; }
    public bool RunOnFriday { get; set; }
    public bool RunOnSaturday { get; set; }
    public bool RunOnSunday { get; set; }
    public bool RunOnFirstDayOfMonth { get; set; }
    public bool RunOnLastDayOfMonth { get; set; }
    public short RunOnThisDayOfMonth { get; set; }
    public string StartTime24hhmm { get; set; } = "";
    public string EndTime24hhmm { get; set; } = "";
    public string? FileDirection { get; set; }
    public string? TradingPartner { get; set; }
    public bool IsValid { get; set; }
    public bool IsSuspended { get; set; }
    public DateTimeOffset? SuspendedChangedAt { get; set; }
    [StringLength(200)]
    public string OperatorId { get; set; } = "";
    public DateTimeOffset LastModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public List<FtsDestinationLocations> Destinations { get; set; } = new();
}

public sealed class FtsDestinationEntity
{
    public Guid DestinationId { get; set; }
    public Guid FtsSourceLocationId { get; set; }
    public string DestinationAddress { get; set; } = "";
    public string DestProtocolCode { get; set; } = "";
    public bool OverwriteAtDest { get; set; }
    public bool CompressAtDest { get; set; }
    public bool RenameAtDest { get; set; }
    public bool DestIsArchive { get; set; }
    public bool IsValid { get; set; }
    public Guid FtpServerId { get; set; }
    public string? RenamePattern { get; set; }
    public string? ContactName { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    [StringLength(200)]
    public string OperatorId { get; set; } = "";
    public DateTimeOffset CreationAt { get; set; }
    public DateTimeOffset LastModifiedAt { get; set; }
    public string? CdeEncryption { get; set; }
    public string? EncryptionPublicKey { get; set; }
    public string? EncryptionPrivateKey { get; set; }
    public string? PrivateKeyPassword { get; set; }
    public Guid? ExternalFileDefnId { get; set; }
    public string? DestinationType { get; set; }
    public string? ContractName { get; set; }
}

public sealed class FtsInboundTargetEntity
{
    public Guid FtsInboundTargetId { get; set; }
    public Guid ExternalFileDefnId { get; set; }
    public string ImportJobName { get; set; } = "";
    public string ContractName { get; set; } = "";
    [StringLength(200)]
    public string OperatorId { get; set; } = "";
    public bool IsValid { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset LastModifiedAt { get; set; }
}

public sealed class FtsIntegrationFileEntity
{
    public Guid FtsIntegrationFileId { get; set; }
    public Guid? GroupId { get; set; }
    public int? ConfigId { get; set; }
    public string FileName { get; set; } = "";
    public Guid OutboundMetadataDefnId { get; set; }
    public bool IsProcessed { get; set; }
    [StringLength(200)]
    public string OperatorId { get; set; } = "";
    public bool IsValid { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset LastModifiedAt { get; set; }
}

public sealed class FtsSuspensionEntity
{
    public Guid SuspensionId { get; set; }
    public Guid FtsSourceLocationId { get; set; }
    public string ErrorText { get; set; } = "";
    [StringLength(4000)]
    public string StackTrace { get; set; } = "";
    [StringLength(200)]
    public string OperatorId { get; set; } = "";
    public DateTimeOffset LastModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public sealed class FtsFileManagementEntity
{
    public Guid FileId { get; set; }
    public Guid FtsSourceLocationId { get; set; }
    public string FileName { get; set; } = "";
    public string FileStatus { get; set; } = "";
    public DateTimeOffset FileStatusAt { get; set; }
    public int? SakFileTrack { get; set; }
    public decimal FileSize { get; set; }
    public string? ChecksumCode { get; set; }
    [StringLength(200)]
    public string OperatorId { get; set; } = "";
    public DateTimeOffset LastModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public sealed class FtsFileDeliveryEntity
{
    public Guid DeliveryId { get; set; }
    public Guid FileId { get; set; }
    public Guid DestinationId { get; set; }
    public DateTimeOffset DeliveryAt { get; set; }
    public string DestinationFileName { get; set; } = "";
    [StringLength(200)]
    public string OperatorId { get; set; } = "";
    public DateTimeOffset LastModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public sealed class FtsFileEventEntity
{
    public Guid EventId { get; set; }
    public Guid FileId { get; set; }
    public Guid DestinationId { get; set; }
    public string EventCode { get; set; } = "";
    public string TrackDescription { get; set; } = "";
    public DateTimeOffset CreatedAt { get; set; }
    [StringLength(200)]
    public string OperatorId { get; set; } = "";
    public DateTimeOffset LastModifiedAt { get; set; }
}

public sealed class FtsFileExceptionEntity
{
    public Guid ExceptionId { get; set; }
    public Guid FileId { get; set; }
    public Guid DestinationId { get; set; }
    public Guid EventId { get; set; }
    [StringLength(2000)]
    public string ExceptionType { get; set; } = "";
    [StringLength(2000)]
    public string MethodName { get; set; } = "";
    [StringLength(2000)]
    public string Message { get; set; } = "";
    [StringLength(4000)]
    public string StackTrace { get; set; } = "";
    public DateTimeOffset CreatedAt { get; set; }
    [StringLength(200)]
    public string OperatorId { get; set; } = "";
    public DateTimeOffset LastModifiedAt { get; set; }
}

public sealed class FtsNotificationEntity
{
    public Guid NotificationId { get; set; }
    public Guid FtsSourceLocationId { get; set; }
    public string EmailAddress { get; set; } = "";
    public string ContactName { get; set; } = "";
    public string ContactPhone { get; set; } = "";
    public bool NotifyOnError { get; set; }
    public bool NotifyOnSuccess { get; set; }
    [StringLength(200)]
    public string OperatorId { get; set; } = "";
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset LastModifiedAt { get; set; }
}

public sealed class OutboxMessageEntity
{
    public Guid OutboxMessageId { get; set; }
    public string TenantId { get; set; } = "";
    public string EventName { get; set; } = "";
    public string EventVersion { get; set; } = "";
    public DateTimeOffset OccurredAt { get; set; }
    public string PayloadJson { get; set; } = "{}";
    public DateTimeOffset? DispatchedAt { get; set; }
}




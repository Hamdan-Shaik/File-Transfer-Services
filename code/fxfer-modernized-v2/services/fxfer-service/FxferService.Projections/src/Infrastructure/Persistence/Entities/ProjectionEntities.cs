namespace FxferService.Projections.Infrastructure.Persistence.Entities;

public sealed class InboxEntry
{
    public required Guid EventId { get; set; }
    public required DateTimeOffset ProcessedAt { get; set; }
}

public sealed class FtpServerProjection
{
    public required Guid FtpServerId { get; set; }
    public required string FtpServerName { get; set; }
    public required string FtpServerIp { get; set; }
    public required string FtpTransferMode { get; set; }
    public required string FtpUser { get; set; }
    public required string FtpPassword { get; set; }
    public required string FtpDescription { get; set; }
    public required short FtpPort { get; set; }
    public string? FtpProxyAddress { get; set; }
    public short? FtpProxyPort { get; set; }
    public required bool UseSshAuth { get; set; }
    public required bool IsValid { get; set; }
    public required string OperatorId { get; set; }
    public required DateTimeOffset LastModifiedTs { get; set; }
    public required DateTimeOffset CreatedTs { get; set; }
}

public sealed class FtsSourceLocationProjection
{
    public required Guid FtsSourceLocationId { get; set; }
    public required int ConfigId { get; set; }
    public required string ConfigName { get; set; }
    public required string SourceProtocolCd { get; set; }
    public required string SourceAddress { get; set; }
    public required string SourceFileMask { get; set; }
    public Guid? FtpServerId { get; set; }
    public required bool SendNotificationsInd { get; set; }
    public required string OwnerName { get; set; }
    public required Guid ModuleId { get; set; }
    public required string OwnerPhone { get; set; }
    public required string OwnerEmail { get; set; }
    public string? ConfigDescription { get; set; }
    public required bool RunOnMondayInd { get; set; }
    public required bool RunOnTuesdayInd { get; set; }
    public required bool RunOnWedInd { get; set; }
    public required bool RunOnThursInd { get; set; }
    public required bool RunOnFriInd { get; set; }
    public required bool RunOnSatInd { get; set; }
    public required bool RunOnSunInd { get; set; }
    public required bool RunOnFrstDayOfMonthInd { get; set; }
    public required bool RunOnLastDayOfMonthInd { get; set; }
    public required short RunOnThisDayOfMonth { get; set; }
    public required string StartTime24hhmm { get; set; }
    public required string EndTime24hhmm { get; set; }
    public string? FileDirection { get; set; }
    public string? TradingPartner { get; set; }
    public required bool IsValid { get; set; }
    public required bool IsSuspended { get; set; }
    public DateTimeOffset? IsSuspendedChangedTs { get; set; }
    public required string OperatorId { get; set; }
    public required DateTimeOffset LastModifiedTs { get; set; }
    public required DateTimeOffset CreatedTs { get; set; }

    public List<FtsDestinationProjection> Destinations { get; set; } = new();
}

public sealed class FtsDestinationProjection
{
    public required Guid DestinationId { get; set; }
    public required Guid FtsSourceLocationId { get; set; }
    public required string DestinationAddress { get; set; }
    public required string DestProtocolCd { get; set; }
    public required bool OverwriteAtDestInd { get; set; }
    public required bool CompressAtDestInd { get; set; }
    public required bool RenameAtDestInd { get; set; }
    public required bool DestIsArchiveInd { get; set; }
    public required bool IsValid { get; set; }
    public required Guid FtpServerId { get; set; }
    public string? RenamePattern { get; set; }
    public string? ContactName { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public required string OperatorId { get; set; }
    public required DateTimeOffset CreationTs { get; set; }
    public required DateTimeOffset LastModifiedTs { get; set; }
    public string? CdeEncryption { get; set; }
    public string? EncryptionPublicKey { get; set; }
    public string? EncryptionPrivateKey { get; set; }
    public string? PrivateKeyPassword { get; set; }
    public Guid? ExternalFileDefnId { get; set; }
    public string? DestinationType { get; set; }
    public string? ContractName { get; set; }
}

public sealed class FtsInboundTargetProjection
{
    public required Guid FtsInboundTargetId { get; set; }
    public required Guid ExternalFileDefnId { get; set; }
    public required string ImportJobName { get; set; }
    public required string ContractName { get; set; }
    public required string OperatorId { get; set; }
    public required bool IsValid { get; set; }
    public required DateTimeOffset CreatedTs { get; set; }
    public required DateTimeOffset LastModifiedTs { get; set; }
}

public sealed class FtsIntegrationFileProjection
{
    public required Guid FtsIntegrationFileId { get; set; }
    public Guid? GroupId { get; set; }
    public int? ConfigId { get; set; }
    public required string FileName { get; set; }
    public required Guid EtfOutboundMetadataDefnId { get; set; }
    public required bool IsProcessed { get; set; }
    public required string OperatorId { get; set; }
    public required bool IsValid { get; set; }
    public required DateTimeOffset CreatedTs { get; set; }
    public required DateTimeOffset LastModifiedTs { get; set; }
}

public sealed class FtsSuspensionProjection
{
    public required Guid SuspensionId { get; set; }
    public required Guid FtsSourceLocationId { get; set; }
    public required string ErrorText { get; set; }
    public required string StackTrace { get; set; }
    public required string OperatorId { get; set; }
    public required DateTimeOffset LastModifiedTs { get; set; }
    public required DateTimeOffset CreatedTs { get; set; }
}

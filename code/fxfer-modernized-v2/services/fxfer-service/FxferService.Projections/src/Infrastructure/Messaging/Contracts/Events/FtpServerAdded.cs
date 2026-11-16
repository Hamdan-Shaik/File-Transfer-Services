namespace FxferService.Projections.Infrastructure.Messaging.Contracts.Events;

public class FtpServerAdded
{
    public Guid FtpServerId { get; init; }
    public required string FtpServerName { get; init; }
    public required string FtpServerIp { get; init; }
    public required string FtpTransferMode { get; init; }
    public required string FtpUser { get; init; }
    public required string FtpPassword { get; init; }
    public required string FtpDescription { get; init; }
    public short FtpPort { get; init; }
    public string? FtpProxyAddress { get; init; }
    public short? FtpProxyPort { get; init; }
    public bool UseSSHAuth { get; init; }
    public bool IsValid { get; init; }
    public required string OperatorId { get; init; }
}


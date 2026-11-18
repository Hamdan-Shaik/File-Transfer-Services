namespace Fxfer.DomainApi.Contracts.Events;

public record FtpServerAdded
{
    public Guid FtpServerId { get; init; }
    public string? FtpServerName { get; init; }
    public string? FtpDescription { get; init; }
    public string? FtpServerIp { get; init; }
    public string? FtpTransferMode { get; init; }
    public string? FtpUser { get; init; }
    public string? FtpPassword { get; init; }
    public short FtpPort { get; init; }
    public string? FtpProxyAddress { get; init; }
    public short? FtpProxyPort { get; init; }
    public bool UseSSHAuth { get; init; }
    public bool IsValid { get; init; }
    public string? OperatorId { get; init; }
}

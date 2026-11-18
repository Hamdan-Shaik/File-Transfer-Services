namespace Fxfer.Bff.Contracts.FtpServers;

public sealed record FtpServer
{
    public Guid FtpServerId { get; init; }
    public required string FtpServerName { get; init; }
    public required string FtpDescription { get; init; }
    public required string FtpServerIp { get; init; }
    public required string FtpTransferMode { get; init; }
    public required string FtpUser { get; init; }
    public int FtpPort { get; init; }
    public string? FtpProxyAddress { get; init; }
    public int? FtpProxyPort { get; init; }
    public bool UseSshAuth { get; init; }
    public bool IsValid { get; init; }
    public required string OperatorId { get; init; }
    public DateTimeOffset CreationDate { get; init; }
    public DateTimeOffset LastModifiedDate { get; init; }
}


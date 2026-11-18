namespace Fxfer.Bff.Contracts.FtpServers;

public sealed record FtpServerPatchRequest
{
    public string? FtpServerName { get; init; }
    public string? FtpDescription { get; init; }
    public string? FtpServerIp { get; init; }
    public string? FtpTransferMode { get; init; }
    public string? FtpUser { get; init; }
    public string? FtpPassword { get; init; }
    public string? FtpPasswordConfirmation { get; init; }
    public int? FtpPort { get; init; }
    public string? FtpProxyAddress { get; init; }
    public int? FtpProxyPort { get; init; }
    public bool? UseSshAuth { get; init; }
    public bool? IsValid { get; init; }
    public required string OperatorId { get; init; }
}


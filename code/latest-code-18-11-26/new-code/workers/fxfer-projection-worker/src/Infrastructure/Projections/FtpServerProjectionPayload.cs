namespace Fxfer.ProjectionWorker.Infrastructure.Projections;

public sealed record FtpServerProjectionPayload
{
    public required Guid FtpServerId { get; init; }
    public required string FtpServerName { get; init; }
    public required string FtpDescription { get; init; }
    public required string FtpServerHost { get; init; }
    public required string FtpTransferMode { get; init; }
    public required string FtpUser { get; init; }
    public required string FtpPasswordSecretRef { get; init; }
    public required short FtpPort { get; init; }
    public string? FtpProxyAddress { get; init; }
    public short? FtpProxyPort { get; init; }
    public required bool UseSshAuth { get; init; }
    public required bool IsValid { get; init; }
    public required string OperatorId { get; init; }
}

namespace Fxfer.Bff.Contracts.FtpServers;

public sealed record FtpServerOption
{
    public Guid FtpServerId { get; init; }
    public required string FtpServerName { get; init; }
}


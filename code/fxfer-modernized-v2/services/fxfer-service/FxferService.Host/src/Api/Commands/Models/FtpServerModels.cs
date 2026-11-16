namespace FxferService.Api.Commands.Models;

public sealed record FtpServer(
    Guid FtpServerId,
    string Name,
    string IpAddress,
    string TransferMode,
    string User,
    string? Password,
    string Description,
    int Port,
    string? ProxyAddress,
    int? ProxyPort,
    bool SshAuthentication,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset LastModifiedAt);

public sealed record FtpServerUpsert(
    string Name,
    string IpAddress,
    string TransferMode,
    string User,
    string? Password,
    string Description,
    int Port,
    string? ProxyAddress,
    int? ProxyPort,
    bool SshAuthentication,
    bool IsActive);

public sealed record FtpServerIdResponse(Guid FtpServerId);

public sealed record FtpServerStateResponse(Guid FtpServerId, bool IsActive);

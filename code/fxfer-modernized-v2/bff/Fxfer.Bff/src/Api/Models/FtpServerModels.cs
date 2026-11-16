using System.ComponentModel.DataAnnotations;

namespace Fxfer.Bff.Api.Models;

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
    [param: Required] string Name,
    [param: Required] string IpAddress,
    [param: Required] string TransferMode,
    [param: Required] string User,
    string? Password,
    [param: Required] string Description,
    int Port,
    string? ProxyAddress,
    int? ProxyPort,
    bool SshAuthentication,
    bool IsActive);

public sealed record FtpServerIdResponse(Guid FtpServerId);
public sealed record FtpServerStateResponse(Guid FtpServerId, bool IsActive);

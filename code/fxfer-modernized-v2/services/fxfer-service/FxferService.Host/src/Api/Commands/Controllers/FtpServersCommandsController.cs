using System.Security.Claims;

using FxferService.Api.Commands.Models;
using FxferService.Application.CommandHandlers;
using FxferService.Domain.Commands;
using FxferService.Domain.Contracts.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FxferService.Api.Commands.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/ftp-servers")]
public sealed class FtpServersCommandsController : ControllerBase
{
    private readonly FtpServerCommandHandler _ftpServerCommandHandler;

    public FtpServersCommandsController(FtpServerCommandHandler ftpServerCommandHandler)
    {
        _ftpServerCommandHandler = ftpServerCommandHandler;
    }

    [HttpPost]
    [ProducesResponseType(typeof(FtpServerIdResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<FtpServerIdResponse>> CreateFtpServer(
        [FromBody] FtpServerUpsert body,
        CancellationToken ct)
    {
        var operatorId = GetOperatorIdOrThrow();

        var command = new AddFtpServerCommand
        {
            FtpServerName = body.Name,
            FtpServerIp = body.IpAddress,
            FtpTransferMode = body.TransferMode,
            FtpUser = body.User,
            FtpPassword = body.Password,
            FtpDescription = body.Description,
            FtpPort = checked((short)body.Port),
            FtpProxyAddress = body.ProxyAddress,
            FtpProxyPort = body.ProxyPort is null ? null : checked((short)body.ProxyPort.Value),
            UseSSHAuth = body.SshAuthentication,
            IsValid = body.IsActive,
            Requestor = new FxferService.Application.LegacyContracts.RequestorModel { IdentifierId = operatorId },
        };

        // F01-015: route the add command and cast the routed event result.
        Event routed = await _ftpServerCommandHandler.HandleAsync(command, ct);
        var added = routed as FtpServerAdded;

        var id = added?.FtpServerId ?? command.FtpServerId;
        return Created($"/api/v1/ftp-servers/{id}", new FtpServerIdResponse(id));
    }

    [HttpPut("{ftpServerId:guid}")]
    [ProducesResponseType(typeof(FtpServerStateResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<FtpServerStateResponse>> UpdateFtpServer(
        [FromRoute] Guid ftpServerId,
        [FromBody] FtpServerUpsert body,
        CancellationToken ct)
    {
        var operatorId = GetOperatorIdOrThrow();

        var command = new UpdateFtpServerCommand
        {
            FtpServerId = ftpServerId,
            FtpServerName = body.Name,
            FtpServerIp = body.IpAddress,
            FtpTransferMode = body.TransferMode,
            FtpUser = body.User,
            FtpPassword = body.Password,
            FtpDescription = body.Description,
            FtpPort = checked((short)body.Port),
            FtpProxyAddress = body.ProxyAddress,
            FtpProxyPort = body.ProxyPort is null ? null : checked((short)body.ProxyPort.Value),
            UseSSHAuth = body.SshAuthentication,
            IsValid = body.IsActive,
            Requestor = new FxferService.Application.LegacyContracts.RequestorModel { IdentifierId = operatorId },
        };

        // F01-017: route the update command and cast the routed event result.
        Event routed = await _ftpServerCommandHandler.HandleAsync(command, ct);
        _ = routed as FtpServerUpdated;

        return Ok(new FtpServerStateResponse(ftpServerId, body.IsActive));
    }

    [HttpDelete("{ftpServerId:guid}")]
    [ProducesResponseType(typeof(FtpServerStateResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<FtpServerStateResponse>> InvalidateFtpServer(
        [FromRoute] Guid ftpServerId,
        CancellationToken ct)
    {
        var operatorId = GetOperatorIdOrThrow();

        var command = new InvalidateFtpServerCommand
        {
            FtpServerId = ftpServerId,
            Requestor = new FxferService.Application.LegacyContracts.RequestorModel { IdentifierId = operatorId },
        };

        // F01-019: route the invalidate command and cast the routed event result.
        Event routed = await _ftpServerCommandHandler.HandleAsync(command, ct);
        _ = routed as FtpServerInvalidated;

        return Ok(new FtpServerStateResponse(ftpServerId, IsActive: false));
    }

    private string GetOperatorIdOrThrow()
    {
        var operatorId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Identity?.Name;
        if (string.IsNullOrWhiteSpace(operatorId))
        {
            throw new InvalidOperationException("Missing authenticated operator id");
        }

        return operatorId;
    }
}

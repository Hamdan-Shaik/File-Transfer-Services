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
[Route("api/v1/integration-files")]
public sealed class IntegrationFilesCommandsController : ControllerBase
{
    private readonly FtsIntegrationFileCommandHandler _integrationFileCommandHandler;

    public IntegrationFilesCommandsController(FtsIntegrationFileCommandHandler integrationFileCommandHandler)
    {
        _integrationFileCommandHandler = integrationFileCommandHandler;
    }

    [HttpPut("{integrationFileId:guid}")]
    [ProducesResponseType(typeof(IntegrationFileIdResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IntegrationFileIdResponse>> UpdateIntegrationFile(
        [FromRoute] Guid integrationFileId,
        [FromBody] IntegrationFileUpdate body,
        CancellationToken ct)
    {
        var operatorId = GetOperatorIdOrThrow();

        var command = new UpdateIntegrationFileCommand
        {
            FtsIntegrationFileId = integrationFileId,
            ConfigId = body.ConfigNumber,
            IsValid = body.IsActive,
            OperatorId = operatorId,
            Requestor = new FxferService.Application.LegacyContracts.RequestorModel { IdentifierId = operatorId },
        };

        // F02-044: route the update command and cast the routed event result.
        Event routed = await _integrationFileCommandHandler.HandleAsync(command, ct);
        _ = routed as IntegrationFileUpdated;

        return Ok(new IntegrationFileIdResponse(integrationFileId));
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


using System.Net;

using Fxfer.Bff.Api.Models;
using Fxfer.Bff.Infrastructure.Downstream;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Fxfer.Bff.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/inbound-targets")]
public sealed class InboundTargetsController : ControllerBase
{
    private readonly FxferServiceClient _fxferServiceClient;

    public InboundTargetsController(FxferServiceClient fxferServiceClient)
    {
        _fxferServiceClient = fxferServiceClient;
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(InboundTarget[]), StatusCodes.Status200OK)]
    public async Task<ActionResult<InboundTarget[]>> Search(
        [FromQuery] string contractName,
        [FromQuery] Guid externalFileDefinitionId,
        CancellationToken ct = default)
    {
        var query = new Dictionary<string, string?>
        {
            ["contractName"] = contractName,
            ["externalFileDefinitionId"] = externalFileDefinitionId.ToString(),
        };

        var qs = QueryHelpers.AddQueryString(string.Empty, query);
        var result = await _fxferServiceClient.SearchInboundTargetsAsync(qs, ct);

        if (result.StatusCode is >= HttpStatusCode.BadRequest)
        {
            return new ContentResult
            {
                StatusCode = (int)result.StatusCode,
                ContentType = result.ContentType ?? "application/problem+json",
                Content = result.RawBody ?? string.Empty,
            };
        }

        return Ok(result.Body ?? Array.Empty<InboundTarget>());
    }
}


using System.Net;

using Fxfer.Bff.Api.Models;
using Fxfer.Bff.Infrastructure.Downstream;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Fxfer.Bff.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/integration-files")]
public sealed class IntegrationFilesController : ControllerBase
{
    private readonly FxferServiceClient _fxferServiceClient;

    public IntegrationFilesController(FxferServiceClient fxferServiceClient)
    {
        _fxferServiceClient = fxferServiceClient;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<IntegrationFile>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<IntegrationFile>>> Get(
        [FromQuery] Guid? integrationFileId,
        [FromQuery] string? fileNameContains,
        [FromQuery, System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)] int page = 1,
        [FromQuery, System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)] int pageSize = 50,
        CancellationToken ct = default)
    {
        var query = new Dictionary<string, string?>
        {
            ["integrationFileId"] = integrationFileId?.ToString(),
            ["fileNameContains"] = fileNameContains,
            ["page"] = page.ToString(),
            ["pageSize"] = pageSize.ToString(),
        };

        var qs = QueryHelpers.AddQueryString(string.Empty, query);
        var result = await _fxferServiceClient.SearchIntegrationFilesAsync(qs, ct);

        if (result.StatusCode is >= HttpStatusCode.BadRequest)
        {
            return new ContentResult
            {
                StatusCode = (int)result.StatusCode,
                ContentType = result.ContentType ?? "application/problem+json",
                Content = result.RawBody ?? string.Empty,
            };
        }

        return Ok(result.Body ?? new PagedResult<IntegrationFile>(Array.Empty<IntegrationFile>(), 0, page, pageSize));
    }

    [HttpPut("{integrationFileId:guid}")]
    [ProducesResponseType(typeof(IntegrationFileIdResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IntegrationFileIdResponse>> Put(
        [FromRoute] Guid integrationFileId,
        [FromBody] IntegrationFileUpdate body,
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey = null,
        CancellationToken ct = default)
    {
        var result = await _fxferServiceClient.UpdateIntegrationFileAsync(integrationFileId, body, idempotencyKey, ct);

        if (result.StatusCode is >= HttpStatusCode.BadRequest)
        {
            return new ContentResult
            {
                StatusCode = (int)result.StatusCode,
                ContentType = result.ContentType ?? "application/problem+json",
                Content = result.RawBody ?? string.Empty,
            };
        }

        return Ok(result.Body ?? new IntegrationFileIdResponse(integrationFileId));
    }
}


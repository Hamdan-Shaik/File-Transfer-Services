using System.Net;

using Fxfer.Bff.Api.Models;
using Fxfer.Bff.Infrastructure.Downstream;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Fxfer.Bff.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/transfer-configurations")]
public sealed class TransferConfigurationsController : ControllerBase
{
    private readonly FxferServiceClient _fxferServiceClient;

    public TransferConfigurationsController(FxferServiceClient fxferServiceClient)
    {
        _fxferServiceClient = fxferServiceClient;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<TransferConfigurationSummary>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<TransferConfigurationSummary>>> Get(
        [FromQuery] bool? isActive,
        [FromQuery] int? configNumber,
        [FromQuery] string? nameContains,
        [FromQuery] string? ownerNameContains,
        [FromQuery] string? protocolCode,
        [FromQuery, System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)] int page = 1,
        [FromQuery, System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)] int pageSize = 50,
        [FromQuery] string sortBy = "configNumber",
        [FromQuery] string? sortDir = null,
        CancellationToken ct = default)
    {
        var query = new Dictionary<string, string?>
        {
            ["isActive"] = isActive?.ToString(),
            ["configNumber"] = configNumber?.ToString(),
            ["nameContains"] = nameContains,
            ["ownerNameContains"] = ownerNameContains,
            ["protocolCode"] = protocolCode,
            ["page"] = page.ToString(),
            ["pageSize"] = pageSize.ToString(),
            ["sortBy"] = sortBy,
            ["sortDir"] = sortDir,
        };

        var qs = QueryHelpers.AddQueryString(string.Empty, query);
        var result = await _fxferServiceClient.SearchTransferConfigurationsAsync(qs, ct);

        if (result.StatusCode is >= HttpStatusCode.BadRequest)
        {
            return new ContentResult
            {
                StatusCode = (int)result.StatusCode,
                ContentType = result.ContentType ?? "application/problem+json",
                Content = result.RawBody ?? string.Empty,
            };
        }

        return Ok(result.Body ?? new PagedResult<TransferConfigurationSummary>(Array.Empty<TransferConfigurationSummary>(), 0, page, pageSize));
    }

    [HttpGet("selection")]
    [ProducesResponseType(typeof(SelectionItem[]), StatusCodes.Status200OK)]
    public async Task<ActionResult<SelectionItem[]>> GetSelection(
        [FromQuery] bool activeOnly = true,
        CancellationToken ct = default)
    {
        var query = new Dictionary<string, string?>
        {
            ["isActive"] = activeOnly ? "true" : null,
            ["page"] = "1",
            ["pageSize"] = "0",
            ["sortBy"] = "configNumber",
            ["sortDir"] = "asc",
        };

        var qs = QueryHelpers.AddQueryString(string.Empty, query);
        var result = await _fxferServiceClient.SearchTransferConfigurationsAsync(qs, ct);

        if (result.StatusCode is >= HttpStatusCode.BadRequest)
        {
            return new ContentResult
            {
                StatusCode = (int)result.StatusCode,
                ContentType = result.ContentType ?? "application/problem+json",
                Content = result.RawBody ?? string.Empty,
            };
        }

        var selection = (result.Body?.Items ?? Array.Empty<TransferConfigurationSummary>())
            .Select(x => new SelectionItem(x.TransferConfigurationId, $"{x.Name} - {x.Address}"))
            .ToArray();

        return Ok(selection);
    }

    [HttpGet("{transferConfigurationId:guid}")]
    [ProducesResponseType(typeof(TransferConfiguration), StatusCodes.Status200OK)]
    public async Task<ActionResult<TransferConfiguration>> GetById(
        [FromRoute] Guid transferConfigurationId,
        CancellationToken ct = default)
    {
        var result = await _fxferServiceClient.GetTransferConfigurationAsync(transferConfigurationId, ct);

        if (result.StatusCode is >= HttpStatusCode.BadRequest)
        {
            return new ContentResult
            {
                StatusCode = (int)result.StatusCode,
                ContentType = result.ContentType ?? "application/problem+json",
                Content = result.RawBody ?? string.Empty,
            };
        }

        return Ok(result.Body);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TransferConfigurationIdResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<TransferConfigurationIdResponse>> Post(
        [FromBody] TransferConfigurationUpsert body,
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey = null,
        CancellationToken ct = default)
    {
        var result = await _fxferServiceClient.UpsertTransferConfigurationAsync(body, idempotencyKey, ct);

        if (result.StatusCode is >= HttpStatusCode.BadRequest)
        {
            return new ContentResult
            {
                StatusCode = (int)result.StatusCode,
                ContentType = result.ContentType ?? "application/problem+json",
                Content = result.RawBody ?? string.Empty,
            };
        }

        var id = result.Body?.TransferConfigurationId ?? body.TransferConfigurationId ?? Guid.NewGuid();
        return Ok(new TransferConfigurationIdResponse(id));
    }

    [HttpDelete("{transferConfigurationId:guid}")]
    [ProducesResponseType(typeof(TransferConfigurationStateResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<TransferConfigurationStateResponse>> Delete(
        [FromRoute] Guid transferConfigurationId,
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey = null,
        CancellationToken ct = default)
    {
        var result = await _fxferServiceClient.InvalidateTransferConfigurationAsync(transferConfigurationId, idempotencyKey, ct);

        if (result.StatusCode is >= HttpStatusCode.BadRequest)
        {
            return new ContentResult
            {
                StatusCode = (int)result.StatusCode,
                ContentType = result.ContentType ?? "application/problem+json",
                Content = result.RawBody ?? string.Empty,
            };
        }

        return Ok(result.Body ?? new TransferConfigurationStateResponse(transferConfigurationId, IsActive: false));
    }
}


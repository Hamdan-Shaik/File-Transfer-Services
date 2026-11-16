using Fxfer.Bff.Api.Models;
using Fxfer.Bff.Infrastructure.Downstream;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Fxfer.Bff.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/ftp-servers")]
public sealed class FtpServersController : ControllerBase
{
    private readonly FxferServiceClient _fxferServiceClient;

    public FtpServersController(FxferServiceClient fxferServiceClient)
    {
        _fxferServiceClient = fxferServiceClient;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<FtpServer>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<FtpServer>>> Get(
        [FromQuery] bool? isActive,
        [FromQuery] string? name,
        [FromQuery] string? description,
        [FromQuery] string? user,
        [FromQuery, System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)] int page = 1,
        [FromQuery, System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)] int pageSize = 50,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortDir = null,
        [FromQuery] bool includePassword = false,
        CancellationToken ct = default)
    {
        var query = new Dictionary<string, string?>
        {
            ["isActive"] = isActive?.ToString(),
            ["name"] = name,
            ["description"] = description,
            ["user"] = user,
            ["page"] = page.ToString(),
            ["pageSize"] = pageSize.ToString(),
            ["sortBy"] = sortBy,
            ["sortDir"] = sortDir,
            ["includePassword"] = includePassword ? "true" : null,
        };

        var qs = QueryHelpers.AddQueryString(string.Empty, query);
        var result = await _fxferServiceClient.SearchFtpServersAsync(qs, ct);

        if (result.StatusCode is >= System.Net.HttpStatusCode.BadRequest)
        {
            return new ContentResult
            {
                StatusCode = (int)result.StatusCode,
                ContentType = result.ContentType ?? "application/problem+json",
                Content = result.RawBody ?? string.Empty,
            };
        }

        return Ok(result.Body ?? new PagedResult<FtpServer>(Array.Empty<FtpServer>(), 0, page, pageSize));
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
            ["sortBy"] = "name",
            ["sortDir"] = "asc",
        };

        var qs = QueryHelpers.AddQueryString(string.Empty, query);
        var result = await _fxferServiceClient.SearchFtpServersAsync(qs, ct);

        if (result.StatusCode is >= System.Net.HttpStatusCode.BadRequest)
        {
            return new ContentResult
            {
                StatusCode = (int)result.StatusCode,
                ContentType = result.ContentType ?? "application/problem+json",
                Content = result.RawBody ?? string.Empty,
            };
        }

        var selection = (result.Body?.Items ?? Array.Empty<FtpServer>())
            .Select(s => new SelectionItem(s.FtpServerId, s.Name))
            .ToArray();

        return Ok(selection);
    }

    [HttpPost]
    [ProducesResponseType(typeof(FtpServerIdResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<FtpServerIdResponse>> Post(
        [FromBody] FtpServerUpsert body,
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey = null,
        CancellationToken ct = default)
    {
        var result = await _fxferServiceClient.CreateFtpServerAsync(body, idempotencyKey, ct);

        if (result.StatusCode is >= System.Net.HttpStatusCode.BadRequest)
        {
            return new ContentResult
            {
                StatusCode = (int)result.StatusCode,
                ContentType = result.ContentType ?? "application/problem+json",
                Content = result.RawBody ?? string.Empty,
            };
        }

        var id = result.Body?.FtpServerId ?? Guid.NewGuid();
        return Created($"/api/v1/ftp-servers/{id}", new FtpServerIdResponse(id));
    }

    [HttpPut("{ftpServerId:guid}")]
    [ProducesResponseType(typeof(FtpServerStateResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<FtpServerStateResponse>> Put(
        [FromRoute] Guid ftpServerId,
        [FromBody] FtpServerUpsert body,
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey = null,
        CancellationToken ct = default)
    {
        var result = await _fxferServiceClient.UpdateFtpServerAsync(ftpServerId, body, idempotencyKey, ct);

        if (result.StatusCode is >= System.Net.HttpStatusCode.BadRequest)
        {
            return new ContentResult
            {
                StatusCode = (int)result.StatusCode,
                ContentType = result.ContentType ?? "application/problem+json",
                Content = result.RawBody ?? string.Empty,
            };
        }

        return Ok(result.Body ?? new FtpServerStateResponse(ftpServerId, body.IsActive));
    }

    [HttpDelete("{ftpServerId:guid}")]
    [ProducesResponseType(typeof(FtpServerStateResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<FtpServerStateResponse>> Delete(
        [FromRoute] Guid ftpServerId,
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey = null,
        CancellationToken ct = default)
    {
        var result = await _fxferServiceClient.InvalidateFtpServerAsync(ftpServerId, idempotencyKey, ct);

        if (result.StatusCode is >= System.Net.HttpStatusCode.BadRequest)
        {
            return new ContentResult
            {
                StatusCode = (int)result.StatusCode,
                ContentType = result.ContentType ?? "application/problem+json",
                Content = result.RawBody ?? string.Empty,
            };
        }

        return Ok(result.Body ?? new FtpServerStateResponse(ftpServerId, IsActive: false));
    }
}

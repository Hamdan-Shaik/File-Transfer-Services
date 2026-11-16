using System.Net;

using Fxfer.Bff.Api.Models;
using Fxfer.Bff.Infrastructure.Downstream;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fxfer.Bff.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/file-transfers")]
public sealed class FileTransfersController : ControllerBase
{
    private readonly FxferServiceClient _fxferServiceClient;

    public FileTransfersController(FxferServiceClient fxferServiceClient)
    {
        _fxferServiceClient = fxferServiceClient;
    }

    [HttpPost("search")]
    [ProducesResponseType(typeof(PagedResult<TrackedFile>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<TrackedFile>>> Search(
        [FromBody] FileTransferSearch body,
        CancellationToken ct = default)
    {
        var query = new FxferServiceClient.FxferServiceFileTransferSearch(
            Page: body.Page,
            PageSize: body.PageSize,
            OrderBy: body.OrderBy,
            OrderDir: body.OrderDir,
            Filters: new FxferServiceClient.FxferServiceFileTransferSearchFilters(
                FileId: body.Filters.FileId,
                FileNameContains: body.Filters.FileNameContains,
                FileStatus: body.Filters.FileStatus,
                FileDirection: body.Filters.FileDirection,
                TransferMethod: body.Filters.TransferMethod,
                CreatedFrom: body.Filters.CreatedFrom,
                CreatedTo: body.Filters.CreatedTo,
                FileSizeMin: body.Filters.FileSizeMin,
                FileSizeMax: body.Filters.FileSizeMax,
                TradingPartner: body.Filters.TradingPartner,
                SourceLocationId: body.Filters.SourceLocationId));

        var result = await _fxferServiceClient.SearchFileTransfersAsync(query, ct);

        if (result.StatusCode is >= HttpStatusCode.BadRequest)
        {
            return new ContentResult
            {
                StatusCode = (int)result.StatusCode,
                ContentType = result.ContentType ?? "application/problem+json",
                Content = result.RawBody ?? string.Empty,
            };
        }

        return Ok(result.Body ?? new PagedResult<TrackedFile>(Array.Empty<TrackedFile>(), 0, body.Page, body.PageSize));
    }
}


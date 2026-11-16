using FxferService.Api.Queries.Models;
using FxferService.Application.Contracts.Queries;
using FxferService.Application.QueryHandlers;
using FxferService.Domain.Contracts.Domain;
using FxferService.Domain.Contracts.Queries.Parameters;
using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Persistence.Batch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FxferService.Api.Queries.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/file-transfers")]
public sealed class FileTransfersQueriesController : ControllerBase
{
    private readonly FxferDbContext _dbContext;

    public FileTransfersQueriesController(FxferDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("search")]
    [ProducesResponseType(typeof(PagedResult<TrackedFile>), StatusCodes.Status200OK)]
    public ActionResult<PagedResult<TrackedFile>> SearchFileTransfers([FromBody] FileTransferSearch body)
    {
        var filters = body.Filters;

        List<SortCriteria>? sortCriteria = null;
        if (!string.IsNullOrWhiteSpace(body.OrderBy))
        {
            sortCriteria = new List<SortCriteria>
            {
                new SortCriteria
                {
                    FieldName = body.OrderBy,
                    Order = string.Equals(body.OrderDir, "desc", StringComparison.OrdinalIgnoreCase)
                        ? SortOrder.Desc
                        : SortOrder.Asc,
                },
            };
        }

        var query = new FileTransferTrackingQuery
        {
            CurrentPage = body.Page,
            PageSize = body.PageSize,
            SortCriteria = sortCriteria,
            Where = new FileTransferTrackingQueryParms
            {
                FileId = filters?.FileId?.ToString() ?? string.Empty,
                FileName = filters?.FileNameContains ?? string.Empty,
                FileStatus = filters?.FileStatus ?? string.Empty,
                FileDirection = filters?.FileDirection ?? string.Empty,
                FileTransportType = filters?.TransferMethod ?? string.Empty,
                FileSourceLocationId = filters?.SourceLocationId?.ToString() ?? string.Empty,
                StartDate = filters?.CreatedFrom?.UtcDateTime,
                EndDate = filters?.CreatedTo?.UtcDateTime,
                FileSizeFrom = filters?.FileSizeMin is null ? null : (decimal)filters.FileSizeMin.Value,
                FileSizeThrough = filters?.FileSizeMax is null ? null : (decimal)filters.FileSizeMax.Value,
                TradingPartner = filters?.TradingPartner ?? string.Empty,
            },
        };

        using var fileTransferContext = new FileTransferEntityDbContext(_dbContext);
        var routed = new FileTransferTrackingQueryExecutor().Execute(fileTransferContext, query);

        var items = (routed.Results ?? new List<FileTransferTrackingModel>())
            .Select(file => new TrackedFile(
                FileId: file.FileId,
                FileName: file.FileName,
                FileStatus: file.FileStatus,
                FileDirection: file.FileDirection,
                FileTransportType: file.FileTransportType,
                CreatedAt: new DateTimeOffset(file.CreatedDate, TimeSpan.Zero),
                FileSize: file.FileSize,
                TradingPartner: string.IsNullOrWhiteSpace(file.TradingPartner) ? null : file.TradingPartner,
                SourceLocationId: file.FileSourceLocationId == Guid.Empty ? null : file.FileSourceLocationId,
                SourceLocationConfigName: string.IsNullOrWhiteSpace(file.SourceLocConfigName) ? null : file.SourceLocConfigName,
                SourceLocationAddress: string.IsNullOrWhiteSpace(file.SourceLocAddress) ? null : file.SourceLocAddress,
                OriginatingServerName: string.IsNullOrWhiteSpace(file.OriginatingServerName) ? null : file.OriginatingServerName,
                Events: (file.Events ?? new List<FileEventModel>())
                    .Select(e => new TrackedFileEvent(
                        EventId: e.EventId,
                        EventCode: e.EventCode,
                        CreatedAt: new DateTimeOffset(e.CreatedDate, TimeSpan.Zero),
                        Description: e.EventTrackDescription ?? string.Empty))
                    .ToArray(),
                Exceptions: (file.Exceptions ?? new List<FileExceptionModel>())
                    .Select(ex => new TrackedFileException(
                        ExceptionId: ex.ExceptionId,
                        ExceptionType: ex.ExceptionType,
                        Message: ex.Message,
                        StackTrace: ex.StackTrace,
                        MethodName: ex.MethodName,
                        CreatedAt: new DateTimeOffset(ex.CreatedDate, TimeSpan.Zero)))
                    .ToArray()))
            .ToArray();

        return Ok(new PagedResult<TrackedFile>(
            Items: items,
            RowCount: routed.RowCount,
            CurrentPage: routed.CurrentPage,
            PageSize: routed.PageSize));
    }
}


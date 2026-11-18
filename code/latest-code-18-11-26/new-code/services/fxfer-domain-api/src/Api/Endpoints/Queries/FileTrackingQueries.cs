using Fxfer.DomainApi.Application.Queries;
using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Contracts.Queries.Parameters;
using Fxfer.DomainApi.Contracts.Utilities;
using Fxfer.DomainApi.Infrastructure.DataAccess;

namespace Fxfer.DomainApi.Api.Endpoints.Queries;

public static class FileTrackingQueries
{
    public static RouteGroupBuilder MapFileTrackingQueries(this RouteGroupBuilder group)
    {
        group.MapGet("/file-tracking", async (
            FxferDbContext dbContext,
            string? fileId,
            string? fileName,
            string? fileStatus,
            string? fileDirection,
            string? fileTransportType,
            string? fileSourceLocationId,
            DateTimeOffset? startDate,
            DateTimeOffset? endDate,
            decimal? fileSizeFrom,
            decimal? fileSizeThrough,
            string? tradingPartner,
            int page = 1,
            int pageSize = 50,
            CancellationToken ct = default) =>
        {
            // F05-019: route the request through the query handler and return results.
            var query = new FileTransferTrackingQuery
            {
                Where = new FileTransferTrackingQueryParms
                {
                    // F05-023: invalid file/source-location reference is treated as the system default.
                    FileId = fileId,
                    FileName = fileName,
                    FileStatus = fileStatus,
                    FileDirection = fileDirection,
                    FileTransportType = fileTransportType,
                    FileSourceLocationId = fileSourceLocationId,
                    StartDate = startDate?.UtcDateTime,
                    EndDate = endDate?.UtcDateTime,
                    FileSizeFrom = fileSizeFrom,
                    FileSizeThrough = fileSizeThrough,
                    TradingPartner = tradingPartner
                }
            };

            var executor = new FileTransferTrackingQueryExecutor();
            var results = await executor.ExecuteAsync(dbContext, query, ct);

            var paged = ApplyPaging(results, page, pageSize);

            return Results.Ok(new PagedResult<FileTransferTracking>
            {
                Items = paged.Items.Select(MapTracking).ToArray(),
                Total = paged.Total,
                Page = page,
                PageSize = pageSize
            });
        });

        return group;
    }

    private static (IReadOnlyList<FileTransferTrackingModel> Items, int Total) ApplyPaging(
        IReadOnlyList<FileTransferTrackingModel> items,
        int page,
        int pageSize)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return (items, items.Count);
        }

        var skip = (page - 1) * pageSize;
        return (items.Skip(skip).Take(pageSize).ToArray(), items.Count);
    }

    private static FileTransferTracking MapTracking(FileTransferTrackingModel model)
    {
        var createdUtc = DateTime.SpecifyKind(model.CreatedDate, DateTimeKind.Utc);

        return new FileTransferTracking
        {
            FileId = model.FileId,
            FileSourceLocationId = model.FileSourceLocationId,
            FileName = model.FileName,
            FileStatus = model.FileStatus,
            FileDirection = model.FileDirection,
            FileTransportType = model.FileTransportType,
            CreatedDate = new DateTimeOffset(createdUtc),
            TradingPartner = model.TradingPartner,
            CheckSumCode = model.CheckSumCode,
            FileSize = model.FileSize,
            SourceLocAddress = model.SourceLocAddress,
            SourceLocConfigName = model.SourceLocConfigName,
            OriginatingServerName = model.OriginatingServerName,
            Events = (model.Events ?? []).Select(MapEvent).ToArray(),
            Exceptions = (model.Exceptions ?? []).Select(MapException).ToArray()
        };
    }

    private static FileEvent MapEvent(FileEventModel model)
    {
        var createdUtc = DateTime.SpecifyKind(model.CreatedDate, DateTimeKind.Utc);

        return new FileEvent
        {
            EventId = model.EventId,
            FileId = model.FileId,
            DestinationId = model.DestinationId,
            EventCode = model.EventCode ?? string.Empty,
            EventTrackDescription = model.EventTrackDescription ?? string.Empty,
            CreatedDate = new DateTimeOffset(createdUtc)
        };
    }

    private static FileException MapException(FileExceptionModel model)
    {
        var createdUtc = DateTime.SpecifyKind(model.CreatedDate, DateTimeKind.Utc);

        return new FileException
        {
            ExceptionId = model.ExceptionId,
            FileId = model.FileId,
            DestinationId = model.DestinationId,
            EventId = model.EventId,
            ExceptionType = model.ExceptionType ?? string.Empty,
            MethodName = model.MethodName ?? string.Empty,
            Message = model.Message,
            StackTrace = model.StackTrace,
            CreatedDate = new DateTimeOffset(createdUtc)
        };
    }
}


using Fxfer.Bff.Contracts.Common;
using Fxfer.Bff.Contracts.Tracking;
using System.Diagnostics;
using Fxfer.Bff.Api;
using Fxfer.Bff.Application.Providers;
using Microsoft.AspNetCore.Mvc;

namespace Fxfer.Bff.Api.Endpoints.V1;

public static class FileTrackingEndpoints
{
    public static RouteGroupBuilder MapFileTrackingEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/file-tracking", async (
            IFileTransferQueryProvider provider,
            string? fileId,
            string? fileName,
            string? fileStatus,
            string? fileDirection,
            string? fileTransportType,
            string? fileSourceLocationId,
            string? startDate,
            string? endDate,
            string? fileSizeFrom,
            string? fileSizeThrough,
            string? tradingPartner,
            int page = 1,
            int pageSize = 5,
            CancellationToken ct = default) =>
        {
            var result = await provider.SearchFileTransferTrackingAsync(
                new FileTrackingSearchParams
                {
                    FileId = fileId,
                    FileName = fileName,
                    FileStatus = fileStatus,
                    FileDirection = fileDirection,
                    FileTransportType = fileTransportType,
                    FileSourceLocationId = fileSourceLocationId,
                    StartDate = startDate,
                    EndDate = endDate,
                    FileSizeFrom = fileSizeFrom,
                    FileSizeThrough = fileSizeThrough,
                    TradingPartner = tradingPartner,
                    Page = page,
                    PageSize = pageSize
                },
                ct);

            return Results.Ok(result);
        });

        group.MapGet("/file-tracking/{fileId:guid}", async (
            IFileTransferQueryProvider provider,
            HttpContext context,
            Guid fileId,
            CancellationToken ct = default) =>
        {
            var tracking = await provider.GetFileTransferTrackingDetailAsync(fileId, ct);
            return tracking is null ? CreateNotFoundProblem(context) : Results.Ok(tracking);
        });

        return group;
    }

    private static IResult CreateNotFoundProblem(HttpContext context)
    {
        var correlationId = RequestContext.GetCorrelationId(context) ?? RequestContext.GetOrCreateCorrelationId(context);
        var traceId = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;

        var problem = new ProblemDetails
        {
            Type = "about:blank",
            Title = "Not Found",
            Status = StatusCodes.Status404NotFound,
            Instance = context.Request.Path
        };

        problem.Extensions["correlation_id"] = correlationId;
        problem.Extensions["trace_id"] = traceId;

        return Results.Json(problem, statusCode: StatusCodes.Status404NotFound, contentType: "application/problem+json");
    }
}


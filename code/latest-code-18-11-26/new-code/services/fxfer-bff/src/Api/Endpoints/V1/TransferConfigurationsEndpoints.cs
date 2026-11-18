using Fxfer.Bff.Contracts.Common;
using Fxfer.Bff.Contracts.TransferConfigurations;
using System.Diagnostics;
using Fxfer.Bff.Api;
using Fxfer.Bff.Application.Providers;
using Microsoft.AspNetCore.Mvc;

namespace Fxfer.Bff.Api.Endpoints.V1;

public static class TransferConfigurationsEndpoints
{
    public static RouteGroupBuilder MapTransferConfigurationsEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/transfer-configurations", async (
            IFileTransferQueryProvider provider,
            int? configId,
            string? configName,
            string? ownerName,
            Guid? ownerSubsystem,
            string? protocol,
            bool? isActive,
            int page = 1,
            int pageSize = 50,
            CancellationToken ct = default) =>
        {
            var result = await provider.SearchFileTransferAsync(
                new TransferConfigurationSearchParams
                {
                    ConfigId = configId,
                    ConfigName = configName,
                    OwnerName = ownerName,
                    OwnerSubsystem = ownerSubsystem,
                    Protocol = protocol,
                    IsActive = isActive,
                    Page = page,
                    PageSize = pageSize
                },
                ct);

            return Results.Ok(result);
        });

        group.MapPost("/transfer-configurations", async (
            IFileTransferCommandProvider provider,
            TransferConfigurationUpsertRequest request,
            CancellationToken ct = default) =>
        {
            var created = await provider.AddFileTransferBaseConfigurationAsync(request, ct);
            return Results.Created($"/api/file-transfer/v1/transfer-configurations/{created.FtsSourceLocationId}", created);
        });

        group.MapGet("/transfer-configurations/{sourceLocationId:guid}", async (
            IFileTransferQueryProvider provider,
            HttpContext context,
            Guid sourceLocationId,
            CancellationToken ct = default) =>
        {
            var configuration = await provider.GetFileTransferConfigurationInformationAsync(sourceLocationId, ct);
            return configuration is null ? CreateNotFoundProblem(context) : Results.Ok(configuration);
        });

        group.MapPut("/transfer-configurations/{sourceLocationId:guid}", (Guid sourceLocationId, TransferConfigurationUpsertRequest request) =>
        async (
            IFileTransferCommandProvider provider,
            Guid sourceLocationId,
            TransferConfigurationUpsertRequest request,
            CancellationToken ct = default) =>
        {
            var updated = await provider.UpdateFileTransferBaseConfigurationAsync(sourceLocationId, request, ct);
            return Results.Ok(updated);
        });

        group.MapPatch("/transfer-configurations/{sourceLocationId:guid}", async (
            IFileTransferCommandProvider commandProvider,
            IFileTransferQueryProvider queryProvider,
            HttpContext context,
            Guid sourceLocationId,
            TransferConfigurationPatchRequest request,
            CancellationToken ct = default) =>
        {
            if (request.IsActive == false)
            {
                await commandProvider.InvalidateFileTransferBaseConfigurationAsync(sourceLocationId, ct);
            }

            var configuration = await queryProvider.GetFileTransferConfigurationInformationAsync(sourceLocationId, ct);
            return configuration is null ? CreateNotFoundProblem(context) : Results.Ok(configuration);
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


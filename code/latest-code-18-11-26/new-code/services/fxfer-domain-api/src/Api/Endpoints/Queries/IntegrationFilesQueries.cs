using Fxfer.DomainApi.Application.Queries;
using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Contracts.Queries.Parameters;
using Fxfer.DomainApi.Infrastructure.DataAccess;

namespace Fxfer.DomainApi.Api.Endpoints.Queries;

public static class IntegrationFilesQueries
{
    public static RouteGroupBuilder MapIntegrationFilesQueries(this RouteGroupBuilder group)
    {
        group.MapGet("/integration-files", async (
            FxferDbContext dbContext,
            Guid? integrationFileId,
            string? fileName,
            bool isValid = true,
            CancellationToken ct = default) =>
        {
            // F03-002: route the request through the query handler and return results.
            // Legacy behavior returns only active integration files; `isValid` does not change that filter.
            _ = isValid;

            var query = new GetIntegrationFileQuery
            {
                Where = new IntegrationFileQueryParms
                {
                    FTSIntegrationFileId = integrationFileId ?? Guid.Empty,
                    FileName = fileName
                }
            };

            var executor = new IntegrationFileQueryExecutor();
            var results = await executor.ExecuteAsync(dbContext, query, ct);
            return Results.Ok(results.Select(MapDefinition).ToArray());
        });

        return group;
    }

    private static IntegrationFileDefinition MapDefinition(FtsIntegrationFileModel model)
    {
        return new IntegrationFileDefinition
        {
            FtsIntegrationFileId = model.FtsIntegrationFileId,
            GroupId = model.GroupId,
            ConfigId = model.ConfigId,
            FileName = model.FileName ?? string.Empty,
            ETFOutboundMetadataDefnId = model.ETFOutboundMetadataDefnId,
            IsProcessed = model.IsProcessed,
            // F03-009: the lookup does not provide validity/time details.
            IsValid = null,
            CreationDate = null,
            LastModifiedDate = null
        };
    }
}


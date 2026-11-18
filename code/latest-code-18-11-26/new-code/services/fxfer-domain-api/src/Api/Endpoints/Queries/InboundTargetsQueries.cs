using Fxfer.DomainApi.Application.Queries;
using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Contracts.Queries.Parameters;
using Fxfer.DomainApi.Infrastructure.DataAccess;

namespace Fxfer.DomainApi.Api.Endpoints.Queries;

public static class InboundTargetsQueries
{
    public static RouteGroupBuilder MapInboundTargetsQueries(this RouteGroupBuilder group)
    {
        group.MapGet("/inbound-targets", async (
            FxferDbContext dbContext,
            Guid? fteMetadataId,
            string? contractName,
            CancellationToken ct = default) =>
        {
            // F05-021: route the request through the query handler and return results.
            var query = new InboundTargetQuery
            {
                Where = new InboundTargetQueryParms
                {
                    FTEMetadataId = fteMetadataId ?? Guid.Empty,
                    ContractName = contractName
                }
            };

            var executor = new InboundTargetQueryExecutor();
            var results = await executor.ExecuteAsync(dbContext, query, ct);
            return Results.Ok(results.Select(MapInboundTarget).ToArray());
        });

        return group;
    }

    private static InboundTarget MapInboundTarget(FtsInboundTargetModel model)
    {
        var creationUtc = DateTime.SpecifyKind(model.CreationDate, DateTimeKind.Utc);
        var lastModifiedUtc = DateTime.SpecifyKind(model.LastModifiedDate, DateTimeKind.Utc);

        return new InboundTarget
        {
            FtsInboundTargetId = model.FtsInboundTargetId,
            FteMetadataId = model.FTEMetadataId,
            ImportJobName = model.ImportJobName ?? string.Empty,
            ContractName = model.ContractName ?? string.Empty,
            IsValid = model.IsValid,
            CreationDate = new DateTimeOffset(creationUtc),
            LastModifiedDate = new DateTimeOffset(lastModifiedUtc)
        };
    }
}


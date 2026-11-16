using FxferService.Application.Contracts.Queries;
using FxferService.Domain.Contracts.Domain;
using FxferService.Infrastructure.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Entities;
using FxferService.Infrastructure.Queries;

namespace FxferService.Application.QueryHandlers;

public class IntegrationFileQueryExecutor : QueryExecutorBase<FtsIntegrationFileModel, GetIntegrationFileQuery>
{
    protected override IQueryable<FtsIntegrationFileModel> ApplySorting(IQueryable<FtsIntegrationFileModel> baseQuery, GetIntegrationFileQuery query)
    {
        return baseQuery;
    }

    protected override void AssignResults(IQueryable<FtsIntegrationFileModel> baseQuery, GetIntegrationFileQuery query)
    {
        query.Results = baseQuery.ToList();
    }

    // F02-065: Returns only valid integration files and applies optional ID/filename filters.
    // F02-066: Projects GroupId, ETFOutboundMetadataDefnId, ConfigId, FileName, IsProcessed, and FtsIntegrationFileId.
    protected override IQueryable<FtsIntegrationFileModel> GetBaseQuery(IDbContextBase context, GetIntegrationFileQuery query)
    {
        query.Validate();

        return from integrationFile in context.Set<FtsIntegrationFileEntity>()
               where (query.Where.FTSIntegrationFileId != Guid.Empty
                       ? integrationFile.FtsIntegrationFileId == query.Where.FTSIntegrationFileId
                       : true) &&
                     (!string.IsNullOrEmpty(query.Where.FileName)
                       ? integrationFile.FileName.Contains(query.Where.FileName)
                       : true) &&
                     integrationFile.IsValid
               select new FtsIntegrationFileModel()
               {
                   GroupId = integrationFile.GroupId,
                   ETFOutboundMetadataDefnId = integrationFile.OutboundMetadataDefnId,
                   ConfigId = integrationFile.ConfigId,
                   FileName = integrationFile.FileName,
                   IsProcessed = integrationFile.IsProcessed,
                   FtsIntegrationFileId = integrationFile.FtsIntegrationFileId,
               };
    }
}


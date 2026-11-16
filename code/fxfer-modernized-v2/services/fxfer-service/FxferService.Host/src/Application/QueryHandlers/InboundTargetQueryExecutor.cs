using FxferService.Application.Contracts.Queries;
using FxferService.Domain.Contracts.Domain;
using FxferService.Infrastructure.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Entities;
using FxferService.Infrastructure.Queries;

namespace FxferService.Application.QueryHandlers;

public class InboundTargetQueryExecutor : QueryExecutorBase<FtsInboundTargetModel, InboundTargetQuery>
{
    protected override IQueryable<FtsInboundTargetModel> ApplySorting(IQueryable<FtsInboundTargetModel> baseQuery, InboundTargetQuery query)
    {
        return baseQuery;
    }

    // F04-038: Inbound-target results are de-duplicated to one record per metadata id.
    protected override void AssignResults(IQueryable<FtsInboundTargetModel> baseQuery, InboundTargetQuery query)
    {
        query.Results = baseQuery
            .GroupBy(x => x.FTEMetadataId)
            .Select(g => g.OrderBy(x => x.FtsInboundTargetId).FirstOrDefault()!)
            .ToList();
    }

    // F04-038: Returns only valid inbound targets and applies optional exact-match filters.
    protected override IQueryable<FtsInboundTargetModel> GetBaseQuery(IDbContextBase context, InboundTargetQuery query)
    {
        query.Validate();

        return from inboundTarget in context.Set<FtsInboundTargetEntity>()
               where (query.Where.FTEMetadataId != Guid.Empty ? inboundTarget.ExternalFileDefnId == query.Where.FTEMetadataId : true) &&
                     (!string.IsNullOrEmpty(query.Where.ContractName) ? inboundTarget.ContractName == query.Where.ContractName : true) &&
                     inboundTarget.IsValid
               select new FtsInboundTargetModel()
               {
                   FTEMetadataId = inboundTarget.ExternalFileDefnId,
                   FtsInboundTargetId = inboundTarget.FtsInboundTargetId,
                   ImportJobName = inboundTarget.ImportJobName,
                   ContractName = inboundTarget.ContractName,
               };
    }
}


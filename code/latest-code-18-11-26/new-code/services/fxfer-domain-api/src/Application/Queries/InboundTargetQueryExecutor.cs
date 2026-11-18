using Microsoft.EntityFrameworkCore;

using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

namespace Fxfer.DomainApi.Application.Queries;

public sealed class InboundTargetQueryExecutor
{
    public async Task<IReadOnlyList<FtsInboundTargetModel>> ExecuteAsync(
        FxferDbContext dbContext,
        InboundTargetQuery query,
        CancellationToken ct)
    {
        query.Validate();

        // F05-032: Optional filtering by FTEMetadataId + ContractName; always requires IsValid.
        IQueryable<FtsInboundTargetModel> baseQuery =
            from inboundTarget in dbContext.Set<FtsInboundTarget>()
            where (query.Where.FTEMetadataId != Guid.Empty ? inboundTarget.FTEMetadataId == query.Where.FTEMetadataId : true)
                && (!string.IsNullOrEmpty(query.Where.ContractName) ? inboundTarget.ContractName == query.Where.ContractName : true)
                && inboundTarget.IsValid
            select new FtsInboundTargetModel
            {
                FTEMetadataId = inboundTarget.FTEMetadataId,
                FtsInboundTargetId = inboundTarget.FtsInboundTargetId,
                ImportJobName = inboundTarget.ImportJobName,
                ContractName = inboundTarget.ContractName,
                IsValid = inboundTarget.IsValid,
                CreationDate = inboundTarget.CreationDate.UtcDateTime,
                LastModifiedDate = inboundTarget.LastModifiedDate.UtcDateTime
            };

        // F05-033: De-duplicate results by FTEMetadataId, selecting the smallest FtsInboundTargetId.
        return await baseQuery
            .GroupBy(x => x.FTEMetadataId)
            .Select(g => g.OrderBy(x => x.FtsInboundTargetId).First())
            .ToListAsync(ct);
    }
}


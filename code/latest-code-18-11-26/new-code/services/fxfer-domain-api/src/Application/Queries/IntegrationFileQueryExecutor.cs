using Microsoft.EntityFrameworkCore;

using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

namespace Fxfer.DomainApi.Application.Queries;

public sealed class IntegrationFileQueryExecutor
{
    public async Task<IReadOnlyList<FtsIntegrationFileModel>> ExecuteAsync(
        FxferDbContext dbContext,
        GetIntegrationFileQuery query,
        CancellationToken ct)
    {
        query.Validate();

        // F03-005: Only return active (IsValid) integration file definitions.
        // F03-006: Filter by exact id only when a non-empty id is supplied.
        // F03-007: Filter by file name fragment only when a fragment is supplied.
        IQueryable<FtsIntegrationFileModel> baseQuery =
            from integrationFile in dbContext.Set<FtsIntegrationFile>()
            where (query.Where.FTSIntegrationFileId != Guid.Empty ? integrationFile.FtsIntegrationFileId == query.Where.FTSIntegrationFileId : true)
                && (!string.IsNullOrEmpty(query.Where.FileName) ? integrationFile.FileName.Contains(query.Where.FileName) : true)
                && integrationFile.IsValid
            select new FtsIntegrationFileModel
            {
                // F03-009: Project only the subset of fields used by the lookup.
                GroupId = integrationFile.GroupId,
                ETFOutboundMetadataDefnId = integrationFile.ETFOutboundMetadataDefnId,
                ConfigId = integrationFile.ConfigId,
                FileName = integrationFile.FileName,
                IsProcessed = integrationFile.IsProcessed,
                FtsIntegrationFileId = integrationFile.FtsIntegrationFileId
            };

        // F03-008: No explicit ordering is applied.
        return await baseQuery.ToListAsync(ct);
    }
}


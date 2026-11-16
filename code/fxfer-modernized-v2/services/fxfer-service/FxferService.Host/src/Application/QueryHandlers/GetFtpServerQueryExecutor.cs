using FxferService.Application.Contracts.Queries;
using FxferService.Domain.Contracts.Domain;
using FxferService.Infrastructure.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Entities;
using FxferService.Infrastructure.Queries;

namespace FxferService.Application.QueryHandlers;

public class GetFtpServerQueryExecutor : QueryExecutorBase<FtpServerModel, FtpServerQuery>
{
    // F01-007: HPP entry points do not apply a default ordering.
    // F01-008: HPP entry points ignore requested ordering.
    protected override IQueryable<FtpServerModel> ApplySorting(IQueryable<FtpServerModel> baseQuery, FtpServerQuery query)
    {
        return baseQuery;
    }

    protected override void AssignResults(IQueryable<FtpServerModel> baseQuery, FtpServerQuery query)
    {
        query.Results = baseQuery.ToList();
    }

    // F01-009: does not apply FTP server id filtering.
    // F01-010: only supports exact FTP server name matching (case-insensitive, no trimming).
    // F01-011: does not apply description filtering.
    // F01-012: does not apply user name filtering.
    // F01-029: duplicate-name detection queries valid records by name.
    // F01-030: name matching ignores case but treats leading/trailing spaces as part of the name.
    protected override IQueryable<FtpServerModel> GetBaseQuery(IDbContextBase context, FtpServerQuery query)
    {
        return from identifier in context.Set<FtpServer>()
               where ((query.Where.IsValid == null) || identifier.IsValid == query.Where.IsValid) &&
                     (string.IsNullOrEmpty(query.Where.FtpServerName) || identifier.FtpServerName.ToUpper() == query.Where.FtpServerName.ToUpper())
               select new FtpServerModel()
               {
                   FtpDescription = identifier.FtpDescription,
                   FtpPassword = identifier.FtpPassword,
                   FtpPort = identifier.FtpPort,
                   FtpProxyAddress = identifier.FtpProxyAddress,
                   FtpProxyPort = identifier.FtpProxyPort,
                   FtpServerId = identifier.FtpServerId,
                   FtpServerIp = identifier.FtpServerIp,
                   FtpServerName = identifier.FtpServerName,
                   FtpTransferMode = identifier.FtpTransferMode,
                   FtpUser = identifier.FtpUser,
                   IsValid = identifier.IsValid,
                   UseSSHAuth = identifier.UseSSHAuth,
                   CreationDate = identifier.CreationDate,
                   LastModifiedDate = identifier.LastModifiedDate,
                   OperatorId = identifier.OperatorId
               };
    }
}

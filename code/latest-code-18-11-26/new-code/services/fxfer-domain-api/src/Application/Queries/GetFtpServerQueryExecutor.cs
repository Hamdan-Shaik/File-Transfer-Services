using Microsoft.EntityFrameworkCore;

using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Contracts.Utilities;
using Fxfer.DomainApi.Domain.Constants;
using FtpServerEntity = Fxfer.DomainApi.Domain.Entities.FtpServer;
using Fxfer.DomainApi.Infrastructure.DataAccess;

namespace Fxfer.DomainApi.Application.Queries;

public sealed class GetFtpServerQueryExecutor
{
    public async Task<IReadOnlyList<FtpServerModel>> ExecuteAsync(
        FxferDbContext dbContext,
        FtpServerQuery query,
        CancellationToken ct)
    {
        IQueryable<FtpServerEntity> baseEntityQuery =
            from identifier in dbContext.Set<FtpServerEntity>()
            where (query.Where!.IsValid == null) || identifier.IsValid == query.Where.IsValid
            select identifier;

        IQueryable<FtpServerModel> baseQuery =
            from identifier in baseEntityQuery
            select new FtpServerModel
            {
                // F02-009: Project a fixed set of FTP server fields, including credentials fields.
                FtpDescription = identifier.FtpDescription,
                FtpPassword = identifier.FtpPasswordSecretRef,
                FtpPort = identifier.FtpPort,
                FtpProxyAddress = identifier.FtpProxyAddress,
                FtpProxyPort = identifier.FtpProxyPort,
                FtpServerId = identifier.FtpServerId,
                FtpServerIp = identifier.FtpServerHost,
                FtpServerName = identifier.FtpServerName,
                FtpTransferMode = identifier.FtpTransferMode,
                FtpUser = identifier.FtpUser,
                IsValid = identifier.IsValid,
                UseSSHAuth = identifier.UseSshAuth,
                CreationDate = identifier.CreationDate.UtcDateTime,
                LastModifiedDate = identifier.LastModifiedDate.UtcDateTime,
                OperatorId = identifier.OperatorId
            };

        var useLegacyDoorSemantics = UseLegacyDoorSemantics(query);

        baseQuery = ApplyWhereClause(baseQuery, query, useLegacyDoorSemantics);
        baseQuery = ApplySorting(baseQuery, query, useLegacyDoorSemantics);
        baseQuery = ApplyPaging(baseQuery, query);

        return await baseQuery.ToListAsync(ct);
    }

    private static bool UseLegacyDoorSemantics(FtpServerQuery query)
    {
        return query.Where?.SearchOptions is not null
            || (query.SortCriteria is not null && query.SortCriteria.Count > 0)
            || query.Where?.FtpServerId != Guid.Empty
            || !string.IsNullOrEmpty(query.Where?.FtpDescription)
            || !string.IsNullOrEmpty(query.Where?.FtpUser);
    }

    private static IQueryable<FtpServerModel> ApplyWhereClause(
        IQueryable<FtpServerModel> baseQuery,
        FtpServerQuery query,
        bool useLegacyDoorSemantics)
    {
        if (!useLegacyDoorSemantics)
        {
            if (!string.IsNullOrEmpty(query.Where!.FtpServerName))
            {
                baseQuery = baseQuery.Where(identifier => identifier.FtpServerName!.ToUpper() == query.Where.FtpServerName!.ToUpper());
            }

            return baseQuery;
        }

        var searchOptions = query.Where!.SearchOptions ?? new List<SearchModeOption>();

        var ftpServerIdSearchMode = SearchModeHelper.GetSearchMode(CommonConstants.SortFields.FtpServer.FtpServerId, searchOptions);
        var ftpServerNameSearchMode = SearchModeHelper.GetSearchMode(CommonConstants.SortFields.FtpServer.FtpServerName, searchOptions);
        var ftpDescriptionSearchMode = SearchModeHelper.GetSearchMode(CommonConstants.SortFields.FtpServer.FtpDescription, searchOptions);
        var ftpUserSearchMode = SearchModeHelper.GetSearchMode(CommonConstants.SortFields.FtpServer.FtpUser, searchOptions);

        var ftpServerIdValue = query.Where.FtpServerId.ToString();
        var ftpServerNameValue = query.Where.FtpServerName;
        var ftpDescriptionValue = query.Where.FtpDescription;
        var ftpUserValue = query.Where.FtpUser;

        if (!string.IsNullOrEmpty(ftpServerIdValue) && ftpServerIdValue != Guid.Empty.ToString())
        {
            if (ftpServerIdSearchMode == SearchModeOption.SearchMode.EqualTo.ToString())
            {
                baseQuery = baseQuery.Where(p => p.FtpServerId == new Guid(ftpServerIdValue));
            }
        }

        if (!string.IsNullOrEmpty(ftpServerNameValue))
        {
            ftpServerNameValue = ftpServerNameValue.ToUpper().Trim();

            baseQuery = ftpServerNameSearchMode switch
            {
                var mode when mode == SearchModeOption.SearchMode.Contains.ToString() => baseQuery.Where(p => p.FtpServerName!.ToUpper().Trim().Contains(ftpServerNameValue)),
                var mode when mode == SearchModeOption.SearchMode.StartsWith.ToString() => baseQuery.Where(p => p.FtpServerName!.ToUpper().Trim().StartsWith(ftpServerNameValue)),
                var mode when mode == SearchModeOption.SearchMode.EndsWith.ToString() => baseQuery.Where(p => p.FtpServerName!.ToUpper().Trim().EndsWith(ftpServerNameValue)),
                // F02-031: Enables duplicate-name detection by exact match (case-insensitive, trimmed).
                var mode when mode == SearchModeOption.SearchMode.EqualTo.ToString() => baseQuery.Where(p => p.FtpServerName!.ToUpper().Trim() == ftpServerNameValue),
                _ => baseQuery
            };
        }

        if (!string.IsNullOrEmpty(ftpDescriptionValue))
        {
            ftpDescriptionValue = ftpDescriptionValue.ToUpper().Trim();

            baseQuery = ftpDescriptionSearchMode switch
            {
                var mode when mode == SearchModeOption.SearchMode.Contains.ToString() => baseQuery.Where(p => p.FtpDescription!.ToUpper().Trim().Contains(ftpDescriptionValue)),
                var mode when mode == SearchModeOption.SearchMode.StartsWith.ToString() => baseQuery.Where(p => p.FtpDescription!.ToUpper().Trim().StartsWith(ftpDescriptionValue)),
                var mode when mode == SearchModeOption.SearchMode.EndsWith.ToString() => baseQuery.Where(p => p.FtpDescription!.ToUpper().Trim().EndsWith(ftpDescriptionValue)),
                var mode when mode == SearchModeOption.SearchMode.EqualTo.ToString() => baseQuery.Where(p => p.FtpDescription!.ToUpper().Trim() == ftpDescriptionValue),
                _ => baseQuery
            };
        }

        if (!string.IsNullOrEmpty(ftpUserValue))
        {
            ftpUserValue = ftpUserValue.ToUpper().Trim();

            baseQuery = ftpUserSearchMode switch
            {
                var mode when mode == SearchModeOption.SearchMode.Contains.ToString() => baseQuery.Where(p => p.FtpUser!.ToUpper().Trim().Contains(ftpUserValue)),
                var mode when mode == SearchModeOption.SearchMode.StartsWith.ToString() => baseQuery.Where(p => p.FtpUser!.ToUpper().Trim().StartsWith(ftpUserValue)),
                var mode when mode == SearchModeOption.SearchMode.EndsWith.ToString() => baseQuery.Where(p => p.FtpUser!.ToUpper().Trim().EndsWith(ftpUserValue)),
                var mode when mode == SearchModeOption.SearchMode.EqualTo.ToString() => baseQuery.Where(p => p.FtpUser!.ToUpper().Trim() == ftpUserValue),
                _ => baseQuery
            };
        }

        return baseQuery;
    }

    private static IQueryable<FtpServerModel> ApplySorting(
        IQueryable<FtpServerModel> baseQuery,
        FtpServerQuery query,
        bool useLegacyDoorSemantics)
    {
        if (!useLegacyDoorSemantics)
        {
            return baseQuery;
        }

        var sortCriteria = query.SortCriteria;

        if (sortCriteria is null || sortCriteria.Count == 0)
        {
            sortCriteria = new List<SortCriteria>
            {
                new SortCriteria
                {
                    FieldName = CommonConstants.SortFields.FtpServer.FtpServerName,
                    Order = SortOrder.Asc
                }
            };
        }

        IOrderedQueryable<FtpServerModel>? ordered = null;

        for (var i = 0; i < sortCriteria.Count; i++)
        {
            var sort = sortCriteria[i];

            ordered = (ordered, sort.FieldName, sort.Order) switch
            {
                (null, var f, SortOrder.Desc) when f == CommonConstants.SortFields.FtpServer.FtpServerName => baseQuery.OrderByDescending(p => p.FtpServerName!.ToUpper()),
                (null, var f, _) when f == CommonConstants.SortFields.FtpServer.FtpServerName => baseQuery.OrderBy(p => p.FtpServerName!.ToUpper()),
                (null, var f, SortOrder.Desc) when f == CommonConstants.SortFields.FtpServer.FtpDescription => baseQuery.OrderByDescending(p => p.FtpDescription!.ToUpper()),
                (null, var f, _) when f == CommonConstants.SortFields.FtpServer.FtpDescription => baseQuery.OrderBy(p => p.FtpDescription!.ToUpper()),
                (null, var f, SortOrder.Desc) when f == CommonConstants.SortFields.FtpServer.FtpServerIp => baseQuery.OrderByDescending(p => p.FtpServerIp),
                (null, var f, _) when f == CommonConstants.SortFields.FtpServer.FtpServerIp => baseQuery.OrderBy(p => p.FtpServerIp),
                (null, var f, SortOrder.Desc) when f == CommonConstants.SortFields.FtpServer.FtpUser => baseQuery.OrderByDescending(p => p.FtpUser!.ToUpper()),
                (null, var f, _) when f == CommonConstants.SortFields.FtpServer.FtpUser => baseQuery.OrderBy(p => p.FtpUser!.ToUpper()),
                (null, var f, SortOrder.Desc) when f == CommonConstants.SortFields.FtpServer.CreationDate => baseQuery.OrderByDescending(p => p.CreationDate),
                (null, var f, _) when f == CommonConstants.SortFields.FtpServer.CreationDate => baseQuery.OrderBy(p => p.CreationDate),
                (null, var f, SortOrder.Desc) when f == CommonConstants.SortFields.FtpServer.IsValid => baseQuery.OrderByDescending(p => p.IsValid),
                (null, var f, _) when f == CommonConstants.SortFields.FtpServer.IsValid => baseQuery.OrderBy(p => p.IsValid),

                (not null, var f, SortOrder.Desc) when f == CommonConstants.SortFields.FtpServer.FtpServerName => ordered.ThenByDescending(p => p.FtpServerName!.ToUpper()),
                (not null, var f, _) when f == CommonConstants.SortFields.FtpServer.FtpServerName => ordered.ThenBy(p => p.FtpServerName!.ToUpper()),
                (not null, var f, SortOrder.Desc) when f == CommonConstants.SortFields.FtpServer.FtpDescription => ordered.ThenByDescending(p => p.FtpDescription!.ToUpper()),
                (not null, var f, _) when f == CommonConstants.SortFields.FtpServer.FtpDescription => ordered.ThenBy(p => p.FtpDescription!.ToUpper()),
                (not null, var f, SortOrder.Desc) when f == CommonConstants.SortFields.FtpServer.FtpServerIp => ordered.ThenByDescending(p => p.FtpServerIp),
                (not null, var f, _) when f == CommonConstants.SortFields.FtpServer.FtpServerIp => ordered.ThenBy(p => p.FtpServerIp),
                (not null, var f, SortOrder.Desc) when f == CommonConstants.SortFields.FtpServer.FtpUser => ordered.ThenByDescending(p => p.FtpUser!.ToUpper()),
                (not null, var f, _) when f == CommonConstants.SortFields.FtpServer.FtpUser => ordered.ThenBy(p => p.FtpUser!.ToUpper()),
                (not null, var f, SortOrder.Desc) when f == CommonConstants.SortFields.FtpServer.CreationDate => ordered.ThenByDescending(p => p.CreationDate),
                (not null, var f, _) when f == CommonConstants.SortFields.FtpServer.CreationDate => ordered.ThenBy(p => p.CreationDate),
                (not null, var f, SortOrder.Desc) when f == CommonConstants.SortFields.FtpServer.IsValid => ordered.ThenByDescending(p => p.IsValid),
                (not null, var f, _) when f == CommonConstants.SortFields.FtpServer.IsValid => ordered.ThenBy(p => p.IsValid),
                _ => throw new ArgumentOutOfRangeException(nameof(sortCriteria), sort.FieldName, "Unknown sort field")
            };
        }

        return ordered ?? baseQuery;
    }

    private static IQueryable<FtpServerModel> ApplyPaging(IQueryable<FtpServerModel> baseQuery, FtpServerQuery query)
    {
        if (query.Page is null || query.PageSize is null)
        {
            return baseQuery;
        }

        if (query.Page <= 0 || query.PageSize <= 0)
        {
            return baseQuery;
        }

        var skip = (query.Page.Value - 1) * query.PageSize.Value;
        return baseQuery.Skip(skip).Take(query.PageSize.Value);
    }
}


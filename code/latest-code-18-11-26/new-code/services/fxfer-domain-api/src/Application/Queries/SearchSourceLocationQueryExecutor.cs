using Microsoft.EntityFrameworkCore;

using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

namespace Fxfer.DomainApi.Application.Queries;

public sealed class SearchSourceLocationQueryExecutor
{
    public async Task<IReadOnlyList<SourceLocationModel>> ExecuteAsync(
        FxferDbContext dbContext,
        SourceLocationQuery query,
        CancellationToken ct)
    {
        // F01-091: Project source locations into SourceLocationModel results.
        IQueryable<SourceLocationModel> baseQuery =
            from identifier in dbContext.Set<FtsSourceLocation>()
            select new SourceLocationModel
            {
                FtsSourceLocationId = identifier.FtsSourceLocationId,
                ConfigId = identifier.ConfigId,
                ConfigName = identifier.ConfigName,
                OwnerName = identifier.OwnerName,
                OwnerSubsystem = identifier.OwnerModule,
                Protocol = identifier.SourceProtocolCD,
                Address = identifier.SourceAddress,
                Mask = identifier.SourceFileMask,
                IsActive = identifier.IsValid
            };

        baseQuery = ApplyWhereClause(baseQuery, query);
        baseQuery = ApplySorting(baseQuery, query);

        // F01-099: Materialize into a list.
        return await baseQuery.ToListAsync(ct);
    }

    private static IQueryable<SourceLocationModel> ApplyWhereClause(
        IQueryable<SourceLocationModel> baseQuery,
        SourceLocationQuery query)
    {
        // Legacy MaintainabilityDetails: ApprovedIndex=55, ReviewedOnSprint=94, ReviewerName=Josue Chavez.
        // F01-092: ConfigId filter applies only when ConfigId > 0.
        if (query.Where.ConfigId.GetValueOrDefault() > 0)
        {
            var configId = query.Where.ConfigId.GetValueOrDefault();
            baseQuery = baseQuery.Where(p => p.ConfigId == configId);
        }

        // F01-093: OwnerName exact match, case-insensitive.
        if (!string.IsNullOrEmpty(query.Where.OwnerName))
        {
            baseQuery = baseQuery.Where(p => p.OwnerName!.ToUpper() == query.Where.OwnerName.ToUpper());
        }

        // F01-094: ConfigName exact match, case-insensitive.
        if (!string.IsNullOrEmpty(query.Where.ConfigName))
        {
            baseQuery = baseQuery.Where(p => p.ConfigName!.ToUpper() == query.Where.ConfigName.ToUpper());
        }

        // F01-095: OwnerSubsystem is parsed as a GUID and compared to OwnerSubsystem.
        if (!string.IsNullOrEmpty(query.Where.OwnerSubsystem))
        {
            baseQuery = baseQuery.Where(p => p.OwnerSubsystem == query.Where.OwnerModuleGuid);
        }

        // F01-096: Protocol exact match.
        if (!string.IsNullOrEmpty(query.Where.Protocol))
        {
            baseQuery = baseQuery.Where(p => p.Protocol == query.Where.Protocol);
        }

        // F01-097: When IsActive is true, apply an active filter; when false, apply nothing.
        if (query.Where.IsActive)
        {
            baseQuery = baseQuery.Where(p => p.IsActive == query.Where.IsActive);
        }

        return baseQuery;
    }

    private static IQueryable<SourceLocationModel> ApplySorting(
        IQueryable<SourceLocationModel> baseQuery,
        SourceLocationQuery query)
    {
        var sortCriteria = query.SortCriteria;

        // F01-098: Default sort is ConfigId ASC then IsActive DESC.
        if (sortCriteria is null || sortCriteria.Count == 0)
        {
            return baseQuery.OrderBy(l => l.ConfigId).ThenByDescending(l => l.IsActive);
        }

        IOrderedQueryable<SourceLocationModel>? ordered = null;

        for (var i = 0; i < sortCriteria.Count; i++)
        {
            var sort = sortCriteria[i];

            ordered = (ordered, sort.FieldName, sort.Order) switch
            {
                (null, "ConfigId", SortOrder.Desc) => baseQuery.OrderByDescending(p => p.ConfigId),
                (null, "ConfigId", _) => baseQuery.OrderBy(p => p.ConfigId),

                (null, "ConfigName", SortOrder.Desc) => baseQuery.OrderByDescending(p => p.ConfigName),
                (null, "ConfigName", _) => baseQuery.OrderBy(p => p.ConfigName),

                (null, "SourceAddress", SortOrder.Desc) => baseQuery.OrderByDescending(p => p.Address),
                (null, "SourceAddress", _) => baseQuery.OrderBy(p => p.Address),

                (null, "SourceFileMask", SortOrder.Desc) => baseQuery.OrderByDescending(p => p.Mask),
                (null, "SourceFileMask", _) => baseQuery.OrderBy(p => p.Mask),

                (null, "OwnerName", SortOrder.Desc) => baseQuery.OrderByDescending(p => p.OwnerName),
                (null, "OwnerName", _) => baseQuery.OrderBy(p => p.OwnerName),

                (null, "ModuleName", SortOrder.Desc) => baseQuery.OrderByDescending(p => p.OwnerSubsystem),
                (null, "ModuleName", _) => baseQuery.OrderBy(p => p.OwnerSubsystem),

                (null, "Status", SortOrder.Desc) => baseQuery.OrderByDescending(p => p.IsActive),
                (null, "Status", _) => baseQuery.OrderBy(p => p.IsActive),

                (not null, "ConfigId", SortOrder.Desc) => ordered.ThenByDescending(p => p.ConfigId),
                (not null, "ConfigId", _) => ordered.ThenBy(p => p.ConfigId),

                (not null, "ConfigName", SortOrder.Desc) => ordered.ThenByDescending(p => p.ConfigName),
                (not null, "ConfigName", _) => ordered.ThenBy(p => p.ConfigName),

                (not null, "SourceAddress", SortOrder.Desc) => ordered.ThenByDescending(p => p.Address),
                (not null, "SourceAddress", _) => ordered.ThenBy(p => p.Address),

                (not null, "SourceFileMask", SortOrder.Desc) => ordered.ThenByDescending(p => p.Mask),
                (not null, "SourceFileMask", _) => ordered.ThenBy(p => p.Mask),

                (not null, "OwnerName", SortOrder.Desc) => ordered.ThenByDescending(p => p.OwnerName),
                (not null, "OwnerName", _) => ordered.ThenBy(p => p.OwnerName),

                (not null, "ModuleName", SortOrder.Desc) => ordered.ThenByDescending(p => p.OwnerSubsystem),
                (not null, "ModuleName", _) => ordered.ThenBy(p => p.OwnerSubsystem),

                (not null, "Status", SortOrder.Desc) => ordered.ThenByDescending(p => p.IsActive),
                (not null, "Status", _) => ordered.ThenBy(p => p.IsActive),

                _ => throw new ArgumentOutOfRangeException(nameof(sortCriteria), sort.FieldName, "Unknown sort field")
            };
        }

        return ordered ?? baseQuery;
    }
}


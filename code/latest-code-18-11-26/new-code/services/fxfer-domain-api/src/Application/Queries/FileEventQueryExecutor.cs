using Microsoft.EntityFrameworkCore;

using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

namespace Fxfer.DomainApi.Application.Queries;

public sealed class FileEventQueryExecutor
{
    public async Task<IReadOnlyList<FileEventModel>> ExecuteAsync(
        FxferDbContext dbContext,
        FileEventQuery query,
        CancellationToken ct)
    {
        IQueryable<FileEventModel> baseQuery =
            from fm in dbContext.Set<FtsFileManagement>()
            join fe in dbContext.Set<FtsFileEvent>() on fm.FileId equals fe.FileId
            select new FileEventModel
            {
                EventId = fe.EventId,
                FileId = fe.FileId,
                EventCode = fe.EventCD,
                CreatedDate = fm.CreatedDate,
                EventTrackDescription = fe.EventTrackDescription
            };

        baseQuery = ApplyWhereClause(baseQuery, query);
        baseQuery = ApplySorting(baseQuery, query);
        baseQuery = ApplyPaging(baseQuery, query);

        return await baseQuery.ToListAsync(ct);
    }

    private static IQueryable<FileEventModel> ApplyWhereClause(IQueryable<FileEventModel> baseQuery, FileEventQuery query)
    {
        Guid fileId = Guid.Empty;
        Guid.TryParse(query.Where!.FileId, out fileId);

        if (query.Where.FileId != null)
        {
            baseQuery = baseQuery.Where(p => p.FileId == fileId);
        }

        return baseQuery;
    }

    private static IQueryable<FileEventModel> ApplySorting(IQueryable<FileEventModel> baseQuery, FileEventQuery query)
    {
        if (query.SortCriteria is null || query.SortCriteria.Count == 0)
        {
            return baseQuery
                .OrderBy(p => p.EventId)
                .ThenBy(p => p.EventCode);
        }

        IOrderedQueryable<FileEventModel>? ordered = null;

        for (var i = 0; i < query.SortCriteria.Count; i++)
        {
            var sort = query.SortCriteria[i];

            ordered = (ordered, sort.FieldName, sort.Order) switch
            {
                (null, "EVENT_ID", SortOrder.Desc) => baseQuery.OrderByDescending(p => p.EventId),
                (null, "EVENT_ID", _) => baseQuery.OrderBy(p => p.EventId),
                (not null, "EVENT_ID", SortOrder.Desc) => ordered.ThenByDescending(p => p.EventId),
                (not null, "EVENT_ID", _) => ordered.ThenBy(p => p.EventId),
                _ => throw new ArgumentOutOfRangeException(nameof(query.SortCriteria), sort.FieldName, "Unknown sort field")
            };
        }

        return ordered ?? baseQuery;
    }

    private static IQueryable<FileEventModel> ApplyPaging(IQueryable<FileEventModel> baseQuery, FileEventQuery query)
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


using Microsoft.EntityFrameworkCore;

using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

namespace Fxfer.DomainApi.Application.Queries;

public sealed class FileExceptionQueryExecutor
{
    public async Task<IReadOnlyList<FileExceptionModel>> ExecuteAsync(
        FxferDbContext dbContext,
        FileExceptionQuery query,
        CancellationToken ct)
    {
        IQueryable<FileExceptionModel> baseQuery =
            from fm in dbContext.Set<FtsFileManagement>()
            join fe in dbContext.Set<FtsFileException>() on fm.FileId equals fe.FileId
            select new FileExceptionModel
            {
                EventId = fe.EventId,
                FileId = fe.FileId,
                ExceptionType = fe.ExceptionType,
                Message = fe.Message,
                StackTrace = fe.StackTrace,
                MethodName = fe.MethodName,
                CreatedDate = fm.CreatedDate,
                ExceptionId = fe.ExceptionId
            };

        baseQuery = ApplyWhereClause(baseQuery, query);
        baseQuery = ApplySorting(baseQuery, query);
        baseQuery = ApplyPaging(baseQuery, query);

        return await baseQuery.ToListAsync(ct);
    }

    private static IQueryable<FileExceptionModel> ApplyWhereClause(IQueryable<FileExceptionModel> baseQuery, FileExceptionQuery query)
    {
        Guid fileId = Guid.Empty;
        Guid.TryParse(query.Where!.FileId, out fileId);

        if (query.Where.FileId != null)
        {
            baseQuery = baseQuery.Where(p => p.FileId == fileId);
        }

        return baseQuery;
    }

    private static IQueryable<FileExceptionModel> ApplySorting(IQueryable<FileExceptionModel> baseQuery, FileExceptionQuery query)
    {
        if (query.SortCriteria is null || query.SortCriteria.Count == 0)
        {
            return baseQuery
                .OrderBy(p => p.ExceptionId)
                .ThenBy(p => p.ExceptionType);
        }

        IOrderedQueryable<FileExceptionModel>? ordered = null;

        for (var i = 0; i < query.SortCriteria.Count; i++)
        {
            var sort = query.SortCriteria[i];

            ordered = (ordered, sort.FieldName, sort.Order) switch
            {
                (null, "EXCEPTION_ID", SortOrder.Desc) => baseQuery.OrderByDescending(p => p.ExceptionId),
                (null, "EXCEPTION_ID", _) => baseQuery.OrderBy(p => p.ExceptionId),
                (not null, "EXCEPTION_ID", SortOrder.Desc) => ordered.ThenByDescending(p => p.ExceptionId),
                (not null, "EXCEPTION_ID", _) => ordered.ThenBy(p => p.ExceptionId),
                _ => throw new ArgumentOutOfRangeException(nameof(query.SortCriteria), sort.FieldName, "Unknown sort field")
            };
        }

        return ordered ?? baseQuery;
    }

    private static IQueryable<FileExceptionModel> ApplyPaging(IQueryable<FileExceptionModel> baseQuery, FileExceptionQuery query)
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


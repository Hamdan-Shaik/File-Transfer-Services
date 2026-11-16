using FxferService.Application.Contracts.Queries;
using FxferService.Infrastructure.Persistence.Batch;

namespace FxferService.Infrastructure.Queries;

public abstract class QueryExecutorBase<TResult, TQuery>
    where TQuery : Query<TResult>
{
    public TQuery Execute(IDbContextBase databaseContext, TQuery query)
    {
        IQueryable<TResult> baseQuery = GetBaseQuery(databaseContext, query);
        baseQuery = ApplyWhereClause(baseQuery, query);

        if (!query.ByPassCountQuery)
        {
            query.RowCount = baseQuery.Count();
        }

        baseQuery = ApplySorting(baseQuery, query);
        baseQuery = ApplyPaging(baseQuery, query);
        AssignResults(baseQuery, query);
        return query;
    }

    protected virtual IQueryable<TResult> ApplyPaging(IQueryable<TResult> baseQuery, TQuery query)
    {
        if (query.PageSize <= 0)
        {
            return baseQuery;
        }

        var pageNumber = query.CurrentPage <= 0 ? 1 : query.CurrentPage;
        var skip = (pageNumber - 1) * query.PageSize;
        return baseQuery.Skip(skip).Take(query.PageSize);
    }

    protected virtual IQueryable<TResult> ApplySorting(IQueryable<TResult> baseQuery, TQuery query) => baseQuery;

    protected virtual IQueryable<TResult> ApplyWhereClause(IQueryable<TResult> baseQuery, TQuery query) => baseQuery;

    protected abstract void AssignResults(IQueryable<TResult> baseQuery, TQuery query);

    protected abstract IQueryable<TResult> GetBaseQuery(IDbContextBase databaseContext, TQuery query);

    protected DynamicSortBuilder<TResult> DynamicSort() => new();

    protected DynamicWhereBuilder<TQuery, TResult> DynamicWhere() => new();
}


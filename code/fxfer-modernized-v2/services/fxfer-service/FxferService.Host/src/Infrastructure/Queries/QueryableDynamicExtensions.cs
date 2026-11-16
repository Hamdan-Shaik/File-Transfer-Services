using System.Linq.Expressions;
using System.Reflection;

using FxferService.Application.Contracts.Queries;

namespace FxferService.Infrastructure.Queries;

internal static class QueryableDynamicExtensions
{
    private static readonly MethodInfo QueryableOrderBy = GetQueryableMethod(nameof(Queryable.OrderBy));
    private static readonly MethodInfo QueryableOrderByDescending = GetQueryableMethod(nameof(Queryable.OrderByDescending));
    private static readonly MethodInfo QueryableThenBy = GetQueryableMethod(nameof(Queryable.ThenBy));
    private static readonly MethodInfo QueryableThenByDescending = GetQueryableMethod(nameof(Queryable.ThenByDescending));

    public static IQueryable<TResult> Where<TQuery, TResult>(
        this IQueryable<TResult> source,
        IDynamicWhereBuilder<TQuery, TResult> whereBuilder,
        TQuery query)
    {
        foreach (var clause in whereBuilder.Clauses)
        {
            if (clause.When(query))
            {
                source = source.Where(clause.Predicate);
            }
        }

        return source;
    }

    public static IQueryable<T> OrderBy<T>(
        this IQueryable<T> source,
        IDynamicSorter<T> sorter,
        List<SortCriteria>? sortCriteria)
    {
        if (sortCriteria == null || sortCriteria.Count == 0)
        {
            return ApplyDefaultSorts(source, sorter);
        }

        IOrderedQueryable<T>? ordered = null;
        foreach (SortCriteria criteria in sortCriteria)
        {
            if (string.IsNullOrWhiteSpace(criteria.FieldName))
            {
                continue;
            }

            if (!sorter.SortMap.TryGetValue(criteria.FieldName, out var keySelector))
            {
                continue;
            }

            var descending = criteria.Order == SortOrder.Desc;
            ordered = ordered == null
                ? ApplyOrder(source, keySelector, descending)
                : ApplyThenBy(ordered, keySelector, descending);
        }

        return ordered ?? ApplyDefaultSorts(source, sorter);
    }

    private static IQueryable<T> ApplyDefaultSorts<T>(IQueryable<T> source, IDynamicSorter<T> sorter)
    {
        IOrderedQueryable<T>? ordered = null;
        foreach (var sort in sorter.DefaultSorts)
        {
            ordered = ordered == null
                ? ApplyOrder(source, sort.KeySelector, sort.Descending)
                : ApplyThenBy(ordered, sort.KeySelector, sort.Descending);
        }

        return ordered ?? source;
    }

    private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, LambdaExpression keySelector, bool descending)
    {
        var method = (descending ? QueryableOrderByDescending : QueryableOrderBy)
            .MakeGenericMethod(typeof(T), keySelector.ReturnType);

        var call = Expression.Call(null, method, source.Expression, Expression.Quote(keySelector));
        return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
    }

    private static IOrderedQueryable<T> ApplyThenBy<T>(IOrderedQueryable<T> source, LambdaExpression keySelector, bool descending)
    {
        var method = (descending ? QueryableThenByDescending : QueryableThenBy)
            .MakeGenericMethod(typeof(T), keySelector.ReturnType);

        var call = Expression.Call(null, method, source.Expression, Expression.Quote(keySelector));
        return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
    }

    private static MethodInfo GetQueryableMethod(string name)
    {
        return typeof(Queryable)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(m => m.Name == name && m.GetParameters().Length == 2);
    }
}


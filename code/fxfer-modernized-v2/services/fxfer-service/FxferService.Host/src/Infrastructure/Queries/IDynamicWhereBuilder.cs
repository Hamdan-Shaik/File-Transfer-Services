using System.Linq.Expressions;

namespace FxferService.Infrastructure.Queries;

public interface IDynamicWhereBuilder<TQuery, TResult>
{
    IReadOnlyList<(Func<TQuery, bool> When, Expression<Func<TResult, bool>> Predicate)> Clauses { get; }
}


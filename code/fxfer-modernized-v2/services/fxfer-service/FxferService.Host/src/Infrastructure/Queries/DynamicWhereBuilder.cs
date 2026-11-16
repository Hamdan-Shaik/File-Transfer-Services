using System.Linq.Expressions;

namespace FxferService.Infrastructure.Queries;

public sealed class DynamicWhereBuilder<TQuery, TResult> : IDynamicWhereBuilder<TQuery, TResult>
{
    private readonly List<(Func<TQuery, bool> When, Expression<Func<TResult, bool>> Predicate)> _clauses = new();
    private Func<TQuery, bool>? _currentCondition;

    public DynamicWhereBuilder<TQuery, TResult> When(Func<TQuery, bool> condition)
    {
        _currentCondition = condition;
        return this;
    }

    public DynamicWhereBuilder<TQuery, TResult> AddWhere(Expression<Func<TResult, bool>> predicate)
    {
        _clauses.Add((_currentCondition!, predicate));
        return this;
    }

    public IDynamicWhereBuilder<TQuery, TResult> Build() => this;

    public IReadOnlyList<(Func<TQuery, bool> When, Expression<Func<TResult, bool>> Predicate)> Clauses => _clauses;
}

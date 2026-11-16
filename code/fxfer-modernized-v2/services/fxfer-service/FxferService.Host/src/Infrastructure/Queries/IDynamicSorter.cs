using System.Linq.Expressions;

namespace FxferService.Infrastructure.Queries;

public interface IDynamicSorter<T>
{
    IReadOnlyDictionary<string, LambdaExpression> SortMap { get; }

    IReadOnlyList<(LambdaExpression KeySelector, bool Descending)> DefaultSorts { get; }
}


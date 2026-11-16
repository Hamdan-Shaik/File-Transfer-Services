using System.Linq.Expressions;

namespace FxferService.Infrastructure.Queries;

public sealed class DynamicSortBuilder<T>
{
    private readonly Dictionary<string, LambdaExpression> _sortMap = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<(LambdaExpression KeySelector, bool Descending)> _defaultSorts = new();

    public DynamicSortFieldBuilder<T, TKey> SortUsing<TKey>(Expression<Func<T, TKey>> keySelector)
    {
        return new DynamicSortFieldBuilder<T, TKey>(this, keySelector);
    }

    public DynamicSortBuilder<T> WithDefaults(Action<DefaultSortBuilder> configure)
    {
        configure(new DefaultSortBuilder(_defaultSorts));
        return this;
    }

    public IDynamicSorter<T> Build()
    {
        return new DynamicSorter<T>(_sortMap, _defaultSorts);
    }

    internal void AddSort(string fieldName, LambdaExpression keySelector)
    {
        _sortMap[fieldName] = keySelector;
    }

    public sealed class DefaultSortBuilder
    {
        private readonly List<(LambdaExpression KeySelector, bool Descending)> _defaultSorts;

        public DefaultSortBuilder(List<(LambdaExpression KeySelector, bool Descending)> defaultSorts)
        {
            _defaultSorts = defaultSorts;
        }

        public DefaultSortThenBuilder ByAscending<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            _defaultSorts.Add((keySelector, Descending: false));
            return new DefaultSortThenBuilder(_defaultSorts);
        }

        public DefaultSortThenBuilder ByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            _defaultSorts.Add((keySelector, Descending: true));
            return new DefaultSortThenBuilder(_defaultSorts);
        }
    }

    public sealed class DefaultSortThenBuilder
    {
        private readonly List<(LambdaExpression KeySelector, bool Descending)> _defaultSorts;

        public DefaultSortThenBuilder(List<(LambdaExpression KeySelector, bool Descending)> defaultSorts)
        {
            _defaultSorts = defaultSorts;
        }

        public DefaultSortThenBuilder ThenByAscending<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            _defaultSorts.Add((keySelector, Descending: false));
            return this;
        }

        public DefaultSortThenBuilder ThenByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            _defaultSorts.Add((keySelector, Descending: true));
            return this;
        }
    }

    public sealed class DynamicSortFieldBuilder<TRoot, TKey>
    {
        private readonly DynamicSortBuilder<TRoot> _parent;
        private readonly Expression<Func<TRoot, TKey>> _keySelector;

        public DynamicSortFieldBuilder(DynamicSortBuilder<TRoot> parent, Expression<Func<TRoot, TKey>> keySelector)
        {
            _parent = parent;
            _keySelector = keySelector;
        }

        public DynamicSortBuilder<TRoot> ForValue(string fieldName)
        {
            _parent.AddSort(fieldName, _keySelector);
            return _parent;
        }
    }

    private sealed class DynamicSorter<TItem> : IDynamicSorter<TItem>
    {
        public DynamicSorter(
            IReadOnlyDictionary<string, LambdaExpression> sortMap,
            IReadOnlyList<(LambdaExpression KeySelector, bool Descending)> defaultSorts)
        {
            SortMap = sortMap;
            DefaultSorts = defaultSorts;
        }

        public IReadOnlyDictionary<string, LambdaExpression> SortMap { get; }

        public IReadOnlyList<(LambdaExpression KeySelector, bool Descending)> DefaultSorts { get; }
    }
}



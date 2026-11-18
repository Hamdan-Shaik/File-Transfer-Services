namespace Fxfer.Bff.Contracts.Common;

public sealed record PagedResult<T>
{
    public required IReadOnlyList<T> Items { get; init; }
    public long Total { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}


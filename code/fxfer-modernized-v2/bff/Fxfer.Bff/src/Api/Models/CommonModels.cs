namespace Fxfer.Bff.Api.Models;

public sealed record PagedResult<T>(
    T[] Items,
    int TotalCount,
    int Page,
    int PageSize);

public sealed record SelectionItem(Guid Id, string DisplayName);

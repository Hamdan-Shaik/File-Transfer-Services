namespace FxferService.Api.Queries.Models;

public sealed record PagedResult<T>(
    T[] Items,
    int RowCount,
    int CurrentPage,
    int PageSize);


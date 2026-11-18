namespace Fxfer.Bff.Application.Providers;

public sealed record SortCriteriaParam
{
    public required string SortField { get; init; }
    public SortDirectionType SortDirection { get; init; } = SortDirectionType.Ascending;
}


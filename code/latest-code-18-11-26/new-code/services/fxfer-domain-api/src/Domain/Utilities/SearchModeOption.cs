namespace Fxfer.DomainApi.Contracts.Utilities;

public sealed class SearchModeOption
{
    public SearchModeOption()
    {
        FilterType = SearchMode.EqualTo;
    }

    public enum SearchMode
    {
        Contains,
        EndsWith,
        EqualTo,
        StartsWith
    }

    public string? FieldName { get; set; }

    public SearchMode FilterType { get; set; }
}


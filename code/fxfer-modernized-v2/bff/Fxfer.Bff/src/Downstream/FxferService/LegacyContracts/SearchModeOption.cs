namespace FileTransferSvcQueryService;

public sealed class SearchModeOption
{
    public enum SearchMode
    {
        EqualTo,
        Contains,
        StartsWith,
        EndsWith,
    }

    public string? FieldName { get; set; }
    public SearchMode FilterType { get; set; }
}


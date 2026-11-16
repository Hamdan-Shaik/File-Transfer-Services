namespace FxferService.Domain.Utilities;

public sealed class SearchModeOption
{
    #region Constructors
    public SearchModeOption()
    {
        FilterType = SearchMode.EqualTo;
    }
    #endregion

    #region Enumeration
    public enum SearchMode
    {
        Contains,
        EndsWith,
        EqualTo,
        StartsWith,
    }
    #endregion

    #region Properties
    public string FieldName { get; set; } = null!;

    public SearchMode FilterType { get; set; }
    #endregion
}

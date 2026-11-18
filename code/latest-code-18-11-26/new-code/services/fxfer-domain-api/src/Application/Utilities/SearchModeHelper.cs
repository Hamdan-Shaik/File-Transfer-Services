using System.Collections.Generic;

namespace Fxfer.DomainApi.Contracts.Utilities;

public static class SearchModeHelper
{
    public static string GetSearchMode(string fieldName, List<SearchModeOption> searchOptions)
    {
        return GetSearchModeValue(fieldName, searchOptions).ToString();
    }

    public static SearchModeOption.SearchMode GetSearchModeValue(string fieldName, List<SearchModeOption> searchOptions)
    {
        SearchModeOption? searchModeOption = searchOptions != null && searchOptions.Count > 0
            ? searchOptions.Find(field => field.FieldName == fieldName)
            : null;

        return searchModeOption != null
            ? searchModeOption.FilterType
            : SearchModeOption.SearchMode.EqualTo;
    }
}


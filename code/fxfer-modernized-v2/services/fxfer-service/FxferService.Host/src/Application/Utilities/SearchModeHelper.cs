using FxferService.Domain.Utilities;

namespace FxferService.Application.Utilities;

public static class SearchModeHelper
{
    public static string GetSearchMode(string fieldName, IReadOnlyList<SearchModeOption>? searchOptions)
    {
        return GetSearchModeValue(fieldName, searchOptions).ToString();
    }

    public static SearchModeOption.SearchMode GetSearchModeValue(string fieldName, IReadOnlyList<SearchModeOption>? searchOptions)
    {
        // F01-010: default to equality when no per-field option exists.
        // F01-011: default to equality when no per-field option exists.
        // F01-012: default to equality when no per-field option exists.
        // F01-030: default to equality when no per-field option exists.
        SearchModeOption? searchModeOption = null;
        if (searchOptions != null && searchOptions.Count > 0)
        {
            foreach (SearchModeOption option in searchOptions)
            {
                if (option.FieldName == fieldName)
                {
                    searchModeOption = option;
                    break;
                }
            }
        }

        return searchModeOption != null ? searchModeOption.FilterType : SearchModeOption.SearchMode.EqualTo;
    }

    public static string NormalizeCaseInsensitive(string value, bool trim)
    {
        return trim ? value.ToUpperInvariant().Trim() : value.ToUpperInvariant();
    }
}


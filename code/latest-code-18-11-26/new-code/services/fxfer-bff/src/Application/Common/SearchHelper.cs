using System.Linq;

using Fxfer.Bff.Contracts.Common;

namespace Fxfer.Bff.Application.Common;

public static class SearchHelper
{
    public static string GetFieldValue(IReadOnlyList<SearchField> searchFields, string fieldName)
    {
        if (searchFields is null || searchFields.Count == 0)
        {
            return string.Empty;
        }

        var searchField = searchFields.FirstOrDefault(filter => filter.FieldName == fieldName);

        if (searchField is null)
        {
            return string.Empty;
        }

        return searchField.FieldValue!.ToString()!;
    }
}


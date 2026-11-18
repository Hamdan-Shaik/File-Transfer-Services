using Fxfer.Bff.Application.Common;
using Fxfer.Bff.Contracts.Common;

namespace Fxfer.Bff.Tests;

public sealed class SearchHelperTests
{
    [Fact]
    public void F02_042_missing_search_fields_are_treated_as_empty_string()
    {
        Assert.Equal(string.Empty, SearchHelper.GetFieldValue(null!, "FtpServerName"));
        Assert.Equal(string.Empty, SearchHelper.GetFieldValue(Array.Empty<SearchField>(), "FtpServerName"));
    }

    [Fact]
    public void F02_044_returns_first_matching_field_value_as_string()
    {
        var searchFields = new[]
        {
            new SearchField { FieldName = "name", FieldValue = 123 },
            new SearchField { FieldName = "name", FieldValue = 456 },
        };

        Assert.Equal("123", SearchHelper.GetFieldValue(searchFields, "name"));
    }

    [Fact]
    public void F02_044_missing_matching_field_returns_empty_string()
    {
        var searchFields = new[]
        {
            new SearchField { FieldName = "other", FieldValue = "x" },
        };

        Assert.Equal(string.Empty, SearchHelper.GetFieldValue(searchFields, "name"));
    }
}


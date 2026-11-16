using FxferService.Domain.Constants;
using FxferService.Application.Utilities;
using FxferService.Domain.Utilities;
using Xunit;

namespace FxferService.Tests;

public sealed class SearchModeHelperRuleTests
{
    [Fact]
    public void F01_010_returns_configured_search_mode_for_ftp_server_name()
    {
        Assert.Equal(
            SearchModeOption.SearchMode.EqualTo,
            SearchModeHelper.GetSearchModeValue(SortFields.FtpServer.FtpServerName, null));

        var searchOptions = new List<SearchModeOption>
        {
            new()
            {
                FieldName = SortFields.FtpServer.FtpServerName,
                FilterType = SearchModeOption.SearchMode.Contains,
            },
        };

        Assert.Equal(
            SearchModeOption.SearchMode.Contains,
            SearchModeHelper.GetSearchModeValue(SortFields.FtpServer.FtpServerName, searchOptions));
    }

    [Fact]
    public void F01_011_returns_configured_search_mode_for_ftp_description()
    {
        var searchOptions = new List<SearchModeOption>
        {
            new()
            {
                FieldName = SortFields.FtpServer.FtpDescription,
                FilterType = SearchModeOption.SearchMode.StartsWith,
            },
        };

        Assert.Equal(
            SearchModeOption.SearchMode.StartsWith,
            SearchModeHelper.GetSearchModeValue(SortFields.FtpServer.FtpDescription, searchOptions));
    }

    [Fact]
    public void F01_012_returns_configured_search_mode_for_ftp_user()
    {
        var searchOptions = new List<SearchModeOption>
        {
            new()
            {
                FieldName = SortFields.FtpServer.FtpUser,
                FilterType = SearchModeOption.SearchMode.EndsWith,
            },
        };

        Assert.Equal(
            SearchModeOption.SearchMode.EndsWith,
            SearchModeHelper.GetSearchModeValue(SortFields.FtpServer.FtpUser, searchOptions));
    }

    [Fact]
    public void F01_030_normalizes_case_insensitive_values_with_optional_trimming()
    {
        Assert.Equal("ABC", SearchModeHelper.NormalizeCaseInsensitive("  Abc ", trim: true));
        Assert.Equal("  ABC ", SearchModeHelper.NormalizeCaseInsensitive("  Abc ", trim: false));
    }
}


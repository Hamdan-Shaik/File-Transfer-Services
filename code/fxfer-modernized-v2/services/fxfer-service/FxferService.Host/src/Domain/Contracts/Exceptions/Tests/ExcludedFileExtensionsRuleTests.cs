using FxferService.Domain.Contracts.Exceptions;
using Xunit;

namespace FxferService.Tests;

public sealed class ExcludedFileExtensionsRuleTests
{
    [Fact]
    public void F02_034_uses_expected_excluded_extension_list()
    {
        Assert.Equal(
            new[]
            {
                "exe",
                "dll",
                "bat",
                "cmd",
                "vbs",
                "ps1",
                "jar",
                "app",
                "com",
                "scr",
                "msi",
                "js",
                "wsf",
                "reg",
                "hta",
                "pif",
                "vb",
                "py",
                "rb",
                "sh",
                "cpl",
                "msc",
            },
            Enum.GetNames(typeof(ExcludedFileExtensions.ExcludedFileExtensionList)));
    }

    [Fact]
    public void F02_051_uses_expected_excluded_extension_list()
    {
        Assert.Equal(
            new[]
            {
                "exe",
                "dll",
                "bat",
                "cmd",
                "vbs",
                "ps1",
                "jar",
                "app",
                "com",
                "scr",
                "msi",
                "js",
                "wsf",
                "reg",
                "hta",
                "pif",
                "vb",
                "py",
                "rb",
                "sh",
                "cpl",
                "msc",
            },
            Enum.GetNames(typeof(ExcludedFileExtensions.ExcludedFileExtensionList)));
    }
}


using FxferService.Domain.Contracts.Queries.Parameters;
using Xunit;

namespace FxferService.Tests;

public sealed class SourceLocationQueryParmsRuleTests
{
    [Fact]
    public void F02_010_parses_owner_subsystem_as_guid()
    {
        var expected = Guid.NewGuid();

        var parms = new SourceLocationQueryParms
        {
            OwnerSubsystem = expected.ToString(),
        };

        Assert.Equal(expected, parms.OwnerModuleGuid);
    }

    [Fact]
    public void F02_010_throws_for_invalid_owner_subsystem_guid()
    {
        var parms = new SourceLocationQueryParms
        {
            OwnerSubsystem = "not-a-guid",
        };

        Assert.Throws<FormatException>(() => _ = parms.OwnerModuleGuid);
    }
}


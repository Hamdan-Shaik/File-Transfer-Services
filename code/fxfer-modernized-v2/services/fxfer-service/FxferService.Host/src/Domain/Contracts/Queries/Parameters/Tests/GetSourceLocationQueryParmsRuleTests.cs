using FxferService.Domain.Contracts.Queries.Parameters;
using Xunit;

namespace FxferService.Tests;

public sealed class GetSourceLocationQueryParmsRuleTests
{
    [Fact]
    public void F02_013_parses_source_location_id_as_guid()
    {
        var expected = Guid.NewGuid();

        var parms = new GetSourceLocationQueryParms
        {
            SourceLocationId = expected.ToString(),
        };

        Assert.Equal(expected, parms.SoureceLocationGuid);
    }

    [Fact]
    public void F02_013_throws_for_invalid_source_location_id_guid()
    {
        var parms = new GetSourceLocationQueryParms
        {
            SourceLocationId = "not-a-guid",
        };

        Assert.Throws<FormatException>(() => _ = parms.SoureceLocationGuid);
    }
}


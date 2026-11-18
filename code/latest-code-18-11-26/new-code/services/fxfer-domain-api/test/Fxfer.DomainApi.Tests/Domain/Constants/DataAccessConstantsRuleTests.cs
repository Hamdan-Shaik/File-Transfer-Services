using Fxfer.DomainApi.Domain.Constants;

namespace Fxfer.DomainApi.Tests.Domain.Constants;

public sealed class DataAccessConstantsRuleTests
{
    [Fact]
    public void data_access_constants_match_legacy()
    {
        Assert.Equal("DefaultConnection", DataAccessConstants.DefaultConnection);
        Assert.Equal("DefaultConnectionProvider", DataAccessConstants.DefaultConnectionProvider);
        Assert.Equal("DefaultConnectionString", DataAccessConstants.DefaultConnectionString);

        Assert.Equal("1", DataAccessConstants.FileStatusConstant.Completed);
        Assert.Equal("2", DataAccessConstants.FileStatusConstant.Error);
        Assert.Equal("3", DataAccessConstants.FileStatusConstant.Transfering);
        Assert.Equal("3", DataAccessConstants.FileStatusConstant.UNCProtocolCD);
    }
}



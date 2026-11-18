using Fxfer.DomainApi.Contracts.Exceptions;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Contracts.Queries.Parameters;

namespace Fxfer.DomainApi.Tests.Contracts.Queries;

public sealed class GetSourceLocationQueryTests
{
    [Fact]
    public void F01_100_throws_when_Where_is_null()
    {
        var query = new GetSourceLocationQuery
        {
            Where = null,
        };

        var ex = Assert.Throws<FileTransferValidationException>(() => query.Validate());

        Assert.Equal("FtpSoureceLocationId", ex.ValidationErrorMessage);
    }

    [Fact]
    public void F01_100_throws_when_SourceLocationId_is_null_or_empty()
    {
        var nullIdQuery = new GetSourceLocationQuery
        {
            Where = new GetSourceLocationQueryParms { SourceLocationId = null },
        };

        var nullEx = Assert.Throws<FileTransferValidationException>(() => nullIdQuery.Validate());
        Assert.Equal("FtpSoureceLocationId", nullEx.ValidationErrorMessage);

        var emptyIdQuery = new GetSourceLocationQuery
        {
            Where = new GetSourceLocationQueryParms { SourceLocationId = string.Empty },
        };

        var emptyEx = Assert.Throws<FileTransferValidationException>(() => emptyIdQuery.Validate());
        Assert.Equal("FtpSoureceLocationId", emptyEx.ValidationErrorMessage);
    }

    [Fact]
    public void F01_100_does_not_throw_when_SourceLocationId_is_non_empty()
    {
        var query = new GetSourceLocationQuery
        {
            Where = new GetSourceLocationQueryParms { SourceLocationId = "not-a-guid-but-non-empty" },
        };

        query.Validate();
    }
}

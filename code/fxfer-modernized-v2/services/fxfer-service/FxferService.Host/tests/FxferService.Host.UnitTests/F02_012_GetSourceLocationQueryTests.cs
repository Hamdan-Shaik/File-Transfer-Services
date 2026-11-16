using FxferService.Domain.Contracts.Exceptions;
using FxferService.Domain.Contracts.Queries;
using FxferService.Domain.Contracts.Queries.Parameters;
using Xunit;

namespace FxferService.Host.UnitTests;

public sealed class F02_012_GetSourceLocationQueryTests
{
    [Fact]
    public void F02_012_rejects_an_empty_source_location_id()
    {
        var query = new GetSourceLocationQuery
        {
            Where = new GetSourceLocationQueryParms
            {
                SourceLocationId = string.Empty,
            },
        };

        var ex = Assert.Throws<FileTransferValidationException>(() => query.Validate());
        Assert.Equal(FileTransferExceptionConstants.Validation.FtpSoureceLocationId, ex.ValidationErrorMessage);
    }

    [Fact]
    public void F02_012_rejects_a_null_source_location_id()
    {
        var query = new GetSourceLocationQuery
        {
            Where = new GetSourceLocationQueryParms
            {
                SourceLocationId = null!,
            },
        };

        var ex = Assert.Throws<FileTransferValidationException>(() => query.Validate());
        Assert.Equal(FileTransferExceptionConstants.Validation.FtpSoureceLocationId, ex.ValidationErrorMessage);
    }
}

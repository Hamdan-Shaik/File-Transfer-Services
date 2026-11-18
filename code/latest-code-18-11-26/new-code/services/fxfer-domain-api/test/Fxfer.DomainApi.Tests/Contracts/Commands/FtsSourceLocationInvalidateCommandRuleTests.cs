using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Exceptions;

namespace Fxfer.DomainApi.Tests.Contracts.Commands;

public sealed class FtsSourceLocationInvalidateCommandRuleTests
{
    [Fact]
    public void F01_066_invalidating_a_source_location_requires_operator_id()
    {
        FtsSourceLocationInvalidateCommand command = new();

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => command.Validate());
        Assert.Equal(FileTransferExceptionConstants.Validation.OperatorId, exception.ParamName);
    }

    [Fact]
    public void F01_067_invalidating_a_source_location_requires_source_location_id()
    {
        FtsSourceLocationInvalidateCommand command = new() { OperatorId = "op" };

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => command.Validate());
        Assert.Equal(FileTransferExceptionConstants.Validation.FtpSoureceLocationId, exception.ParamName);
    }

    [Fact]
    public void F01_068_invalidating_a_source_location_requires_config_id()
    {
        FtsSourceLocationInvalidateCommand command = new() { OperatorId = "op", FtsSourceLocationId = "id" };

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => command.Validate());
        Assert.Equal(FileTransferExceptionConstants.Validation.ConfigIdNotFound, exception.ParamName);
    }
}


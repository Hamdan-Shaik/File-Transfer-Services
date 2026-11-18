using Fxfer.TransferWorker.Contracts.Batch;
using Fxfer.TransferWorker.Contracts.Exceptions;
using Fxfer.TransferWorker.Domain.Constants;

namespace Fxfer.TransferWorker.Tests;

public sealed class BackendFileTransferMgmtCtrlTests
{
    [Fact]
    public void UpdateIntegrationFileCommand_validate_throws_business_exception_when_config_id_missing()
    {
        var command = new UpdateIntegrationFileCommand
        {
            ConfigId = null,
        };

        var exception = Assert.Throws<BusinessException>(command.Validate);

        Assert.Equal(CommonConstants.Validation.Messages.CommandError, exception.Message);
        Assert.Equal(CommonConstants.Validation.BusinessErrorCode, exception.BusinessErrorCode);

        Assert.Contains(
            exception.Validations,
            msg => msg.Name == FileTransferExceptionConstants.Validation.ConfigId
                && msg.KeyName == FileTransferExceptionConstants.Validation.ConfigId
                && msg.Message == CommonConstants.Validation.Messages.MissingConfigId);
    }

    [Fact]
    public void UpdateIntegrationFileCommand_validate_allows_non_null_config_id()
    {
        var command = new UpdateIntegrationFileCommand
        {
            ConfigId = 123,
        };

        command.Validate();
    }
}


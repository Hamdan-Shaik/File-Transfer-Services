using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Exceptions;
using Fxfer.DomainApi.Domain.Constants;
using HPP.Core.BAS.CQRS.Base;

namespace Fxfer.DomainApi.Tests.Contracts.Commands;

public sealed class UpdateIntegrationFileCommandRuleTests
{
    [Fact]
    public void F03_010_missing_config_id_throws_business_exception_with_validation_entry()
    {
        UpdateIntegrationFileCommand command = new()
        {
            FtsIntegrationFileId = Guid.NewGuid(),
            ConfigId = null,
        };

        BusinessException exception = Assert.Throws<BusinessException>(() => command.Validate());
        object validation = BusinessExceptionTestHelpers.GetBusinessMessages(exception).Single();
        IReadOnlyList<string> values = BusinessExceptionTestHelpers.GetStringValues(validation);

        Assert.Equal(CommonConstants.Validation.Messages.CommandError, exception.Message);
        Assert.Equal(CommonConstants.Validation.BusinessErrorCode, BusinessExceptionTestHelpers.GetBusinessErrorCode(exception));
        Assert.Contains(FileTransferExceptionConstants.Validation.ConfigId, values);
        Assert.Contains(CommonConstants.Validation.Messages.MissingConfigId, values);
    }

    [Fact]
    public void F03_010_config_id_present_does_not_throw()
    {
        UpdateIntegrationFileCommand command = new()
        {
            FtsIntegrationFileId = Guid.NewGuid(),
            ConfigId = 123,
        };

        command.Validate();
    }
}


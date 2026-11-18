using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Domain.Constants;
using HPP.Core.BAS.CQRS.Base;

namespace Fxfer.DomainApi.Tests.Contracts.Commands;

public sealed class FtsCommandRuleTests
{
    [Fact]
    public void F01_038_each_validation_run_starts_with_an_empty_validations_list()
    {
        FtsCommand command = new();
        command.FtsCommandValidate();

        Assert.NotNull(command.Validations);
        Assert.Empty(command.Validations);
    }

    [Fact]
    public void F01_040_validations_use_the_same_key_and_field_value()
    {
        FtsCommand command = new();
        command.FtsCommandValidate();
        command.AddValidation("Key", "Message");

        object validation = command.Validations.Single();
        IReadOnlyList<string> values = BusinessExceptionTestHelpers.GetStringValues(validation);

        Assert.Equal(2, values.Count(value => value == "Key"));
        Assert.Contains("Message", values);
    }

    [Fact]
    public void F01_039_handle_validations_throws_business_exception_when_any_validation_exists()
    {
        FtsCommand command = new();
        command.FtsCommandValidate();
        command.AddValidation("Key", "Message");

        BusinessException exception = Assert.Throws<BusinessException>(() => command.HandleValidations());

        Assert.Equal(CommonConstants.Validation.Messages.CommandError, exception.Message);
        Assert.Equal(CommonConstants.Validation.BusinessErrorCode, BusinessExceptionTestHelpers.GetBusinessErrorCode(exception));
    }
}


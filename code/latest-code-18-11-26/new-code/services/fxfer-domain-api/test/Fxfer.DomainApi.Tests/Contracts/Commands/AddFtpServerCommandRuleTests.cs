using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Domain.Constants;
using HPP.Core.BAS.CQRS.Base;

namespace Fxfer.DomainApi.Tests.Contracts.Commands;

public sealed class AddFtpServerCommandRuleTests
{
    [Fact]
    public void F02_026_missing_required_fields_add_one_validation_per_property()
    {
        AddFtpServerCommand command = CreateValidCommand();
        command.FtpServerName = null;

        BusinessException exception = Assert.Throws<BusinessException>(() => command.Validate());
        object validation = BusinessExceptionTestHelpers.GetBusinessMessages(exception).Single();
        IReadOnlyList<string> values = BusinessExceptionTestHelpers.GetStringValues(validation);

        Assert.Contains("FtpServerName", values);
        Assert.Contains(CommonConstants.Validation.ErrorType.RequiredFields, values);
        Assert.Contains("Missing field: FtpServerName", values);
    }

    [Fact]
    public void F02_027_commands_with_validations_throw_business_exception_with_code_and_message()
    {
        AddFtpServerCommand command = CreateValidCommand();
        command.FtpServerName = null;

        BusinessException exception = Assert.Throws<BusinessException>(() => command.Validate());

        Assert.Equal(CommonConstants.Validation.Messages.CommandError, exception.Message);
        Assert.Equal(CommonConstants.Validation.BusinessErrorCode, BusinessExceptionTestHelpers.GetBusinessErrorCode(exception));
    }

    [Fact]
    public void F02_028_add_ftp_server_rejects_empty_guid_ftp_server_id()
    {
        AddFtpServerCommand command = CreateValidCommand();
        command.FtpServerId = Guid.Empty;

        BusinessException exception = Assert.Throws<BusinessException>(() => command.Validate());
        object validation = BusinessExceptionTestHelpers.GetBusinessMessages(exception).Single();
        IReadOnlyList<string> values = BusinessExceptionTestHelpers.GetStringValues(validation);

        Assert.Contains(CommonConstants.Validation.Properties.FtpServerId, values);
        Assert.Contains(CommonConstants.Validation.ErrorType.InvalidFtpServerId, values);
        Assert.Contains("Invalid value for: FtpServerId", values);
    }

    [Fact]
    public void F02_029_add_ftp_server_validates_ftp_server_ip_against_ipv4_pattern()
    {
        AddFtpServerCommand command = CreateValidCommand();
        command.FtpServerIp = "not-an-ip";

        BusinessException exception = Assert.Throws<BusinessException>(() => command.Validate());
        object validation = BusinessExceptionTestHelpers.GetBusinessMessages(exception).Single();
        IReadOnlyList<string> values = BusinessExceptionTestHelpers.GetStringValues(validation);

        Assert.Contains(CommonConstants.Validation.Properties.FtpServerIp, values);
        Assert.Contains(CommonConstants.Validation.ErrorType.InvalidIpAddressFormat, values);
        Assert.Contains(CommonConstants.Validation.Messages.IpAddressInvalidFormat, values);
    }

    [Fact]
    public void F02_030_proxy_port_is_required_and_must_be_positive_when_proxy_address_is_supplied()
    {
        AddFtpServerCommand command = CreateValidCommand();
        command.FtpProxyAddress = "proxy";
        command.FtpProxyPort = null;

        BusinessException exception = Assert.Throws<BusinessException>(() => command.Validate());
        object validation = BusinessExceptionTestHelpers.GetBusinessMessages(exception).Single();
        IReadOnlyList<string> values = BusinessExceptionTestHelpers.GetStringValues(validation);

        Assert.Contains(CommonConstants.Validation.Properties.FtpProxyPort, values);
        Assert.Contains(CommonConstants.Validation.ErrorType.InvalidFtpProxyPort, values);
        Assert.Contains("Invalid value for: FtpProxyPort", values);
    }

    [Fact]
    public void F02_062_add_ftp_server_does_not_validate_ftp_port_is_greater_than_zero()
    {
        AddFtpServerCommand command = CreateValidCommand();
        command.FtpPort = 0;

        command.Validate();
    }

    private static AddFtpServerCommand CreateValidCommand()
    {
        return new AddFtpServerCommand
        {
            FtpServerId = Guid.NewGuid(),
            FtpServerName = "server",
            FtpDescription = "desc",
            FtpServerIp = "1.2.3.4",
            FtpTransferMode = "I",
            FtpUser = "user",
            FtpPassword = "password",
            FtpPort = 21,
            FtpProxyAddress = null,
            FtpProxyPort = null,
        };
    }
}


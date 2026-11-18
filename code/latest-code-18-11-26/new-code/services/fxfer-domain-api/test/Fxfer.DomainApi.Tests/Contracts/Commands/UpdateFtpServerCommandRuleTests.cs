using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Domain.Constants;
using HPP.Core.BAS.CQRS.Base;

namespace Fxfer.DomainApi.Tests.Contracts.Commands;

public sealed class UpdateFtpServerCommandRuleTests
{
    [Fact]
    public void F02_024_update_ftp_server_inherits_add_command_validation_rules()
    {
        UpdateFtpServerCommand command = CreateValidCommand();
        command.FtpServerId = Guid.Empty;

        BusinessException exception = Assert.Throws<BusinessException>(() => command.Validate());
        object validation = BusinessExceptionTestHelpers.GetBusinessMessages(exception).Single();
        IReadOnlyList<string> values = BusinessExceptionTestHelpers.GetStringValues(validation);

        Assert.Contains(CommonConstants.Validation.Properties.FtpServerId, values);
        Assert.Contains(CommonConstants.Validation.ErrorType.InvalidFtpServerId, values);
    }

    [Fact]
    public void F02_034_invalidate_ftp_server_command_is_a_update_command_type()
    {
        Assert.IsType<InvalidateFtpServerCommand>(new InvalidateFtpServerCommand());
        Assert.True(new InvalidateFtpServerCommand() is UpdateFtpServerCommand);
    }

    private static UpdateFtpServerCommand CreateValidCommand()
    {
        return new UpdateFtpServerCommand
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


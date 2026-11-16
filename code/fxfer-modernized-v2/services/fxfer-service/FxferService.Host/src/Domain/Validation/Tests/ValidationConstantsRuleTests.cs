using FxferService.Domain.Validation;
using Xunit;

namespace FxferService.Tests;

public sealed class ValidationConstantsRuleTests
{
    [Fact]
    public void F01_023_uses_expected_command_error_message()
    {
        Assert.Equal("Command Execution Error", Messages.CommandError);
    }

    [Fact]
    public void F01_026_uses_expected_ftp_server_id_validation_constants()
    {
        Assert.Equal("FtpServerId", Properties.FtpServerId);
        Assert.Equal("InvalidFtpServerId", ErrorType.InvalidFtpServerId);
        Assert.Equal("Invalid value for", Messages.InvalidValue);
    }

    [Fact]
    public void F01_027_uses_expected_ip_address_validation_constants()
    {
        Assert.Equal("InvalidIpAddressFormat", ErrorType.InvalidIpAddressFormat);
        Assert.Equal("Invalid Format for IP Address", Messages.IpAddressInvalidFormat);
    }

    [Fact]
    public void F01_028_uses_expected_proxy_port_validation_constants()
    {
        Assert.Equal("FtpProxyPort", Properties.FtpProxyPort);
        Assert.Equal("InvalidFtpProxyPort", ErrorType.InvalidFtpProxyPort);
    }

    [Fact]
    public void F02_029_uses_expected_command_error_message()
    {
        Assert.Equal("Command Execution Error", Messages.CommandError);
    }

    [Fact]
    public void F02_030_uses_expected_missing_field_message()
    {
        Assert.Equal("Missing field", Messages.MissingField);
    }

    [Fact]
    public void F02_031_uses_expected_missing_field_message()
    {
        Assert.Equal("Missing field", Messages.MissingField);
    }

    [Fact]
    public void F02_032_uses_expected_run_schedule_validation_constants()
    {
        Assert.Equal("Run Schedule", Properties.RunSchedule);
        Assert.Equal("Invalid value for", Messages.InvalidValue);
    }

    [Fact]
    public void F02_034_uses_expected_excluded_extension_message_prefix()
    {
        Assert.Equal("Source file mask extension is not Allowed:", Messages.ExcludedExtension);
    }

    [Fact]
    public void F02_045_uses_expected_command_error_message()
    {
        Assert.Equal("Command Execution Error", Messages.CommandError);
    }

    [Fact]
    public void F02_046_uses_expected_missing_field_message()
    {
        Assert.Equal("Missing field", Messages.MissingField);
    }

    [Fact]
    public void F02_047_uses_expected_run_schedule_validation_constants()
    {
        Assert.Equal("Run Schedule", Properties.RunSchedule);
        Assert.Equal("Invalid value for", Messages.InvalidValue);
    }

    [Fact]
    public void F02_051_uses_expected_excluded_extension_message_prefix()
    {
        Assert.Equal("Source file mask extension is not Allowed:", Messages.ExcludedExtension);
    }

    [Fact]
    public void F02_067_uses_expected_missing_config_id_message()
    {
        Assert.Equal("Missing ConfigId", Messages.MissingConfigId);
    }
}


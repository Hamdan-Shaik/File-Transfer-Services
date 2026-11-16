using FxferService.Domain.Constants;
using Xunit;

namespace FxferService.Tests;

public sealed class CommonConstantsRuleTests
{
    [Fact]
    public void F01_023_uses_expected_command_error_and_business_error_code()
    {
        Assert.Equal("Command Execution Error", CommonConstants.Validation.Messages.CommandError);
        Assert.Equal("E", CommonConstants.Validation.BusinessErrorCode);
    }

    [Fact]
    public void F01_026_uses_expected_invalid_ftp_server_id_validation_constants()
    {
        Assert.Equal("FtpServerId", CommonConstants.Validation.Properties.FtpServerId);
        Assert.Equal("InvalidFtpServerId", CommonConstants.Validation.ErrorType.InvalidFtpServerId);
        Assert.Equal("Invalid value for", CommonConstants.Validation.Messages.InvalidValue);
    }

    [Fact]
    public void F01_027_uses_expected_ip_address_validation_constants()
    {
        Assert.Equal(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b", CommonConstants.RegexPatterns.IpAddressPattern);
        Assert.Equal("InvalidIpAddressFormat", CommonConstants.Validation.ErrorType.InvalidIpAddressFormat);
        Assert.Equal("Invalid Format for IP Address", CommonConstants.Validation.Messages.IpAddressInvalidFormat);
    }

    [Fact]
    public void F01_028_uses_expected_proxy_port_validation_constants()
    {
        Assert.Equal("FtpProxyPort", CommonConstants.Validation.Properties.FtpProxyPort);
        Assert.Equal("InvalidFtpProxyPort", CommonConstants.Validation.ErrorType.InvalidFtpProxyPort);
    }

    [Fact]
    public void F02_029_uses_expected_command_error_and_business_error_code()
    {
        Assert.Equal("Command Execution Error", CommonConstants.Validation.Messages.CommandError);
        Assert.Equal("E", CommonConstants.Validation.BusinessErrorCode);
    }

    [Fact]
    public void F02_030_uses_expected_missing_field_validation_message()
    {
        Assert.Equal("Missing field", CommonConstants.Validation.Messages.MissingField);
    }

    [Fact]
    public void F02_031_uses_expected_missing_field_validation_message()
    {
        Assert.Equal("Missing field", CommonConstants.Validation.Messages.MissingField);
    }

    [Fact]
    public void F02_032_uses_expected_run_schedule_validation_constants()
    {
        Assert.Equal("Run Schedule", CommonConstants.Validation.Properties.RunSchedule);
        Assert.Equal("Invalid value for", CommonConstants.Validation.Messages.InvalidValue);
    }

    [Fact]
    public void F02_033_uses_expected_ftp_protocol_code()
    {
        Assert.Equal("1", CommonConstants.FtpServerCode);
    }

    [Fact]
    public void F02_034_uses_expected_excluded_extension_validation_message_prefix()
    {
        Assert.Equal("Source file mask extension is not Allowed:", CommonConstants.Validation.Messages.ExcludedExtension);
    }

    [Fact]
    public void F02_045_uses_expected_command_error_and_business_error_code()
    {
        Assert.Equal("Command Execution Error", CommonConstants.Validation.Messages.CommandError);
        Assert.Equal("E", CommonConstants.Validation.BusinessErrorCode);
    }

    [Fact]
    public void F02_046_uses_expected_missing_field_validation_message()
    {
        Assert.Equal("Missing field", CommonConstants.Validation.Messages.MissingField);
    }

    [Fact]
    public void F02_047_uses_expected_run_schedule_validation_constants()
    {
        Assert.Equal("Run Schedule", CommonConstants.Validation.Properties.RunSchedule);
        Assert.Equal("Invalid value for", CommonConstants.Validation.Messages.InvalidValue);
    }

    [Fact]
    public void F02_051_uses_expected_excluded_extension_validation_message_prefix()
    {
        Assert.Equal("Source file mask extension is not Allowed:", CommonConstants.Validation.Messages.ExcludedExtension);
    }

    [Fact]
    public void F02_067_uses_expected_missing_config_id_validation_message()
    {
        Assert.Equal("Missing ConfigId", CommonConstants.Validation.Messages.MissingConfigId);
    }
}


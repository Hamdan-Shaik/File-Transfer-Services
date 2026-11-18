using Fxfer.DomainApi.Domain.Constants;

namespace Fxfer.DomainApi.Tests.Domain.Constants;

public sealed class CommonConstantsRuleTests
{
    [Fact]
    public void F01_039_business_exception_constants_match_legacy()
    {
        Assert.Equal("E", CommonConstants.Validation.BusinessErrorCode);
        Assert.Equal("Command Execution Error", CommonConstants.Validation.Messages.CommandError);
    }

    [Fact]
    public void F01_041_missing_field_message_matches_legacy()
    {
        Assert.Equal("Missing field", CommonConstants.Validation.Messages.MissingField);
    }

    [Fact]
    public void F01_042_missing_field_message_matches_legacy()
    {
        Assert.Equal("Missing field", CommonConstants.Validation.Messages.MissingField);
    }

    [Fact]
    public void F01_043_run_schedule_validation_constants_match_legacy()
    {
        Assert.Equal("Run Schedule", CommonConstants.Validation.Properties.RunSchedule);
        Assert.Equal("Invalid value for", CommonConstants.Validation.Messages.InvalidValue);
    }

    [Fact]
    public void F01_044_ftp_protocol_code_and_field_name_constants_match_legacy()
    {
        Assert.Equal("1", CommonConstants.FtpServerCode);
        Assert.Equal("FtpServerId", CommonConstants.Validation.Properties.FtpServerId);
    }

    [Fact]
    public void F01_045_excluded_extension_message_prefix_matches_legacy()
    {
        Assert.Equal("Source file mask extension is not Allowed:", CommonConstants.Validation.Messages.ExcludedExtension);
    }

    [Fact]
    public void F01_059_required_field_and_business_exception_constants_match_legacy()
    {
        Assert.Equal("Missing field", CommonConstants.Validation.Messages.MissingField);
        Assert.Equal("E", CommonConstants.Validation.BusinessErrorCode);
        Assert.Equal("Command Execution Error", CommonConstants.Validation.Messages.CommandError);
    }

    [Fact]
    public void F01_060_run_schedule_validation_constants_match_legacy()
    {
        Assert.Equal("Run Schedule", CommonConstants.Validation.Properties.RunSchedule);
        Assert.Equal("Invalid value for", CommonConstants.Validation.Messages.InvalidValue);
    }

    [Fact]
    public void F01_065_excluded_extension_message_prefix_matches_legacy()
    {
        Assert.Equal("Source file mask extension is not Allowed:", CommonConstants.Validation.Messages.ExcludedExtension);
    }

    [Fact]
    public void F02_026_missing_field_message_prefix_matches_legacy()
    {
        Assert.Equal("Missing field", CommonConstants.Validation.Messages.MissingField);
    }

    [Fact]
    public void F02_027_business_exception_constants_match_legacy()
    {
        Assert.Equal("E", CommonConstants.Validation.BusinessErrorCode);
        Assert.Equal("Command Execution Error", CommonConstants.Validation.Messages.CommandError);
    }

    [Fact]
    public void F02_029_ip_address_regex_pattern_matches_legacy()
    {
        Assert.Equal(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b", CommonConstants.RegexPatterns.IpAddressPattern);
    }

    [Fact]
    public void F03_010_missing_config_id_constants_match_legacy()
    {
        Assert.Equal("Missing ConfigId", CommonConstants.Validation.Messages.MissingConfigId);
        Assert.Equal("E", CommonConstants.Validation.BusinessErrorCode);
        Assert.Equal("Command Execution Error", CommonConstants.Validation.Messages.CommandError);
    }
}


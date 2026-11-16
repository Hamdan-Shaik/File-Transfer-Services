using FxferService.Domain.Commands;
using FxferService.Domain.Constants;
using FxferService.Domain.Contracts.Events;
using FxferService.Domain.Contracts.Exceptions;
using Xunit;

namespace FxferService.Tests;

public sealed class SharedContractsCommandsRuleTests
{
    [Fact(DisplayName = "F01-031 default AddFtpServerCommand is active and has a new identifier")]
    public void F01_031_default_constructed_add_ftp_server_command_assigns_new_guid_and_is_valid_true()
    {
        var command = new AddFtpServerCommand();

        Assert.NotEqual(Guid.Empty, command.FtpServerId);
        Assert.True(command.IsValid);
    }

    [Fact]
    public void F01_023_throws_business_exception_with_command_error_and_business_error_code()
    {
        var command = BuildValidAddFtpServerCommand();
        command.FtpServerId = Guid.Empty;

        var exception = Assert.Throws<BusinessException>(() => command.Validate());
        Assert.Equal(CommonConstants.Validation.Messages.CommandError, exception.ErrorType);
        Assert.Equal(CommonConstants.Validation.BusinessErrorCode, exception.ProcessingSystem);
    }

    [Fact]
    public void F01_024_missing_required_field_creates_missing_field_validation_entry()
    {
        var command = BuildValidAddFtpServerCommand();
        command.FtpServerName = null;

        var exception = Assert.Throws<BusinessException>(() => command.Validate());
        var message = Assert.Single(exception.BusinessMessages, m => m.FieldName == nameof(AddFtpServerCommand.FtpServerName));
        Assert.Equal(CommonConstants.Validation.ErrorType.RequiredFields, message.MessageKey);
        Assert.Equal("Missing field: FtpServerName", message.MessageDefault);
    }

    [Fact]
    public void F01_025_set_command_validations_runs_all_three_validation_checks()
    {
        var command = BuildValidAddFtpServerCommand();
        command.FtpServerId = Guid.Empty;
        command.FtpServerIp = "999";
        command.FtpProxyAddress = "proxy";
        command.FtpProxyPort = null;

        var exception = Assert.Throws<BusinessException>(() => command.Validate());

        Assert.Contains(exception.BusinessMessages, m => m.FieldName == CommonConstants.Validation.Properties.FtpServerId);
        Assert.Contains(exception.BusinessMessages, m => m.FieldName == CommonConstants.Validation.Properties.FtpServerIp);
        Assert.Contains(exception.BusinessMessages, m => m.FieldName == CommonConstants.Validation.Properties.FtpProxyPort);
    }

    [Fact]
    public void F01_026_guid_empty_ftp_server_id_adds_invalid_ftp_server_id_validation()
    {
        var command = BuildValidAddFtpServerCommand();
        command.FtpServerId = Guid.Empty;

        var exception = Assert.Throws<BusinessException>(() => command.Validate());
        var message = Assert.Single(exception.BusinessMessages, m => m.FieldName == CommonConstants.Validation.Properties.FtpServerId);
        Assert.Equal(CommonConstants.Validation.ErrorType.InvalidFtpServerId, message.MessageKey);
        Assert.Equal("Invalid value for: FtpServerId", message.MessageDefault);
    }

    [Fact]
    public void F01_027_invalid_ftp_server_ip_adds_invalid_ip_address_format_validation()
    {
        var command = BuildValidAddFtpServerCommand();
        command.FtpServerIp = "not-an-ip";

        var exception = Assert.Throws<BusinessException>(() => command.Validate());
        var message = Assert.Single(exception.BusinessMessages, m => m.FieldName == CommonConstants.Validation.Properties.FtpServerIp);
        Assert.Equal(CommonConstants.Validation.ErrorType.InvalidIpAddressFormat, message.MessageKey);
        Assert.Equal(CommonConstants.Validation.Messages.IpAddressInvalidFormat, message.MessageDefault);
    }

    [Fact]
    public void F01_028_proxy_address_requires_proxy_port_greater_than_zero()
    {
        var command = BuildValidAddFtpServerCommand();
        command.FtpProxyAddress = "proxy";
        command.FtpProxyPort = 0;

        var exception = Assert.Throws<BusinessException>(() => command.Validate());
        var message = Assert.Single(exception.BusinessMessages, m => m.FieldName == CommonConstants.Validation.Properties.FtpProxyPort);
        Assert.Equal(CommonConstants.Validation.ErrorType.InvalidFtpProxyPort, message.MessageKey);
        Assert.Equal("Invalid value for: FtpProxyPort", message.MessageDefault);
    }

    [Fact]
    public void F02_029_source_location_add_validate_throws_business_exception_when_any_validations_exist()
    {
        var command = BuildValidFtsSourceLocationAddCommand();
        command.ConfigName = null;

        var exception = Assert.Throws<BusinessException>(() => command.Validate());
        Assert.Equal(CommonConstants.Validation.Messages.CommandError, exception.ErrorType);
        Assert.Equal(CommonConstants.Validation.BusinessErrorCode, exception.ProcessingSystem);
    }

    [Fact]
    public void F02_030_validate_required_field_adds_missing_field_validation()
    {
        var command = BuildValidFtsSourceLocationAddCommand();
        command.ConfigName = "";

        var exception = Assert.Throws<BusinessException>(() => command.Validate());
        var message = Assert.Single(exception.BusinessMessages, m => m.FieldName == nameof(FtsSourceLocationAddCommand.ConfigName));
        Assert.Equal(nameof(FtsSourceLocationAddCommand.ConfigName), message.MessageKey);
        Assert.Equal(CommonConstants.Validation.Messages.MissingField, message.MessageDefault);
    }

    [Fact]
    public void F02_031_destination_address_required_when_missing_adds_missing_field_validation()
    {
        var command = BuildValidFtsSourceLocationAddCommand();
        command.DestinationAddress = new List<FtsDestinationLocationAdded>();

        var exception = Assert.Throws<BusinessException>(() => command.Validate());
        var message = Assert.Single(exception.BusinessMessages, m => m.FieldName == nameof(FtsSourceLocationAddCommand.DestinationAddress));
        Assert.Equal(CommonConstants.Validation.Messages.MissingField, message.MessageDefault);
    }

    [Fact]
    public void F02_032_missing_run_schedule_adds_run_schedule_invalid_value_validation()
    {
        var command = BuildValidFtsSourceLocationAddCommand();
        command.DestinationAddress = new List<FtsDestinationLocationAdded> { new() { IsValid = true } };
        command.RunOnMonday = false;
        command.RunOnTuesday = false;
        command.RunOnWed = false;
        command.RunOnThrus = false;
        command.RunOnFri = false;
        command.RunOnSat = false;
        command.RunOnSun = false;
        command.RunOnFrstDayOfMonthInd = false;
        command.RunOnLastDayOfMonthInd = false;
        command.RunOnThisDayOfMonth = 0;

        var exception = Assert.Throws<BusinessException>(() => command.Validate());
        var message = Assert.Single(exception.BusinessMessages, m => m.FieldName == CommonConstants.Validation.Properties.RunSchedule);
        Assert.Equal(CommonConstants.Validation.Messages.InvalidValue, message.MessageDefault);
    }

    [Fact]
    public void F02_033_ftp_protocol_requires_ftp_server_id_and_throws_argument_null_exception_when_missing()
    {
        var command = BuildValidFtsSourceLocationAddCommand();
        command.Protocol = CommonConstants.FtpServerCode;
        command.FtpServerId = null;

        var exception = Assert.Throws<ArgumentNullException>(() => command.Validate());
        Assert.Equal(FileTransferExceptionConstants.Validation.FtpServerId, exception.ParamName);
    }

    [Fact]
    public void F02_034_excluded_mask_extension_adds_mask_validation_message_with_extension()
    {
        var command = BuildValidFtsSourceLocationAddCommand();
        command.DestinationAddress = new List<FtsDestinationLocationAdded> { new() { IsValid = true } };
        command.Mask = "file.exe";

        var exception = Assert.Throws<BusinessException>(() => command.Validate());
        var message = Assert.Single(exception.BusinessMessages, m => m.FieldName == nameof(FtsSourceLocationAddCommand.Mask));
        Assert.Equal("Source file mask extension is not Allowed: exe", message.MessageDefault);
    }

    [Fact]
    public void F02_052_invalidate_requires_operator_id()
    {
        var command = new FtsSourceLocationInvalidateCommand
        {
            OperatorId = null,
            FtsSourceLocationId = "id",
            ConfigId = 123,
        };

        var exception = Assert.Throws<ArgumentNullException>(() => command.Validate());
        Assert.Equal(FileTransferExceptionConstants.Validation.OperatorId, exception.ParamName);
    }

    [Fact]
    public void F02_052_invalidate_requires_source_location_id()
    {
        var command = new FtsSourceLocationInvalidateCommand
        {
            OperatorId = "op",
            FtsSourceLocationId = null,
            ConfigId = 123,
        };

        var exception = Assert.Throws<ArgumentNullException>(() => command.Validate());
        Assert.Equal(FileTransferExceptionConstants.Validation.FtpSoureceLocationId, exception.ParamName);
    }

    [Fact]
    public void F02_052_invalidate_requires_non_zero_config_id()
    {
        var command = new FtsSourceLocationInvalidateCommand
        {
            OperatorId = "op",
            FtsSourceLocationId = "id",
            ConfigId = 0,
        };

        var exception = Assert.Throws<ArgumentNullException>(() => command.Validate());
        Assert.Equal(FileTransferExceptionConstants.Validation.ConfigIdNotFound, exception.ParamName);
    }

    [Fact]
    public void F02_056_requires_source_location_id_even_when_config_id_present()
    {
        var command = new FtsSourceLocationInvalidateCommand
        {
            OperatorId = "op",
            FtsSourceLocationId = "",
            ConfigId = 123,
        };

        var exception = Assert.Throws<ArgumentNullException>(() => command.Validate());
        Assert.Equal(FileTransferExceptionConstants.Validation.FtpSoureceLocationId, exception.ParamName);
    }

    [Fact]
    public void F02_045_update_source_location_collects_validations_and_throws_business_exception_when_any_exist()
    {
        var command = BuildValidFtsSourceLocationUpdateCommand();
        command.ConfigName = null;

        var exception = Assert.Throws<BusinessException>(() => command.Validate());
        Assert.Equal(CommonConstants.Validation.Messages.CommandError, exception.ErrorType);
        Assert.Equal(CommonConstants.Validation.BusinessErrorCode, exception.ProcessingSystem);
    }

    [Fact]
    public void F02_046_update_source_location_requires_non_empty_required_fields()
    {
        var command = BuildValidFtsSourceLocationUpdateCommand();
        command.OwnerEmail = string.Empty;

        var exception = Assert.Throws<BusinessException>(() => command.Validate());
        var message = Assert.Single(exception.BusinessMessages, m => m.FieldName == nameof(FtsSourceLocationUpdateCommand.OwnerEmail));
        Assert.Equal(nameof(FtsSourceLocationUpdateCommand.OwnerEmail), message.MessageKey);
        Assert.Equal(CommonConstants.Validation.Messages.MissingField, message.MessageDefault);
    }

    [Fact]
    public void F02_047_update_source_location_requires_at_least_one_run_schedule_choice()
    {
        var command = BuildValidFtsSourceLocationUpdateCommand();
        command.RunOnMonday = false;
        command.RunOnTuesday = false;
        command.RunOnWed = false;
        command.RunOnThrus = false;
        command.RunOnFri = false;
        command.RunOnSat = false;
        command.RunOnSun = false;
        command.RunOnFrstDayOfMonthInd = false;
        command.RunOnLastDayOfMonthInd = false;
        command.RunOnThisDayOfMonth = 0;

        var exception = Assert.Throws<BusinessException>(() => command.Validate());
        var message = Assert.Single(exception.BusinessMessages, m => m.FieldName == CommonConstants.Validation.Properties.RunSchedule);
        Assert.Equal(CommonConstants.Validation.Messages.InvalidValue, message.MessageDefault);
    }

    [Fact]
    public void F02_048_update_source_location_requires_ftp_server_id_when_protocol_is_1()
    {
        var command = BuildValidFtsSourceLocationUpdateCommand();
        command.Protocol = FtsSourceLocationUpdateCommand.FtpServerCode;
        command.FtpServerId = null;

        var exception = Assert.Throws<ArgumentNullException>(() => command.Validate());
        Assert.Equal(FileTransferExceptionConstants.Validation.FtpServerId, exception.ParamName);
    }

    [Fact]
    public void F02_049_update_source_location_requires_non_empty_source_location_id()
    {
        var command = BuildValidFtsSourceLocationUpdateCommand();
        command.FtsSourceLocationId = null;

        var exception = Assert.Throws<ArgumentNullException>(() => command.Validate());
        Assert.Equal(FileTransferExceptionConstants.Validation.FtpSoureceLocationId, exception.ParamName);
    }

    [Fact]
    public void F02_049_update_source_location_requires_non_zero_config_id()
    {
        var command = BuildValidFtsSourceLocationUpdateCommand();
        command.ConfigId = 0;

        var exception = Assert.Throws<ArgumentNullException>(() => command.Validate());
        Assert.Equal(FileTransferExceptionConstants.Validation.ConfigIdNotFound, exception.ParamName);
    }

    [Fact]
    public void F02_050_update_source_location_requires_is_suspended_changed_date_when_suspended()
    {
        var command = BuildValidFtsSourceLocationUpdateCommand();
        command.IsSuspended = true;
        command.IsSuspendedChangedDate = null;

        var exception = Assert.Throws<ArgumentNullException>(() => command.Validate());
        Assert.Equal(FileTransferExceptionConstants.Validation.IsSuspendedChangedDate, exception.ParamName);
    }

    [Fact]
    public void F02_051_update_source_location_rejects_excluded_mask_extensions()
    {
        var command = BuildValidFtsSourceLocationUpdateCommand();
        command.Mask = new string(new[] { 'f', 'i', 'l', 'e', '.', 'e', 'x', 'e' });

        var exception = Assert.Throws<BusinessException>(() => command.Validate());
        var message = Assert.Single(exception.BusinessMessages, m => m.FieldName == nameof(FtsSourceLocationUpdateCommand.Mask));
        var expected = CommonConstants.Validation.Messages.ExcludedExtension + ' ' + ExcludedFileExtensions.ExcludedFileExtensionList.exe;
        Assert.Equal(expected, message.MessageDefault);
    }

    [Fact]
    public void F02_067_update_integration_file_requires_config_id()
    {
        var command = new UpdateIntegrationFileCommand
        {
            ConfigId = null,
        };

        var exception = Assert.Throws<BusinessException>(() => command.Validate());
        Assert.Equal(CommonConstants.Validation.Messages.CommandError, exception.ErrorType);
        Assert.Equal(CommonConstants.Validation.BusinessErrorCode, exception.ProcessingSystem);

        var message = Assert.Single(exception.BusinessMessages);
        Assert.Equal(FileTransferExceptionConstants.Validation.ConfigId, message.FieldName);
        Assert.Equal(FileTransferExceptionConstants.Validation.ConfigId, message.MessageKey);
        Assert.Equal(CommonConstants.Validation.Messages.MissingConfigId, message.MessageDefault);
    }

    private static AddFtpServerCommand BuildValidAddFtpServerCommand()
    {
        return new AddFtpServerCommand
        {
            FtpServerId = Guid.NewGuid(),
            FtpServerName = "name",
            FtpDescription = "desc",
            FtpServerIp = "127.0.0.1",
            FtpTransferMode = "I",
            FtpUser = "user",
            FtpPassword = "pw",
            FtpPort = 21,
            FtpProxyAddress = null,
            FtpProxyPort = null,
            UseSSHAuth = false,
            IsValid = true,
            OperatorId = "op",
        };
    }

    private static FtsSourceLocationAddCommand BuildValidFtsSourceLocationAddCommand()
    {
        return new FtsSourceLocationAddCommand
        {
            ConfigName = "cfg",
            Protocol = "0",
            Address = "addr",
            OwnerName = "owner",
            OwnerModule = "module",
            OwnerPhone = "phone",
            OwnerEmail = "email",
            StartTime = "00:00",
            EndTime = "23:59",
            RunOnMonday = true,
            Mask = "file.txt",
            DestinationAddress = new List<FtsDestinationLocationAdded> { new() { IsValid = true } },
        };
    }

    private static FtsSourceLocationUpdateCommand BuildValidFtsSourceLocationUpdateCommand()
    {
        return new FtsSourceLocationUpdateCommand
        {
            FtsSourceLocationId = FileTransferExceptionConstants.Validation.FtpSoureceLocationId,
            ConfigId = 123,
            ConfigName = FileTransferExceptionConstants.Validation.ConfigName,
            Protocol = CommonConstants.ModuleName,
            Address = FileTransferExceptionConstants.Validation.Address,
            OwnerName = FileTransferExceptionConstants.Validation.OwnerName,
            OwnerModule = FileTransferExceptionConstants.Validation.OwnerSubsystem,
            OwnerPhone = FileTransferExceptionConstants.Validation.OwnerName,
            OwnerEmail = FileTransferExceptionConstants.Validation.OwnerName,
            StartTime = FileTransferExceptionConstants.Validation.OwnerName,
            EndTime = FileTransferExceptionConstants.Validation.OwnerName,
            RunOnMonday = true,
            Mask = FileTransferExceptionConstants.Validation.Mask,
            IsSuspended = false,
        };
    }
}

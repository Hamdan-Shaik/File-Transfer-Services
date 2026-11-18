using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Contracts.Exceptions;
using Fxfer.DomainApi.Domain.Constants;
using HPP.Core.BAS.CQRS.Base;

namespace Fxfer.DomainApi.Tests.Contracts.Commands;

public sealed class FtsSourceLocationAddCommandRuleTests
{
    [Fact]
    public void F01_041_missing_required_string_field_adds_missing_field_validation()
    {
        FtsSourceLocationAddCommand command = CreateValidCommand();
        command.ConfigName = null;

        BusinessException exception = Assert.Throws<BusinessException>(() => command.Validate());
        object validation = BusinessExceptionTestHelpers.GetBusinessMessages(exception).Single();
        IReadOnlyList<string> values = BusinessExceptionTestHelpers.GetStringValues(validation);

        Assert.Contains("ConfigName", values);
        Assert.Contains(CommonConstants.Validation.Messages.MissingField, values);
    }

    [Fact]
    public void F01_042_destination_address_requires_at_least_one_valid_destination()
    {
        FtsSourceLocationAddCommand command = CreateValidCommand();
        command.DestinationAddress = new List<FtsDestinationLocationAdded>();

        BusinessException exception = Assert.Throws<BusinessException>(() => command.Validate());
        object validation = BusinessExceptionTestHelpers.GetBusinessMessages(exception).Single();
        IReadOnlyList<string> values = BusinessExceptionTestHelpers.GetStringValues(validation);

        Assert.Contains(nameof(command.DestinationAddress), values);
        Assert.Contains(CommonConstants.Validation.Messages.MissingField, values);
    }

    [Fact]
    public void F01_043_run_schedule_requires_at_least_one_option_to_be_selected()
    {
        FtsSourceLocationAddCommand command = CreateValidCommand();
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

        BusinessException exception = Assert.Throws<BusinessException>(() => command.Validate());
        object validation = BusinessExceptionTestHelpers.GetBusinessMessages(exception).Single();
        IReadOnlyList<string> values = BusinessExceptionTestHelpers.GetStringValues(validation);

        Assert.Contains(CommonConstants.Validation.Properties.RunSchedule, values);
        Assert.Contains(CommonConstants.Validation.Messages.InvalidValue, values);
    }

    [Fact]
    public void F01_044_protocol_ftp_requires_a_non_empty_ftp_server_id()
    {
        FtsSourceLocationAddCommand command = CreateValidCommand();
        command.Protocol = CommonConstants.FtpServerCode;
        command.FtpServerId = string.Empty;

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => command.Validate());
        Assert.Equal(FileTransferExceptionConstants.Validation.FtpServerId, exception.ParamName);
    }

    [Fact]
    public void F01_045_mask_extension_in_excluded_list_adds_a_mask_validation_message()
    {
        FtsSourceLocationAddCommand command = CreateValidCommand();
        command.Mask = "file.exe";

        BusinessException exception = Assert.Throws<BusinessException>(() => command.Validate());
        object validation = BusinessExceptionTestHelpers.GetBusinessMessages(exception).Single();
        IReadOnlyList<string> values = BusinessExceptionTestHelpers.GetStringValues(validation);

        Assert.Contains(nameof(command.Mask), values);
        Assert.Contains("Source file mask extension is not Allowed: exe", values);
    }

    private static FtsSourceLocationAddCommand CreateValidCommand()
    {
        FtsSourceLocationAddCommand command = new()
        {
            ConfigName = "name",
            Protocol = "2",
            Address = "address",
            OwnerName = "owner",
            OwnerModule = "module",
            OwnerPhone = "555",
            OwnerEmail = "email@example.com",
            StartTime = "09:00",
            EndTime = "17:00",
            RunOnMonday = true,
        };

        command.DestinationAddress.Add(new FtsDestinationLocationAdded { IsValid = true });
        return command;
    }
}


using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Exceptions;
using Fxfer.DomainApi.Domain.Constants;
using HPP.Core.BAS.CQRS.Base;

namespace Fxfer.DomainApi.Tests.Contracts.Commands;

public sealed class FtsSourceLocationUpdateCommandRuleTests
{
    [Fact]
    public void F01_059_required_fields_add_validation_entries_and_throw_business_exception()
    {
        FtsSourceLocationUpdateCommand command = CreateValidCommand();
        command.OwnerEmail = null;

        BusinessException exception = Assert.Throws<BusinessException>(() => command.Validate());
        object validation = BusinessExceptionTestHelpers.GetBusinessMessages(exception).Single();
        IReadOnlyList<string> values = BusinessExceptionTestHelpers.GetStringValues(validation);

        Assert.Equal(CommonConstants.Validation.Messages.CommandError, exception.Message);
        Assert.Equal(CommonConstants.Validation.BusinessErrorCode, BusinessExceptionTestHelpers.GetBusinessErrorCode(exception));
        Assert.Contains(nameof(command.OwnerEmail), values);
        Assert.Contains(CommonConstants.Validation.Messages.MissingField, values);
    }

    [Fact]
    public void F01_060_run_schedule_requires_at_least_one_indicator()
    {
        FtsSourceLocationUpdateCommand command = CreateValidCommand();
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
    public void F01_061_ftp_server_id_is_required_when_protocol_is_ftp_server_code()
    {
        FtsSourceLocationUpdateCommand command = CreateValidCommand();
        command.Protocol = FtsSourceLocationUpdateCommand.FtpServerCode;
        command.FtpServerId = null;

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => command.Validate());
        Assert.Equal(FileTransferExceptionConstants.Validation.FtpServerId, exception.ParamName);
    }

    [Fact]
    public void F01_062_source_location_id_is_required()
    {
        FtsSourceLocationUpdateCommand command = CreateValidCommand();
        command.FtsSourceLocationId = null;

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => command.Validate());
        Assert.Equal(FileTransferExceptionConstants.Validation.FtpSoureceLocationId, exception.ParamName);
    }

    [Fact]
    public void F01_063_suspended_requires_a_changed_date()
    {
        FtsSourceLocationUpdateCommand command = CreateValidCommand();
        command.IsSuspended = true;
        command.IsSuspendedChangedDate = null;

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => command.Validate());
        Assert.Equal(FileTransferExceptionConstants.Validation.IsSuspendedChangedDate, exception.ParamName);
    }

    [Fact]
    public void F01_064_update_requires_a_non_zero_config_id()
    {
        FtsSourceLocationUpdateCommand command = CreateValidCommand();
        command.ConfigId = 0;

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => command.Validate());
        Assert.Equal(FileTransferExceptionConstants.Validation.ConfigIdNotFound, exception.ParamName);
    }

    [Fact]
    public void F01_065_mask_excluded_extension_adds_validation_entry()
    {
        FtsSourceLocationUpdateCommand command = CreateValidCommand();
        command.Mask = "file.EXE";

        BusinessException exception = Assert.Throws<BusinessException>(() => command.Validate());
        object validation = BusinessExceptionTestHelpers.GetBusinessMessages(exception).Single();
        IReadOnlyList<string> values = BusinessExceptionTestHelpers.GetStringValues(validation);

        Assert.Contains(nameof(command.Mask), values);
        Assert.Contains(
            string.Format("{0} {1}", CommonConstants.Validation.Messages.ExcludedExtension, "exe"),
            values);
    }

    private static FtsSourceLocationUpdateCommand CreateValidCommand()
    {
        return new FtsSourceLocationUpdateCommand
        {
            FtsSourceLocationId = "123",
            ConfigId = 1,
            ConfigName = "config",
            Protocol = "2",
            Address = "address",
            OwnerName = "owner",
            OwnerModule = "module",
            OwnerPhone = "phone",
            OwnerEmail = "email",
            StartTime = "00:00",
            EndTime = "23:59",
            RunOnMonday = true,
            Mask = "file.txt",
        };
    }
}


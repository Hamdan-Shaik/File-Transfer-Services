using System.Runtime.Serialization;

using FxferService.Domain.Constants;
using FxferService.Domain.Contracts.Events;
using FxferService.Domain.Contracts.Exceptions;

namespace FxferService.Domain.Commands;

[DataContract]
public class FtsSourceLocationUpdateCommand : FtsCommand
{
    public const string FtpServerCode = CommonConstants.FtpServerCode;

    [DataMember]
    public string? FtsSourceLocationId { get; set; }

    [DataMember]
    public int ConfigId { get; set; }

    [DataMember]
    public string? ConfigName { get; set; }

    [DataMember]
    public string? Description { get; set; }

    [DataMember]
    public string? FtpServerId { get; set; }

    [DataMember]
    public string? Address { get; set; }

    [DataMember]
    public string? Mask { get; set; }

    [DataMember]
    public string? Protocol { get; set; }

    [DataMember]
    public string? FileDirection { get; set; }

    [DataMember]
    public string? OwnerEmail { get; set; }

    [DataMember]
    public string? OwnerName { get; set; }

    [DataMember]
    public string? OwnerPhone { get; set; }

    [DataMember]
    public string? OwnerModule { get; set; }

    [DataMember]
    public string? TradingPartner { get; set; }

    [DataMember]
    public string? OperatorId { get; set; }

    [DataMember]
    public bool RunOnMonday { get; set; }

    [DataMember]
    public bool RunOnTuesday { get; set; }

    [DataMember]
    public bool RunOnWed { get; set; }

    [DataMember]
    public bool RunOnThrus { get; set; }

    [DataMember]
    public bool RunOnFri { get; set; }

    [DataMember]
    public bool RunOnSat { get; set; }

    [DataMember]
    public bool RunOnSun { get; set; }

    [DataMember]
    public bool RunOnFrstDayOfMonthInd { get; set; }

    [DataMember]
    public bool RunOnLastDayOfMonthInd { get; set; }

    [DataMember]
    public byte RunOnThisDayOfMonth { get; set; }

    [DataMember]
    public string? StartTime { get; set; }

    [DataMember]
    public string? EndTime { get; set; }

    [DataMember]
    public bool IsValid { get; set; }

    [DataMember]
    public bool IsSuspended { get; set; }

    [DataMember]
    public DateTime? IsSuspendedChangedDate { get; set; }

    [DataMember]
    public bool SendNotifications { get; set; }

    [DataMember]
    public DateTime LastModifiedDate { get; set; }

    [DataMember]
    public DateTime CreationDate { get; set; }

    [DataMember]
    public List<FtsDestinationLocationAdded>? DestinationAddress { get; set; }

    // F02-045, F02-046: validation sequencing and business exception behavior.
    public void Validate()
    {
        FtsCommandValidate();
        ValidateRequiredField(nameof(ConfigName).ToString(), ConfigName);
        ValidateRequiredField(nameof(Protocol).ToString(), Protocol);
        ValidateRequiredField(nameof(Address).ToString(), Address);
        ValidateRequiredField(nameof(OwnerName).ToString(), OwnerName);
        ValidateRequiredField(nameof(OwnerModule).ToString(), OwnerModule);
        ValidateRequiredField(nameof(OwnerPhone).ToString(), OwnerPhone);
        ValidateRequiredField(nameof(OwnerEmail).ToString(), OwnerEmail);
        ValidateRequiredField(nameof(StartTime).ToString(), StartTime);
        ValidateRequiredField(nameof(EndTime).ToString(), EndTime);
        ValidateRunSchedule();
        ValidateFtpServerId();
        ValidateFtpSourceLocationId();
        ValidateFtpSuspendedDate();
        ValidateConfigId();
        ValidateMaskExtension();
        HandleValidations();
    }

    // F02-048: FTP protocol requires FtpServerId.
    private void ValidateFtpServerId()
    {
        if (Protocol == FtpServerCode)
        {
            if (string.IsNullOrEmpty(FtpServerId))
            {
                throw new ArgumentNullException(FileTransferExceptionConstants.Validation.FtpServerId);
            }
        }
    }

    // F02-049, F02-054: update requires a non-empty FtsSourceLocationId.
    private void ValidateFtpSourceLocationId()
    {
        if (string.IsNullOrEmpty(FtsSourceLocationId))
        {
            throw new ArgumentNullException(FileTransferExceptionConstants.Validation.FtpSoureceLocationId);
        }
    }

    // F02-050: when IsSuspended is true, IsSuspendedChangedDate is required.
    private void ValidateFtpSuspendedDate()
    {
        if (IsSuspended)
        {
            if (!IsSuspendedChangedDate.HasValue)
            {
                throw new ArgumentNullException(FileTransferExceptionConstants.Validation.IsSuspendedChangedDate);
            }
        }
    }

    // F02-049: update requires a non-zero ConfigId.
    private void ValidateConfigId()
    {
        if (ConfigId == 0)
        {
            throw new ArgumentNullException(FileTransferExceptionConstants.Validation.ConfigIdNotFound);
        }
    }

    // F02-051: excluded mask extensions add a validation.
    private void ValidateMaskExtension()
    {
        string extension =
            !string.IsNullOrEmpty(Mask) && Mask.LastIndexOf('.') != -1
                ? Mask.Substring(Mask.LastIndexOf('.') + 1).ToLower()
                : string.Empty;

        if (Enum.IsDefined(typeof(ExcludedFileExtensions.ExcludedFileExtensionList), extension))
        {
            AddValidation(nameof(Mask).ToString(), BuildMaskValidationMessage(extension));
        }
    }

    // F02-051: message formatting for excluded extensions.
    private string BuildMaskValidationMessage(string extension)
    {
        return string.Format(
            new string(new[] { '{', '0', '}', ' ', '{', '1', '}' }),
            CommonConstants.Validation.Messages.ExcludedExtension,
            extension);
    }

    // F02-046: required strings add MissingField validation.
    private void ValidateRequiredField(string fieldName, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            AddValidation(fieldName, CommonConstants.Validation.Messages.MissingField);
        }
    }

    // F02-047: at least one run schedule option must be set.
    private void ValidateRunSchedule()
    {
        if (RunOnThisDayOfMonth == 0
            && !RunOnFrstDayOfMonthInd
            && !RunOnLastDayOfMonthInd
            && !RunOnSun
            && !RunOnMonday
            && !RunOnTuesday
            && !RunOnWed
            && !RunOnThrus
            && !RunOnFri
            & !RunOnSat)
        {
            AddValidation(CommonConstants.Validation.Properties.RunSchedule, CommonConstants.Validation.Messages.InvalidValue);
        }
    }

}


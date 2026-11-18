using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Domain.Constants;
using MediatR;

namespace Fxfer.DomainApi.Contracts.Commands;

// Rules: F02-028, F02-029, F02-030, F02-062
[DataContract]
public class AddFtpServerCommand : FileTransferCommand
    , IRequest<FxferDomainEvent<FxferConfigurationChangedV1>>
{
    public AddFtpServerCommand()
    {
        FtpServerId = Guid.NewGuid();
        IsValid = true;
    }

    [DataMember]
    [Required]
    public Guid FtpServerId { get; set; }

    [DataMember]
    [Required(AllowEmptyStrings = false)]
    public string? FtpServerName { get; set; }

    [DataMember]
    [Required(AllowEmptyStrings = false)]
    public string? FtpDescription { get; set; }

    [DataMember]
    [Required(AllowEmptyStrings = false)]
    public string? FtpServerIp { get; set; }

    [DataMember]
    [Required(AllowEmptyStrings = false)]
    public string? FtpTransferMode { get; set; }

    [DataMember]
    [Required(AllowEmptyStrings = false)]
    public string? FtpUser { get; set; }

    [DataMember]
    public string? FtpPassword { get; set; }

    [DataMember]
    public short FtpPort { get; set; }

    [DataMember]
    public string? FtpProxyAddress { get; set; }

    [DataMember]
    public short? FtpProxyPort { get; set; }

    [DataMember]
    [DefaultValue(false)]
    public bool UseSSHAuth { get; set; }

    [DataMember]
    [DefaultValue(true)]
    public bool IsValid { get; set; }

    [DataMember]
    public DateTime LastModifiedDate { get; set; }

    [DataMember]
    public DateTime CreationDate { get; set; }

    [DataMember]
    public string? OperatorId { get; set; }

    // F02-062: do not validate FtpPort.
    public override void SetCommandValidations()
    {
        ValidateFtpServerId();
        ValidateFtpServerIp();
        ValidateFtpPasswordRequiredForCreate();
        ValidateFtpPasswordRequiredForCreate();
        ValidateProxyPort();
    }

    // F02-028
    private void ValidateFtpServerId()
    {
        if (FtpServerId.Equals(Guid.Empty))
        {
            AddValidation(
                CommonConstants.Validation.Properties.FtpServerId,
                CommonConstants.Validation.ErrorType.InvalidFtpServerId,
                string.Format(
                    "{0}: {1}",
                    CommonConstants.Validation.Messages.InvalidValue,
                    CommonConstants.Validation.Properties.FtpServerId));
        }
    }

    // F02-029
    private void ValidateFtpServerIp()
    {
        if (!string.IsNullOrEmpty(FtpServerIp) && !ValidateFtpServerIpPattern())
        {
            AddValidation(
                CommonConstants.Validation.Properties.FtpServerIp,
                CommonConstants.Validation.ErrorType.InvalidIpAddressFormat,
                CommonConstants.Validation.Messages.IpAddressInvalidFormat);
        }
    }

    // F02-029
    private bool ValidateFtpServerIpPattern()
    {
        return Regex.IsMatch(FtpServerIp, CommonConstants.RegexPatterns.IpAddressPattern);
    }

    // F02-030
    private void ValidateProxyPort()
    {
        if (!string.IsNullOrEmpty(FtpProxyAddress) && (!FtpProxyPort.HasValue || FtpProxyPort <= 0))
        {
            AddValidation(
                CommonConstants.Validation.Properties.FtpProxyPort,
                CommonConstants.Validation.ErrorType.InvalidFtpProxyPort,
                string.Format(
                    "{0}: {1}",
                    CommonConstants.Validation.Messages.InvalidValue,
                    CommonConstants.Validation.Properties.FtpProxyPort));
        }
    }

    private void ValidateFtpPasswordRequiredForCreate()
    {
        if (GetType() == typeof(AddFtpServerCommand) && string.IsNullOrEmpty(FtpPassword))
        {
            AddValidation(
                nameof(FtpPassword),
                CommonConstants.Validation.ErrorType.RequiredFields,
                string.Format(
                    "{0}: {1}",
                    CommonConstants.Validation.Messages.MissingField,
                    nameof(FtpPassword)));
        }
    }}


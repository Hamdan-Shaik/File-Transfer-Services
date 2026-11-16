using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

using FxferService.Domain.Constants;

namespace FxferService.Domain.Commands;

[DataContract]
public class AddFtpServerCommand : FileTransferCommand
{
    public AddFtpServerCommand()
    {
        // F01-031: default-constructed command starts active with a new identifier.
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
    [Required(AllowEmptyStrings = false)]
    public string? FtpPassword { get; set; }

    [DataMember]
    [Required(AllowEmptyStrings = false)]
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

    // F01-025: runs ValidateFtpServerId, ValidateFtpServerIp, and ValidateProxyPort.
    public override void SetCommandValidations()
    {
        ValidateFtpServerId();
        ValidateFtpServerIp();
        ValidateProxyPort();
    }

    // F01-026: Guid.Empty adds InvalidFtpServerId validation.
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

    // F01-027: invalid IPv4 format adds InvalidIpAddressFormat validation.
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

    // F01-027: IPv4 regex match.
    private bool ValidateFtpServerIpPattern()
    {
        return Regex.IsMatch(FtpServerIp!, CommonConstants.RegexPatterns.IpAddressPattern);
    }

    // F01-028: proxy address requires proxy port > 0.
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
}

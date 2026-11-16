using System.Runtime.Serialization;

using FxferService.Application.LegacyContracts;
using FxferService.Domain.Constants;
using FxferService.Domain.Contracts.Exceptions;

namespace FxferService.Domain.Commands;

[DataContract]
public class UpdateIntegrationFileCommand : AddIntegrationFileCommand
{
    [DataMember]
    public RequestorModel? Requestor { get; set; }

    [DataMember]
    public Guid FtsIntegrationFileId { get; set; }

    [DataMember]
    public int? ConfigId { get; set; }

    [DataMember]
    public string? OperatorId { get; set; }

    // F02-067: validation entry point.
    public void Validate()
    {
        ValidateConfigId();
    }

    // F02-067: missing ConfigId throws BusinessException with one validation.
    private void ValidateConfigId()
    {
        List<BusinessExceptionMessage> validations = new List<BusinessExceptionMessage>();

        if (ConfigId == null)
        {
            validations.Add(new BusinessExceptionMessage(
                FileTransferExceptionConstants.Validation.ConfigId,
                FileTransferExceptionConstants.Validation.ConfigId,
                CommonConstants.Validation.Messages.MissingConfigId));

            throw new BusinessException(
                validations,
                CommonConstants.Validation.Messages.CommandError,
                CommonConstants.Validation.BusinessErrorCode);
        }
    }
}



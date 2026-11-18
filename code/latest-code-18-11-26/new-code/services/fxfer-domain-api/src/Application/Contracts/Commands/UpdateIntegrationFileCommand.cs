using System.Runtime.Serialization;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Contracts.Exceptions;
using Fxfer.DomainApi.Domain.Constants;
using HPP.Core.BAS.CQRS.Base;
using MediatR;

namespace Fxfer.DomainApi.Contracts.Commands;

// Rules: F03-010
[DataContract]
public sealed class UpdateIntegrationFileCommand : AddIntegrationFileCommand
    , IRequest<FxferDomainEvent<FxferConfigurationChangedV1>>
{
    [DataMember]
    public Guid FtsIntegrationFileId { get; set; }

    [DataMember]
    public int? ConfigId { get; set; }

    public void Validate()
    {
        ValidateConfigId();
    }

    // F03-010
    private void ValidateConfigId()
    {
        List<BusinessExceptionMessage> validations = new();

        if (ConfigId == null)
        {
            validations.Add(
                new BusinessExceptionMessage(
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


using System.Runtime.Serialization;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Contracts.Exceptions;
using MediatR;

namespace Fxfer.DomainApi.Contracts.Commands;

// Rules: F01-066, F01-067, F01-068
[DataContract]
public sealed class FtsSourceLocationInvalidateCommand : Command
    , IRequest<FxferDomainEvent<FxferConfigurationChangedV1>>
{
    [DataMember]
    public string? FtsSourceLocationId { get; set; }

    [DataMember]
    public int ConfigId { get; set; }

    [DataMember]
    public string? OperatorId { get; set; }

    [DataMember]
    public List<FtsDestinationLocationAdded>? DestinationAddress { get; set; }

    public void Validate()
    {
        ValidateOperatorId();
        ValidateFtpSourceLocationId();
        ValidateConfigId();
    }

    // F01-068
    private void ValidateConfigId()
    {
        if (ConfigId == 0)
        {
            throw new ArgumentNullException(FileTransferExceptionConstants.Validation.ConfigIdNotFound);
        }
    }

    // F01-067
    private void ValidateFtpSourceLocationId()
    {
        if (string.IsNullOrEmpty(FtsSourceLocationId))
        {
            throw new ArgumentNullException(FileTransferExceptionConstants.Validation.FtpSoureceLocationId);
        }
    }

    // F01-066
    private void ValidateOperatorId()
    {
        if (string.IsNullOrEmpty(OperatorId))
        {
            throw new ArgumentNullException(FileTransferExceptionConstants.Validation.OperatorId);
        }
    }
}


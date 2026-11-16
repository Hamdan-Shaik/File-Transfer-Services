using System.Runtime.Serialization;

using FxferService.Domain.Contracts.Events;
using FxferService.Domain.Contracts.Exceptions;

namespace FxferService.Domain.Commands;

[DataContract]
public class FtsSourceLocationInvalidateCommand
{
    [DataMember]
    public string? FtsSourceLocationId { get; set; }

    [DataMember]
    public int ConfigId { get; set; }

    [DataMember]
    public string? OperatorId { get; set; }

    [DataMember]
    public List<FtsDestinationLocationAdded>? DestinationAddress { get; set; }

    // F02-052: fail-fast for missing OperatorId, FtsSourceLocationId, and ConfigId.
    public void Validate()
    {
        ValidateOperatorId();
        ValidateFtpSourceLocationId();
        ValidateConfigId();
    }

    // F02-052, F02-056: requires FtsSourceLocationId even though downstream invalidation uses ConfigId.
    private void ValidateFtpSourceLocationId()
    {
        if (string.IsNullOrEmpty(FtsSourceLocationId))
        {
            throw new ArgumentNullException(FileTransferExceptionConstants.Validation.FtpSoureceLocationId);
        }
    }

    // F02-052: requires OperatorId.
    private void ValidateOperatorId()
    {
        if (string.IsNullOrEmpty(OperatorId))
        {
            throw new ArgumentNullException(FileTransferExceptionConstants.Validation.OperatorId);
        }
    }

    // F02-052: requires non-zero ConfigId.
    private void ValidateConfigId()
    {
        if (ConfigId == 0)
        {
            throw new ArgumentNullException(FileTransferExceptionConstants.Validation.ConfigIdNotFound);
        }
    }
}
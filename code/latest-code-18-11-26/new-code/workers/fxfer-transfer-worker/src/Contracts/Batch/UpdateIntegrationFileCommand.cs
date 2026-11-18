using Fxfer.TransferWorker.Contracts.Exceptions;
using Fxfer.TransferWorker.Domain.Constants;

namespace Fxfer.TransferWorker.Contracts.Batch;

public sealed class UpdateIntegrationFileCommand
{
    public RequestorModel? Requestor { get; set; }

    public List<GroupInfo>? GroupInfo { get; set; }

    public int? ConfigId { get; set; }

    public Guid FtsIntegrationFileId { get; set; }

    public string? FileName { get; set; }

    public Guid ETFOutboundMetadataDefnId { get; set; }

    public bool IsValid { get; set; }

    public void Validate()
    {
        ValidateConfigId();
    }

    private void ValidateConfigId()
    {
        List<BusinessExceptionMessage> validations = new();

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



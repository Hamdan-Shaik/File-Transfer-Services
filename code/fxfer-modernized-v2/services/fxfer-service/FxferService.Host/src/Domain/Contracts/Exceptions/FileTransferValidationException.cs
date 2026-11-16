using FxferService.Application.LegacyContracts;

namespace FxferService.Domain.Contracts.Exceptions;

public class FileTransferValidationException : Exception, IFaultGenerator
{
    public FileTransferValidationException(string fieldName, string validationMessage, string message)
        : base(message)
    {
        FieldName = fieldName;
        ValidationErrorMessage = validationMessage;
    }

    public FileTransferValidationException(string fieldName, string validationMessage)
        : this(fieldName, validationMessage, string.Empty)
    {
    }

    public FileTransferValidationException(string validationMessage)
        : this(string.Empty, validationMessage)
    {
    }

    public FileTransferValidationException()
        : this(string.Empty)
    {
    }

    public string FieldName { get; set; }

    public string ValidationErrorMessage { get; set; }

    public FaultException GenerateFault()
    {
        return new FaultException<FileTransfertBASValidationException>(
            new FileTransfertBASValidationException(FieldName, ValidationErrorMessage),
            new FaultReason(ValidationErrorMessage));
    }
}


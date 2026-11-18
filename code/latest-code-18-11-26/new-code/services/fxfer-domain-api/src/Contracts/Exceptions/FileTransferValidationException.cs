namespace Fxfer.DomainApi.Contracts.Exceptions;

public class FileTransferValidationException : Exception
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
}


using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Exceptions;

[DataContract]
public class FileTransfertBASValidationException
{
    public FileTransfertBASValidationException(string fieldName, string message)
    {
        FieldName = fieldName;
        ValidationErrorMessage = message;
    }

    public FileTransfertBASValidationException(string message)
        : this(string.Empty, message)
    {
    }

    public FileTransfertBASValidationException()
        : this(string.Empty)
    {
    }

    [DataMember]
    public string FieldName { get; set; }

    [DataMember]
    public string ValidationErrorMessage { get; set; }
}


using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Exceptions;

[DataContract]
public class BusinessExceptionMessage
{
    public BusinessExceptionMessage()
    {
    }

    public BusinessExceptionMessage(string fieldName, string messageKey, string messageDefault)
    {
        FieldName = fieldName;
        MessageKey = messageKey;
        MessageDefault = messageDefault;
    }

    [DataMember]
    public string? FieldName { get; set; }

    [DataMember]
    public string? MessageDefault { get; set; }

    [DataMember]
    public string? MessageKey { get; set; }

    [DataMember]
    public MessageTypes MessageType { get; set; }

    [DataContract]
    public enum MessageTypes
    {
        [EnumMember]
        Informational = 0,

        [EnumMember]
        Warning = 1,

        [EnumMember]
        Error = 2,
    }
}

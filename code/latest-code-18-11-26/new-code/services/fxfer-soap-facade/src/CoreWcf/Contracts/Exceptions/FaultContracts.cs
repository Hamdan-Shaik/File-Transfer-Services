using System.Runtime.Serialization;

namespace Fxfer.SoapFacade.CoreWcf.Contracts.Exceptions;

[DataContract]
public sealed class ServiceException
{
    [DataMember(Order = 1)]
    public StatusCodeType ErrorCode { get; set; }

    [DataMember(Order = 2)]
    public List<string> ErrorMessages { get; set; } = [];

    [DataMember(Order = 3)]
    public string Reason { get; set; } = string.Empty;
}

[DataContract]
public sealed class BusinessValidationException
{
    [DataMember(Order = 1)]
    public List<BusinessExceptionMessage> BusinessMessages { get; set; } = [];
}

[DataContract]
public sealed class BusinessExceptionMessage
{
    [DataMember(Order = 1)]
    public string? FieldName { get; set; }

    [DataMember(Order = 2)]
    public string? MessageDefault { get; set; }

    [DataMember(Order = 3)]
    public string? MessageKey { get; set; }

    [DataMember(Order = 4)]
    public MessageTypes MessageType { get; set; }
}

public enum MessageTypes
{
    Informational = 0,
    Warning = 1,
    Error = 2
}

public enum StatusCodeType
{
    OK = 0,
    CorruptFile = 9000,
    Unauthorized = 9010,
    Duplicate = 9020,
    NotFound = 9030,
    Stale = 9040,
    Error = 9999
}



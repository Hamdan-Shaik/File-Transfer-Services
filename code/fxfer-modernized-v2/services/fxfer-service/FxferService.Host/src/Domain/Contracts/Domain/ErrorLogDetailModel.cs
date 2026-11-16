using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Domain;

/// <summary>
/// Describes Error Log
/// </summary>
[DataContract]
public class ErrorLogDetailModel
{
    [DataMember]
    public Guid DestinationId { get; set; }

    [DataMember]
    public Guid EventId { get; set; }

    [DataMember]
    public Guid FileId { get; set; }

    [DataMember]
    public string? ErrorType { get; set; }

    [DataMember]
    public string? ErrorText { get; set; }

    [DataMember]
    public string? StackTrace { get; set; }

    [DataMember]
    public string? MethodName { get; set; }

    [DataMember]
    public string? AssemblyName { get; set; }

    [DataMember]
    public string? ClassName { get; set; }

    [DataMember]
    public string? MachineName { get; set; }

    [DataMember]
    public string? OperatorId { get; set; }
}


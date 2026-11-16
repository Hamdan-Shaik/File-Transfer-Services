using System;
using System.Runtime.Serialization;

#nullable disable


namespace FxferService.Domain.Contracts.Domain;

/// <summary>
/// <para>File Exception</para>
/// <para>Contains results from File Exception Query.</para>
/// </summary>
[Serializable]
[DataContract]
public class FileExceptionModel
{
    public FileExceptionModel()
        : base()
    {
    }

    [DataMember]
    public Guid ExceptionId { get; set; }

    [DataMember]
    public Guid FileId { get; set; }

    [DataMember]
    public Guid DestinationId { get; set; }

    [DataMember]
    public Guid EventId { get; set; }

    [DataMember]
    public string ExceptionType { get; set; }

    [DataMember]
    public string MethodName { get; set; }

    [DataMember]
    public string Message { get; set; }

    [DataMember]
    public string StackTrace { get; set; }

    [DataMember]
    public DateTime CreatedDate { get; set; }
}

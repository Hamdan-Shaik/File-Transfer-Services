using System;
using System.Runtime.Serialization;

#nullable disable


namespace FxferService.Domain.Contracts.Domain;

/// <summary>
/// <para>File Event</para>
/// <para>Contains results from File Event Query.</para>
/// </summary>
[Serializable]
[DataContract]
public class FileEventModel
{
    public FileEventModel()
        : base()
    {
    }

    [DataMember]
    public Guid EventId { get; set; }

    [DataMember]
    public Guid FileId { get; set; }

    [DataMember]
    public Guid DestinationId { get; set; }

    [DataMember]
    public string EventCode { get; set; }

    [DataMember]
    public string EventTrackDescription { get; set; }

    [DataMember]
    public DateTime CreatedDate { get; set; }
}

using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Domain;

/// <summary>
/// Describes File Notification Query
/// </summary>
[DataContract]
public class FtsNotificationQueryModel
{
    [DataMember]
    public int ConfigId { get; set; }

    [DataMember]
    public bool IncludeError { get; set; }

    [DataMember]
    public bool IncludeSuccess { get; set; }
}


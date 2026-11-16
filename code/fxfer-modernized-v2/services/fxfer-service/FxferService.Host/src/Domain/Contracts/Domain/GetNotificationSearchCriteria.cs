using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Domain;

/// <summary>
/// Get Notification Search Criteria
/// </summary>
[DataContract]
public class GetNotificationSearchCriteria
{
    /// <summary>
    /// Configuration Id
    /// </summary>
    [DataMember]
    public Guid ConfigId { get; set; }

    /// <summary>
    /// Notify on Error
    /// </summary>
    [DataMember]
    public bool NotifyOnErrorInd { get; set; }

    /// <summary>
    /// Notify on Success
    /// </summary>
    [DataMember]
    public bool NotifyOnSuccessInd { get; set; }
}


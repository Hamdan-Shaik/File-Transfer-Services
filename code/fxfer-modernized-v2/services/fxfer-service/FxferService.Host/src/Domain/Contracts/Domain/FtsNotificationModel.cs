using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Domain;

/// <summary>
/// File Transfer Notification model
/// </summary>
[DataContract]
public class FtsNotificationModel
{
    /// <summary>
    /// Notification Email Address
    /// </summary>
    [DataMember]
    public string? EmailAddress { get; set; }

    /// <summary>
    /// Contact name for Notification
    /// </summary>
    [DataMember]
    public string? ContactName { get; set; }

    /// <summary>
    /// Notify Error
    /// </summary>
    [DataMember]
    public bool NotifyOnErrorInd { get; set; }

    /// <summary>
    /// Notify Sucess
    /// </summary>
    [DataMember]
    public bool NotifyOnSuccessInd { get; set; }

    /// <summary>
    /// Configuration Id
    /// </summary>
    [DataMember]
    public Guid ConfigId { get; set; }
}


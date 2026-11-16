namespace FxferService.Domain.Contracts.Batch;

public class FtsNotificationModel
{
    /// <summary>
    /// Notification Email Address
    /// </summary>
    public string? EmailAddress { get; set; }

    /// <summary>
    /// Contact name for Notification
    /// </summary>
    public string? ContactName { get; set; }

    /// <summary>
    /// Notify Error
    /// </summary>
    public bool NotifyOnErrorInd { get; set; }

    /// <summary>
    /// Notify Sucess
    /// </summary>
    public bool NotifyOnSuccessInd { get; set; }

    /// <summary>
    /// Configuration Id
    /// </summary>
    public Guid ConfigId { get; set; }
}


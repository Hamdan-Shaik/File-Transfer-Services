namespace FxferService.Domain.Contracts.Batch;

public class GetNotificationSearchCriteria
{
    /// <summary>
    /// Configuration Id
    /// </summary>
    public Guid ConfigId { get; set; }

    /// <summary>
    /// Notify on Error
    /// </summary>
    public bool NotifyOnErrorInd { get; set; }

    /// <summary>
    /// Notify on Success
    /// </summary>
    public bool NotifyOnSuccessInd { get; set; }
}


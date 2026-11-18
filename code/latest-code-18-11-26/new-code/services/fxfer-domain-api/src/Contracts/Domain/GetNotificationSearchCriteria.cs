namespace Fxfer.DomainApi.Contracts.Domain;

public sealed class GetNotificationSearchCriteria
{
    public Guid ConfigId { get; set; }

    public bool NotifyOnErrorInd { get; set; }

    public bool NotifyOnSuccessInd { get; set; }
}
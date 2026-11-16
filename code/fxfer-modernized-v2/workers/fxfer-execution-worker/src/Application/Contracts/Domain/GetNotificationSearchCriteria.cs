using System;

namespace Fxfer.ExecutionWorker.Application.Contracts.Domain;

public class GetNotificationSearchCriteria
{
    public Guid ConfigId { get; set; }

    public bool NotifyOnErrorInd { get; set; }

    public bool NotifyOnSuccessInd { get; set; }
}

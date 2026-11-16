using System;

namespace Fxfer.ExecutionWorker.Application.Contracts.Domain;

public class FtsNotificationModel
{
    public string EmailAddress { get; set; } = string.Empty;

    public string ContactName { get; set; } = string.Empty;

    public bool NotifyOnErrorInd { get; set; }

    public bool NotifyOnSuccessInd { get; set; }

    public Guid ConfigId { get; set; }
}

using System;
using Fxfer.ExecutionWorker.Application.NotificationService;

namespace Fxfer.ExecutionWorker.Batch.Business.NotificationService;

public interface INotificationService
{
    PostEvent PostEvent(
        string eventName,
        string subSystem,
        string messageBody,
        bool isArchived,
        DateTime utcNow,
        Guid? lastEventId,
        string operatorId);
}
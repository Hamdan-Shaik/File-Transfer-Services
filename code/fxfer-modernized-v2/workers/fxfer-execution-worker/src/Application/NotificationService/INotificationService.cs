using System;
using System.Collections.Generic;

namespace Fxfer.ExecutionWorker.Application.NotificationService;

public interface INotificationService
{
    PostEvent PostEvent(
        string eventName,
        string subSystem,
        string messageBody,
        bool isArchived,
        DateTime utcNow,
        Guid? lastEventId,
        string operatorId,
        string tenantId);

    void SendEmail(
        string tenantId,
        string subject,
        string emailBody,
        List<Address> addressList,
        Address fromAddress,
        MailPriority priority,
        List<Attachments> attachmentList);
}

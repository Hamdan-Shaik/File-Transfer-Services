using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Fxfer.ExecutionWorker.Application.NotificationService;

public sealed class NullNotificationService : INotificationService
{
    private readonly ILogger<NullNotificationService> _logger;

    public NullNotificationService(ILogger<NullNotificationService> logger)
    {
        _logger = logger;
    }

    public PostEvent PostEvent(
        string eventName,
        string subSystem,
        string messageBody,
        bool isArchived,
        DateTime utcNow,
        Guid? lastEventId,
        string operatorId,
        string tenantId)
    {
        _logger.LogInformation(
            "Notification event suppressed (tenant={TenantId}, name={EventName}, subsystem={SubSystem})",
            tenantId,
            eventName,
            subSystem);

        return new PostEvent
        {
            EventId = Guid.NewGuid(),
        };
    }

    public void SendEmail(
        string tenantId,
        string subject,
        string emailBody,
        List<Address> addressList,
        Address fromAddress,
        MailPriority priority,
        List<Attachments> attachmentList)
    {
        _logger.LogInformation(
            "Notification email suppressed (tenant={TenantId}, subject={Subject}, recipients={RecipientCount})",
            tenantId,
            subject,
            addressList?.Count ?? 0);
    }
}


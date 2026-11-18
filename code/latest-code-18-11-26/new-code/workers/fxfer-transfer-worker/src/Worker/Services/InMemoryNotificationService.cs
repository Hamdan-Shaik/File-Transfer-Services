using Fxfer.TransferWorker.Contracts.NotificationService;

namespace Fxfer.TransferWorker.Worker.Services;

public sealed class InMemoryNotificationService : INotificationService
{
    private readonly ILogger<InMemoryNotificationService> _logger;

    public InMemoryNotificationService(ILogger<InMemoryNotificationService> logger)
    {
        _logger = logger;
    }

    public PostEvent PostEvent(
        string eventName,
        string subSystem,
        string messageBody,
        bool isArchived,
        DateTime createdDateTimeUtc,
        Guid? lastEventId,
        string operatorId,
        string tenantId)
    {
        _logger.LogInformation(
            "PostEvent called (eventName={EventName}, subSystem={SubSystem}, isArchived={IsArchived}, tenantId={TenantId}, messageLength={MessageLength})",
            eventName,
            subSystem,
            isArchived,
            tenantId,
            messageBody?.Length ?? 0);

        return new PostEvent { EventId = Guid.NewGuid() };
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
            "SendEmail called (tenantId={TenantId}, subjectLength={SubjectLength}, bodyLength={BodyLength}, toCount={ToCount}, attachmentCount={AttachmentCount})",
            tenantId,
            subject?.Length ?? 0,
            emailBody?.Length ?? 0,
            addressList?.Count ?? 0,
            attachmentList?.Count ?? 0);
    }
}


namespace Fxfer.TransferWorker.Contracts.NotificationService;

public interface INotificationService
{
    PostEvent PostEvent(
        string eventName,
        string subSystem,
        string messageBody,
        bool isArchived,
        DateTime createdDateTimeUtc,
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
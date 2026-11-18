using System.ServiceModel;
using System.ServiceModel.Channels;
using Fxfer.TransferWorker.Contracts.NotificationService;

namespace Fxfer.TransferWorker.Application.Helpers;

public class NotificationServiceHelper
{
    private ChannelFactory? _serviceChannelFactory;
    private INotificationService _notificationServiceProxy;
    private string _tenantId;

    public NotificationServiceHelper(string tenantId, INotificationService notificationService)
    {
        _tenantId = tenantId;
        _notificationServiceProxy = notificationService;
    }

    public NotificationServiceHelper(string tenantId, string url, string bindingConfiguration)
        : this(tenantId, null!)
    {
        _notificationServiceProxy = CreateServiceProxy<INotificationService>(url, bindingConfiguration);
    }

    public PostEvent PreviousEvent { get; private set; } = null!;

    public void PostEvent(string eventName, string subSystem, string messageBody, bool isArchived, string operatorId)
    {
        Guid? lastEventId = null;
        if (PreviousEvent != null)
        {
            lastEventId = PreviousEvent.EventId;
        }

        PreviousEvent = _notificationServiceProxy.PostEvent(
            eventName,
            subSystem,
            messageBody,
            isArchived,
            DateTime.UtcNow,
            lastEventId,
            operatorId,
            _tenantId);

        CloseChannelFactory();
    }

    public void SendEmail(
        string subject,
        string emailBody,
        List<Address> addressList,
        Address fromAddress,
        MailPriority priority,
        List<Attachments> attachmentList)
    {
        _notificationServiceProxy.SendEmail(_tenantId, subject, emailBody, addressList, fromAddress, priority, attachmentList);
        CloseChannelFactory();
    }

    public virtual T CreateChannel<T>(ChannelFactory channelFactory)
    {
        ChannelFactory<T> channel = channelFactory as ChannelFactory<T>;
        T proxy = channel!.CreateChannel();
        ((IContextChannel)proxy!).OperationTimeout = new TimeSpan(0, 50, 0);
        return proxy;
    }

    public virtual T CreateServiceProxy<T>(string url, string bindingConfigurationName)
    {
        _serviceChannelFactory = InitializeChannelFactory<T>(url, bindingConfigurationName);
        return CreateChannel<T>(_serviceChannelFactory);
    }

    public virtual ChannelFactory<T> InitializeChannelFactory<T>(string endpointaddress, string bindingConfigurationName)
    {
        EndpointAddress endpoint = new EndpointAddress(endpointaddress);
        return new ChannelFactory<T>(ReturnBinding(bindingConfigurationName), endpoint);
    }

    private Binding ReturnBinding(string bindingType)
    {
        try
        {
            return WCFBinding.ReturnBinding(ParseEnum<WCFBinding.BindingTypes>(bindingType));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    private void CloseChannelFactory()
    {
        if (_serviceChannelFactory != null)
        {
            _serviceChannelFactory.Close();
        }
    }
}
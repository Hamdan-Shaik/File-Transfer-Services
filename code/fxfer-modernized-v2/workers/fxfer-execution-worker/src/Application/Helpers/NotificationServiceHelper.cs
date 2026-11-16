using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Fxfer.ExecutionWorker.Application.NotificationService;

namespace Fxfer.ExecutionWorker.Application.Helpers;

public class NotificationServiceHelper
{
    private ChannelFactory? serviceChannelFactory;
    private INotificationService notificationServiceProxy;
    private string tenantId;

    public NotificationServiceHelper(string tenantId, INotificationService notificationService)
    {
        this.tenantId = tenantId;
        this.notificationServiceProxy = notificationService;
    }

    public NotificationServiceHelper(string tenantId, string url, string bindingConfiguration)
        : this(tenantId, null!)
    {
        this.notificationServiceProxy = this.CreateServiceProxy<INotificationService>(url, bindingConfiguration);
    }

    public PostEvent? PreviousEvent { get; private set; }

    public void PostEvent(string eventName, string subSystem, string messageBody, bool isArchived, string operatorId)
    {
        Guid? lastEventId = null;
        if (this.PreviousEvent != null)
        {
            lastEventId = this.PreviousEvent.EventId;
        }

        this.PreviousEvent = this.notificationServiceProxy.PostEvent(eventName, subSystem, messageBody, isArchived, DateTime.UtcNow, lastEventId, operatorId, this.tenantId);
        this.CloseChannelFactory();
    }

    public void SendEmail(string subject, string emailBody, List<Address> addressList, Address fromAddress, MailPriority priority, List<Attachments> attachmentList)
    {
        this.notificationServiceProxy.SendEmail(this.tenantId, subject, emailBody, addressList, fromAddress, priority, attachmentList);
        this.CloseChannelFactory();
    }

    public virtual T CreateChannel<T>(ChannelFactory channelFactory)
    {
        ChannelFactory<T>? channel = channelFactory as ChannelFactory<T>;
        T proxy = channel!.CreateChannel();
        ((IContextChannel)proxy!).OperationTimeout = new TimeSpan(0, 50, 0);
        return proxy;
    }

    public virtual T CreateServiceProxy<T>(string url, string bindingConfigurationName)
    {
        this.serviceChannelFactory = this.InitializeChannelFactory<T>(url, bindingConfigurationName);
        return this.CreateChannel<T>(this.serviceChannelFactory!);
    }

    public virtual ChannelFactory<T> InitializeChannelFactory<T>(string endpointaddress, string bindingConfigurationName)
    {
        EndpointAddress endpoint = new EndpointAddress(endpointaddress);
        return new ChannelFactory<T>(this.ReturnBinding(bindingConfigurationName), endpoint);
    }

    private Binding ReturnBinding(string bindingType)
    {
        try
        {
            return WCFBinding.ReturnBinding(this.ParseEnum<WCFBinding.BindingTypes>(bindingType));
        }
        catch (Exception ex)
        {
            #pragma warning disable CA2200
            throw ex;
#pragma warning restore CA2200
        }
    }

    private T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    private void CloseChannelFactory()
    {
        if (this.serviceChannelFactory != null)
        {
            this.serviceChannelFactory.Close();
        }
    }
}




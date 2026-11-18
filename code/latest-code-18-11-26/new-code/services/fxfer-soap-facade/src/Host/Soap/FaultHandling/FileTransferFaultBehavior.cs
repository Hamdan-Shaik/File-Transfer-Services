using System.Collections.ObjectModel;
using System.Reflection;
using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Description;
using CoreWCF.Dispatcher;
using Fxfer.SoapFacade.CoreWcf.Contracts.Exceptions;
using Microsoft.Extensions.Logging;

namespace Fxfer.SoapFacade.Host.Soap.FaultHandling;

public sealed class FileTransferFaultBehavior : IServiceBehavior
{
    private readonly ILogger<FileTransferFaultBehavior> _logger;

    public FileTransferFaultBehavior(ILogger<FileTransferFaultBehavior> logger)
    {
        _logger = logger;
    }

    public void AddBindingParameters(
        ServiceDescription serviceDescription,
        ServiceHostBase serviceHostBase,
        Collection<ServiceEndpoint> endpoints,
        BindingParameterCollection bindingParameters)
    {
    }

    public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    {
        foreach (var channelDispatcherBase in serviceHostBase.ChannelDispatchers)
        {
            if (channelDispatcherBase is not ChannelDispatcher channelDispatcher)
            {
                continue;
            }

            channelDispatcher.ErrorHandlers.Add(new FileTransferFaultErrorHandler(_logger));
        }
    }

    public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    {
    }

    public sealed class FileTransferFaultErrorHandler : IErrorHandler
    {
        private readonly ILogger _logger;

        public FileTransferFaultErrorHandler(ILogger logger)
        {
            _logger = logger;
        }

        public bool HandleError(Exception error)
        {
            return true;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            var faultException = ToFaultException(error);
            var messageFault = faultException.CreateMessageFault();
            fault = Message.CreateMessage(version, messageFault, faultException.Action ?? string.Empty);
        }

        public FaultException ToFaultException(Exception ex)
        {
            Exception actualEx = (ex is TargetInvocationException tie) ? tie.InnerException ?? ex : ex;

            var svcException = new ServiceException
            {
                ErrorCode = StatusCodeType.Error,
                ErrorMessages = [actualEx.Message],
                Reason = actualEx.Message
            };

            if (actualEx is IFaultGenerator faultGenerator)
            {
                FaultException? customFault = faultGenerator.GenerateFault();

                if (customFault is not null)
                {
                    _logger.LogCritical(customFault, "{message}", FileTransferExceptionConstants.FaultExceptions.CustomFault + customFault.Message);
                    return customFault;
                }
            }

            _logger.LogCritical(actualEx, "{message}", FileTransferExceptionConstants.FaultExceptions.ApplicationError);
            var reason = string.IsNullOrEmpty(svcException.Reason) ? string.Empty : svcException.Reason;
            _ = reason;

            return new FaultException<ServiceException>(svcException, new FaultReason(svcException.Reason));
        }
    }
}

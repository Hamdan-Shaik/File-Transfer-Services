using System.Reflection;
using CoreWCF;
using Fxfer.SoapFacade.CoreWcf.Contracts.Exceptions;
using Fxfer.SoapFacade.Host.Soap.FaultHandling;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fxfer.SoapFacade.Tests;

public sealed class FaultHandlingTests
{
    [Fact]
    public void ToFaultException_UnwrapsTargetInvocationException()
    {
        var handler = new FileTransferFaultBehavior.FileTransferFaultErrorHandler(NullLogger.Instance);

        var inner = new InvalidOperationException("boom");
        var ex = new TargetInvocationException(inner);

        var faultException = handler.ToFaultException(ex);

        var typed = Assert.IsType<FaultException<ServiceException>>(faultException);
        Assert.Equal(StatusCodeType.Error, typed.Detail.ErrorCode);
        Assert.Contains("boom", typed.Detail.ErrorMessages);
        Assert.Equal("boom", typed.Detail.Reason);
    }

    [Fact]
    public void ToFaultException_PassthroughsCustomFault_WhenIFaultGenerator()
    {
        var handler = new FileTransferFaultBehavior.FileTransferFaultErrorHandler(NullLogger.Instance);
        var expectedFault = new FaultException("custom-fault");

        var ex = new CustomFaultException(expectedFault);
        var faultException = handler.ToFaultException(ex);

        Assert.Same(expectedFault, faultException);
    }

    private sealed class CustomFaultException : Exception, IFaultGenerator
    {
        private readonly FaultException _fault;

        public CustomFaultException(FaultException fault)
        {
            _fault = fault;
        }

        public FaultException? GenerateFault()
        {
            return _fault;
        }
    }
}

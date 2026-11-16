using System.Reflection;
using FxferService.Application.Profiles;
using FxferService.Domain.Contracts.Exceptions;

namespace FxferService.Application.LegacyContracts;

public class Global
{
    private const string InstrumentationKey = nameof(InstrumentationKey);

    public virtual Exception? GetLastException()
    {
        // shortcut: ASP.NET Core has no Server.GetLastError; middleware must populate LegacyErrorStore.LastError.
        return LegacyErrorStore.LastError;
    }

    protected void Application_Error(object sender, EventArgs e)
    {
        Exception ex = GetLastException() ?? new Exception();
        Exception actualEx = (ex is TargetInvocationException) ? (ex as TargetInvocationException)!.InnerException ?? ex : ex;

        ServiceException svcException = new ServiceException(actualEx);

        if (actualEx is IFaultGenerator)
        {
            FaultException? customFault = (actualEx as IFaultGenerator)!.GenerateFault();

            if (customFault != null)
            {
                LoggerManager.Logger.LogFatal(FileTransferExceptionConstants.FaultExceptions.CustomFault + customFault.Message, customFault);
                throw customFault;
            }
        }

        LoggerManager.Logger.LogFatal(FileTransferExceptionConstants.FaultExceptions.ApplicationError, actualEx);
        string reason = string.IsNullOrEmpty(svcException.Reason) ? string.Empty : svcException.Reason;

        _ = reason;
        throw new FaultException<ServiceException>(svcException, new FaultReason(svcException.Reason));
    }

    protected void Application_Start(object sender, EventArgs e)
    {
        SetProfiles();
        InitializeInstrumentationSettings();
    }

    private void InitializeInstrumentationSettings()
    {
        string? appInsightInstrumentationKey = Environment.GetEnvironmentVariable(InstrumentationKey);
        if (!string.IsNullOrEmpty(appInsightInstrumentationKey))
        {
            LegacyTelemetryConfiguration.Active.InstrumentationKey = appInsightInstrumentationKey;
        }
    }

    private void SetProfiles()
    {
        Mapper.Initialize(cfg =>
        {
            cfg.DetectAndRegisterProfiles(typeof(InboundTargetProfile).Assembly);
            cfg.DetectAndRegisterProfiles(typeof(IntegrationFileProfile).Assembly);
        });
    }
}


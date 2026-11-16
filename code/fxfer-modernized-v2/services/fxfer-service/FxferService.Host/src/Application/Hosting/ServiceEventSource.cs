using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace FxferService.Application.Hosting;

public sealed class ServiceEventSource
{
    private readonly ILogger _logger;

    public ServiceEventSource(ILogger logger)
    {
        _logger = logger;
    }

    // Legacy: ServiceEventSource.ServiceTypeRegistered(int, string)
    public void ServiceTypeRegistered(int hostProcessId, string serviceType)
    {
        _logger.LogInformation(
            "Service host process {0} registered service type {1}",
            hostProcessId,
            serviceType);
    }

    // Legacy: ServiceEventSource.ServiceHostInitializationFailed(string)
    public void ServiceHostInitializationFailed(string exception)
    {
        using (_logger.BeginScope(new Dictionary<string, object?> { ["exception"] = exception }))
        {
            _logger.LogError("Service host initialization failed");
        }
    }
}

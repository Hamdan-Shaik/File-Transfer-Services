using System.Diagnostics;

namespace Fxfer.SoapFacade.Host.Logging;

public sealed class ServiceEventSource
{
    private readonly ILogger<ServiceEventSource> _logger;

    public ServiceEventSource(ILogger<ServiceEventSource> logger)
    {
        _logger = logger;
    }

    public void Message(string message, params object[] args)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            var finalMessage = string.Format(message, args);
            Message(finalMessage);
        }
    }

    public void Message(string message)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("{Message}", message);
            Activity.Current?.AddEvent(new ActivityEvent("message", tags: new ActivityTagsCollection { { "message", message } }));
        }
    }

    public void ServiceMessage(HttpContext httpContext, string message, params object[] args)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            var finalMessage = string.Format(message, args);

            var serviceName = httpContext.Request.Host.ToString();
            var serviceTypeName = "fxfer-soap-facade";
            var replicaOrInstanceId = (long)Environment.ProcessId;
            var partitionId = Guid.Empty;
            var applicationName = Environment.GetEnvironmentVariable("K8S_NAMESPACE_NAME") ?? string.Empty;
            var applicationTypeName = "fxfer";
            var nodeName = Environment.MachineName;

            ServiceMessage(serviceName, serviceTypeName, replicaOrInstanceId, partitionId, applicationName, applicationTypeName, nodeName, finalMessage);
        }
    }

    public void ServiceMessage(
        string serviceName,
        string serviceTypeName,
        long replicaOrInstanceId,
        Guid partitionId,
        string applicationName,
        string applicationTypeName,
        string nodeName,
        string message)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            using var scope = _logger.BeginScope(new Dictionary<string, object?>
            {
                ["service.name"] = serviceName,
                ["service.type"] = serviceTypeName,
                ["replica_or_instance_id"] = replicaOrInstanceId,
                ["partition_id"] = partitionId,
                ["application.name"] = applicationName,
                ["application.type"] = applicationTypeName,
                ["node.name"] = nodeName
            });

            _logger.LogInformation("{Message}", message);

            Activity.Current?.AddEvent(new ActivityEvent("service_message", tags: new ActivityTagsCollection
            {
                { "service.name", serviceName },
                { "service.type", serviceTypeName },
                { "replica_or_instance_id", replicaOrInstanceId },
                { "partition_id", partitionId },
                { "application.name", applicationName },
                { "application.type", applicationTypeName },
                { "node.name", nodeName },
                { "message", message }
            }));
        }
    }

    public void ServiceTypeRegistered(int hostProcessId, string serviceType)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            var message = string.Format("Service host process {0} registered service type {1}", hostProcessId, serviceType);
            _logger.LogInformation("{Message}", message);
        }
    }

    public void ServiceHostInitializationFailed(string exception)
    {
        if (_logger.IsEnabled(LogLevel.Error))
        {
            using var scope = _logger.BeginScope(new Dictionary<string, object?> { ["exception"] = exception });
            _logger.LogError("{Message}", "Service host initialization failed");
        }
    }

    public void ServiceRequestStart(string requestTypeName)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            var message = string.Format("Service request '{0}' started", requestTypeName);
            _logger.LogInformation("{Message}", message);
        }
    }

    public void ServiceRequestStop(string requestTypeName, string exception = "")
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            var message = string.Format("Service request '{0}' finished", requestTypeName);
            using var scope = string.IsNullOrEmpty(exception) ? null : _logger.BeginScope(new Dictionary<string, object?> { ["exception"] = exception });
            _logger.LogInformation("{Message}", message);
        }
    }
}


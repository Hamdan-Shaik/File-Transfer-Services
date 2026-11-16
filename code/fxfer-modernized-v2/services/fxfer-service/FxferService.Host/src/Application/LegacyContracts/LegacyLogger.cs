using Microsoft.Extensions.Logging;

namespace FxferService.Application.LegacyContracts;

public sealed class LegacyLogger
{
    private readonly ILogger _logger;

    public LegacyLogger(ILogger logger)
    {
        _logger = logger;
    }

    public void LogFatal(string message, Exception exception)
    {
        _logger.LogCritical(exception, message);
    }

    public void LogError(string message)
    {
        _logger.LogError(message);
    }
}


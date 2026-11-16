using Microsoft.Extensions.Logging.Abstractions;

namespace FxferService.Application.LegacyContracts;

public static class LoggerManager
{
    public static LegacyLogger Logger { get; set; } = new(new NullLogger<LegacyLogger>());
}


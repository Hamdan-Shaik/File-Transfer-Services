using Fxfer.Bff.Infrastructure.Downstream;
using Microsoft.Extensions.DependencyInjection;

namespace Fxfer.Bff.Host.Hosting;

public static class StartupChecks
{
    public static async Task RunFxferBffStartupChecks(
        IServiceProvider services,
        IConfiguration configuration,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        _ = configuration;

        try
        {
            using var scope = services.CreateScope();
            var client = scope.ServiceProvider.GetRequiredService<FxferServiceClient>();
            _ = await client.CanReachHealthEndpoint(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "BFF startup checks partially succeeded");
        }
    }
}

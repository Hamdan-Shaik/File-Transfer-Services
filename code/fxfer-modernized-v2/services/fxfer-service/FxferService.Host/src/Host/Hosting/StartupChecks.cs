using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FxferService.Host.Hosting;

public static class StartupChecks
{
    public static async Task RunFileTransferSvcMainStartupChecks(
        IServiceProvider services,
        IConfiguration configuration,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        var tenantsSection = configuration.GetSection("Tenancy:Tenants");
        var tenants = tenantsSection.GetChildren()
            .Select(t => t.Key)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .ToList();

        if (tenants.Count == 0)
        {
            return;
        }

        try
        {
            foreach (var tenant in tenants)
            {
                await CheckServiceHealth(services, tenant, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Service startup partially succeeded");
        }
    }

    private static async Task CheckServiceHealth(
        IServiceProvider services,
        string tenant,
        CancellationToken cancellationToken)
    {
        using var scope = services.CreateScope();

        var requestContext = scope.ServiceProvider.GetRequiredService<RequestContext>();
        requestContext.TenantId = tenant;

        var db = scope.ServiceProvider.GetRequiredService<FxferDbContext>();

        if (db.Database.ProviderName?.Contains("InMemory", StringComparison.OrdinalIgnoreCase) == true)
        {
            return;
        }

        var canConnect = await db.Database.CanConnectAsync(cancellationToken);
        if (!canConnect)
        {
            throw new InvalidOperationException("Database connectivity failed");
        }
    }

}

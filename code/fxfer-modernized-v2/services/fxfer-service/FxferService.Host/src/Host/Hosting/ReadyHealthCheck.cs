using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FxferService.Host.Hosting;

public sealed class ReadyHealthCheck : IHealthCheck
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;

    public ReadyHealthCheck(IServiceScopeFactory scopeFactory, IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        if (_configuration.GetValue("Tenancy:UseInMemoryPerTenant", false))
        {
            return HealthCheckResult.Healthy("InMemory provider");
        }

        var tenantIds = _configuration
            .GetSection("Tenancy:Tenants")
            .GetChildren()
            .Select(t => t.Key)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .ToArray();

        if (tenantIds.Length == 0)
        {
            return HealthCheckResult.Unhealthy("No tenants configured");
        }

        foreach (var tenantId in tenantIds)
        {
            using var scope = _scopeFactory.CreateScope();
            var requestContext = scope.ServiceProvider.GetRequiredService<RequestContext>();
            requestContext.TenantId = tenantId;

            var db = scope.ServiceProvider.GetRequiredService<FxferDbContext>();
            if (db.Database.ProviderName?.Contains("InMemory", StringComparison.OrdinalIgnoreCase) == true)
            {
                continue;
            }

            var canConnect = await db.Database.CanConnectAsync(cancellationToken);
            if (!canConnect)
            {
                return HealthCheckResult.Unhealthy($"Database connectivity failed for tenant {tenantId}");
            }
        }

        return HealthCheckResult.Healthy();
    }
}
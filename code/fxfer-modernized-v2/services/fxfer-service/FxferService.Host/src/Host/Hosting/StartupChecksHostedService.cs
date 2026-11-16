using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;

namespace FxferService.Host.Hosting;

public sealed class StartupChecksHostedService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<StartupChecksHostedService> _logger;

    public StartupChecksHostedService(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<StartupChecksHostedService> logger)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var applyMigrations = _configuration.GetValue("Persistence:ApplyMigrationsAtStartup", true);
        if (!applyMigrations)
        {
            return;
        }

        if (_configuration.GetValue("Tenancy:UseInMemoryPerTenant", false))
        {
            return;
        }

        var tenants = _configuration
            .GetSection("Tenancy:Tenants")
            .GetChildren()
            .Select(t => t.Key)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .ToArray();

        if (tenants.Length == 0)
        {
            return;
        }

        foreach (var tenant in tenants)
        {
            using var scope = _scopeFactory.CreateScope();
            var requestContext = scope.ServiceProvider.GetRequiredService<RequestContext>();
            requestContext.TenantId = tenant;

            var db = scope.ServiceProvider.GetRequiredService<FxferDbContext>();

            if (db.Database.ProviderName?.Contains("InMemory", StringComparison.OrdinalIgnoreCase) == true)
            {
                continue;
            }

            _logger.LogInformation("Applying database migrations for tenant {tenantId}", tenant);
            await db.Database.MigrateAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
using FxferService.Projections.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FxferService.Projections.Host.Hosting;

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

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<FxferProjectionsDbContext>();

        if (db.Database.ProviderName?.Contains("InMemory", StringComparison.OrdinalIgnoreCase) == true)
        {
            return;
        }

        _logger.LogInformation("Applying database migrations");
        await db.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

public sealed class LiveHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}

public sealed class ReadyHealthCheck : IHealthCheck
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ReadyHealthCheck(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<FxferProjectionsDbContext>();

        if (db.Database.ProviderName?.Contains("InMemory", StringComparison.OrdinalIgnoreCase) == true)
        {
            return HealthCheckResult.Healthy("InMemory provider");
        }

        var canConnect = await db.Database.CanConnectAsync(cancellationToken);
        return canConnect ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy("Database connectivity failed");
    }
}

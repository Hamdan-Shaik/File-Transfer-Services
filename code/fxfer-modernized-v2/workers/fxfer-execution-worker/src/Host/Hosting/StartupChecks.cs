using Fxfer.ExecutionWorker.Infrastructure.DomainApi;
using Fxfer.ExecutionWorker.Infrastructure.Secrets;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Fxfer.ExecutionWorker.Host.Hosting;

public sealed class StartupChecksHostedService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;
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
    private readonly FxferServiceClient _fxferService;
    private readonly SecretsResolver _secrets;

    public ReadyHealthCheck(FxferServiceClient fxferService, SecretsResolver secrets)
    {
        _fxferService = fxferService;
        _secrets = secrets;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var fxferOk = await _fxferService.CanReachAsync(cancellationToken);
        if (!fxferOk)
        {
            return HealthCheckResult.Unhealthy("fxfer-service reachability check failed");
        }

        var secretsOk = await _secrets.CanResolveAsync(cancellationToken);
        if (!secretsOk)
        {
            return HealthCheckResult.Unhealthy("secrets resolution check failed");
        }

        return HealthCheckResult.Healthy();
    }
}

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FxferService.Host.Hosting;

public sealed class LiveHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}


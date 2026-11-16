using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Fxfer.Bff.Host.Hosting;

public sealed class LiveHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        _ = context;
        _ = cancellationToken;
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}

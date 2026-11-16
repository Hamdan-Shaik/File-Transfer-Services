using Fxfer.Bff.Infrastructure.Downstream;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Fxfer.Bff.Host.Hosting;

public sealed class ReadyHealthCheck : IHealthCheck
{
    private readonly FxferServiceClient _client;

    public ReadyHealthCheck(FxferServiceClient client)
    {
        _client = client;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        _ = context;
        return await _client.CanReachHealthEndpoint(cancellationToken)
            ? HealthCheckResult.Healthy()
            : HealthCheckResult.Unhealthy("Downstream fxfer-service reachability failed");
    }
}

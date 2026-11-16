using Fxfer.SchedulerWorker.Infrastructure.DomainApi;
using Fxfer.SchedulerWorker.Infrastructure.Messaging.Publisher;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Fxfer.SchedulerWorker.Host.Hosting;

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
    private readonly TransferRunRequestedPublisher _publisher;

    public ReadyHealthCheck(FxferServiceClient fxferService, TransferRunRequestedPublisher publisher)
    {
        _fxferService = fxferService;
        _publisher = publisher;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var fxferOk = await _fxferService.CanReachAsync(cancellationToken);
        if (!fxferOk)
        {
            return HealthCheckResult.Unhealthy("fxfer-service reachability check failed");
        }

        var publishOk = await _publisher.CanPublishAsync(cancellationToken);
        if (!publishOk)
        {
            return HealthCheckResult.Unhealthy("work-item publish check failed");
        }

        return HealthCheckResult.Healthy();
    }
}

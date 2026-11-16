using Fxfer.SchedulerWorker.Application.Scheduling;

namespace Fxfer.SchedulerWorker.Host.Hosting;

public sealed class WorkerHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _tickInterval;
    private readonly ILogger<WorkerHostedService> _logger;

    public WorkerHostedService(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<WorkerHostedService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;

        var seconds = configuration.GetValue<int?>("Scheduling:TickSeconds") ?? 30;
        seconds = Math.Clamp(seconds, 1, 3600);
        _tickInterval = TimeSpan.FromSeconds(seconds);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Fxfer scheduler worker started");

        using var timer = new PeriodicTimer(_tickInterval);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            using var scope = _scopeFactory.CreateScope();
            var loop = scope.ServiceProvider.GetRequiredService<DispatchLoop>();
            await loop.DispatchAsync(DateTimeOffset.UtcNow, stoppingToken);
        }
    }
}

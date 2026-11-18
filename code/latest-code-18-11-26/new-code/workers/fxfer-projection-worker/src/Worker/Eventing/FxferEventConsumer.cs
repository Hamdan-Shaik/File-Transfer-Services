using Fxfer.ProjectionWorker.Worker.Configuration;
using Microsoft.Extensions.Options;

namespace Fxfer.ProjectionWorker.Worker.Eventing;

public sealed class FxferEventConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<EventBackboneConsumerOptions> _options;
    private readonly ILogger<FxferEventConsumer> _logger;

    public FxferEventConsumer(
        IServiceProvider serviceProvider,
        IOptions<EventBackboneConsumerOptions> options,
        ILogger<FxferEventConsumer> logger)
    {
        _serviceProvider = serviceProvider;
        _options = options;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = _options.Value;

        if (!config.Enabled)
        {
            _logger.LogInformation("Event consumer disabled (EventBackboneConsumer:Enabled=false)");
            await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
            return;
        }

        _logger.LogInformation(
            "Starting event consumer (subscription={Subscription})",
            config.SubscriptionName ?? "(unset)");

        while (!stoppingToken.IsCancellationRequested)
        {
            // skeleton: event adapter subscription consumption is wired in later.
            await Task.Delay(TimeSpan.FromSeconds(Math.Max(1, config.PollIntervalSeconds)), stoppingToken);

            _ = _serviceProvider;
        }
    }
}


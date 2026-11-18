using System.Diagnostics;
using Fxfer.TransferWorker.Worker.Configuration;
using Fxfer.TransferWorker.Worker.Services;
using Microsoft.Extensions.Options;

namespace Fxfer.TransferWorker.Worker.Eventing;

public sealed class TransferWorkItemConsumer : BackgroundService
{
    public const string ActivitySourceName = "Fxfer.TransferWorker.WorkItemConsumer";
    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    private readonly IOptions<EventBackboneConsumerOptions> _options;
    private readonly CorrelationIdContext _correlationIdContext;
    private readonly ILogger<TransferWorkItemConsumer> _logger;

    public TransferWorkItemConsumer(
        IOptions<EventBackboneConsumerOptions> options,
        CorrelationIdContext correlationIdContext,
        ILogger<TransferWorkItemConsumer> logger)
    {
        _options = options;
        _correlationIdContext = correlationIdContext;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = _options.Value;

        if (!config.Enabled)
        {
            _logger.LogInformation("Transfer work item consumer disabled (EventBackboneConsumer:Enabled=false)");
            await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
            return;
        }

        _logger.LogInformation(
            "Starting transfer work item consumer (subscription={Subscription})",
            config.SubscriptionName ?? "(unset)");

        while (!stoppingToken.IsCancellationRequested)
        {
            // skeleton: event adapter subscription consumption + DLQ behavior is wired in later.
            await Task.Delay(TimeSpan.FromSeconds(Math.Max(1, config.PollIntervalSeconds)), stoppingToken);

            using var activity = ActivitySource.StartActivity("work_item.poll", ActivityKind.Consumer);
            activity?.SetTag("messaging.operation.type", "receive");

            _correlationIdContext.Set(Guid.NewGuid().ToString());
        }
    }
}


namespace Fxfer.TransferWorker.Worker.Configuration;

public sealed class EventBackboneConsumerOptions
{
    public const string SectionName = "EventBackboneConsumer";

    public bool Enabled { get; init; }

    public string? EventAdapterBaseUrl { get; init; }

    public string? SubscriptionName { get; init; }

    public int PollIntervalSeconds { get; init; } = 5;

    public int TimeoutSeconds { get; init; } = 10;
}


namespace Fxfer.TransferWorker.Worker.Configuration;

public sealed class TransferExecutionOptions
{
    public const string SectionName = "TransferExecution";

    public int MaxAttempts { get; init; } = 3;

    public int AttemptBackoffBaseMs { get; init; } = 200;
}


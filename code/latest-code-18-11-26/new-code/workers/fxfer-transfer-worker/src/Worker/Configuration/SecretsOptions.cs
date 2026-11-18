namespace Fxfer.TransferWorker.Worker.Configuration;

public sealed class SecretsOptions
{
    public const string SectionName = "Secrets";

    public string? Provider { get; init; }

    public string? Prefix { get; init; }
}


namespace Fxfer.TransferWorker.Worker.Configuration;

public sealed class DomainApiClientOptions
{
    public const string SectionName = "DomainApi";

    public string? BaseUrl { get; init; }

    public int TimeoutSeconds { get; init; } = 10;
}


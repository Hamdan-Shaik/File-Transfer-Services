namespace Fxfer.TransferWorker.Worker.Configuration;

public sealed class StagingOptions
{
    public const string SectionName = "Staging";

    public string? RootPath { get; init; }
}


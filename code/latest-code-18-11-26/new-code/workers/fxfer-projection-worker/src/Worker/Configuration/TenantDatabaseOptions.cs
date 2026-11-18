namespace Fxfer.ProjectionWorker.Worker.Configuration;

public sealed class TenantDatabaseOptions
{
    public const string SectionName = "TenantDatabase";

    public string? DefaultConnectionString { get; init; }
}


namespace Fxfer.DomainApi.Contracts.Queries.Parameters;

public sealed class IntegrationFileQueryParms
{
    // F03-006: Defaults FTSIntegrationFileId to Guid.Empty (no id filter when empty).
    public IntegrationFileQueryParms()
    {
        FTSIntegrationFileId = Guid.Empty;
    }

    public Guid FTSIntegrationFileId { get; set; }

    public string? FileName { get; set; }
}
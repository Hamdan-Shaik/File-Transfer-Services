namespace Fxfer.DomainApi.Contracts.Queries.Parameters;

public sealed class InboundTargetQueryParms
{
    // F05-031: Defaults FTEMetadataId to Guid.Empty.
    public InboundTargetQueryParms()
    {
        FTEMetadataId = Guid.Empty;
    }

    public Guid FTEMetadataId { get; set; }

    public string? ContractName { get; set; }
}
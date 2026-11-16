namespace FileTransferSvcQueryService;

public sealed class InboundTargetQueryParms
{
    public Guid FTEMetadataId { get; set; }
    public string? ContractName { get; set; }
}
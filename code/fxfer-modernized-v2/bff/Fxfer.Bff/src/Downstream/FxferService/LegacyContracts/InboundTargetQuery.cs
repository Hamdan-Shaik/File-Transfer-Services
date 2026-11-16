namespace FileTransferSvcQueryService;

public sealed class InboundTargetQuery
{
    public RequestorModel? Requestor { get; set; }
    public InboundTargetQueryParms? Where { get; set; }
    public List<FtsInboundTargetModel>? Results { get; set; }
}
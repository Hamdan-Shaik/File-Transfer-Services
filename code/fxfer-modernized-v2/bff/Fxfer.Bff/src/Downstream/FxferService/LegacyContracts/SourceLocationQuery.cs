namespace FileTransferSvcQueryService;

public sealed class SourceLocationQuery
{
    public RequestorModel? Requestor { get; set; }
    public SourceLocationQueryParms? Where { get; set; }
    public List<SourceLocationModel>? Results { get; set; }
}
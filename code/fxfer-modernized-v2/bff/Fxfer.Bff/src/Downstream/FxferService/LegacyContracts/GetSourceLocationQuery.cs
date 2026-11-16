namespace FileTransferSvcQueryService;

public sealed class GetSourceLocationQuery
{
    public RequestorModel? Requestor { get; set; }
    public GetSourceLocationQueryParms? Where { get; set; }
    public List<SourceLocationModel>? Results { get; set; }
}


namespace FileTransferSvcQueryService;

public sealed class FtpServerQuery
{
    public RequestorModel? Requestor { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public List<SortCriteria>? SortCriteria { get; set; }
    public FtpServerQueryParms? Where { get; set; }
    public List<FtpServerModel>? Results { get; set; }
    public int RowCount { get; set; }
}


namespace FileTransferSvcQueryService;

public sealed class FileTransferTrackingQuery
{
    public RequestorModel? Requestor { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public List<SortCriteria>? SortCriteria { get; set; }
    public FileTransferTrackingQueryParms? Where { get; set; }
    public List<FileTransferTrackingModel>? Results { get; set; }
    public int RowCount { get; set; }
}


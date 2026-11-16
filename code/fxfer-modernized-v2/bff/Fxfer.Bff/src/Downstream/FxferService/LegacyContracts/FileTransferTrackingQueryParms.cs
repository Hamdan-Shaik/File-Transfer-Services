namespace FileTransferSvcQueryService;

public sealed class FileTransferTrackingQueryParms
{
    public string? FileId { get; set; }
    public string? FileName { get; set; }
    public string? FileStatus { get; set; }
    public string? FileDirection { get; set; }
    public string? FileTransportType { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? FileSizeFrom { get; set; }
    public decimal? FileSizeThrough { get; set; }
    public string? TradingPartner { get; set; }
    public string? FileSourceLocationId { get; set; }
}


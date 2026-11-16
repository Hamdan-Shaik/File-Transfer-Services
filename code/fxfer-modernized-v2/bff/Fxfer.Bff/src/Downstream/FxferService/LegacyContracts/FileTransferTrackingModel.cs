namespace FileTransferSvcQueryService;

public sealed class FileTransferTrackingModel
{
    public string? FileId { get; set; }
    public string? FileName { get; set; }
    public string? FileDirection { get; set; }
    public string? FileTransportType { get; set; }
    public string? FileStatus { get; set; }
    public DateTime CreatedDate { get; set; }
    public decimal FileSize { get; set; }
    public string? TradingPartner { get; set; }
    public string? SourceLocConfigName { get; set; }
    public string? SourceLocAddress { get; set; }
    public List<FileEventModel> Events { get; set; } = new();
    public List<FileExceptionModel> Exceptions { get; set; } = new();
}


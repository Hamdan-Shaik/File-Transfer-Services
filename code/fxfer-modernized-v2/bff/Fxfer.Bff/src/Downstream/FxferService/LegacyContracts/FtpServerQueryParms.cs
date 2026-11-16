namespace FileTransferSvcQueryService;

public sealed class FtpServerQueryParms
{
    public Guid FtpServerId { get; set; }
    public string? FtpServerName { get; set; }
    public string? FtpDescription { get; set; }
    public string? FtpUser { get; set; }
    public bool? IsValid { get; set; }
    public List<SearchModeOption>? SearchOptions { get; set; }
}


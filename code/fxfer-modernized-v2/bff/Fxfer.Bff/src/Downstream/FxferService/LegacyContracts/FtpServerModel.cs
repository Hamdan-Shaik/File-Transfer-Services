namespace FileTransferSvcQueryService;

public sealed class FtpServerModel
{
    public Guid FtpServerId { get; set; }
    public string? FtpServerName { get; set; }
    public string? FtpServerIp { get; set; }
    public string? FtpTransferMode { get; set; }
    public string? FtpUser { get; set; }
    public string? FtpPassword { get; set; }
    public string? FtpDescription { get; set; }
    public short FtpPort { get; set; }
    public string? FtpProxyAddress { get; set; }
    public short? FtpProxyPort { get; set; }
    public bool IsValid { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public DateTime CreationDate { get; set; }
    public string? OperatorId { get; set; }
}


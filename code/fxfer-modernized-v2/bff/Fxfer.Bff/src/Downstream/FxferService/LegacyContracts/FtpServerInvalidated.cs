namespace FileTransferConfig;

public sealed class FtpServerInvalidated
{
    public Guid FtpServerId { get; set; }
    public bool IsValid { get; set; }
}

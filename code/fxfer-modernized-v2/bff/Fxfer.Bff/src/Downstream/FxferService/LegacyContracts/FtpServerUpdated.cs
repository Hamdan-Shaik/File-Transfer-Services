namespace FileTransferConfig;

public sealed class FtpServerUpdated
{
    public Guid FtpServerId { get; set; }
    public bool IsValid { get; set; }
}

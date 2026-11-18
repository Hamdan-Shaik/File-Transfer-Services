namespace Fxfer.TransferWorker.Contracts.Batch;

public sealed class FtpServerModel
{
    public Guid FtpServerId { get; set; }

    public string? FtpServerName { get; set; }

    public string? FtpServerIp { get; set; }

    public string? FtpTransferMode { get; set; }

    public string? FtpUser { get; set; }

    public string? FtpPassword { get; set; }

    public int FtpPort { get; set; }

    public bool SSHAuth { get; set; }
}


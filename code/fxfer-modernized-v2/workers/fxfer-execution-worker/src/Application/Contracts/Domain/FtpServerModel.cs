using System;

namespace Fxfer.ExecutionWorker.Application.Contracts.Domain;

public class FtpServerModel
{
    public Guid FtpServerId { get; set; }

    public string FtpServerName { get; set; } = null!;

    public string FtpServerIp { get; set; } = null!;

    public string FtpTransferMode { get; set; } = null!;

    public string FtpUser { get; set; } = null!;

    public string FtpPassword { get; set; } = null!;

    public int FtpPort { get; set; }

    public bool SSHAuth { get; set; }
}

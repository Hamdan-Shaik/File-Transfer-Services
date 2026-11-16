namespace FxferService.Domain.Contracts.Batch;

public class FtpServerModel
{
    /// <summary>
    /// <para>FTP Server model details.</para>
    /// <list type="bullet">
    /// <item><term>FtpServerId</term><description>MaxLength 36</description></item>
    /// <item><term>FtpServerName</term><description>MaxLength 50</description></item>
    /// <item><term>FtpServerIp</term><description>MaxLength 15</description></item>
    /// <item><term>Ports</term><description>Valid Values -32,768 to 32,7672</description></item>
    /// <item><term>Proxy</term><description>MaxLength 200</description></item>
    /// </list>
    /// </summary>
    /// <summary>
    /// <para>Unique identifier for the ftp server location.</para>
    /// </summary>
    public Guid FtpServerId { get; set; }

    /// <summary>
    /// <para>The name of the ftp server.</para>
    /// </summary>
    public string? FtpServerName { get; set; }

    /// <summary>
    /// <para>The IP address of the ftp server.</para>
    /// </summary>
    public string? FtpServerIp { get; set; }

    /// <summary>
    /// <para>Transfer mode for the ftp server.</para>
    /// </summary>
    public string? FtpTransferMode { get; set; }

    /// <summary>
    /// <para>Name of the user who have permission for this ftp server.</para>
    /// </summary>
    public string? FtpUser { get; set; }

    /// <summary>
    /// <para>Passsword for the ftp server.</para>
    /// </summary>
    public string? FtpPassword { get; set; }
}


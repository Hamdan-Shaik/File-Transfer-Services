namespace Fxfer.DomainApi.Contracts.Domain;

public sealed record FtpServerUpsertRequest
{
    /// <summary>
    /// <para>The unique Name assigned to this server.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>AlphaNumeric</description></item>
    /// <item><term>MaxLength</term><description>50</description></item>
    /// </list>
    /// </summary>
    public required string FtpServerName { get; init; }

    /// <summary>
    /// <para>The FTP Server IP Address.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>AlphaNumeric</description></item>
    /// <item><term>MaxLength</term><description>15</description></item>
    /// </list>
    /// </summary>
    public required string FtpServerIp { get; init; }

    /// <summary>
    /// <para>The File Transfer Direction, Inbound or Outbound.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Char</description></item>
    /// <item><term>MaxLength</term><description>1</description></item>
    /// <item><term>Valid Values</term><description>1 or 2</description></item>
    /// </list>
    /// </summary>
    public required string FtpTransferMode { get; init; }

    /// <summary>
    /// <para>The FTP Server User Id.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Alphanumeric</description></item>
    /// <item><term>MaxLength</term><description>50</description></item>
    /// </list>
    /// </summary>
    public required string FtpUser { get; init; }

    /// <summary>
    /// <para>The FTP Server password.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Alphanumeric</description></item>
    /// <item><term>MaxLength</term><description>50</description></item>
    /// </list>
    /// </summary>
    public string? CredentialSecretRef { get; init; }

    /// <summary>
    /// <para>FTP Server description.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>AlphaNumeric</description></item>
    /// <item><term>MaxLength</term><description>100</description></item>
    /// </list>
    /// </summary>
    public required string FtpDescription { get; init; }

    /// <summary>
    /// <para>The FTP Server Port.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>short</description></item>
    /// <item><term>MaxLength</term><description>5</description></item>
    /// <item><term>Valid Values</term><description>-32,768 to 32,7672</description></item>
    /// </list>
    /// </summary>
    public int FtpPort { get; init; }

    /// <summary>
    /// <para>IP Address of the proxy to use for the connection.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>No</description></item>
    /// <item><term>Format</term><description>Alphanumeric</description></item>
    /// <item><term>MaxLength</term><description>200</description></item>
    /// </list>
    /// </summary>
    public string? FtpProxyAddress { get; init; }

    /// <summary>
    /// <para>Port of the proxy to use for the connection.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>No</description></item>
    /// <item><term>Format</term><description>short</description></item>
    /// <item><term>MaxLength</term><description>5</description></item>
    /// <item><term>Valid Values</term><description>-32,768 to 32,7672</description></item>
    /// </list>
    /// </summary>
    public int? FtpProxyPort { get; init; }

    /// <summary>
    /// <para>UseSSHAuth</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Boolean</description></item>
    /// <item><term>Valid Values</term><description>true or false</description></item>
    /// </list>
    /// </summary>
    public bool UseSshAuth { get; init; }

    /// <summary>
    /// <para>IsValid</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Boolean</description></item>
    /// <item><term>Valid Values</term><description>true or false</description></item>
    /// </list>
    /// </summary>
    public bool IsValid { get; init; }

    /// <summary>
    /// <para>OperatorId</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>AlphaNumeric</description></item>
    /// <item><term>MaxLength</term><description>200</description></item>
    /// </list>
    /// </summary>
    public required string OperatorId { get; init; }
}


using System.Runtime.Serialization;

using FxferService.Domain.Utilities;

namespace FxferService.Domain.Contracts.Queries.Parameters;

/// <summary>
/// <para>Query parameters for Ftp Server</para>
/// </summary>
[DataContract]
public class FtpServerQueryParms : SerializableBase
{
    #region Properties
    /// <summary>
    /// <para>Flag to identify if it is a valid FTP Server record.</para>
    /// <list type=bullet>
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Boolean</description></item>
    /// <item><term>Valid Values</term><description>true or false</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public bool? IsValid { get; set; }

    /// <summary>
    /// <para>The internal identifier for the FtpServer record.</para>
    /// <list type=bullet>
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Guid</description></item>
    /// <item><term>MaxLength</term><description>36</description></item>
    /// <item><term>Valid Values</term><description>XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public Guid FtpServerId { get; set; }

    /// <summary>
    /// <para>The unique Name assigned to FTP Server.</para>
    /// <list type=bullet>
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>AlphaNumeric</description></item>
    /// <item><term>MaxLength</term><description>50/description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public string FtpServerName { get; set; } = null!;

    /// <summary>
    /// <para>Brief description of the FTP Server.</para>
    /// <list type=bullet>
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>AlphaNumeric</description></item>
    /// <item><term>MaxLength</term><description>100</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public string FtpDescription { get; set; } = null!;

    /// <summary>
    /// <para>The FTP Server Username.</para>
    /// <list type=bullet>
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>AlphaNumeric</description></item>
    /// <item><term>MaxLength</term><description>50</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public string FtpUser { get; set; } = null!;

    [DataMember]
    public List<SearchModeOption> SearchOptions { get; set; } = null!;
    #endregion
}


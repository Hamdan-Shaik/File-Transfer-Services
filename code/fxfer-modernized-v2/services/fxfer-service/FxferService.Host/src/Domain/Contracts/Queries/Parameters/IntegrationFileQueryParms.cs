using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Queries.Parameters;

/// <summary>
/// <para>Parameters to get from Integration File Table</para>
/// </summary>
[DataContract]
public class IntegrationFileQueryParms
{
    #region Constructor
    public IntegrationFileQueryParms()
    {
        this.FTSIntegrationFileId = Guid.Empty;
    }
    #endregion

    #region Properties
    /// <summary>
    /// <para>key that identify an Integration File record</para>
    /// <list type=bullet>
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Guid</description></item>
    /// <item><term>MaxLength</term><description>36</description></item>
    /// <item><term>Valid Values</term><description>XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public Guid FTSIntegrationFileId { get; set; }

    /// <summary>
    /// <para>File Name</para>
    /// <list type=bullet>
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>AlphaNumeric</description></item>
    /// <item><term>MaxLength</term><description>200</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public string FileName { get; set; } = null!;
    #endregion
}


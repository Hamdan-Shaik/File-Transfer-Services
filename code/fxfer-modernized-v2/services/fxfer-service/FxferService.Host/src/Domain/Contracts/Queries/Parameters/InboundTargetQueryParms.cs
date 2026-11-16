using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Queries.Parameters;

/// <summary>
/// <para>Parameters to get from Inbound Target Table</para>
/// </summary>
[DataContract]
public class InboundTargetQueryParms
{
    #region Constructor
    public InboundTargetQueryParms()
    {
        this.FTEMetadataId = Guid.Empty;
    }
    #endregion

    #region Properties
    /// <summary>
    /// <para>key identify an External File Definition record</para>
    /// <list type=bullet>
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Guid</description></item>
    /// <item><term>MaxLength</term><description>36</description></item>
    /// <item><term>Valid Values</term><description>XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public Guid FTEMetadataId { get; set; }

    /// <summary>
    /// <para>Contract Name</para>
    /// <list type=bullet>
    /// <item><term>Required</term><description>No</description></item>
    /// <item><term>Format</term><description>AlphaNumeric</description></item>
    /// <item><term>MaxLength</term><description>200</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public string ContractName { get; set; } = null!;
    #endregion
}


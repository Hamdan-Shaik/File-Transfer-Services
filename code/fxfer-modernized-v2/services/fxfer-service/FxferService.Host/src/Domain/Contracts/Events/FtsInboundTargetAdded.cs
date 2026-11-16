using System;
using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Events;

public class FtsInboundTargetAdded : FileTransferEvents
{
    #region Constructor
    /// <exclude/>
    public FtsInboundTargetAdded()
    {
    }
    #endregion

    #region Properties
    /// <summary>
    /// <para>Inbound Target File Unique Identifier</para>        
    /// <list type=bullet>        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Guid</description></item>        
    /// <item><term>MaxLength</term><description>Guid</description></item>
    /// <item><term>Valid Values</term><description>Guid</description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public Guid FtsInboundTargetId { get; set; }

    /// <summary>
    /// <para>MetadataId value from the inbound file</para>        
    /// <list type=bullet>        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Guid</description></item>        
    /// <item><term>MaxLength</term><description>Guid</description></item>
    /// <item><term>Valid Values</term><description>Guid</description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public Guid FTEMetadataId { get; set; }

    /// <summary>
    /// <para>Name of the Import Job</para>        
    /// <list type=bullet>        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>string</description></item>        
    /// <item><term>MaxLength</term><description>string</description></item>
    /// <item><term>Valid Values</term><description>string</description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public string? ImportJobName { get; set; }

    /// <summary>
    /// <para>Name of the Contract</para>        
    /// <list type=bullet>        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>string</description></item>        
    /// <item><term>MaxLength</term><description>string</description></item>
    /// <item><term>Valid Values</term><description>string</description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public string? ContractName { get; set; }

    /// <summary>
    /// <para>Operator Id</para>        
    /// <list type=bullet>        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>string</description></item>        
    /// <item><term>MaxLength</term><description>string</description></item>
    /// <item><term>Valid Values</term><description>string</description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public string? OperatorId { get; set; }

    /// <summary>
    /// <para>Is Valid</para>        
    /// <list type=bullet>        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>bool</description></item>        
    /// <item><term>MaxLength</term><description>bool</description></item>
    /// <item><term>Valid Values</term><description>true or false</description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public bool IsValid { get; set; }
    #endregion
}

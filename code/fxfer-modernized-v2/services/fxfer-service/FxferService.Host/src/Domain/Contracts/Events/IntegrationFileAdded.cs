using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FxferService.Domain.Contracts.Domain;

namespace FxferService.Domain.Contracts.Events;

public class IntegrationFileAdded : FileTransferEvents
{
    #region Constructor
    /// <exclude/>
    public IntegrationFileAdded()
    {
    }
    #endregion

    #region Properties
    /// <summary>
    /// <para>Integration File Unique Identifier</para>        
    /// <list type="bullet">        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Guid</description></item>        
    /// <item><term>MaxLength</term><description>Guid</description></item>
    /// <item><term>Valid Values</term><description>Guid</description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public Guid FtsIntegrationFileId { get; set; }

    /// <summary>
    /// <para>GroupIds values from the integration file</para>        
    /// <list type="bullet">        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>List</description></item>        
    /// <item><term>Valid Values</term><description><see cref="GroupInfo"/></description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public List<GroupInfo>? GroupInfo { get; set; }

    /// <summary>
    /// <para>ConfigId value from the integration file</para>        
    /// <list type="bullet">        
    /// <item><term>Required</term><description>No</description></item>
    /// <item><term>Format</term><description>Numeric</description></item>  
    /// <item><term>MaxLength</term><description>5</description></item>
    /// <item><term>Valid Values</term><description>-32,768 to 32,7672</description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public int? ConfigId { get; set; }

    /// <summary>
    /// <para>Name of the File</para>        
    /// <list type="bullet">        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>string</description></item>        
    /// <item><term>MaxLength</term><description>string</description></item>
    /// <item><term>Valid Values</term><description>string/description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public string? FileName { get; set; }

    /// <summary>
    /// <para>Metadata Id value from the integration file</para>        
    /// <list type="bullet">        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Guid</description></item>        
    /// <item><term>MaxLength</term><description>Guid</description></item>
    /// <item><term>Valid Values</term><description>Guid</description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public Guid ETFOutboundMetadataId { get; set; }

    /// <summary>
    /// <para>Is Processed</para>        
    /// <list type="bullet">        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>bool</description></item>        
    /// <item><term>MaxLength</term><description>bool</description></item>
    /// <item><term>Valid Values</term><description>true or false</description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public bool IsProcessed { get; set; }

    /// <summary>
    /// <para>Operator Id</para>        
    /// <list type="bullet">        
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
    /// <list type="bullet">        
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

using System;
using System.Runtime.Serialization;

#nullable disable


namespace FxferService.Domain.Contracts.Domain;

/// <summary>
/// <para>Object containing properties of an Inbound File.</para>
/// </summary>
[DataContract]
public class FtsInboundTargetModel
{
    #region Constructors
    /// <exclude/>
    public FtsInboundTargetModel()
    {
    }
    #endregion

    #region Properties
    /// <summary>
    /// <para>FtsInboundTargetId</para>        
    /// <list type="bullet">        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Guid</description></item>        
    /// <item><term>MaxLength</term><description>Guid</description></item>
    /// <item><term>Valid Values</term><description>Guid</description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public Guid FtsInboundTargetId { get; set; }

    /// <summary>
    /// <para>FTEMetadataId</para>        
    /// <list type="bullet">        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Guid</description></item>        
    /// <item><term>MaxLength</term><description>Guid</description></item>
    /// <item><term>Valid Values</term><description>Guid</description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public Guid FTEMetadataId { get; set; }

    /// <summary>
    /// <para>Name of the import job </para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Text</description></item>        
    /// <item><term>MaxLength</term><description>200</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public string ImportJobName { get; set; }

    /// <summary>
    /// <para>Name of the contract</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Text</description></item>        
    /// <item><term>MaxLength</term><description>200</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public string ContractName { get; set; }

    /// <summary>
    /// <para>Is Valid</para>        
    /// <list type="bullet">        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>bool</description></item>        
    /// <item><term>MaxLength</term><description>1</description></item>
    /// <item><term>Valid Values</term><description>true or false</description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public bool IsValid { get; set; }

    /// <summary>
    /// <para>Date and time the Inbound Target record was created</para>        
    /// <list type="bullet">        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>DateTime</description></item>        
    /// <item><term>Valid Values</term><description>dd-MM-YY HH:MI:SS</description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// <para>Date and time the Inbound Target record was last modified</para>        
    /// <list type="bullet">        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>DateTime</description></item>        
    /// <item><term>Valid Values</term><description>dd-MM-YY HH:MI:SS</description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public DateTime LastModifiedDate { get; set; }
    #endregion
}

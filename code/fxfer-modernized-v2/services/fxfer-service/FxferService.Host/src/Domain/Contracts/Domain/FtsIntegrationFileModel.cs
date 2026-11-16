using System;
using System.Runtime.Serialization;

#nullable disable


namespace FxferService.Domain.Contracts.Domain;

/// <summary>
/// <para>Object containing properties of an Integration File.</para>
/// </summary>
[DataContract]
public class FtsIntegrationFileModel
{
    #region Constructors
    /// <exclude/>
    public FtsIntegrationFileModel()
    {
    }
    #endregion

    #region Properties
    /// <summary>
    /// <para>FtsIntegrationFileId</para>        
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
    /// <para>GroupId</para>        
    /// <list type="bullet">        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Guid</description></item>        
    /// <item><term>MaxLength</term><description>Guid</description></item>
    /// <item><term>Valid Values</term><description>Guid</description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public Guid? GroupId { get; set; }

    /// <summary>
    /// <para>ConfigId</para>        
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
    /// <para>Name of the integration file </para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Text</description></item>        
    /// <item><term>MaxLength</term><description>500</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public string FileName { get; set; }

    /// <summary>
    /// <para>ETFOutboundMetadataDefnId</para>        
    /// <list type="bullet">        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Guid</description></item>        
    /// <item><term>MaxLength</term><description>Guid</description></item>
    /// <item><term>Valid Values</term><description>Guid</description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public Guid ETFOutboundMetadataDefnId { get; set; }

    /// <summary>
    /// <para>Is Processed</para>        
    /// <list type="bullet">        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>bool</description></item>        
    /// <item><term>MaxLength</term><description>1</description></item>
    /// <item><term>Valid Values</term><description>true or false</description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public bool IsProcessed { get; set; }

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
    /// <para>Date and time that the Integration File record was created</para>        
    /// <list type="bullet">        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>DateTime</description></item>        
    /// <item><term>Valid Values</term><description>dd-MM-YY HH:MI:SS</description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// <para>Date and time that the Integration File record was last modified</para>        
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

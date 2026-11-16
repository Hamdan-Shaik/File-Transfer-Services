using System.ComponentModel;
using System.Runtime.Serialization;

namespace FxferService.Domain.Commands;

/// <summary>
/// <para>Input to add an Inbound target file.</para>
/// </summary>
[DataContract]
public class AddFtsInboundTargetCommand
{
    #region Properties

    /// <summary>
    /// <para>The Metadata identifier coming from FTEMetadataUpdated event</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Guid</description></item>
    /// <item><term>MaxLength</term><description>36</description></item>
    /// <item><term>Valid Values</term><description>XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public Guid FTEMetadataId { get; set; }

    /// <summary>
    /// <para>Name of the import job.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>AlphaNumeric</description></item>        
    /// <item><term>MaxLength</term><description>200</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public string? ImportJobName { get; set; }

    /// <summary>
    /// <para>Name of the contract.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>AlphaNumeric</description></item>        
    /// <item><term>MaxLength</term><description>200</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public string? ContractName { get; set; }

    /// <summary>
    /// <para>Status of the record (valid or invalid)</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>No</description></item>
    /// <item><term>Format</term><description>Boolean</description></item>
    /// <item><term>Valid Values</term><description>true or false</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    [DefaultValue(true)]
    public bool IsValid { get; set; }

    #endregion
}

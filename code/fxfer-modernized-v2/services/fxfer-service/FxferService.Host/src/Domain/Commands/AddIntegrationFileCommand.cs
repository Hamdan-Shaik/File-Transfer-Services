using System.ComponentModel;
using System.Runtime.Serialization;

using FxferService.Domain.Contracts.Domain;

namespace FxferService.Domain.Commands;

/// <summary>
/// <para>Input to add an Integration file record.</para>
/// </summary>
[DataContract]
public class AddIntegrationFileCommand
{
    #region Properties

    /// <summary>
    /// <para>The GroupIds coming from ExtractFileGenerated event</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>List</description></item>        
    /// <item><term>Valid Values</term><description><see cref="GroupInfo"/></description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public List<GroupInfo>? GroupInfo { get; set; }

    /// <summary>
    /// <para>Name of the file.</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>AlphaNumeric</description></item>        
    /// <item><term>MaxLength</term><description>500</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public string? FileName { get; set; }

    /// <summary>
    /// <para>The Metadata identifier coming from ExtractFileGenerated event</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Guid</description></item>
    /// <item><term>MaxLength</term><description>36</description></item>
    /// <item><term>Valid Values</term><description>XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public Guid ETFOutboundMetadataDefnId { get; set; }

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

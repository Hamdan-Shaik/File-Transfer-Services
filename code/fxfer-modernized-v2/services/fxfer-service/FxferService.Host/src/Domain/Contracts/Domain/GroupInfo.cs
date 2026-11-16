using System;
using System.Runtime.Serialization;

#nullable disable


namespace FxferService.Domain.Contracts.Domain;

[DataContract]
public class GroupInfo
{
    #region Properties
    /// <summary>
    /// <para>System generated unique identifier for the group record</para>
    /// <list type="bullet">
    /// <item><term>Format</term><description>Guid</description></item>
    /// <item><term>MaxLength</term><description>36</description></item>
    /// <item><term> Valid Values</term><description>XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXX</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public Guid? GroupId { get; set; }

    /// <summary>
    /// <para>Number of Outbound Records in Extract File</para>
    /// <list type="bullet">
    /// <item><term>Format</term><description>int</description></item>
    /// <item><term>MaxLength</term><description>-2,147,483,648 to 2,147,483,647</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public int NumberOfRecords { get; set; }
    #endregion
}

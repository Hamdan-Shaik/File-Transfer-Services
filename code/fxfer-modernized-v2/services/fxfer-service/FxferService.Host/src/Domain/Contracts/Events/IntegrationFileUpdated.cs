using System;
using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Events;

/// <summary>
/// <para>Contains the information of an updated record to be saved on the FTS_INTEGRATION_FILE table</para>
/// </summary>
public class IntegrationFileUpdated : IntegrationFileAdded
{
    #region Constructors
    /// <exclude/>
    public IntegrationFileUpdated()
    {
    }
    #endregion

    #region Properties
    /// <summary>
    /// <para>Contains the date time the Integration File Record was last modified</para>
    /// <list type="bullet">        
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Date</description></item>
    /// <item><term>MaxLength</term><description>Datetime</description></item>
    /// <item><term>Valid Values</term><description>yyyy-MM-ddTHH.mm.ss.fffffffZ</description></item>
    /// </list>        
    /// </summary>
    [DataMember]
    public DateTime LastModifiedDate { get; set; }
    #endregion
}

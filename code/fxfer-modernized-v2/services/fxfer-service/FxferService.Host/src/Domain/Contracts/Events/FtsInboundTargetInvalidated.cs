using System;
using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Events;

/// <summary>
/// <para>Contains the information of an invalidated record to be saved on the FTS_INBOUND_TARGET table</para>
/// </summary>
public class FtsInboundTargetInvalidated : FtsInboundTargetAdded
{
    #region Constructors
    /// <exclude/>
    public FtsInboundTargetInvalidated()
    {
    }
    #endregion

    #region Properties
    /// <summary>
    /// <para>Fts Inbound Target LastModified Date</para>
    /// <para>The Fts Inbound Target LastModified Date.</para>
    /// </summary>
    [DataMember]
    public DateTime LastModifiedDate { get; set; }
    #endregion
}

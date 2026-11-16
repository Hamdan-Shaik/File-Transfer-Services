using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Events;

/// <summary>
/// <para>Fts Source Location invalidated</para>
/// <para>Fts Source Location invalidated event</para>
/// </summary>
[DataContract]
public class FtsSourceLocationInvalidated : FtsSourceLocationEvents
{
    public FtsSourceLocationInvalidated()
    {
    }

    /// <summary>
    /// <para>Congifg Id</para>
    /// <para>The Config Id</para>
    /// </summary>
    [DataMember]
    public long ConfigId { get; set; }

    /// <summary>
    /// <para>Fts Source Location Events</para>
    /// <para>The Operator Id</para>
    /// </summary>
    [DataMember]
    public string? OperatorId { get; set; }

    /// <summary>
    /// <para>Fts Source Location Events</para>
    /// <para>DestinationAddress details</para>
    /// </summary>
    [DataMember]
    public List<FtsDestinationLocationAdded>? DestinationAddress { get; set; }
}

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Events;

/// <summary>
/// <para>Fts Source Location Events</para>
/// <para>The current state of the Fts Source Location event.</para>
/// </summary>
[DataContract]
public class FtsSourceLocationEvents : Event
{
    [ExcludeFromCodeCoverage]
    [IgnoreDataMember]
    public override string ModelType
    {
        get
        {
            return "FileTransfer";
        }
    }

    [DataMember]
    public string FtsSourceLocationsId { get; set; } = null!;
}
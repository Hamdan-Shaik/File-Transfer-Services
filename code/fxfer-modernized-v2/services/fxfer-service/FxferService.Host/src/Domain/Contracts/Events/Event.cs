using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Events;

[DataContract]
public abstract class Event
{
    public abstract string ModelType { get; }
}
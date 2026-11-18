using System.Collections.Concurrent;
using Fxfer.TransferWorker.Contracts.Eventing;

namespace Fxfer.TransferWorker.Infrastructure.Eventing;

public sealed class InMemoryDistributedEventStorage : IDistributedEventStorage
{
    private readonly ConcurrentQueue<EventMessage> _events = new();

    public void Add(EventMessage eventMessage)
    {
        _events.Enqueue(eventMessage);
    }

    public IReadOnlyCollection<EventMessage> Snapshot()
    {
        return _events.ToArray();
    }
}


using System.Collections.Concurrent;

namespace Fxfer.TransferWorker.Infrastructure.Eventing;

public sealed class DistributedEventStorageFactory
{
    private static readonly ConcurrentDictionary<string, IDistributedEventStorage> Stores =
        new(StringComparer.OrdinalIgnoreCase);

    public IDistributedEventStorage Create(string eventStoreName)
    {
        var key = string.IsNullOrWhiteSpace(eventStoreName) ? "default" : eventStoreName;
        return Stores.GetOrAdd(key, static _ => new InMemoryDistributedEventStorage());
    }
}

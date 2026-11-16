namespace Fxfer.ExecutionWorker.Application.Helpers;

internal sealed class DistributedEventStorageFactory
{
    public IDistributedEventStorage Create(string eventStore)
    {
        return new NullDistributedEventStorage();
    }
}


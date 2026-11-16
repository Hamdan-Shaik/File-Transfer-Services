namespace Fxfer.ExecutionWorker.Application.Helpers;

internal sealed class NullDistributedEventStorage : IDistributedEventStorage
{
    public void Add(EventMessage message)
    {
    }
}


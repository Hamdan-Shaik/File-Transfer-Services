namespace Fxfer.ExecutionWorker.Application.Helpers;

public interface IDistributedEventStorage
{
    void Add(EventMessage message);
}


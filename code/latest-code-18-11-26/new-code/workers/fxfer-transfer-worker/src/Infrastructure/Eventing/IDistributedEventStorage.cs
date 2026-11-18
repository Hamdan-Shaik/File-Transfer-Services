using Fxfer.TransferWorker.Contracts.Eventing;

namespace Fxfer.TransferWorker.Infrastructure.Eventing;

public interface IDistributedEventStorage
{
    void Add(EventMessage eventMessage);
}
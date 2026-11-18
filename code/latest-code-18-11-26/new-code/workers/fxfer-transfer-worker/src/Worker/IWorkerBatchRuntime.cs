using Fxfer.TransferWorker.Application.Batch;

namespace Fxfer.TransferWorker.Worker;

public interface IWorkerBatchRuntime : IDisposable
{
    void Initialize();

    IServiceProvider Services { get; }

    IBatchExecutor<int> ResolveBatchExecutor(string key);
}
namespace Fxfer.TransferWorker.Worker;

public sealed class WorkerBatchRuntimeFactory : IWorkerBatchRuntimeFactory
{
    public IWorkerBatchRuntime Create()
    {
        return new WorkerBatchRuntime();
    }
}
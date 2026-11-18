namespace Fxfer.TransferWorker.Worker;

public interface IWorkerBatchRuntimeFactory
{
    IWorkerBatchRuntime Create();
}
namespace Fxfer.ExecutionWorker.BatchProcess;

public interface IBatchExecutor<out TResult>
{
    TResult Execute(string[] runDetails);
}

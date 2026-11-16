namespace Fxfer.ExecutionWorker.Batch.Business;

public interface IBatchExecutor<Toutput>
{
    Toutput Execute(string[] inputArgs);
}


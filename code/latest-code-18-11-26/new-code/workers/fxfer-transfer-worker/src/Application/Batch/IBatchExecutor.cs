namespace Fxfer.TransferWorker.Application.Batch;

public interface IBatchExecutor<Toutput>
{
    Toutput Execute(string[] inputArgs);
}

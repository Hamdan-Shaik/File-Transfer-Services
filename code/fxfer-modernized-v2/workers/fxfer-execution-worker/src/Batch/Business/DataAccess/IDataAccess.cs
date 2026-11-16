namespace Fxfer.ExecutionWorker.Batch.Business.DataAccess;

public interface IDataAccess<out T>
{
    T Execute();
}
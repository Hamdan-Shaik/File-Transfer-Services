namespace Fxfer.TransferWorker.Application.DataAccess;

public interface IDataAccess<TOutput>
{
    Task<TOutput> ExecuteAsync(CancellationToken cancellationToken);
}


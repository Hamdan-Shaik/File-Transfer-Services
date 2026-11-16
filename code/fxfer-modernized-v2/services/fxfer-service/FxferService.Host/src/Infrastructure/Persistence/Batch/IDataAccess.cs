namespace FxferService.Infrastructure.Persistence.Batch;

public interface IDataAccess<out TdacOutput>
{
    TdacOutput Execute();

    TdacOutput ExecuteWithNoTransaction();
}

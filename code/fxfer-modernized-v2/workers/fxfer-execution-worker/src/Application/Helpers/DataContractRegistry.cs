namespace Fxfer.ExecutionWorker.Application.Helpers;

internal sealed class DataContractRegistry : IDisposable
{
    public IEnumerable<Type> GetContractTypes()
    {
        return new[] { typeof(FileTransferred) };
    }

    public void Dispose()
    {
    }
}


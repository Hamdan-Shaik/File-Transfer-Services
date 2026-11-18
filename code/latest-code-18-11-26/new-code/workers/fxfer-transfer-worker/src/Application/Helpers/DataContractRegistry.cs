using Fxfer.TransferWorker.Contracts.Eventing;

namespace Fxfer.TransferWorker.Application.Helpers;

internal sealed class DataContractRegistry : IDisposable
{
    public IEnumerable<Type> GetContractTypes()
    {
        // shortcut: only FileTransferred is registered; expand when new event payload types are introduced.
        return new[] { typeof(FileTransferred) };
    }

    public void Dispose()
    {
    }
}
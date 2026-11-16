namespace Fxfer.ExecutionWorker.Batch.Business;

public interface IAppSettings
{
    string? this[string key] { get; }
}
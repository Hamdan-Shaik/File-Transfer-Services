namespace Fxfer.TransferWorker.Worker.Configuration;

public interface ITenantAppSettings
{
    string? this[string key] { get; }
}
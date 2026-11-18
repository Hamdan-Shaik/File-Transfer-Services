namespace Fxfer.TransferWorker.Worker.Configuration;

public interface ITenantAppSettingsManager
{
    ITenantAppSettings? AppSettings { get; }

    void LoadApplicationConfiguration(string tenantId);
}
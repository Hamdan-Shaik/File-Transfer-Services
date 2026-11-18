using Microsoft.Extensions.Configuration;

namespace Fxfer.TransferWorker.Worker.Configuration;

public sealed class ConfigurationTenantAppSettingsManager : ITenantAppSettingsManager
{
    private readonly IConfiguration _configuration;

    public ConfigurationTenantAppSettingsManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ITenantAppSettings? AppSettings { get; private set; }

    public void LoadApplicationConfiguration(string tenantId)
    {
        AppSettings ??= new ConfigurationTenantAppSettings(_configuration, tenantId);
    }
}
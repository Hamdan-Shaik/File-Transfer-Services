using Microsoft.Extensions.Configuration;

namespace Fxfer.TransferWorker.Worker.Configuration;

public sealed class ConfigurationTenantAppSettings : ITenantAppSettings
{
    private readonly IConfiguration _configuration;
    private readonly string _tenantId;

    public ConfigurationTenantAppSettings(IConfiguration configuration, string tenantId)
    {
        _configuration = configuration;
        _tenantId = tenantId;
    }

    public string? this[string key]
        => _configuration[$"Tenants:{_tenantId}:{key}"]
        ?? _configuration[key];
}
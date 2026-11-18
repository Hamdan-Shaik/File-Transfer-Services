namespace Fxfer.TransferWorker.Worker.Services;

public sealed class TenantIdContext
{
    private static readonly AsyncLocal<string?> AmbientTenantId = new();

    public string? Get()
    {
        return AmbientTenantId.Value;
    }

    public void Set(string tenantId)
    {
        AmbientTenantId.Value = tenantId;
    }
}

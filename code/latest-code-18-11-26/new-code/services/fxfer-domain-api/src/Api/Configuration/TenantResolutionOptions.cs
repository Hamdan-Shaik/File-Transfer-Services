namespace Fxfer.DomainApi.Api.Configuration;

public sealed class TenantResolutionOptions
{
    public string TenantIdClaim { get; init; } = "tenant_id";
}

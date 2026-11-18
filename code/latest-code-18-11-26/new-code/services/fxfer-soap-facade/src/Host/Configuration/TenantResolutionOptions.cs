namespace Fxfer.SoapFacade.Host.Configuration;

public sealed class TenantResolutionOptions
{
    public string TenantIdClaim { get; set; } = "tenant_id";
}
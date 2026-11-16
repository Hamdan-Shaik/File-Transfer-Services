namespace Fxfer.Bff.Infrastructure.Tenancy;

public sealed class TenantContext
{
    public string? TenantId { get; set; }
    public string? CorrelationId { get; set; }
}

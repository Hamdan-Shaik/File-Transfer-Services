namespace Fxfer.SchedulerWorker.Infrastructure.Tenancy;

public sealed class TenantContext
{
    public string TenantId { get; set; } = string.Empty;
    public string? CorrelationId { get; set; }
    public string? TraceId { get; set; }
}

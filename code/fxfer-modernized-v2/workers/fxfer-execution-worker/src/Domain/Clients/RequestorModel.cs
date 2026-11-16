namespace Fxfer.ExecutionWorker.Domain.Clients;

public sealed class RequestorModel
{
    public string? IdentifierId { get; set; }

    public string TenantId { get; set; } = string.Empty;
}

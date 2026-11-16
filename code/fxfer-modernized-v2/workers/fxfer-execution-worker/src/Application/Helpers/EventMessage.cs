namespace Fxfer.ExecutionWorker.Application.Helpers;

public sealed class EventMessage
{
    public string? EventNamespace { get; set; }
    public string? AggregateRootID { get; set; }
    public string? AggregateRootType { get; set; }
    public string? CommitID { get; set; }
    public string? EventType { get; set; }
    public string? ModuleName { get; set; }
    public byte[]? EventPayLoad { get; set; }
    public string? TenantID { get; set; }
    public Guid INGroupId { get; set; }
    public string? EventFilterMetadata { get; set; }
}


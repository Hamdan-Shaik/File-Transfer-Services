namespace Fxfer.DomainApi.Infrastructure.Eventing.Outbox;

public sealed class OutboxMessage
{
    public required Guid Id { get; set; }
    public required string TenantId { get; set; }
    public required string EventName { get; set; }
    public required string EventVersion { get; set; }
    public required Guid EventId { get; set; }
    public required DateTimeOffset OccurredAt { get; set; }
    public required string CorrelationId { get; set; }
    public required string TraceId { get; set; }
    public required string Producer { get; set; }
    public required string PayloadJson { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }
}

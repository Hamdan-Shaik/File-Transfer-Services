namespace Fxfer.DomainApi.Contracts.Events;

public static class FxferEventMetadata
{
    public const string ConfigurationChangedEventName = "Fxfer.ConfigurationChanged";
    public const string SuspensionStateChangedEventName = "Fxfer.SuspensionStateChanged";
    public const string DeliveryOutcomeRecordedEventName = "Fxfer.DeliveryOutcomeRecorded";

    public const string V1 = "v1";

    public static class HeaderNames
    {
        public const string TenantId = "tenant_id";
        public const string EventName = "event_name";
        public const string EventVersion = "event_version";
        public const string EventId = "event_id";
        public const string OccurredAt = "occurred_at";
        public const string CorrelationId = "correlation_id";
        public const string TraceId = "trace_id";
        public const string Producer = "producer";
    }
}


using System.Diagnostics;
using System.Reflection;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Contracts.Utilities;

namespace Fxfer.DomainApi.Infrastructure.Eventing;

// T-006: Publish tenant-aware, schema-versioned domain events using the canonical event envelope.
public sealed class FxferDomainEventFactory : IFxferDomainEventFactory
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _producer;

    public FxferDomainEventFactory(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0";
        _producer = $"fxfer-domain-api@{assemblyVersion}";
    }

    public FxferDomainEvent<TPayload> Create<TPayload>(string eventName, string eventVersion, TPayload payload, DateTimeOffset? occurredAt = null)
    {
        var context = _httpContextAccessor.HttpContext;
        var tenantId = context?.Items[ContextKeys.TenantIdItemKey] as string;
        if (string.IsNullOrWhiteSpace(tenantId))
        {
            throw new InvalidOperationException("Missing tenant_id in request context.");
        }

        var correlationId = context?.Items[ContextKeys.CorrelationIdItemKey] as string;
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
        }

        var traceId = Activity.Current?.TraceId.ToString();
        if (string.IsNullOrWhiteSpace(traceId))
        {
            traceId = ActivityTraceId.CreateRandom().ToString();
        }

        return new FxferDomainEvent<TPayload>
        {
            Envelope = new FxferEventEnvelope
            {
                TenantId = tenantId,
                EventName = eventName,
                EventVersion = eventVersion,
                EventId = Guid.NewGuid(),
                OccurredAt = occurredAt ?? DateTimeOffset.UtcNow,
                CorrelationId = correlationId,
                TraceId = traceId,
                Producer = _producer
            },
            Payload = payload
        };
    }
}


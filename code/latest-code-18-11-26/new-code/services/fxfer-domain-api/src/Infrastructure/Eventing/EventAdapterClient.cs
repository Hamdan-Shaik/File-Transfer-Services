using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Infrastructure.Eventing.Outbox;

namespace Fxfer.DomainApi.Infrastructure.Eventing;

public sealed class EventAdapterClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<EventAdapterClient> _logger;

    public EventAdapterClient(HttpClient httpClient, ILogger<EventAdapterClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    // T-006: Publish tenant-aware, schema-versioned events to the Event Adapter.
    public async Task PublishAsync<TPayload>(FxferDomainEvent<TPayload> domainEvent, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(domainEvent);

        await PublishAsync(
            tenantId: domainEvent.Envelope.TenantId,
            eventName: domainEvent.Envelope.EventName,
            eventVersion: domainEvent.Envelope.EventVersion,
            eventId: domainEvent.Envelope.EventId,
            occurredAt: domainEvent.Envelope.OccurredAt,
            correlationId: domainEvent.Envelope.CorrelationId,
            traceId: domainEvent.Envelope.TraceId,
            producer: domainEvent.Envelope.Producer,
            bodyJson: json,
            cancellationToken: cancellationToken);
    }

    public Task PublishAsync(OutboxMessage message, CancellationToken cancellationToken = default)
    {
        return PublishAsync(
            tenantId: message.TenantId,
            eventName: message.EventName,
            eventVersion: message.EventVersion,
            eventId: message.EventId,
            occurredAt: message.OccurredAt,
            correlationId: message.CorrelationId,
            traceId: message.TraceId,
            producer: message.Producer,
            bodyJson: message.PayloadJson,
            cancellationToken: cancellationToken);
    }

    private async Task PublishAsync(
        string tenantId,
        string eventName,
        string eventVersion,
        Guid eventId,
        DateTimeOffset occurredAt,
        string correlationId,
        string traceId,
        string producer,
        string bodyJson,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Publishing event {EventName} v{EventVersion} {EventId} for tenant {TenantId} correlation {CorrelationId} trace {TraceId}",
            eventName,
            eventVersion,
            eventId,
            tenantId,
            correlationId,
            traceId);

        using var request = new HttpRequestMessage(HttpMethod.Post, "/events")
        {
            Content = new StringContent(bodyJson, Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        // Target-state: canonical envelope must be present in headers/attributes.
        request.Headers.TryAddWithoutValidation(FxferEventMetadata.HeaderNames.TenantId, tenantId);
        request.Headers.TryAddWithoutValidation(FxferEventMetadata.HeaderNames.EventName, eventName);
        request.Headers.TryAddWithoutValidation(FxferEventMetadata.HeaderNames.EventVersion, eventVersion);
        request.Headers.TryAddWithoutValidation(FxferEventMetadata.HeaderNames.EventId, eventId.ToString());
        request.Headers.TryAddWithoutValidation(FxferEventMetadata.HeaderNames.OccurredAt, occurredAt.ToString("O"));
        request.Headers.TryAddWithoutValidation(FxferEventMetadata.HeaderNames.CorrelationId, correlationId);
        request.Headers.TryAddWithoutValidation(FxferEventMetadata.HeaderNames.TraceId, traceId);
        request.Headers.TryAddWithoutValidation(FxferEventMetadata.HeaderNames.Producer, producer);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}

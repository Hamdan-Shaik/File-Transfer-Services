using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Contracts.Utilities;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.Eventing;
using Fxfer.DomainApi.Infrastructure.Eventing.Outbox;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fxfer.DomainApi.Tests;

public sealed class T006EventingTests
{
    [Fact]
    public async Task T_006_publishes_canonical_event_envelope_and_versioned_payload()
    {
        const string tenantId = "tenant-test";
        var correlationId = Guid.NewGuid().ToString();
        const string expectedTraceId = "0123456789abcdef0123456789abcdef";

        var httpContext = new DefaultHttpContext();
        httpContext.Items[ContextKeys.TenantIdItemKey] = tenantId;
        httpContext.Items[ContextKeys.CorrelationIdItemKey] = correlationId;

        var httpContextAccessor = new HttpContextAccessor { HttpContext = httpContext };

        using var activity = new Activity("t006");
        activity.SetIdFormat(ActivityIdFormat.W3C);
        activity.SetParentId($"00-{expectedTraceId}-0123456789abcdef-01");
        activity.Start();

        var factory = new FxferDomainEventFactory(httpContextAccessor);

        var dbOptions = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var dbContext = new FxferDbContext(dbOptions);
        var outboxWriter = new OutboxMessageWriter(dbContext, factory);

        var occurredAt = DateTimeOffset.UtcNow;
        var payload = new FxferConfigurationChangedV1
        {
            AggregateType = "ftp_server",
            AggregateId = Guid.NewGuid(),
            ChangeKind = "created",
            OperatorId = "operator-test",
            ChangedAt = occurredAt
        };

        var message = outboxWriter.Enqueue(FxferEventMetadata.ConfigurationChangedEventName, FxferEventMetadata.V1, payload, occurredAt);
        await dbContext.SaveChangesAsync();

        var handler = new CapturingHandler(HttpStatusCode.OK);
        using var client = new HttpClient(handler) { BaseAddress = new Uri("http://event-adapter.test") };
        var adapter = new EventAdapterClient(client, NullLogger<EventAdapterClient>.Instance);
        var publisher = new OutboxPublisher(dbContext, adapter);

        var published = await publisher.PublishPendingAsync(batchSize: 10, CancellationToken.None);
        Assert.Equal(1, published);

        var persisted = await dbContext.OutboxMessages.SingleAsync();
        Assert.NotNull(persisted.PublishedAt);

        Assert.NotNull(handler.Request);
        Assert.Equal("/events", handler.Request!.RequestUri!.AbsolutePath);

        AssertHeader(handler.Request, FxferEventMetadata.HeaderNames.TenantId, tenantId);
        AssertHeader(handler.Request, FxferEventMetadata.HeaderNames.EventName, FxferEventMetadata.ConfigurationChangedEventName);
        AssertHeader(handler.Request, FxferEventMetadata.HeaderNames.EventVersion, FxferEventMetadata.V1);
        AssertHeader(handler.Request, FxferEventMetadata.HeaderNames.EventId, message.EventId.ToString());
        AssertHeader(handler.Request, FxferEventMetadata.HeaderNames.OccurredAt, occurredAt.ToString("O"));
        AssertHeader(handler.Request, FxferEventMetadata.HeaderNames.CorrelationId, correlationId);
        AssertHeader(handler.Request, FxferEventMetadata.HeaderNames.TraceId, expectedTraceId);
        AssertHeader(handler.Request, FxferEventMetadata.HeaderNames.Producer, persisted.Producer);

        Assert.NotNull(handler.Body);
        using var doc = JsonDocument.Parse(handler.Body);

        var envelope = doc.RootElement.GetProperty("envelope");
        Assert.Equal(tenantId, envelope.GetProperty("tenant_id").GetString());
        Assert.Equal(FxferEventMetadata.ConfigurationChangedEventName, envelope.GetProperty("event_name").GetString());
        Assert.Equal(FxferEventMetadata.V1, envelope.GetProperty("event_version").GetString());
        Assert.Equal(message.EventId.ToString(), envelope.GetProperty("event_id").GetGuid().ToString());
        Assert.Equal(correlationId, envelope.GetProperty("correlation_id").GetString());
        Assert.Equal(expectedTraceId, envelope.GetProperty("trace_id").GetString());

        var payloadElement = doc.RootElement.GetProperty("payload");
        Assert.Equal("ftp_server", payloadElement.GetProperty("aggregate_type").GetString());
    }

    [Fact]
    public async Task T_006_does_not_mark_outbox_message_published_when_adapter_call_fails()
    {
        const string tenantId = "tenant-test";

        var httpContext = new DefaultHttpContext();
        httpContext.Items[ContextKeys.TenantIdItemKey] = tenantId;
        httpContext.Items[ContextKeys.CorrelationIdItemKey] = Guid.NewGuid().ToString();
        var httpContextAccessor = new HttpContextAccessor { HttpContext = httpContext };

        using var activity = new Activity("t006");
        activity.SetIdFormat(ActivityIdFormat.W3C);
        activity.Start();

        var factory = new FxferDomainEventFactory(httpContextAccessor);

        var dbOptions = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var dbContext = new FxferDbContext(dbOptions);
        var outboxWriter = new OutboxMessageWriter(dbContext, factory);

        var payload = new FxferDeliveryOutcomeRecordedV1
        {
            FileId = Guid.NewGuid(),
            SourceLocationId = Guid.NewGuid(),
            DestinationId = Guid.NewGuid(),
            OutcomeStatus = "failed",
            OccurredAt = DateTimeOffset.UtcNow
        };

        outboxWriter.Enqueue(FxferEventMetadata.DeliveryOutcomeRecordedEventName, FxferEventMetadata.V1, payload);
        await dbContext.SaveChangesAsync();

        var handler = new CapturingHandler(HttpStatusCode.InternalServerError);
        using var client = new HttpClient(handler) { BaseAddress = new Uri("http://event-adapter.test") };
        var adapter = new EventAdapterClient(client, NullLogger<EventAdapterClient>.Instance);
        var publisher = new OutboxPublisher(dbContext, adapter);

        await Assert.ThrowsAsync<HttpRequestException>(
            () => publisher.PublishPendingAsync(batchSize: 10, CancellationToken.None));

        var persisted = await dbContext.OutboxMessages.SingleAsync();
        Assert.Null(persisted.PublishedAt);
    }

    [Fact]
    public void T_006_factory_uses_request_context_for_tenant_correlation_and_trace()
    {
        const string tenantId = "tenant-test";
        var correlationId = Guid.NewGuid().ToString();
        const string expectedTraceId = "0123456789abcdef0123456789abcdef";

        var httpContext = new DefaultHttpContext();
        httpContext.Items[ContextKeys.TenantIdItemKey] = tenantId;
        httpContext.Items[ContextKeys.CorrelationIdItemKey] = correlationId;

        var httpContextAccessor = new HttpContextAccessor { HttpContext = httpContext };

        using var activity = new Activity("t006");
        activity.SetIdFormat(ActivityIdFormat.W3C);
        activity.SetParentId($"00-{expectedTraceId}-0123456789abcdef-01");
        activity.Start();

        var factory = new FxferDomainEventFactory(httpContextAccessor);
        var domainEvent = factory.Create(
            FxferEventMetadata.SuspensionStateChangedEventName,
            FxferEventMetadata.V1,
            new FxferSuspensionStateChangedV1
            {
                SourceLocationId = Guid.NewGuid(),
                Suspended = true,
                ReasonCode = "operator",
                ChangedAt = DateTimeOffset.UtcNow
            });

        Assert.Equal(tenantId, domainEvent.Envelope.TenantId);
        Assert.Equal(correlationId, domainEvent.Envelope.CorrelationId);
        Assert.Equal(expectedTraceId, domainEvent.Envelope.TraceId);
    }

    private static void AssertHeader(HttpRequestMessage request, string name, string expected)
    {
        Assert.True(request.Headers.TryGetValues(name, out var values));
        Assert.Equal(expected, values!.Single());
    }

    private sealed class CapturingHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _status;

        public CapturingHandler(HttpStatusCode status)
        {
            _status = status;
        }

        public HttpRequestMessage? Request { get; private set; }
        public string? Body { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Request = request;
            Body = request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken);
            return new HttpResponseMessage(_status);
        }
    }
}


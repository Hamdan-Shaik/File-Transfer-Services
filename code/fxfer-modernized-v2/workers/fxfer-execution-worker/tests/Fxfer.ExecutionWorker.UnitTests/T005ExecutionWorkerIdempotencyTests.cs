using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Fxfer.ExecutionWorker.Host.Hosting;
using Fxfer.ExecutionWorker.Infrastructure.DomainApi;
using Fxfer.ExecutionWorker.Infrastructure.Messaging;
using Fxfer.ExecutionWorker.Infrastructure.Messaging.Consumer;
using Fxfer.ExecutionWorker.Infrastructure.Tenancy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Fxfer.ExecutionWorker.UnitTests;

public sealed class T005ExecutionWorkerIdempotencyTests
{
    [Fact]
    public async Task T_005_records_outcomes_with_idempotency_key_header_and_treats_conflict_as_success()
    {
        var snapshots = new ConcurrentQueue<RequestSnapshot>();
        var handler = new RecordingHandler((request, _) =>
        {
            snapshots.Enqueue(RequestSnapshot.From(request));
            return new HttpResponseMessage(HttpStatusCode.Conflict);
        });

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["FxferService:BaseUrl"] = "http://example.test"
            })
            .Build();

        var httpClientFactory = new SingleClientFactory(new HttpClient(handler));
        var client = new FxferServiceClient(httpClientFactory, configuration);

        var runId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        await client.RecordSkeletonOutcomeAsync(runId, "idem-1", CancellationToken.None);

        Assert.True(snapshots.TryDequeue(out var snapshot));
        Assert.Equal("idem-1", snapshot.IdempotencyKey);
        Assert.Equal("POST", snapshot.Method);
        Assert.EndsWith($"/api/v1/transfer-runs/{runId:D}/outcomes", snapshot.PathAndQuery);
    }

    [Fact]
    public async Task T_005_redelivered_work_items_reuse_the_same_idempotency_key()
    {
        var snapshots = new ConcurrentQueue<RequestSnapshot>();
        var handler = new RecordingHandler((request, count) =>
        {
            snapshots.Enqueue(RequestSnapshot.From(request));
            if (count == 1)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            return new HttpResponseMessage(HttpStatusCode.Conflict);
        });

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["FxferService:BaseUrl"] = "http://example.test"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton<IHttpClientFactory>(_ => new SingleClientFactory(new HttpClient(handler)));

        services.AddSingleton<IWorkItemQueue, InMemoryWorkItemQueue>();
        services.AddScoped<TenantContext>();
        services.AddScoped<FxferServiceClient>();
        services.AddScoped<TransferRunProcessor>();
        services.AddScoped<TransferRunRequestedConsumer>();

        services.AddOptions<WorkItemRetryOptions>().Configure(o => o.RetryDelayMs = 0);

        using var provider = services.BuildServiceProvider();
        var queue = provider.GetRequiredService<IWorkItemQueue>();

        var worker = new WorkerHostedService(
            queue,
            provider.GetRequiredService<IServiceScopeFactory>(),
            provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<WorkerHostedService>>(),
            provider.GetRequiredService<IOptions<WorkItemRetryOptions>>());

        using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        var runId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        using var payloadDoc = JsonDocument.Parse($@"{{""run_id"":""{runId:D}""}}");

        var envelope = new MessageEnvelope(
            TenantId: "t-1",
            EventName: "TransferRunRequested",
            EventVersion: "0.1.0",
            EventId: Guid.NewGuid(),
            OccurredAt: DateTimeOffset.UtcNow,
            CorrelationId: "c-1",
            TraceId: "tr-1",
            Producer: "test",
            Payload: payloadDoc.RootElement.Clone());

        await worker.HandleEnvelopeAsync(envelope, timeoutCts.Token);
        var redeliveredEnvelope = await queue.Reader.ReadAsync(timeoutCts.Token);
        await worker.HandleEnvelopeAsync(redeliveredEnvelope, timeoutCts.Token);

        Assert.True(snapshots.TryDequeue(out var first));
        Assert.True(snapshots.TryDequeue(out var second));
        Assert.Equal(first.IdempotencyKey, second.IdempotencyKey);
    }

    private static async Task WaitForAsync(Func<bool> condition, CancellationToken cancellationToken)
    {
        while (!condition())
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(10, cancellationToken);
        }
    }

    private sealed record RequestSnapshot(string Method, string PathAndQuery, string? IdempotencyKey)
    {
        public static RequestSnapshot From(HttpRequestMessage request)
        {
            request.Headers.TryGetValues("Idempotency-Key", out var values);
            var key = values?.FirstOrDefault();

            return new RequestSnapshot(
                Method: request.Method.Method,
                PathAndQuery: request.RequestUri?.PathAndQuery ?? string.Empty,
                IdempotencyKey: key);
        }
    }

    private sealed class SingleClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _client;

        public SingleClientFactory(HttpClient client)
        {
            _client = client;
        }

        public HttpClient CreateClient(string name) => _client;
    }

    private sealed class RecordingHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, int, HttpResponseMessage> _responder;
        private int _count;

        public RecordingHandler(Func<HttpRequestMessage, int, HttpResponseMessage> responder)
        {
            _responder = responder;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var count = Interlocked.Increment(ref _count);
            try
            {
                return Task.FromResult(_responder(request, count));
            }
            catch (Exception ex)
            {
                return Task.FromException<HttpResponseMessage>(ex);
            }
        }
    }
}

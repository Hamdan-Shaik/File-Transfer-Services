using System.Diagnostics;
using System.Net;
using System.Diagnostics.Metrics;
using Fxfer.DomainApi.Api.Middleware;
using Fxfer.DomainApi.Contracts.Utilities;
using Fxfer.DomainApi.Domain.Constants;
using Fxfer.DomainApi.Tests.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Fxfer.DomainApi.Tests;

public sealed class T005ObservabilityTests
{
    [Fact]
    public async Task T_005_request_span_contains_tenant_correlation_and_trace_tags()
    {
        await using var factory = new FxferDomainApiFactory();
        var client = factory.CreateClient();

        var correlationId = Guid.NewGuid().ToString();
        const string expectedTraceId = "0123456789abcdef0123456789abcdef";
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/fxfer-domain/v1/ftp-servers");
        request.Headers.Add(CommonConstants.CorrelationIdHeaderName, correlationId);
        request.Headers.TryAddWithoutValidation("traceparent", $"00-{expectedTraceId}-0123456789abcdef-01");

        var activity = await CaptureServerActivityAsync(
            () => client.SendAsync(request),
            expectedPath: "/api/fxfer-domain/v1/ftp-servers",
            expectedMethod: HttpMethod.Get.Method);

        Assert.Equal("tenant-test", activity.GetTagItem("tenant_id"));
        Assert.Equal(correlationId, activity.GetTagItem("correlation_id"));
        Assert.Equal(expectedTraceId, activity.TraceId.ToString());
        Assert.Equal(expectedTraceId, activity.GetTagItem("trace_id"));
    }

    [Fact]
    public async Task T_005_correlation_id_is_returned_when_missing_and_tagged_on_span()
    {
        await using var factory = new FxferDomainApiFactory();
        var client = factory.CreateClient();

        HttpResponseMessage? response = null;
        var activity = await CaptureServerActivityAsync(
            async () => response = await client.GetAsync("/api/fxfer-domain/v1/ftp-servers"),
            expectedPath: "/api/fxfer-domain/v1/ftp-servers",
            expectedMethod: HttpMethod.Get.Method);

        response!.EnsureSuccessStatusCode();
        Assert.True(response.Headers.TryGetValues(CommonConstants.CorrelationIdHeaderName, out var values));

        var correlationId = values!.Single();
        Assert.Equal(correlationId, activity.GetTagItem("correlation_id"));
    }

    [Fact]
    public async Task T_005_outbound_http_calls_include_the_correlation_header()
    {
        var correlationId = Guid.NewGuid().ToString();
        var context = new DefaultHttpContext();
        context.Items[ContextKeys.CorrelationIdItemKey] = correlationId;

        var accessor = new HttpContextAccessor { HttpContext = context };
        var capture = new CapturingHandler();

        var handler = new HttpClientCorrelationIdHandler(accessor) { InnerHandler = capture };
        using var client = new HttpClient(handler);

        await client.GetAsync("http://example.test/");

        Assert.NotNull(capture.Request);
        Assert.True(capture.Request!.Headers.TryGetValues(CommonConstants.CorrelationIdHeaderName, out var values));
        Assert.Equal(correlationId, values!.Single());
    }

    [Fact]
    public async Task T_005_adds_log_scope_with_tenant_correlation_trace()
    {
        var correlationId = Guid.NewGuid().ToString();
        const string tenantId = "tenant-test";

        var context = new DefaultHttpContext();
        context.Items[ContextKeys.CorrelationIdItemKey] = correlationId;
        context.Items[ContextKeys.TenantIdItemKey] = tenantId;

        using var activity = new Activity("test");
        activity.SetIdFormat(ActivityIdFormat.W3C);
        activity.Start();

        var logger = new CapturingLogger<RequestLoggingScopeMiddleware>();
        var middleware = new RequestLoggingScopeMiddleware(_ => Task.CompletedTask, logger);

        await middleware.InvokeAsync(context);

        Assert.NotNull(logger.LastScope);
        Assert.Equal(correlationId, logger.LastScope!["correlation_id"]);
        Assert.Equal(tenantId, logger.LastScope!["tenant_id"]);
        Assert.Equal(activity.TraceId.ToString(), logger.LastScope!["trace_id"]);
    }

    [Fact]
    public async Task T_005_emits_request_metrics_tagged_with_tenant_correlation_trace()
    {
        await using var factory = new FxferDomainApiFactory();
        var client = factory.CreateClient();

        var correlationId = Guid.NewGuid().ToString();
        const string expectedTraceId = "0123456789abcdef0123456789abcdef";

        var tcs = new TaskCompletionSource<KeyValuePair<string, object?>[]>(TaskCreationOptions.RunContinuationsAsynchronously);

        using var listener = new MeterListener();
        listener.InstrumentPublished = (instrument, meterListener) =>
        {
            if (instrument.Meter.Name == RequestLoggingScopeMiddleware.MeterName
                && instrument.Name == "fxfer.domainapi.request.count")
            {
                meterListener.EnableMeasurementEvents(instrument);
            }
        };

        listener.SetMeasurementEventCallback<long>((instrument, measurement, tags, state) =>
        {
            if (tcs.Task.IsCompleted)
            {
                return;
            }

            var captured = tags.ToArray();
            var correlation = captured.SingleOrDefault(t => t.Key == "correlation_id").Value as string;
            if (string.Equals(correlation, correlationId, StringComparison.OrdinalIgnoreCase))
            {
                tcs.TrySetResult(captured);
            }
        });

        listener.Start();

        var request = new HttpRequestMessage(HttpMethod.Get, "/api/fxfer-domain/v1/ftp-servers");
        request.Headers.Add(CommonConstants.CorrelationIdHeaderName, correlationId);
        request.Headers.TryAddWithoutValidation("traceparent", $"00-{expectedTraceId}-0123456789abcdef-01");

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var tags = await tcs.Task.WaitAsync(TimeSpan.FromSeconds(10));
        Assert.Equal("tenant-test", tags.Single(t => t.Key == "tenant_id").Value);
        Assert.Equal(correlationId, tags.Single(t => t.Key == "correlation_id").Value);
        Assert.Equal(expectedTraceId, tags.Single(t => t.Key == "trace_id").Value);
    }

    private static async Task<Activity> CaptureServerActivityAsync(
        Func<Task> action,
        string expectedPath,
        string? expectedMethod)
    {
        var tcs = new TaskCompletionSource<Activity>(TaskCreationOptions.RunContinuationsAsynchronously);

        using var listener = new ActivityListener
        {
            ShouldListenTo = _ => true,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllDataAndRecorded,
            ActivityStopped = activity =>
            {
                if (activity.Kind != ActivityKind.Server)
                {
                    return;
                }

                if (MatchesPath(activity, expectedPath) && MatchesMethod(activity, expectedMethod))
                {
                    tcs.TrySetResult(activity);
                }
            },
        };

        ActivitySource.AddActivityListener(listener);

        await action();
        return await tcs.Task.WaitAsync(TimeSpan.FromSeconds(10));
    }

    private static bool MatchesPath(Activity activity, string expectedPath)
    {
        var route = activity.GetTagItem("http.route") as string;
        var path = activity.GetTagItem("url.path") as string;
        var target = activity.GetTagItem("http.target") as string;

        return string.Equals(route, expectedPath, StringComparison.Ordinal)
               || string.Equals(path, expectedPath, StringComparison.Ordinal)
               || string.Equals(target, expectedPath, StringComparison.Ordinal);
    }

    private static bool MatchesMethod(Activity activity, string? expectedMethod)
    {
        if (string.IsNullOrWhiteSpace(expectedMethod))
        {
            return true;
        }

        var method = activity.GetTagItem("http.request.method") as string
                     ?? activity.GetTagItem("http.method") as string;

        return string.Equals(method, expectedMethod, StringComparison.OrdinalIgnoreCase);
    }

    private sealed class CapturingHandler : HttpMessageHandler
    {
        public HttpRequestMessage? Request { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Request = request;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }
    }

    private sealed class CapturingLogger<T> : ILogger<T>
    {
        public IReadOnlyDictionary<string, object?>? LastScope { get; private set; }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            if (state is IEnumerable<KeyValuePair<string, object?>> pairs)
            {
                LastScope = pairs.ToDictionary(p => p.Key, p => p.Value);
            }

            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
        }

        private sealed class NullScope : IDisposable
        {
            public static readonly NullScope Instance = new();

            public void Dispose()
            {
            }
        }
    }
}


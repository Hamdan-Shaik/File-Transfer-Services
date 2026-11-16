using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using FxferService.Host.Composition;
using FxferService.Infrastructure.Tenancy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace FxferService.Host.UnitTests;

public sealed class T_006_ObservabilityTaggingTests
{
    [Fact]
    public async Task T_006_sets_required_otel_tags_and_baggage_on_request_activity()
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;
        Activity.ForceDefaultIdFormat = true;

        var middleware = new RequestContextMiddleware(_ => Task.CompletedTask, NullLogger<RequestContextMiddleware>.Instance);

        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("tenant_id", "t1"),
                new Claim("sub", "user"),
            }, authenticationType: "jwt")),
        };
        httpContext.Request.Headers[RequestContextHeaderNames.CorrelationId] = "c1";

        using var activity = new Activity("request").Start();

        var requestContext = new RequestContext();
        await middleware.Invoke(httpContext, requestContext);

        Assert.Equal("t1", activity.GetTagItem("tenant_id"));
        Assert.Equal("c1", activity.GetTagItem("correlation_id"));
        Assert.Equal(activity.TraceId.ToString(), activity.GetTagItem("trace_id"));

        Assert.Equal("t1", activity.GetBaggageItem("tenant_id"));
        Assert.Equal("c1", activity.GetBaggageItem("correlation_id"));
    }

    [Fact]
    public void T_006_propagates_required_tags_to_child_spans_via_baggage()
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;
        Activity.ForceDefaultIdFormat = true;

        var processorType = typeof(DependencyInjection).Assembly.GetType(
            "FxferService.Host.Composition.TelemetryTaggingProcessor",
            throwOnError: true);

        dynamic processor = System.Activator.CreateInstance(processorType!, nonPublic: true)!;

        using var root = new Activity("root").Start();
        root.AddBaggage("tenant_id", "t2");
        root.AddBaggage("correlation_id", "c2");

        using var child = new Activity("child").Start();
        processor.OnStart(child);

        Assert.Equal("t2", child.GetTagItem("tenant_id"));
        Assert.Equal("c2", child.GetTagItem("correlation_id"));
        Assert.Equal(child.TraceId.ToString(), child.GetTagItem("trace_id"));
    }
}

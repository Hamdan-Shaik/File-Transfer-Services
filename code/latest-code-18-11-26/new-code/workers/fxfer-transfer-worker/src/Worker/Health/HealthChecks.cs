using Fxfer.TransferWorker.Worker.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace Fxfer.TransferWorker.Worker.Health;

public static class HealthChecks
{
    public static IServiceCollection AddFxferHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<LiveHealthCheck>("live")
            .AddCheck<ReadyHealthCheck>("ready");

        return services;
    }

    public static IEndpointRouteBuilder MapFxferHealthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/health/live", async (HealthCheckService healthChecks) =>
        {
            var report = await healthChecks.CheckHealthAsync(r => r.Name == "live");
            return Results.Ok(ToResponse(report));
        }).AllowAnonymous();

        app.MapGet("/api/health/ready", async (HealthCheckService healthChecks) =>
        {
            var report = await healthChecks.CheckHealthAsync(r => r.Name == "ready");

            return report.Status == HealthStatus.Healthy
                ? Results.Ok(ToResponse(report))
                : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
        }).AllowAnonymous();

        return app;
    }

    private static HealthResponse ToResponse(HealthReport report)
    {
        return new HealthResponse
        {
            Status = report.Status == HealthStatus.Healthy ? "ok" : "degraded",
            TimeUtc = DateTimeOffset.UtcNow
        };
    }

    private sealed class LiveHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }

    private sealed class ReadyHealthCheck : IHealthCheck
    {
        private readonly IOptions<DomainApiClientOptions> _domainApi;
        private readonly IOptions<EventBackboneConsumerOptions> _consumer;

        public ReadyHealthCheck(IOptions<DomainApiClientOptions> domainApi, IOptions<EventBackboneConsumerOptions> consumer)
        {
            _domainApi = domainApi;
            _consumer = consumer;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var domainReady = !string.IsNullOrWhiteSpace(_domainApi.Value.BaseUrl);
            var consumerReady = !string.IsNullOrWhiteSpace(_consumer.Value.EventAdapterBaseUrl);

            return Task.FromResult(domainReady && consumerReady ? HealthCheckResult.Healthy() : HealthCheckResult.Degraded());
        }
    }
}


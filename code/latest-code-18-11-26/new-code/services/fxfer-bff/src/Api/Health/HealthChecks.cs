using Fxfer.Bff.Contracts.Common;
using Fxfer.Bff.Api.Middleware;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Fxfer.Bff.Api.Health;

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

        app.MapGet("/api/health/ready", async (HttpContext context, HealthCheckService healthChecks) =>
        {
            var report = await healthChecks.CheckHealthAsync(r => r.Name == "ready");

            if (report.Status == HealthStatus.Healthy)
            {
                return Results.Ok(ToResponse(report));
            }

            await ProblemDetailsMiddleware.WriteAsync(
                context,
                statusCode: StatusCodes.Status503ServiceUnavailable,
                title: "Service not ready");

            return Results.Empty;
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
        private readonly IConfiguration _configuration;

        public ReadyHealthCheck(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var hasDownstream = !string.IsNullOrWhiteSpace(_configuration["Downstream:FxferDomainApiBaseUrl"]);
            var hasJwt = !string.IsNullOrWhiteSpace(_configuration["Authentication:Authority"]) && !string.IsNullOrWhiteSpace(_configuration["Authentication:Audience"]);

            return Task.FromResult(hasDownstream && hasJwt ? HealthCheckResult.Healthy() : HealthCheckResult.Degraded());
        }
    }
}


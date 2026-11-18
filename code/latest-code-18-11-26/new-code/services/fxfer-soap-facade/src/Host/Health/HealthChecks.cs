using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Fxfer.SoapFacade.Host.Health;

public static class HealthChecks
{
    public static IServiceCollection AddFxferSoapFacadeHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<LiveHealthCheck>("live")
            .AddCheck<ReadyHealthCheck>("ready");

        return services;
    }

    public static IEndpointRouteBuilder MapFxferSoapFacadeHealthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/health/live", async (HealthCheckService healthChecks) =>
        {
            var report = await healthChecks.CheckHealthAsync(r => r.Name == "live");
            return Results.Ok(ToResponse(report));
        }).AllowAnonymous();

        app.MapGet("/api/health/ready", async (HealthCheckService healthChecks) =>
        {
            var report = await healthChecks.CheckHealthAsync(r => r.Name == "ready");

            if (report.Status == HealthStatus.Healthy)
            {
                return Results.Ok(ToResponse(report));
            }

            return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
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
            var hasJwt = !string.IsNullOrWhiteSpace(_configuration["Authentication:Authority"]) && !string.IsNullOrWhiteSpace(_configuration["Authentication:Audience"]);
            var hasDomainApi = !string.IsNullOrWhiteSpace(_configuration["DomainApi:BaseUrl"]);

            return Task.FromResult(hasJwt && hasDomainApi ? HealthCheckResult.Healthy() : HealthCheckResult.Degraded());
        }
    }
}

public sealed class HealthResponse
{
    public string? Status { get; set; }
    public DateTimeOffset TimeUtc { get; set; }
}
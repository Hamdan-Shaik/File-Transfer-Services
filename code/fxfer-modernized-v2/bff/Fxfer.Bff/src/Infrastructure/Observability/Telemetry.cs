using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Fxfer.Bff.Infrastructure.Observability;

public static class Telemetry
{
    public static IServiceCollection AddFxferBffTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceName = configuration["OpenTelemetry:ServiceName"] ?? "fxfer-bff";
        var serviceNamespace = configuration["OpenTelemetry:ServiceNamespace"] ?? "provider-file-transfer";

        services.AddOpenTelemetry()
            .ConfigureResource(r => r.AddService(serviceName: serviceName, serviceNamespace: serviceNamespace))
            .WithTracing(b => b
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter())
            .WithMetrics(b => b
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter());

        return services;
    }
}

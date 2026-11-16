using Fxfer.SchedulerWorker.Application.Scheduling;
using Fxfer.SchedulerWorker.Host.Hosting;
using Fxfer.SchedulerWorker.Infrastructure.DomainApi;
using Fxfer.SchedulerWorker.Infrastructure.Messaging.Publisher;
using Fxfer.SchedulerWorker.Infrastructure.Policy;
using Fxfer.SchedulerWorker.Infrastructure.Tenancy;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Fxfer.SchedulerWorker.Host.Composition;

public static class DependencyInjection
{
    public static IServiceCollection AddFxferSchedulerWorker(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddHttpClient();

        services.AddScoped<TenantContext>();

        services.AddSingleton<FxferServiceClient>();
        services.AddSingleton<TenantPolicyClient>();
        services.AddSingleton<TransferRunRequestedPublisher>();

        services.AddSingleton<DueRunPlanner>();
        services.AddScoped<DispatchLoop>();

        services.AddHostedService<WorkerHostedService>();
        services.AddHostedService<StartupChecksHostedService>();

        services.AddHealthChecks()
            .AddCheck<LiveHealthCheck>("live", tags: new[] { "live" })
            .AddCheck<ReadyHealthCheck>("ready", tags: new[] { "ready" });

        var serviceName = configuration["OpenTelemetry:ServiceName"] ?? "fxfer-scheduler-worker";
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

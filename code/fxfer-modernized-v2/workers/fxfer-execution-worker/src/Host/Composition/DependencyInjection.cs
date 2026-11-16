using Amazon.SecretsManager;
using Fxfer.ExecutionWorker.Application.NotificationService;
using Fxfer.ExecutionWorker.Host.Hosting;
using Fxfer.ExecutionWorker.Infrastructure.DomainApi;
using Fxfer.ExecutionWorker.Infrastructure.Messaging.Consumer;
using Fxfer.ExecutionWorker.Infrastructure.Secrets;
using Fxfer.ExecutionWorker.Infrastructure.Staging;
using Fxfer.ExecutionWorker.Infrastructure.Tenancy;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Fxfer.ExecutionWorker.Host.Composition;

public static class DependencyInjection
{
    public static IServiceCollection AddFxferExecutionWorker(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddHttpClient();

        services.AddSingleton<Fxfer.ExecutionWorker.Application.NotificationService.INotificationService, NullNotificationService>();

        services.AddSingleton<IAmazonSecretsManager>(_ => new AmazonSecretsManagerClient());
        services.AddSingleton<ISecretsManagerClient, AwsSecretsManagerClient>();

        services.AddScoped<TenantContext>();

        services.Configure<WorkItemRetryOptions>(configuration.GetSection("Messaging:Retry"));

        services.AddSingleton<IWorkItemQueue, InMemoryWorkItemQueue>();
        services.AddScoped<TransferRunRequestedConsumer>();
        services.AddScoped<TransferRunProcessor>();

        services.AddSingleton<FxferServiceClient>();
        services.AddSingleton<SecretsResolver>();
        services.AddSingleton<StagingWorkspace>();

        services.AddHostedService<WorkerHostedService>();
        services.AddHostedService<StartupChecksHostedService>();

        services.AddHealthChecks()
            .AddCheck<LiveHealthCheck>("live", tags: new[] { "live" })
            .AddCheck<ReadyHealthCheck>("ready", tags: new[] { "ready" });

        var serviceName = configuration["OpenTelemetry:ServiceName"] ?? "fxfer-execution-worker";
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

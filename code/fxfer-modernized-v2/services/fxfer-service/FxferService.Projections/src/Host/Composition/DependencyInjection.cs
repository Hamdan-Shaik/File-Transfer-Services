using FxferService.Projections.Host.Hosting;
using FxferService.Projections.Infrastructure.Messaging.Consumer;
using FxferService.Projections.Infrastructure.Messaging.Idempotency;
using FxferService.Projections.Infrastructure.Persistence;
using FxferService.Projections.Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace FxferService.Projections.Host.Composition;

public static class DependencyInjection
{
    public static IServiceCollection AddFxferProjections(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddHttpClient();

        services.AddSingleton<TimeProvider>(TimeProvider.System);

        services.AddScoped<RequestContext>();
        services.AddScoped<ITenantDbResolver, TenantDbResolver>();

        services.AddDbContext<FxferProjectionsDbContext>((sp, options) =>
        {
            var resolver = sp.GetRequiredService<ITenantDbResolver>();
            var connectionString = resolver.ResolveConnectionString();

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                // shortcut: in-memory DB for local bootstrap without tenant registry + Secrets Manager, upgrade when platform wiring exists.
                options.UseInMemoryDatabase("fxfer-projections");
                return;
            }

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<InboxStore>();
        services.AddSingleton<IFxferEventQueue, FxferEventQueue>();
        services.AddHostedService<FxferEventConsumer>();
        services.AddHostedService<StartupChecksHostedService>();

        services.AddHealthChecks()
            .AddCheck<LiveHealthCheck>("live", tags: new[] { "live" })
            .AddCheck<ReadyHealthCheck>("ready", tags: new[] { "ready" });

        var serviceName = configuration["OpenTelemetry:ServiceName"] ?? "fxfer-projections";
        var serviceNamespace = configuration["OpenTelemetry:ServiceNamespace"] ?? "provider-file-transfer";

        services.AddOpenTelemetry()
            .ConfigureResource(r => r.AddService(serviceName: serviceName, serviceNamespace: serviceNamespace))
            .WithTracing(b => b
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()
                .AddOtlpExporter())
            .WithMetrics(b => b
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter());

        return services;
    }
}

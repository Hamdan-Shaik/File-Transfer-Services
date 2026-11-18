using System.Diagnostics;
using System.Reflection;
using Fxfer.ProjectionWorker.Worker.Configuration;
using Fxfer.ProjectionWorker.Worker.Eventing;
using Fxfer.ProjectionWorker.Worker.Health;
using Fxfer.ProjectionWorker.Worker.Middleware;
using Fxfer.ProjectionWorker.Worker.Processing;
using Fxfer.ProjectionWorker.Worker.Services;
using Fxfer.ProjectionWorker.Infrastructure.DataAccess;
using Fxfer.ProjectionWorker.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<DomainApiClientOptions>()
    .Bind(builder.Configuration.GetSection(DomainApiClientOptions.SectionName));

builder.Services.AddOptions<EventBackboneConsumerOptions>()
    .Bind(builder.Configuration.GetSection(EventBackboneConsumerOptions.SectionName));

builder.Services.AddOptions<TenantDatabaseOptions>()
    .Bind(builder.Configuration.GetSection(TenantDatabaseOptions.SectionName));

builder.Services.AddSingleton<ProcessingContextAccessor>();
builder.Services.AddSingleton<TenantConnectionStringProvider>();
builder.Services.AddTransient<TransientHttpRetryHandler>();

builder.Services.AddHttpClient<DomainApiClient>((serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<DomainApiClientOptions>>().Value;

    if (Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out var uri))
    {
        client.BaseAddress = uri;
    }

    client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
}).AddHttpMessageHandler<TransientHttpRetryHandler>();

builder.Services.AddHttpClient<EventAdapterClient>((serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<EventBackboneConsumerOptions>>().Value;

    if (Uri.TryCreate(options.EventAdapterBaseUrl, UriKind.Absolute, out var uri))
    {
        client.BaseAddress = uri;
    }

    client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
}).AddHttpMessageHandler<TransientHttpRetryHandler>();

builder.Services.AddDbContext<FxferProjectionDbContext>((serviceProvider, options) =>
{
    var connectionString = serviceProvider.GetRequiredService<TenantConnectionStringProvider>().TryGetConnectionString();
    connectionString ??= builder.Configuration.GetConnectionString("Fxfer");
    connectionString ??= "Host=localhost;Database=fxfer;Username=fxfer;Password=fxfer";

    options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
});

builder.Services.AddSingleton<FxferEventDispatcher>();
builder.Services.AddSingleton<IFxferEventHandler, ConfigurationChangedProjectionHandler>();
builder.Services.AddSingleton<IFxferEventHandler, SuspensionStateChangedProjectionHandler>();
builder.Services.AddSingleton<IFxferEventHandler, DeliveryOutcomeRecordedProjectionHandler>();
builder.Services.AddHostedService<FxferEventConsumer>();

builder.Services.AddFxferHealthChecks();

var serviceName = "fxfer-projection-worker";
var serviceNamespace = "fxfer";
var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0";
var otlpEndpoint = builder.Configuration["OpenTelemetry:OtlpEndpoint"];

builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
    logging.ParseStateValues = true;

    if (Uri.TryCreate(otlpEndpoint, UriKind.Absolute, out var uri))
    {
        logging.AddOtlpExporter(exporter => exporter.Endpoint = uri);
    }
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(serviceName: serviceName, serviceNamespace: serviceNamespace, serviceVersion: version)
        .AddAttributes(new[] { new KeyValuePair<string, object>("gainwell.domain", "fxfer") }))
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation();
        tracing.AddHttpClientInstrumentation();

        if (Uri.TryCreate(otlpEndpoint, UriKind.Absolute, out var uri))
        {
            tracing.AddOtlpExporter(exporter => exporter.Endpoint = uri);
        }
    })
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation();
        metrics.AddHttpClientInstrumentation();

        if (Uri.TryCreate(otlpEndpoint, UriKind.Absolute, out var uri))
        {
            metrics.AddOtlpExporter(exporter => exporter.Endpoint = uri);
        }
    });

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();

app.MapFxferHealthEndpoints();

app.MapGet("/", () => Results.Redirect("/api/health/live"))
    .AllowAnonymous();

app.Run();

public partial class Program;


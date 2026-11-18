using System.Diagnostics;
using System.Reflection;
using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Worker;
using Fxfer.TransferWorker.Worker.Configuration;
using Fxfer.TransferWorker.Worker.Eventing;
using Fxfer.TransferWorker.Worker.Health;
using Fxfer.TransferWorker.Worker.Middleware;
using Fxfer.TransferWorker.Worker.Services;
using Microsoft.Extensions.Options;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

if (CliEntrypoint.TryHandle(args, out var exitCode))
{
    Environment.ExitCode = exitCode;
    return;
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<DomainApiClientOptions>()
    .Bind(builder.Configuration.GetSection(DomainApiClientOptions.SectionName));

builder.Services.AddOptions<EventBackboneConsumerOptions>()
    .Bind(builder.Configuration.GetSection(EventBackboneConsumerOptions.SectionName));

builder.Services.AddOptions<SecretsOptions>()
    .Bind(builder.Configuration.GetSection(SecretsOptions.SectionName));

builder.Services.AddOptions<StagingOptions>()
    .Bind(builder.Configuration.GetSection(StagingOptions.SectionName));

builder.Services.AddOptions<TransferExecutionOptions>()
    .Bind(builder.Configuration.GetSection(TransferExecutionOptions.SectionName));

builder.Services.AddSingleton<CorrelationIdContext>();
builder.Services.AddSingleton<TenantIdContext>();
builder.Services.AddSingleton<IRequestContextAccessor, RequestContextAccessor>();
builder.Services.AddTransient<IDataAccessFactory>(sp => new DataAccessFactory(
    sp.GetRequiredService<DomainApiClient>(),
    sp.GetRequiredService<IRequestContextAccessor>()));

builder.Services.AddTransient<CorrelationIdDelegatingHandler>();
builder.Services.AddTransient<TransientHttpRetryHandler>();

builder.Services.AddHttpClient<DomainApiClient>((serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<DomainApiClientOptions>>().Value;

    if (Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out var uri))
    {
        client.BaseAddress = uri;
    }

    client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
}).AddHttpMessageHandler<CorrelationIdDelegatingHandler>()
  .AddHttpMessageHandler<TransientHttpRetryHandler>();

builder.Services.AddHttpClient<EventAdapterClient>((serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<EventBackboneConsumerOptions>>().Value;

    if (Uri.TryCreate(options.EventAdapterBaseUrl, UriKind.Absolute, out var uri))
    {
        client.BaseAddress = uri;
    }

    client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
}).AddHttpMessageHandler<CorrelationIdDelegatingHandler>()
  .AddHttpMessageHandler<TransientHttpRetryHandler>();

builder.Services.AddHostedService<TransferWorkItemConsumer>();

builder.Services.AddFxferHealthChecks();

var serviceName = "fxfer-transfer-worker";
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
        .AddEnvironmentVariableDetector()
        .AddAttributes(new[] { new KeyValuePair<string, object>("gainwell.domain", "fxfer") }))
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation();
        tracing.AddHttpClientInstrumentation();
        tracing.AddSource(TransferWorkItemConsumer.ActivitySourceName);

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

using System.Reflection;
using Fxfer.TransferWorker.Application.Batch;
using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Worker.Configuration;
using Fxfer.TransferWorker.Worker.Services;
using Fxfer.TransferWorker.Contracts.NotificationService;
using Fxfer.TransferWorker.Domain.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Fxfer.TransferWorker.Worker;

public sealed class WorkerBatchRuntime : IWorkerBatchRuntime
{
    private IServiceProvider? _services;

    public void Initialize()
    {
        if (_services is not null)
        {
            return;
        }

        var configuration = BuildConfiguration();

        var services = new ServiceCollection();

        services.AddSingleton<IConfiguration>(configuration);

        services.AddOptions<DomainApiClientOptions>()
            .Bind(configuration.GetSection(DomainApiClientOptions.SectionName));

        services.AddOptions<EventBackboneConsumerOptions>()
            .Bind(configuration.GetSection(EventBackboneConsumerOptions.SectionName));

        services.AddOptions<SecretsOptions>()
            .Bind(configuration.GetSection(SecretsOptions.SectionName));

        services.AddOptions<StagingOptions>()
            .Bind(configuration.GetSection(StagingOptions.SectionName));

        services.AddOptions<TransferExecutionOptions>()
            .Bind(configuration.GetSection(TransferExecutionOptions.SectionName));

        services.AddSingleton<CorrelationIdContext>();
        services.AddSingleton<TenantIdContext>();
        services.AddSingleton<IRequestContextAccessor, RequestContextAccessor>();
        services.AddTransient<IDataAccessFactory>(sp => new DataAccessFactory(
            sp.GetRequiredService<DomainApiClient>(),
            sp.GetRequiredService<IRequestContextAccessor>()));

        services.AddTransient<CorrelationIdDelegatingHandler>();
        services.AddTransient<TransientHttpRetryHandler>();

        services.AddHttpClient<DomainApiClient>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<DomainApiClientOptions>>().Value;

            if (Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out var uri))
            {
                client.BaseAddress = uri;
            }

            client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
        }).AddHttpMessageHandler<CorrelationIdDelegatingHandler>()
          .AddHttpMessageHandler<TransientHttpRetryHandler>();

        services.AddHttpClient<EventAdapterClient>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<EventBackboneConsumerOptions>>().Value;

            if (Uri.TryCreate(options.EventAdapterBaseUrl, UriKind.Absolute, out var uri))
            {
                client.BaseAddress = uri;
            }

            client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
        }).AddHttpMessageHandler<CorrelationIdDelegatingHandler>()
          .AddHttpMessageHandler<TransientHttpRetryHandler>();

        services.AddSingleton<ITenantAppSettingsManager, ConfigurationTenantAppSettingsManager>();

        services.AddTransient<IPatternMatcher, PatternMatcher>();
        services.AddTransient<IFileTransferProtocols, FileTransferProtocols>();
        services.AddTransient<IFileTransferOperations, FileTransferOperations>();
        services.AddTransient<INotificationService, InMemoryNotificationService>();
        services.AddTransient<IFTSProducer, FTSProducer>();
        services.AddTransient<IConsumerFactory, ConsumerFactory>();

        services.AddKeyedTransient<IBatchExecutor<int>, FileTransferBatchExecutor>(IocConstants.FileTransferExecutor);
        services.AddTransient<IBatchExecutor<int>, FileTransferBatchExecutor>();

        var serviceName = "fxfer-transfer-worker";
        var serviceNamespace = "fxfer";
        var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0";
        var otlpEndpoint = configuration["OpenTelemetry:OtlpEndpoint"];

        services.AddLogging(logging =>
        {
            logging.AddOpenTelemetry(otelLogging =>
            {
                otelLogging.IncludeFormattedMessage = true;
                otelLogging.IncludeScopes = true;
                otelLogging.ParseStateValues = true;

                if (Uri.TryCreate(otlpEndpoint, UriKind.Absolute, out var uri))
                {
                    otelLogging.AddOtlpExporter(exporter => exporter.Endpoint = uri);
                }
            });

            logging.AddSimpleConsole();
        });

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(serviceName: serviceName, serviceNamespace: serviceNamespace, serviceVersion: version)
                .AddEnvironmentVariableDetector()
                .AddAttributes(new[] { new KeyValuePair<string, object>("gainwell.domain", "fxfer") }))
            .WithTracing(tracing =>
            {
                tracing.AddHttpClientInstrumentation();
                tracing.AddSource("Fxfer.TransferWorker.FileTransferBatchBasicRunner");
                tracing.AddSource("Fxfer.TransferWorker.FileTransferBatchManagedRunner");
                tracing.AddSource("Fxfer.TransferWorker.FileTransferBatchRerunCommand");

                if (Uri.TryCreate(otlpEndpoint, UriKind.Absolute, out var uri))
                {
                    tracing.AddOtlpExporter(exporter => exporter.Endpoint = uri);
                }
            })
            .WithMetrics(metrics =>
            {
                metrics.AddHttpClientInstrumentation();

                if (Uri.TryCreate(otlpEndpoint, UriKind.Absolute, out var uri))
                {
                    metrics.AddOtlpExporter(exporter => exporter.Endpoint = uri);
                }
            });

        _services = services.BuildServiceProvider();
    }

    public IServiceProvider Services => _services ?? throw new InvalidOperationException("Batch runtime has not been initialized");

    public IBatchExecutor<int> ResolveBatchExecutor(string key)
    {
        try
        {
            return Services.GetRequiredKeyedService<IBatchExecutor<int>>(key);
        }
        catch (InvalidOperationException)
        {
            return Services.GetRequiredService<IBatchExecutor<int>>();
        }
    }

    public void Dispose()
    {
        if (_services is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    private static IConfiguration BuildConfiguration()
    {
        var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? "Production";

        return new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }
}

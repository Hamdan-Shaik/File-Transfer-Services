using System.Reflection;
using System.Text.Json.Serialization;
using AutoMapper;
using CoreWCF.Configuration;
using CoreWCF.Description;
using Fxfer.SoapFacade.CoreWcf.Contracts.Services;
using Fxfer.SoapFacade.Host;
using Fxfer.SoapFacade.Host.Clients;
using Fxfer.SoapFacade.Host.Configuration;
using Fxfer.SoapFacade.Host.Health;
using Fxfer.SoapFacade.Host.Logging;
using Fxfer.SoapFacade.Host.Middleware;
using Fxfer.SoapFacade.Host.Soap;
using Fxfer.SoapFacade.Host.Soap.FaultHandling;
using Fxfer.SoapFacade.Host.Soap.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

public partial class Program
{
    public static async Task Main(string[] args)
    {
        ServiceEventSource? serviceEventSource = null;

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureLogging(builder.Logging, builder.Configuration);
            SetProfiles(builder.Services);
            ConfigureServices(builder.Services, builder.Configuration);
            InitializeInstrumentationSettings(builder.Configuration, builder.Services);

            var app = builder.Build();
            serviceEventSource = app.Services.GetRequiredService<ServiceEventSource>();

            ConfigureApp(app);

            var tenantIds = app.Configuration.GetSection("Tenants")
                .GetChildren()
                .Select(section => section.Key)
                .ToList();

            if (tenantIds.Count > 0)
            {
                var startup = app.Services.GetRequiredService<Startup>();
                await startup.InitializeTenants(tenantIds, app.Lifetime.ApplicationStopping);
            }

            serviceEventSource.ServiceTypeRegistered(Environment.ProcessId, typeof(Program).Name);

            await app.RunAsync();
        }
        catch (Exception e)
        {
            serviceEventSource?.ServiceHostInitializationFailed(e.ToString());
            throw;
        }
    }

    private static void ConfigureLogging(ILoggingBuilder loggingBuilder, IConfiguration configuration)
    {
        var otlpEndpoint = configuration["OpenTelemetry:OtlpEndpoint"];

        loggingBuilder.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
            logging.ParseStateValues = true;

            if (Uri.TryCreate(otlpEndpoint, UriKind.Absolute, out var uri))
            {
                logging.AddOtlpExporter(exporter => exporter.Endpoint = uri);
            }
        });
    }

    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        services.AddOptions<AuthenticationOptions>().Bind(configuration.GetSection("Authentication"));
        services.AddOptions<TenantResolutionOptions>().Bind(configuration.GetSection("TenantResolution"));
        services.AddOptions<DomainApiOptions>().Bind(configuration.GetSection("DomainApi"));

        services.AddFxferDomainApiClient();

        var authSection = configuration.GetSection("Authentication");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authSection["Authority"];
                options.Audience = authSection["Audience"];
                options.RequireHttpsMetadata = authSection.GetValue("RequireHttpsMetadata", true);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(2),
                    RequireExpirationTime = true,
                    RequireSignedTokens = true
                };
            });

        services.AddAuthorization();

        services.AddServiceModelServices();
        services.AddServiceModelMetadata();
        services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();
        services.AddSingleton<IServiceBehavior, FileTransferFaultBehavior>();

        services.AddTransient<FileTransferSvcService>();
        services.AddTransient<FileTransferSvcQueryService>();
        services.AddTransient<FileTransferSvcEventService>();

        services.AddFxferSoapFacadeHealthChecks();

        services.AddSingleton<ServiceEventSource>();
        services.AddTransient<Startup>();
    }

    internal static void InitializeInstrumentationSettings(IConfiguration configuration, IServiceCollection services)
    {
        var appInsightInstrumentationKey = configuration["InstrumentationKey"];
        _ = appInsightInstrumentationKey;

        // W3C trace context propagation.
        Sdk.SetDefaultTextMapPropagator(new CompositeTextMapPropagator(new TextMapPropagator[]
        {
            new TraceContextPropagator(),
            new BaggagePropagator()
        }));

        var serviceName = "fxfer-soap-facade";
        var serviceNamespace = "fxfer";
        var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0";
        var otlpEndpoint = configuration["OpenTelemetry:OtlpEndpoint"];
        var deploymentEnvironment = configuration["OpenTelemetry:DeploymentEnvironment"]
                                 ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                                 ?? "unknown";

        var resourceAttributes = new List<KeyValuePair<string, object>>
        {
            new("gainwell.domain", "fxfer"),
            new("deployment.environment", deploymentEnvironment)
        };

        var cloudProvider = Environment.GetEnvironmentVariable("CLOUD_PROVIDER");
        if (!string.IsNullOrWhiteSpace(cloudProvider))
        {
            resourceAttributes.Add(new("cloud.provider", cloudProvider));
        }

        var cloudRegion = Environment.GetEnvironmentVariable("CLOUD_REGION");
        if (!string.IsNullOrWhiteSpace(cloudRegion))
        {
            resourceAttributes.Add(new("cloud.region", cloudRegion));
        }

        var podName = Environment.GetEnvironmentVariable("K8S_POD_NAME") ?? Environment.GetEnvironmentVariable("HOSTNAME");
        if (!string.IsNullOrWhiteSpace(podName))
        {
            resourceAttributes.Add(new("k8s.pod.name", podName));
        }

        var k8sNamespace = Environment.GetEnvironmentVariable("K8S_NAMESPACE_NAME");
        if (!string.IsNullOrWhiteSpace(k8sNamespace))
        {
            resourceAttributes.Add(new("k8s.namespace.name", k8sNamespace));
        }

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(serviceName: serviceName, serviceNamespace: serviceNamespace, serviceVersion: version)
                .AddAttributes(resourceAttributes))
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
    }

    internal static void SetProfiles(IServiceCollection services)
    {
        var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
        services.AddSingleton(mapperConfiguration);
        services.AddSingleton<IMapper>(sp => mapperConfiguration.CreateMapper(sp.GetService));
    }

    private static void ConfigureApp(WebApplication app)
    {
        app.Logger.LogInformation("Current Environment is {environmentName}", app.Environment.EnvironmentName);

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<TenantContextMiddleware>();

        app.MapFxferSoapFacadeHealthEndpoints();

        SoapEndpointHost.MapSoapEndpoints(app);
    }
}





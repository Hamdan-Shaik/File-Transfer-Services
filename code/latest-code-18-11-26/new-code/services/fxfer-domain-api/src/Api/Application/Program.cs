using System.Reflection;
using System.Text.Json.Serialization;
using AutoMapper;
using Fxfer.DomainApi.Api;
using Fxfer.DomainApi.Api.Configuration;
using Fxfer.DomainApi.Api.Endpoints.Commands;
using Fxfer.DomainApi.Api.Endpoints.Queries;
using Fxfer.DomainApi.Api.Health;
using Fxfer.DomainApi.Api.Middleware;
using Fxfer.DomainApi.Application;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.Eventing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MediatR;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

public partial class Program
{
    public static void Main(string[] args)
    {
        // drop: legacy TenantBasHelper.Initialize(new FileTransferModuleConfigurator());
        // Target-state tenant context is established via JWT + tenant resolution middleware.

        var builder = WebApplication.CreateBuilder(args);

        ConfigureLogging(builder.Logging, builder.Configuration);
        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        ConfigureApp(app);

        app.Run();
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

        services.AddOptions<AuthenticationOptions>()
            .Bind(configuration.GetSection("Authentication"));

        services.AddOptions<TenantResolutionOptions>()
            .Bind(configuration.GetSection("TenantResolution"));

        services.AddHttpContextAccessor();
        services.AddTransient<HttpClientCorrelationIdHandler>();
        services.AddMemoryCache();

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

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        await FxferProblemDetails.WriteAsync(context.HttpContext, StatusCodes.Status401Unauthorized, "Unauthorized");
                    },
                    OnForbidden = async context =>
                    {
                        await FxferProblemDetails.WriteAsync(context.HttpContext, StatusCodes.Status403Forbidden, "Forbidden");
                    },
                    OnAuthenticationFailed = async context =>
                    {
                        await FxferProblemDetails.WriteAsync(context.HttpContext, StatusCodes.Status401Unauthorized, "Unauthorized");
                    }
                };
            });

        services.AddAuthorization();

        services.AddTransient<IFileTransferSvcService, FileTransferSvcService>();
        services.AddTransient<IFileTransferSvcQueryService, FileTransferSvcQueryService>();

        services.AddScoped<IFileTransferReadOnlyRepository, FileTransferReadOnlyRepository>();

        services.AddSingleton<TenantConnectionStringProvider>();

        services.AddDbContext<FxferDbContext>((serviceProvider, options) =>
        {
            var connectionString = serviceProvider.GetRequiredService<TenantConnectionStringProvider>().TryGetConnectionString();
            connectionString ??= configuration.GetConnectionString("Fxfer");
            connectionString ??= "Host=localhost;Database=fxfer;Username=fxfer;Password=fxfer";

            options.UseNpgsql(connectionString);
        });

        services.AddHttpClient<EventAdapterClient>((serviceProvider, client) =>
            {
                var baseUrl = serviceProvider.GetRequiredService<IConfiguration>()["EventAdapter:BaseUrl"];
                if (Uri.TryCreate(baseUrl, UriKind.Absolute, out var uri))
                {
                    client.BaseAddress = uri;
                }
            })
            // T-005: Propagate correlation_id to downstream services.
            .AddHttpMessageHandler<HttpClientCorrelationIdHandler>();

        // T-006: Publish outbox messages to the Event Adapter.
        services.AddScoped<FxferDomainEventFactory>();
        services.AddScoped<IFxferDomainEventFactory>(sp => sp.GetRequiredService<FxferDomainEventFactory>());
        services.AddScoped<Fxfer.DomainApi.Infrastructure.Eventing.Outbox.OutboxMessageWriter>();
        services.AddScoped<Fxfer.DomainApi.Infrastructure.Eventing.Outbox.OutboxPublisher>();
        services.AddHostedService<Fxfer.DomainApi.Infrastructure.Eventing.Outbox.OutboxPublisherHostedService>();

        // STD-APP-EVT-004: CQRS posture via MediatR command handlers + pipeline behaviors.
        services.AddMediatR(typeof(Fxfer.DomainApi.Application.Commands.FtpServers.AddFtpServerCommandHandler).Assembly);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(Fxfer.DomainApi.Application.Commands.Behaviors.TenantAuthorizationBehavior<,>));
        services.AddScoped<Fxfer.DomainApi.Application.Commands.Behaviors.ITenantContext, Fxfer.DomainApi.Application.Commands.Behaviors.HttpContextTenantContext>();

        services.AddFxferHealthChecks();

        services.AddTransient<ApiExceptionHandlingMiddleware>();

        var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
        services.AddSingleton(mapperConfiguration);
        services.AddSingleton<IMapper>(sp => mapperConfiguration.CreateMapper(sp.GetService));

        // T-005: Propagate W3C trace context and baggage (traceparent/tracestate/baggage).
        Sdk.SetDefaultTextMapPropagator(new CompositeTextMapPropagator(new TextMapPropagator[]
        {
            new TraceContextPropagator(),
            new BaggagePropagator()
        }));

        var serviceName = "fxfer-domain-api";
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

        // T-005: Emit OpenTelemetry traces/metrics/logs and tag with tenant_id/correlation_id/trace_id.
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
                metrics.AddMeter(RequestLoggingScopeMiddleware.MeterName);

                if (Uri.TryCreate(otlpEndpoint, UriKind.Absolute, out var uri))
                {
                    metrics.AddOtlpExporter(exporter => exporter.Endpoint = uri);
                }
            });

        // drop: legacy Autofac registrations for CQRS handlers/projection writers.
        // Target-state replaces these with ASP.NET endpoints + separate xfer-projection-worker projection processing.
    }

    private static void ConfigureApp(WebApplication app)
    {
        app.Logger.LogInformation("Current Environment is {environmentName}", app.Environment.EnvironmentName);

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            // drop: legacy Swagger/SwaggerUI middleware; the target contract is the checked-in OpenAPI YAML.
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }
        app.UseRouting();

        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseMiddleware<ApiExceptionHandlingMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<TenantContextMiddleware>();
        app.UseMiddleware<RequestLoggingScopeMiddleware>();
        app.UseMiddleware<IdempotencyMiddleware>();

        app.MapFxferHealthEndpoints();

        var domainApi = app.MapGroup("/api/fxfer-domain/v1")
            .RequireAuthorization();

        domainApi.MapQueryEndpoints();
        domainApi.MapCommandEndpoints();
    }
}

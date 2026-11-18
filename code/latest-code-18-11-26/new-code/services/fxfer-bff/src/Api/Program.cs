using System.Reflection;
using System.Text.Json.Serialization;
using Fxfer.Bff.Api.Configuration;
using Fxfer.Bff.Api.Endpoints.V1;
using Fxfer.Bff.Api.Health;
using Fxfer.Bff.Api.Middleware;
using Fxfer.Bff.Application.Providers;
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
    public static void Main(string[] args)
    {
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
        // T-008: Prohibit direct database access from the BFF; all persistence must occur via domain services.
        EnsureNoDirectDatabaseDependencies(
            typeof(Program).Assembly,
            typeof(IFileTransferCommandProvider).Assembly,
            typeof(IFileTransferQueryProvider).Assembly);

        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        services.AddOptions<AuthenticationOptions>()
            .Bind(configuration.GetSection("Authentication"));

        services.AddOptions<TenantResolutionOptions>()
            .Bind(configuration.GetSection("TenantResolution"));

        services.AddOptions<CorsOptions>()
            .Bind(configuration.GetSection("Cors"));

        services.AddOptions<DownstreamOptions>()
            .Bind(configuration.GetSection("Downstream"));

        services.AddOptions<FileTransferManagementOptions>()
            .Bind(configuration.GetSection("FileTransferManagement"));

        services.AddHttpContextAccessor();

        var authSection = configuration.GetSection("Authentication");

        // T-002: Stateless bearer-JWT authentication only (no server-side sessions/cookies).
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
                        await ProblemDetailsMiddleware.WriteAsync(context.HttpContext, StatusCodes.Status401Unauthorized, "Unauthorized");
                    },
                    OnForbidden = async context =>
                    {
                        await ProblemDetailsMiddleware.WriteAsync(context.HttpContext, StatusCodes.Status403Forbidden, "Forbidden");
                    },
                    OnAuthenticationFailed = async context =>
                    {
                        await ProblemDetailsMiddleware.WriteAsync(context.HttpContext, StatusCodes.Status401Unauthorized, "Unauthorized");
                    }
                };
            });

        services.AddAuthorization();

        // drop: legacy FileTransferSvcEventServiceController (CQRS event-distribution controller wrapper);
        // fxfer-bff uses explicit endpoint modules plus middleware for auth/tenant/origin policies.

        services.AddCors(options =>
        {
            options.AddPolicy("BffCors", policy =>
            {
                // T-002: Strict CORS allow-list for UI shell/micro-frontend origins.
                var allowedOriginsCsv = configuration["Cors:AllowedOriginsCsv"];
                var allowedOrigins = CorsOptions.ParseCsv(allowedOriginsCsv);

                if (allowedOrigins.Length > 0)
                {
                    policy.WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
            });
        });

        services.AddHttpClient<FxferDomainApiClient>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<DownstreamOptions>>().Value;

                if (Uri.TryCreate(options.FxferDomainApiBaseUrl, UriKind.Absolute, out var uri))
                {
                    client.BaseAddress = uri;
                }

                if (options.TimeoutSeconds > 0)
                {
                    client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
                }
            });

        services.AddTransient<DataListsClient>();

        services.AddTransient<IFileTransferCommandProvider, FileTransferCommandProvider>();
        services.AddTransient<IFileTransferQueryProvider, FileTransferQueryProvider>();

        services.AddFxferHealthChecks();

        Sdk.SetDefaultTextMapPropagator(new CompositeTextMapPropagator(new TextMapPropagator[]
        {
            new TraceContextPropagator(),
            new BaggagePropagator()
        }));

        var serviceName = "fxfer-bff";
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

    private static void ConfigureApp(WebApplication app)
    {
        app.Logger.LogInformation("Current Environment is {environmentName}", app.Environment.EnvironmentName);

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("BffCors");

        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseMiddleware<ProblemDetailsMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<TenantContextMiddleware>();

        // T-002: Origin/Referer enforcement for state-changing requests.
        app.UseMiddleware<OriginValidationMiddleware>();

        app.MapFxferHealthEndpoints();

        var api = app.MapGroup("/api/file-transfer/v1")
            .RequireAuthorization();

        api.MapTransferConfigurationsEndpoints();
        api.MapFtpServersEndpoints();
        api.MapFileTrackingEndpoints();
        api.MapSourceLocationsEndpoints();
        api.MapDataListsEndpoints();
        api.MapInboundTargetMetadataEndpoints();
    }

    internal static void EnsureNoDirectDatabaseDependencies(params Assembly[] assemblies)
    {
        var forbiddenAssemblyPrefixes = new[]
        {
            "Microsoft.EntityFrameworkCore",
            "Npgsql",
            "Dapper",
            "Microsoft.Data.SqlClient",
            "System.Data.SqlClient"
        };

        var referencedAssemblyNames = assemblies
            .Distinct()
            .SelectMany(a => a.GetReferencedAssemblies().Select(r => r.Name ?? string.Empty))
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var forbiddenReferences = referencedAssemblyNames
            .Where(name => forbiddenAssemblyPrefixes.Any(prefix => name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
            .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (forbiddenReferences.Length > 0)
        {
            throw new InvalidOperationException(
                $"BFF must not reference database client/ORM libraries. Found: {string.Join(", ", forbiddenReferences)}");
        }
    }
}


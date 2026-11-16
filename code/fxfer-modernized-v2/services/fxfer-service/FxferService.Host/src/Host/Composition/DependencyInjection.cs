using FxferService.Application.CommandHandlers;
using FxferService.Application.Composition;
using FxferService.Application.LegacyContracts;
using FxferService.Application.ProjectionWriters;
using FxferService.Application.Profiles;
using FxferService.Host.Hosting;
using FxferService.Infrastructure.Messaging.EventAdapter;
using FxferService.Infrastructure.Messaging.Outbox;
using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Tenancy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace FxferService.Host.Composition;

public static class DependencyInjection
{
    public static IServiceCollection AddFileTransferSvcMainHost(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Legacy: Program.SetProfiles()
        Mapper.Initialize(cfg =>
        {
            cfg.DetectAndRegisterProfiles(typeof(InboundTargetProfile).Assembly);
            cfg.DetectAndRegisterProfiles(typeof(IntegrationFileProfile).Assembly);
        });

        // Legacy: Program.InitializeInstrumentationSettings()
        var appInsightInstrumentationKey = configuration["InstrumentationKey"]; 
        if (!string.IsNullOrEmpty(appInsightInstrumentationKey))
        {
            LegacyTelemetryConfiguration.Active.InstrumentationKey = appInsightInstrumentationKey;

            services.Configure<OtlpExporterOptions>(options =>
            {
                var header = $"InstrumentationKey={appInsightInstrumentationKey}";
                options.Headers = string.IsNullOrWhiteSpace(options.Headers) ? header : $"{options.Headers},{header}";
            });
        }

        return services;
    }

    public static IServiceCollection AddBasFileTransferApiHost(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<ApiExceptionHandlingMiddleware>();

        services.AddControllers()
            .AddJsonOptions(o => { o.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase; })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var traceId = System.Diagnostics.Activity.Current?.TraceId.ToString() ?? context.HttpContext.TraceIdentifier;
                    var details = context.ModelState
                        .Where(kvp => kvp.Value?.Errors.Count > 0)
                        .SelectMany(kvp => kvp.Value!.Errors.Select(e => new ErrorDetail(kvp.Key, e.ErrorMessage, "INVALID")))
                        .ToArray();

                    var problem = new ProblemDetails
                    {
                        Title = "Validation failed",
                        Status = StatusCodes.Status400BadRequest,
                        Type = "https://httpstatuses.com/400",
                    };

                    problem.Extensions["error"] = new ErrorEnvelope(
                        code: "VALIDATION_ERROR",
                        message: "Validation failed for the request",
                        details: details,
                        timestamp: DateTimeOffset.UtcNow,
                        trace_id: traceId);

                    return new ObjectResult(problem)
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        ContentTypes = { "application/problem+json" },
                    };
                };
            });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            options.CustomSchemaIds(s => s.FullName!.Replace("+", "."));

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "fxfer-service",
                Version = "v1",
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "JWT Bearer token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                    },
                    Array.Empty<string>()
                },
            });
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var authority = configuration["Authentication:Jwt:Authority"];
                var audience = configuration["Authentication:Jwt:Audience"];

                options.RequireHttpsMetadata = configuration.GetValue("Authentication:Jwt:RequireHttpsMetadata", true);

                if (!string.IsNullOrWhiteSpace(authority))
                {
                    options.Authority = authority;
                }

                options.Audience = audience;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = !string.IsNullOrWhiteSpace(authority),
                    ValidateAudience = !string.IsNullOrWhiteSpace(audience),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    NameClaimType = "sub",
                };
            });

        services.AddAuthorization();

        services.AddHttpContextAccessor();
        services.AddScoped<RequestContext>();
        services.AddScoped<ITenantDbResolver, TenantDbResolver>();

        services.AddDbContext<FxferDbContext>((sp, options) =>
        {
            var requestContext = sp.GetRequiredService<RequestContext>();
            var resolver = sp.GetRequiredService<ITenantDbResolver>();
            var connectionString = resolver.ResolveConnectionString();

            var tenantId = requestContext.TenantId;
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                // T-003: Tenant isolation is enforced as database-per-tenant routing using tenant_id from validated JWTs.
                throw new InvalidOperationException("Tenant context is required for persistence access");
            }

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                options.UseNpgsql(connectionString);
                return;
            }

            var useInMemoryPerTenant = configuration.GetValue("Tenancy:UseInMemoryPerTenant", false);
            if (useInMemoryPerTenant)
            {
                options.UseInMemoryDatabase($"fxfer-{tenantId}");
                return;
            }

            // T-003: Tenant isolation is enforced as database-per-tenant routing using tenant_id from validated JWTs.
            throw new InvalidOperationException($"Missing configured tenant database connection for tenant_id '{tenantId}'");
        });
        services.AddScoped<IFileTransferReadOnlyRepository, FileTransferReadOnlyRepository>();

        services.AddBackendBasFileTransferSvc2();

        services.AddSingleton<IEventAdapter, NoopEventAdapter>();
        services.AddHostedService<OutboxPublisherService>();
        services.AddHostedService<StartupChecksHostedService>();

        services.AddHealthChecks()
            .AddCheck<LiveHealthCheck>("live", tags: new[] { "live" })
            .AddCheck<ReadyHealthCheck>("ready", tags: new[] { "ready" });

        var serviceName = configuration["OpenTelemetry:ServiceName"] ?? "fxfer-service";
        var serviceNamespace = configuration["OpenTelemetry:ServiceNamespace"] ?? "provider-file-transfer";

        services.AddOpenTelemetry()
            .ConfigureResource(r => r.AddService(serviceName: serviceName, serviceNamespace: serviceNamespace))
            .WithTracing(b => b
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.EnrichWithHttpRequest = (activity, request) =>
                    {
                        if (request.Headers.TryGetValue(RequestContextHeaderNames.CorrelationId, out var correlationId))
                        {
                            activity.SetTag("correlation_id", correlationId.ToString());
                        }
                    };
                })
                .AddHttpClientInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()
                .AddProcessor(new TelemetryTaggingProcessor())
                .AddOtlpExporter())
            .WithMetrics(b => b
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter());

        return services;
    }

    public static IServiceCollection AddFxferService(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        _ = environment;
        return services.AddFileTransferSvcMainHost(configuration)
            .AddBasFileTransferApiHost(configuration);
    }
}

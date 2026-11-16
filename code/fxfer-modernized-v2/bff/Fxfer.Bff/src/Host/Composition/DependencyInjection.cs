using System.Text;
using Fxfer.Bff.Application.LegacyContracts;
using Fxfer.Bff.Host.Hosting;
using Fxfer.Bff.Host.Middleware;
using Fxfer.Bff.Infrastructure.Downstream;
using Fxfer.Bff.Infrastructure.Events;
using Fxfer.Bff.Infrastructure.Observability;
using Fxfer.Bff.Infrastructure.Tenancy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Fxfer.Bff.Host.Composition;

public static class DependencyInjection
{
    public static IServiceCollection AddFxferBff(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        _ = environment;

        services.AddTransient<ErrorNormalizationMiddleware>();

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
                Title = "fxfer-bff",
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
                var issuer = configuration["Authentication:Jwt:Issuer"];
                var audience = configuration["Authentication:Jwt:Audience"];
                var signingKey = configuration["Authentication:Jwt:SigningKey"];

                options.RequireHttpsMetadata = configuration.GetValue("Authentication:Jwt:RequireHttpsMetadata", true);

                if (!string.IsNullOrWhiteSpace(authority))
                {
                    options.Authority = authority;
                }

                options.Audience = audience;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // T-002: Validate the operator JWT before any downstream calls.
                    ValidateIssuer = !string.IsNullOrWhiteSpace(authority) || !string.IsNullOrWhiteSpace(issuer),
                    ValidIssuer = string.IsNullOrWhiteSpace(authority) ? issuer : null,
                    ValidateAudience = !string.IsNullOrWhiteSpace(audience),
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = string.IsNullOrWhiteSpace(signingKey)
                        ? null
                        : new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                    NameClaimType = "sub",
                };
            });

        services.AddAuthorization();

        services.AddSingleton<IEventDistribution, NoOpEventDistribution>();

        services.AddHttpContextAccessor();
        services.AddScoped<TenantContext>();

        services.AddHttpClient<FxferServiceClient>(client =>
        {
            var baseUrl = configuration["Downstream:FxferService:BaseUrl"];
            if (!string.IsNullOrWhiteSpace(baseUrl))
            {
                client.BaseAddress = new Uri(baseUrl, UriKind.Absolute);
            }

            var timeoutSeconds = configuration.GetValue("Downstream:FxferService:TimeoutSeconds", 10);
            client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
        });

        services.AddHealthChecks()
            .AddCheck<LiveHealthCheck>("live", tags: new[] { "live" })
            .AddCheck<ReadyHealthCheck>("ready", tags: new[] { "ready" });

        services.AddFxferBffTelemetry(configuration);

        return services;
    }
}

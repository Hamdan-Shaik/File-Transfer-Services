using Fxfer.DomainApi.Api.Configuration;
using Microsoft.Extensions.Options;

namespace Fxfer.DomainApi.Api.Middleware;

public sealed class TenantContextMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TenantResolutionOptions _options;
    private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

    public TenantContextMiddleware(
        RequestDelegate next,
        IOptions<TenantResolutionOptions> options,
        Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        _next = next;
        _options = options.Value;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api/fxfer-domain", StringComparison.OrdinalIgnoreCase))
        {
            var tenantId = context.User.FindFirst(_options.TenantIdClaim)?.Value;
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                await FxferProblemDetails.WriteAsync(context, statusCode: StatusCodes.Status403Forbidden, title: "Missing tenant context");
                return;
            }

            // T-003: Tenant-scoped command/query operations must route only to configured tenants.
            var tenantConnectionString = _configuration[$"Tenants:{tenantId}:ConnectionString"];
            if (string.IsNullOrWhiteSpace(tenantConnectionString))
            {
                await FxferProblemDetails.WriteAsync(context, statusCode: StatusCodes.Status403Forbidden, title: "Unknown tenant context");
                return;
            }

            RequestContext.SetTenantId(context, tenantId);
        }

        await _next(context);
    }
}

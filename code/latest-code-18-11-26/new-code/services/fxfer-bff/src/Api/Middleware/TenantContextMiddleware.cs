using Fxfer.Bff.Api.Configuration;
using Microsoft.Extensions.Options;

namespace Fxfer.Bff.Api.Middleware;

public sealed class TenantContextMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TenantResolutionOptions _options;

    public TenantContextMiddleware(RequestDelegate next, IOptions<TenantResolutionOptions> options)
    {
        _next = next;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api/file-transfer", StringComparison.OrdinalIgnoreCase))
        {
            // T-001: Derive tenant scope from verified identity context (JWT claim tenant_id) and never from request bodies or caller-supplied headers.
            var tenantId = context.User.FindFirst(_options.TenantIdClaim)?.Value;
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                await ProblemDetailsMiddleware.WriteAsync(context, StatusCodes.Status401Unauthorized, "Unauthorized");
                return;
            }

            RequestContext.SetTenantId(context, tenantId);
        }

        await _next(context);
    }
}


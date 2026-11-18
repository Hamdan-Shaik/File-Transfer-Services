using Fxfer.SoapFacade.Host.Configuration;
using Microsoft.Extensions.Options;

namespace Fxfer.SoapFacade.Host.Middleware;

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
        if (context.Request.Path.StartsWithSegments("/soap", StringComparison.OrdinalIgnoreCase))
        {
            var tenantId = context.User.FindFirst(_options.TenantIdClaim)?.Value;
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Missing tenant context");
                return;
            }

            RequestContext.SetTenantId(context, tenantId);
        }

        await _next(context);
    }
}
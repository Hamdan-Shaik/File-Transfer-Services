using Fxfer.Bff.Api.Configuration;
using Microsoft.Extensions.Options;

namespace Fxfer.Bff.Api.Middleware;

public sealed class OriginValidationMiddleware
{
    private static readonly HashSet<string> StateChangingMethods = new(StringComparer.OrdinalIgnoreCase)
    {
        HttpMethods.Post,
        HttpMethods.Put,
        HttpMethods.Patch,
        HttpMethods.Delete
    };

    private readonly RequestDelegate _next;
    private readonly string[] _allowedOrigins;

    public OriginValidationMiddleware(RequestDelegate next, IOptions<CorsOptions> options)
    {
        _next = next;
        _allowedOrigins = CorsOptions.ParseCsv(options.Value.AllowedOriginsCsv);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // T-002: Enforce Origin/Referer validation for state-changing browser requests.
        if (!context.Request.Path.StartsWithSegments("/api/file-transfer", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        if (!StateChangingMethods.Contains(context.Request.Method))
        {
            await _next(context);
            return;
        }

        if (_allowedOrigins.Length == 0)
        {
            await ProblemDetailsMiddleware.WriteAsync(context, StatusCodes.Status403Forbidden, "Origin validation is not configured");
            return;
        }

        var origin = context.Request.Headers["Origin"].FirstOrDefault();
        var referer = context.Request.Headers["Referer"].FirstOrDefault();

        var candidateOrigin = origin;
        if (string.IsNullOrWhiteSpace(candidateOrigin) && !string.IsNullOrWhiteSpace(referer) && Uri.TryCreate(referer, UriKind.Absolute, out var refererUri))
        {
            candidateOrigin = $"{refererUri.Scheme}://{refererUri.Authority}";
        }

        if (string.IsNullOrWhiteSpace(candidateOrigin))
        {
            await ProblemDetailsMiddleware.WriteAsync(context, StatusCodes.Status403Forbidden, "Missing Origin/Referer");
            return;
        }

        var allowed = _allowedOrigins.Any(o => string.Equals(o, candidateOrigin, StringComparison.OrdinalIgnoreCase));
        if (!allowed)
        {
            await ProblemDetailsMiddleware.WriteAsync(context, StatusCodes.Status403Forbidden, "Origin is not allowed");
            return;
        }

        await _next(context);
    }
}


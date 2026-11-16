using Microsoft.AspNetCore.Mvc;

namespace FxferService.Host.Composition;

internal sealed class ApiExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiExceptionHandlingMiddleware> _logger;

    public ApiExceptionHandlingMiddleware(RequestDelegate next, ILogger<ApiExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (context.Response.HasStarted)
            {
                throw;
            }

            var traceId = System.Diagnostics.Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;
            _logger.LogError(ex, "Unhandled exception (trace_id={trace_id})", traceId);

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";

            var problem = new ProblemDetails
            {
                Title = "Unhandled error",
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://httpstatuses.com/500",
            };

            problem.Extensions["error"] = new ErrorEnvelope(
                code: "INTERNAL_ERROR",
                message: "An unexpected error occurred",
                details: Array.Empty<ErrorDetail>(),
                timestamp: DateTimeOffset.UtcNow,
                trace_id: traceId);

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}

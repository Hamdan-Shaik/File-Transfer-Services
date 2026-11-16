using Microsoft.AspNetCore.Mvc;

namespace Fxfer.Bff.Host.Middleware;

public sealed class ErrorNormalizationMiddleware : IMiddleware
{
    private readonly ILogger<ErrorNormalizationMiddleware> _logger;

    public ErrorNormalizationMiddleware(ILogger<ErrorNormalizationMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (BadHttpRequestException)
        {
            var traceId = System.Diagnostics.Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;

            var problem = new ProblemDetails
            {
                Title = "Invalid request",
                Status = StatusCodes.Status400BadRequest,
                Type = "https://httpstatuses.com/400",
            };

            problem.Extensions["error"] = new ErrorEnvelope(
                code: "INVALID_REQUEST",
                message: "Invalid request",
                details: Array.Empty<ErrorDetail>(),
                timestamp: DateTimeOffset.UtcNow,
                trace_id: traceId);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problem, options: null, contentType: "application/problem+json");
        }
        catch (Microsoft.AspNetCore.Mvc.Formatters.InputFormatterException)
        {
            var traceId = System.Diagnostics.Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;

            var problem = new ProblemDetails
            {
                Title = "Invalid request",
                Status = StatusCodes.Status400BadRequest,
                Type = "https://httpstatuses.com/400",
            };

            problem.Extensions["error"] = new ErrorEnvelope(
                code: "INVALID_REQUEST",
                message: "Invalid request",
                details: Array.Empty<ErrorDetail>(),
                timestamp: DateTimeOffset.UtcNow,
                trace_id: traceId);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problem, options: null, contentType: "application/problem+json");
        }
        catch (System.Text.Json.JsonException)
        {
            var traceId = System.Diagnostics.Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;

            var problem = new ProblemDetails
            {
                Title = "Invalid request",
                Status = StatusCodes.Status400BadRequest,
                Type = "https://httpstatuses.com/400",
            };

            problem.Extensions["error"] = new ErrorEnvelope(
                code: "INVALID_REQUEST",
                message: "Invalid request",
                details: Array.Empty<ErrorDetail>(),
                timestamp: DateTimeOffset.UtcNow,
                trace_id: traceId);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problem, options: null, contentType: "application/problem+json");
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            // Client disconnected.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception while processing request");

            var traceId = System.Diagnostics.Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;

            var problem = new ProblemDetails
            {
                Title = "Request failed",
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://httpstatuses.com/500",
            };

            var code = ex switch
            {
                HttpRequestException => "DOWNSTREAM_HTTP_ERROR",
                TimeoutException => "DOWNSTREAM_TIMEOUT",
                _ => "UNHANDLED_ERROR",
            };

            problem.Extensions["error"] = new ErrorEnvelope(
                code: code,
                message: "Request failed",
                details: Array.Empty<ErrorDetail>(),
                timestamp: DateTimeOffset.UtcNow,
                trace_id: traceId);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(problem, options: null, contentType: "application/problem+json");
        }
    }
}

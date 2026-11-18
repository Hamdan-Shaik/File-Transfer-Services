using System.Diagnostics;
using Fxfer.Bff.Contracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace Fxfer.Bff.Api.Middleware;

public sealed class ProblemDetailsMiddleware
{
    private readonly RequestDelegate _next;

    public ProblemDetailsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            // Client canceled; avoid rewriting response.
            throw;
        }
        catch (HttpRequestException)
        {
            await WriteAsync(context, StatusCodes.Status502BadGateway, "Bad Gateway");
        }
        catch (Exception)
        {
            await WriteAsync(context, StatusCodes.Status500InternalServerError, "Internal Server Error");
        }
    }

    public static async Task WriteAsync(
        HttpContext context,
        int statusCode,
        string title,
        string? detail = null,
        string? type = null,
        IReadOnlyList<ValidationError>? errors = null)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var correlationId = RequestContext.GetCorrelationId(context) ?? RequestContext.GetOrCreateCorrelationId(context);
        var traceId = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;

        var problem = new ProblemDetails
        {
            Type = type ?? "about:blank",
            Title = title,
            Status = statusCode,
            Detail = detail,
            Instance = context.Request.Path
        };

        problem.Extensions["correlation_id"] = correlationId;
        problem.Extensions["trace_id"] = traceId;

        if (errors is not null)
        {
            problem.Extensions["errors"] = errors;
        }

        await context.Response.WriteAsJsonAsync(problem);
    }
}


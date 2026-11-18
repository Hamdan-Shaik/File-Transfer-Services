using System.Diagnostics;
using Fxfer.DomainApi.Contracts.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Fxfer.DomainApi.Api;

internal static class FxferProblemDetails
{
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

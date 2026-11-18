namespace Fxfer.SoapFacade.Host.Middleware;

public sealed class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers.TryGetValue(RequestContext.CorrelationIdHeaderName, out var values)
            ? values.FirstOrDefault()
            : null;

        if (!Guid.TryParse(correlationId, out _))
        {
            correlationId = Guid.NewGuid().ToString();
        }

        RequestContext.SetCorrelationId(context, correlationId);

        context.Response.OnStarting(() =>
        {
            context.Response.Headers[RequestContext.CorrelationIdHeaderName] = correlationId;
            return Task.CompletedTask;
        });

        return _next(context);
    }
}
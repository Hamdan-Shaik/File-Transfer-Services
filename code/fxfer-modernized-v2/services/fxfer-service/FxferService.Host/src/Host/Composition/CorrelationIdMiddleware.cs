namespace FxferService.Host.Composition;

internal sealed class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var correlationId = context.Request.Headers[RequestContextHeaderNames.CorrelationId].ToString();
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString("D");
            context.Request.Headers[RequestContextHeaderNames.CorrelationId] = correlationId;
        }

        context.Response.Headers[RequestContextHeaderNames.CorrelationId] = correlationId;

        await _next(context);
    }
}

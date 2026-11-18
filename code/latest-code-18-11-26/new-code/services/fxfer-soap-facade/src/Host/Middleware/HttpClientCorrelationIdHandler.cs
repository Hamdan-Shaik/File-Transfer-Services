using System.Diagnostics;

namespace Fxfer.SoapFacade.Host.Middleware;

public sealed class HttpClientCorrelationIdHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext;
        var correlationId = httpContext is null
            ? Activity.Current?.GetBaggageItem("correlation_id")
            : RequestContext.GetCorrelationId(httpContext) ?? Activity.Current?.GetBaggageItem("correlation_id");

        if (!string.IsNullOrWhiteSpace(correlationId) && !request.Headers.Contains(RequestContext.CorrelationIdHeaderName))
        {
            request.Headers.TryAddWithoutValidation(RequestContext.CorrelationIdHeaderName, correlationId);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
using System.Diagnostics;
using Fxfer.DomainApi.Domain.Constants;

namespace Fxfer.DomainApi.Api.Middleware;

// T-005: Propagate correlation_id end-to-end for outbound HTTP calls.
public sealed class HttpClientCorrelationIdHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext;
        var correlationId = httpContext is null
            ? Activity.Current?.GetBaggageItem("correlation_id")
            : RequestContext.GetCorrelationId(httpContext) ?? Activity.Current?.GetBaggageItem("correlation_id");

        if (!string.IsNullOrWhiteSpace(correlationId) && !request.Headers.Contains(CommonConstants.CorrelationIdHeaderName))
        {
            request.Headers.TryAddWithoutValidation(CommonConstants.CorrelationIdHeaderName, correlationId);
        }

        return base.SendAsync(request, cancellationToken);
    }
}


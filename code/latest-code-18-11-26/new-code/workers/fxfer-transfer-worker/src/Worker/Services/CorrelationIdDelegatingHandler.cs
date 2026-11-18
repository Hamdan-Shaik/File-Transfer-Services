using Fxfer.TransferWorker.Worker.Middleware;

namespace Fxfer.TransferWorker.Worker.Services;

public sealed class CorrelationIdDelegatingHandler : DelegatingHandler
{
    private readonly CorrelationIdContext _correlationIdContext;

    public CorrelationIdDelegatingHandler(CorrelationIdContext correlationIdContext)
    {
        _correlationIdContext = correlationIdContext;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!request.Headers.Contains(CorrelationIdMiddleware.HeaderName))
        {
            var correlationId = _correlationIdContext.Get();

            if (!string.IsNullOrWhiteSpace(correlationId))
            {
                request.Headers.TryAddWithoutValidation(CorrelationIdMiddleware.HeaderName, correlationId);
            }
        }

        return base.SendAsync(request, cancellationToken);
    }
}


namespace Fxfer.TransferWorker.Worker.Services;

public sealed class TransientHttpRetryHandler : DelegatingHandler
{
    private static readonly TimeSpan BaseDelay = TimeSpan.FromMilliseconds(200);

    private readonly ILogger<TransientHttpRetryHandler> _logger;

    public TransientHttpRetryHandler(ILogger<TransientHttpRetryHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!IsIdempotent(request.Method))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        const int maxAttempts = 3;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                var response = await base.SendAsync(request, cancellationToken);

                if (!ShouldRetry(response.StatusCode) || attempt == maxAttempts)
                {
                    return response;
                }

                response.Dispose();
            }
            catch (HttpRequestException ex) when (attempt < maxAttempts)
            {
                _logger.LogWarning(ex, "Transient HTTP failure; retrying attempt {Attempt}/{MaxAttempts}", attempt, maxAttempts);
            }

            await Task.Delay(GetDelay(attempt), cancellationToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private static bool IsIdempotent(HttpMethod method)
    {
        return method == HttpMethod.Get || method == HttpMethod.Head || method == HttpMethod.Options;
    }

    private static bool ShouldRetry(System.Net.HttpStatusCode statusCode)
    {
        var code = (int)statusCode;
        return statusCode == System.Net.HttpStatusCode.RequestTimeout || (code >= 500 && code <= 599);
    }

    private static TimeSpan GetDelay(int attempt)
    {
        var exponential = Math.Pow(2, attempt - 1);
        var jitterMs = Random.Shared.Next(0, 150);
        return TimeSpan.FromMilliseconds(BaseDelay.TotalMilliseconds * exponential + jitterMs);
    }
}


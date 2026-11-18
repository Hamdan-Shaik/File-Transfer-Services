using Fxfer.Bff.Api.Configuration;
using Microsoft.Extensions.Options;

namespace Fxfer.Bff.Application.Providers;

public sealed class FxferDomainApiClient
{
    private const string CorrelationIdHeaderName = "X-Correlation-ID";
    private const string IdempotencyKeyHeaderName = "Idempotency-Key";

    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DownstreamOptions _options;

    public FxferDomainApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IOptions<DownstreamOptions> options)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _options = options.Value;
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        CopyHeader("Authorization", request);
        CopyHeader(CorrelationIdHeaderName, request);
        CopyHeader(IdempotencyKeyHeaderName, request);

        var attempts = Math.Max(0, _options.MaxRetries) + 1;

        for (var attempt = 1; attempt <= attempts; attempt++)
        {
            try
            {
                return await _httpClient.SendAsync(request, cancellationToken);
            }
            catch (HttpRequestException) when (attempt < attempts)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(200 * attempt), cancellationToken);
            }
        }

        throw new HttpRequestException("Downstream request failed after retries.");
    }

    private void CopyHeader(string headerName, HttpRequestMessage request)
    {
        var incoming = _httpContextAccessor.HttpContext;
        if (incoming is null)
        {
            return;
        }

        if (!incoming.Request.Headers.TryGetValue(headerName, out var values))
        {
            return;
        }

        if (request.Headers.Contains(headerName))
        {
            return;
        }

        request.Headers.TryAddWithoutValidation(headerName, values.ToArray());
    }
}


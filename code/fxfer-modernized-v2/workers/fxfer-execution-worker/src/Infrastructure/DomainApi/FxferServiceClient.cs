using System.Net.Http.Headers;

using System.Net;
using System.Net.Http.Json;

namespace Fxfer.ExecutionWorker.Infrastructure.DomainApi;

public sealed class FxferServiceClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public FxferServiceClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<bool> CanReachAsync(CancellationToken cancellationToken)
    {
        var baseUrl = _configuration["FxferService:BaseUrl"];
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            return false;
        }

        try
        {
            var client = _httpClientFactory.CreateClient(nameof(FxferServiceClient));
            client.BaseAddress = new Uri(baseUrl, UriKind.Absolute);
            client.Timeout = TimeSpan.FromSeconds(2);

            using var request = new HttpRequestMessage(HttpMethod.Get, "/health/live");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

            using var response = await client.SendAsync(request, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task RecordSkeletonOutcomeAsync(Guid runId, string idempotencyKey, CancellationToken cancellationToken)
    {
        // T-005: workers must report outcomes idempotently so retries/redrives do not create duplicate authoritative outcomes.
        var baseUrl = _configuration["FxferService:BaseUrl"];
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException("FxferService:BaseUrl is not configured");
        }

        var client = _httpClientFactory.CreateClient(nameof(FxferServiceClient));
        client.BaseAddress = new Uri(baseUrl, UriKind.Absolute);

        var requestBody = new RecordRunOutcomeRequest(runId, OutcomeCode: "accepted", OccurredAt: DateTimeOffset.UtcNow);

        using var request = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/transfer-runs/{runId:D}/outcomes")
        {
            Content = JsonContent.Create(requestBody)
        };

        request.Headers.TryAddWithoutValidation("Idempotency-Key", idempotencyKey);

        using var response = await client.SendAsync(request, cancellationToken);
        if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.Conflict)
        {
            return;
        }

        response.EnsureSuccessStatusCode();
    }
}

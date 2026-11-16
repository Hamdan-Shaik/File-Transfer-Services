using System.Diagnostics;
using System.Net.Http.Headers;

namespace Fxfer.SchedulerWorker.Infrastructure.DomainApi;

public sealed record RunIntent(Guid RunId, string CorrelationId, string TraceId);

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

    public async Task<RunIntent?> TryCreateRunIntentAsync(string tenantId, CancellationToken cancellationToken)
    {
        var reachable = await CanReachAsync(cancellationToken);
        if (!reachable)
        {
            return null;
        }

        var traceId = Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString("N");
        var correlationId = Guid.NewGuid().ToString("N");

        // shortcut: no authoritative run-intent API contract wired yet, upgrade when fxfer-service intent endpoint is specified.
        return new RunIntent(RunId: Guid.NewGuid(), CorrelationId: correlationId, TraceId: traceId);
    }
}

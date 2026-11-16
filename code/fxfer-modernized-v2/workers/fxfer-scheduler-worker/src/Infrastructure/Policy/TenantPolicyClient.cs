using System.Net.Http.Headers;

namespace Fxfer.SchedulerWorker.Infrastructure.Policy;

public sealed class TenantPolicyClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public TenantPolicyClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<bool> CanReachAsync(CancellationToken cancellationToken)
    {
        var baseUrl = _configuration["TenantPolicy:BaseUrl"];
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            return false;
        }

        try
        {
            var client = _httpClientFactory.CreateClient(nameof(TenantPolicyClient));
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
}

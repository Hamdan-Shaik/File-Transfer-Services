using Fxfer.SoapFacade.Host.Configuration;
using Microsoft.Extensions.Options;

namespace Fxfer.SoapFacade.Host;

public sealed class Startup
{
    private readonly ILogger<Startup> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly DomainApiOptions _domainApiOptions;

    public Startup(ILogger<Startup> logger, IHttpClientFactory httpClientFactory, IOptions<DomainApiOptions> domainApiOptions)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _domainApiOptions = domainApiOptions.Value;
    }

    public async Task InitializeTenants(IReadOnlyCollection<string> tenantIds, CancellationToken ct = default)
    {
        try
        {
            foreach (var tenantId in tenantIds)
            {
                await CheckServiceHealth(tenantId, ct);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Service startup partially succeeded");
        }
    }

    // drop: legacy CQRS module configurator

    private async Task CheckServiceHealth(string tenantId, CancellationToken ct)
    {
        _ = tenantId;

        var client = _httpClientFactory.CreateClient();
        if (Uri.TryCreate(_domainApiOptions.BaseUrl, UriKind.Absolute, out var uri))
        {
            client.BaseAddress = uri;
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/health/ready");
        request.Headers.TryAddWithoutValidation(RequestContext.CorrelationIdHeaderName, Guid.NewGuid().ToString());

        using var response = await client.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();
    }
}


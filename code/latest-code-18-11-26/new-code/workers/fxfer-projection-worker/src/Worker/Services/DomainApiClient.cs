namespace Fxfer.ProjectionWorker.Worker.Services;

public sealed class DomainApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DomainApiClient> _logger;

    public DomainApiClient(HttpClient httpClient, ILogger<DomainApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public Task<HttpResponseMessage> GetAsync(string relativeUrl, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Calling fxfer-domain-api GET {Url}", relativeUrl);
        return _httpClient.GetAsync(relativeUrl, cancellationToken);
    }
}


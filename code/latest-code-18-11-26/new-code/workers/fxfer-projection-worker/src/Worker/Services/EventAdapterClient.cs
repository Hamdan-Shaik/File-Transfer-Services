namespace Fxfer.ProjectionWorker.Worker.Services;

public sealed class EventAdapterClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<EventAdapterClient> _logger;

    public EventAdapterClient(HttpClient httpClient, ILogger<EventAdapterClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public Task<HttpResponseMessage> GetAsync(string relativeUrl, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Calling event-adapter GET {Url}", relativeUrl);
        return _httpClient.GetAsync(relativeUrl, cancellationToken);
    }
}


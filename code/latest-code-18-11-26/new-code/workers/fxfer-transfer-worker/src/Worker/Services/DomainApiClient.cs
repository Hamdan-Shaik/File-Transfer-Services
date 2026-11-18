using Fxfer.TransferWorker.Application.DataAccess;

namespace Fxfer.TransferWorker.Worker.Services;

public sealed class DomainApiClient : IFxferDomainApiClient
{
    private readonly HttpClient _httpClient;

    public DomainApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return _httpClient.SendAsync(request, cancellationToken);
    }
}


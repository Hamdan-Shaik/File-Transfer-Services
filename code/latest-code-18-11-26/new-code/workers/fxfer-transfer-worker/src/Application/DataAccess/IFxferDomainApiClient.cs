namespace Fxfer.TransferWorker.Application.DataAccess;

public interface IFxferDomainApiClient
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
}


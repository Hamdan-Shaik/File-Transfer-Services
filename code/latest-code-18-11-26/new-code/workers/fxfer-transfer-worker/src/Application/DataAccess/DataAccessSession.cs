namespace Fxfer.TransferWorker.Application.DataAccess;

public sealed class DataAccessSession : IDisposable
{
    public DataAccessSession(IFxferDomainApiClient domainApiClient, IRequestContextAccessor requestContextAccessor)
    {
        DomainApiClient = domainApiClient;
        RequestContextAccessor = requestContextAccessor;
    }

    public IFxferDomainApiClient DomainApiClient { get; }

    public IRequestContextAccessor RequestContextAccessor { get; }

    public DataAccessTransaction BeginTransaction()
    {
        return new DataAccessTransaction();
    }

    public void Dispose()
    {
    }
}


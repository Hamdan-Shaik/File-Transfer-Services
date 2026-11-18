namespace Fxfer.TransferWorker.Application.DataAccess;

public abstract class DataAccessBase<TOutput> : IDataAccess<TOutput>
{
    protected DataAccessBase(IFxferDomainApiClient domainApiClient, IRequestContextAccessor requestContextAccessor)
    {
        DomainApiClient = domainApiClient;
        RequestContextAccessor = requestContextAccessor;
    }

    protected IFxferDomainApiClient DomainApiClient { get; }

    protected IRequestContextAccessor RequestContextAccessor { get; }

    public async Task<TOutput> ExecuteAsync(CancellationToken cancellationToken)
    {
        return await ExecuteQueryAsync(cancellationToken);
    }

    protected abstract Task<TOutput> ExecuteQueryAsync(CancellationToken cancellationToken);
}


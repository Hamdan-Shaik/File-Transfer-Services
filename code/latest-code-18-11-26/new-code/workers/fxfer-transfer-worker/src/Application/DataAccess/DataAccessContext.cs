namespace Fxfer.TransferWorker.Application.DataAccess;

public sealed record DataAccessContext(
    IFxferDomainApiClient DomainApiClient,
    IRequestContextAccessor RequestContextAccessor);


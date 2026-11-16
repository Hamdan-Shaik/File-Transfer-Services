using FxferService.Infrastructure.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Entities;

namespace FxferService.Application.Persistence.Batch;

public sealed class GetSuspendedLocation : DataAccessBase<FtsSourceLocationEntity?>
{
    private readonly Guid _sourceLocationId;

    public GetSuspendedLocation(Guid sourceLocationId)
    {
        _sourceLocationId = sourceLocationId;
    }

    protected override FtsSourceLocationEntity? ExecuteQuery(IDbContextBase dbcontext)
    {
        return dbcontext.Set<FtsSourceLocationEntity>().SingleOrDefault(s => s.FtsSourceLocationId == _sourceLocationId);
    }
}
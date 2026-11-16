using FxferService.Application.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Entities;

namespace FxferService.Infrastructure.Persistence.Batch;

public sealed class UpdateSourceSuspendStatusDataAccess : DataAccessBase<bool>
{
    private readonly FtsSourceLocationEntity? _ftsSourceLocation;

    public UpdateSourceSuspendStatusDataAccess(FtsSourceLocationEntity ftsFile)
    {
        _ftsSourceLocation = ftsFile;
    }

    protected override bool ExecuteQuery(IDbContextBase dbcontext)
    {
        bool result = false;

        if (_ftsSourceLocation != null)
        {
            _ftsSourceLocation.IsSuspended = false;
            _ftsSourceLocation.SuspendedChangedAt = DateTimeOffset.UtcNow;
            result = dbcontext.SaveChanges() > 0;
        }

        return result;
    }
}
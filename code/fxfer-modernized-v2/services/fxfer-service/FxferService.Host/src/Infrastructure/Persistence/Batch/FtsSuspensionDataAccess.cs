using FxferService.Application.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Batch.Contracts;
using FxferService.Infrastructure.Persistence.Entities;

namespace FxferService.Infrastructure.Persistence.Batch;

public sealed class FtsSuspensionDataAccess : DataAccessBase<FtsSuspensionModel>
{
    private readonly FtsSuspensionSearchCriteria _query;

    public FtsSuspensionDataAccess(FtsSuspensionSearchCriteria searchQuery)
    {
        _query = searchQuery;
    }

    protected override FtsSuspensionModel ExecuteQuery(IDbContextBase dbcontext)
    {
        FtsSuspensionModel suspensionModel = new();
        FtsSourceLocationEntity? sourceLocations = dbcontext.Set<FtsSourceLocationEntity>()
            .Where(x => x.FtsSourceLocationId == _query.SourceLocationId)
            .FirstOrDefault();

        if (sourceLocations != null)
        {
            FtsSuspensionEntity entitySourceLocation = CreateSourceLocationEntityData(dbcontext);
            dbcontext.Set<FtsSuspensionEntity>().Add(entitySourceLocation);

            sourceLocations.IsSuspended = true;
            sourceLocations.SuspendedChangedAt = DateTimeOffset.UtcNow;
            dbcontext.SaveChanges();

            suspensionModel.SuspensionId = entitySourceLocation.SuspensionId;
        }

        return suspensionModel;
    }

    private FtsSuspensionEntity CreateSourceLocationEntityData(IDbContextBase dbcontext)
    {
        _ = dbcontext;
        FtsSuspensionEntity entitySourceLocation = new()
        {
            SuspensionId = Guid.NewGuid(),
            FtsSourceLocationId = _query.SourceLocationId,
            ErrorText = _query.ErrorText,
            StackTrace = LimitLength(_query.StackTrace, 1000),
            OperatorId = _query.OperatorId,
            LastModifiedAt = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow,
        };

        return entitySourceLocation;
    }

    private string LimitLength(string source, int maxLength)
    {
        if (source.Length <= maxLength)
        {
            return source;
        }

        return source.Substring(0, maxLength);
    }
}
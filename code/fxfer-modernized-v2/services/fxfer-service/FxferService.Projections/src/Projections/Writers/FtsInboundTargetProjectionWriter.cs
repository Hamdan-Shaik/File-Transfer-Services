using FxferService.Projections.Infrastructure.Messaging.Contracts.Events;
using FxferService.Projections.Infrastructure.Persistence;
using FxferService.Projections.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FxferService.Projections.Projections.Writers;

public sealed class FtsInboundTargetProjectionWriter
{
    private readonly FxferProjectionsDbContext _dbContext;
    private readonly TimeProvider _timeProvider;

    public FtsInboundTargetProjectionWriter(FxferProjectionsDbContext dbContext, TimeProvider timeProvider)
    {
        _dbContext = dbContext;
        _timeProvider = timeProvider;
    }

    public async Task HandleAsync(FtsInboundTargetAdded @event, CancellationToken ct)
    {
        await AddFtsInboundTargetAsync(@event, ct);
        await _dbContext.SaveChangesAsync(ct);
        return;
    }

    public async Task HandleAsync(FtsInboundTargetInvalidated @event, CancellationToken ct)
    {
        var entity = await _dbContext.FtsInboundTargets.SingleAsync(x => x.FtsInboundTargetId == @event.FtsInboundTargetId, ct);
        entity.IsValid = false;
        entity.LastModifiedTs = _timeProvider.GetUtcNow();
        entity.OperatorId = @event.UserID;
        await _dbContext.SaveChangesAsync(ct);
    }

    private Task AddFtsInboundTargetAsync(FtsInboundTargetAdded @event, CancellationToken ct)
    {
        var utcNow = _timeProvider.GetUtcNow().UtcDateTime;
        _dbContext.FtsInboundTargets.Add(MapFtsInboundTarget(@event, utcNow));
        return Task.CompletedTask;
    }

    private static FtsInboundTargetProjection MapFtsInboundTarget(FtsInboundTargetAdded @event, DateTime utcNow)
    {
        var utcNowOffset = new DateTimeOffset(utcNow);

        return new FtsInboundTargetProjection
        {
            FtsInboundTargetId = Guid.NewGuid(),
            ExternalFileDefnId = @event.FTEMetadataId,
            ImportJobName = @event.ImportJobName,
            ContractName = @event.ContractName,
            OperatorId = @event.OperatorId,
            IsValid = @event.IsValid,
            CreatedTs = utcNowOffset,
            LastModifiedTs = utcNowOffset,
        };
    }
}


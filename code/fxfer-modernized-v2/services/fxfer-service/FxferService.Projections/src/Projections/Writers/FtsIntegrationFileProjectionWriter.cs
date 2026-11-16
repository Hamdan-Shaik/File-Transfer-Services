using FxferService.Projections.Infrastructure.Messaging.Contracts.Domain;
using FxferService.Projections.Infrastructure.Messaging.Contracts.Events;
using FxferService.Projections.Infrastructure.Persistence;
using FxferService.Projections.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FxferService.Projections.Projections.Writers;

public sealed class FtsIntegrationFileProjectionWriter
{
    private readonly FxferProjectionsDbContext _dbContext;
    private readonly TimeProvider _timeProvider;

    public FtsIntegrationFileProjectionWriter(FxferProjectionsDbContext dbContext, TimeProvider timeProvider)
    {
        _dbContext = dbContext;
        _timeProvider = timeProvider;
    }

    public async Task HandleAsync(IntegrationFileAdded @event, CancellationToken ct)
    {
        await AddFtsIntegrationFileAsync(@event, ct);
        await _dbContext.SaveChangesAsync(ct);
        return;
    }

    public async Task HandleAsync(IntegrationFileUpdated @event, CancellationToken ct)
    {
        // F02-069: update integration file config/processed/valid/operator + stamp UTC last-modified.
        var entity = await _dbContext.FtsIntegrationFiles.SingleAsync(x => x.FtsIntegrationFileId == @event.FtsIntegrationFileId, ct);
        entity.ConfigId = @event.ConfigId;
        entity.IsProcessed = true;
        entity.IsValid = @event.IsValid;
        entity.LastModifiedTs = _timeProvider.GetUtcNow();
        entity.OperatorId = @event.OperatorId;
        await _dbContext.SaveChangesAsync(ct);
    }

    private Task AddFtsIntegrationFileAsync(IntegrationFileAdded @event, CancellationToken ct)
    {
        var utcNow = _timeProvider.GetUtcNow().UtcDateTime;

        foreach (GroupInfo groupInfo in @event.GroupInfo.Where(e => e.NumberOfRecords > 0))
        {
            _dbContext.FtsIntegrationFiles.Add(MapFtsIntegrationFile(@event, groupInfo, utcNow));
        }

        return Task.CompletedTask;
    }

    private static FtsIntegrationFileProjection MapFtsIntegrationFile(IntegrationFileAdded @event, GroupInfo groupInfo, DateTime utcNow)
    {
        var utcNowOffset = new DateTimeOffset(utcNow);

        return new FtsIntegrationFileProjection
        {
            FtsIntegrationFileId = Guid.NewGuid(),
            GroupId = groupInfo.GroupId,
            ConfigId = null,
            FileName = @event.FileName,
            EtfOutboundMetadataDefnId = @event.ETFOutboundMetadataId,
            IsProcessed = false,
            CreatedTs = utcNowOffset,
            LastModifiedTs = utcNowOffset,
            IsValid = true,
            OperatorId = @event.OperatorId,
        };
    }
}


using Fxfer.ProjectionWorker.Infrastructure.DataAccess;
using Fxfer.ProjectionWorker.Infrastructure.DataAccess.Entities;
using Fxfer.ProjectionWorker.Worker.Eventing;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.ProjectionWorker.Infrastructure.Projections;

public sealed class FtsIntegrationFileProjectionWriter
{
    private readonly FxferProjectionDbContext _db;

    public FtsIntegrationFileProjectionWriter(FxferProjectionDbContext db)
    {
        _db = db;
    }

    public async Task HandleIntegrationFileCreatedAsync(
        FxferEventEnvelope envelope,
        IntegrationFileCreatedProjectionPayload payload,
        CancellationToken ct)
    {
        var now = DateTimeOffset.UtcNow;

        foreach (var groupInfo in payload.GroupInfo.Where(e => e.NumberOfRecords > 0))
        {
            var ftsIntegrationFile = new FtsIntegrationFile
            {
                FtsIntegrationFileId = Guid.NewGuid(),
                GroupId = groupInfo.GroupId,
                ConfigId = null,
                FileName = payload.FileName,
                EtfOutboundMetadataDefnId = payload.EtfOutboundMetadataDefnId,
                IsProcessed = false,
                CreatedTs = now,
                LastModifiedTs = now,
                IsValid = true,
                OperatorId = payload.OperatorId,
            };

            _db.IntegrationFiles.Add(ftsIntegrationFile);
        }

        await _db.SaveChangesAsync(ct);
    }

    public async Task HandleIntegrationFileUpdatedAsync(
        FxferEventEnvelope envelope,
        IntegrationFileUpdatedProjectionPayload payload,
        CancellationToken ct)
    {
        var existing = await _db.IntegrationFiles
            .Where(x => x.FtsIntegrationFileId == payload.FtsIntegrationFileId)
            .FirstOrDefaultAsync(ct);

        if (existing is null)
        {
            throw new NullReferenceException();
        }

        var now = DateTimeOffset.UtcNow;

        var updated = existing with
        {
            // F03-012: store ConfigId, mark processed, store active flag and operator id, and update last-modified.
            ConfigId = payload.ConfigId,
            IsProcessed = true,
            // F03-013: persist the active/inactive flag used by later lookups.
            IsValid = payload.IsValid,
            LastModifiedTs = now,
            OperatorId = payload.OperatorId,
        };

        _db.Entry(existing).CurrentValues.SetValues(updated);
        await _db.SaveChangesAsync(ct);
    }
}

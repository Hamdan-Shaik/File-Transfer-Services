using System.Text.Json;
using Fxfer.ProjectionWorker.Infrastructure.DataAccess;
using Fxfer.ProjectionWorker.Infrastructure.DataAccess.Entities;
using Fxfer.ProjectionWorker.Worker.Eventing;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.ProjectionWorker.Infrastructure.Projections;

public sealed class FtsInboundTargetProjectionWriter
{
    private readonly FxferProjectionDbContext _db;

    public FtsInboundTargetProjectionWriter(FxferProjectionDbContext db)
    {
        _db = db;
    }

    public async Task HandleInboundTargetCreatedAsync(
        FxferEventEnvelope envelope,
        InboundTargetProjectionPayload payload,
        CancellationToken ct)
    {
        var now = DateTimeOffset.UtcNow;

        var inboundTarget = new FtsInboundTarget
        {
            FtsInboundTargetId = Guid.NewGuid(),
            FteMetadataId = payload.FteMetadataId,
            ImportJobName = payload.ImportJobName,
            ContractName = payload.ContractName,
            OperatorId = payload.OperatorId,
            IsValid = payload.IsValid,
            CreatedTs = now,
            LastModifiedTs = now,
        };

        _db.InboundTargets.Add(inboundTarget);
        await _db.SaveChangesAsync(ct);
    }

    public async Task HandleInboundTargetInvalidatedAsync(
        FxferEventEnvelope envelope,
        InboundTargetProjectionPayload payload,
        CancellationToken ct)
    {
        var existing = await _db.InboundTargets
            .Where(x => x.FtsInboundTargetId == payload.FtsInboundTargetId)
            .FirstOrDefaultAsync(ct);

        if (existing is null)
        {
            throw new NullReferenceException();
        }

        var configurationEvent = envelope.Payload.Deserialize<ConfigurationChangedV1>();
        if (configurationEvent is null)
        {
            throw new NullReferenceException();
        }
        var now = DateTimeOffset.UtcNow;

        var updated = existing with
        {
            IsValid = false,
            LastModifiedTs = now,
            OperatorId = configurationEvent.OperatorId,
        };

        _db.Entry(existing).CurrentValues.SetValues(updated);
        await _db.SaveChangesAsync(ct);
    }
}



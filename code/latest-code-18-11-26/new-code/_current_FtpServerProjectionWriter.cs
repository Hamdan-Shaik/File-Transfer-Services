using Fxfer.ProjectionWorker.Infrastructure.DataAccess;
using Fxfer.ProjectionWorker.Infrastructure.DataAccess.Entities;
using Fxfer.ProjectionWorker.Worker.Eventing;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.ProjectionWorker.Infrastructure.Projections;

public sealed class FtpServerProjectionWriter
{
    private readonly FxferProjectionDbContext _db;

    public FtpServerProjectionWriter(FxferProjectionDbContext db)
    {
        _db = db;
    }

    public async Task HandleFtpServerCreatedAsync(
        FxferEventEnvelope envelope,
        FtpServerProjectionPayload payload,
        CancellationToken ct)
    {
        // F02-035: When a FtpServerAdded event is handled, CreationDate/LastModifiedDate use UTC now.
        var now = DateTimeOffset.UtcNow;

        var newFtpServer = new FtpServer
        {
            FtpServerId = payload.FtpServerId,
            FtpDescription = payload.FtpDescription,
            FtpServerHost = payload.FtpServerHost,
            FtpServerName = payload.FtpServerName,
            FtpPasswordSecretRef = payload.FtpPasswordSecretRef,
            FtpPort = payload.FtpPort,
            FtpProxyAddress = payload.FtpProxyAddress,
            FtpProxyPort = payload.FtpProxyPort,
            FtpTransferMode = payload.FtpTransferMode,
            FtpUser = payload.FtpUser,
            UseSshAuth = payload.UseSshAuth,
            IsValid = payload.IsValid,
            OperatorId = payload.OperatorId,
            CreatedTs = now,
            LastModifiedTs = now,
        };

        _db.FtpServers.Add(newFtpServer);
        await _db.SaveChangesAsync(ct);
    }

    public async Task HandleFtpServerUpdatedAsync(
        FxferEventEnvelope envelope,
        FtpServerProjectionPayload payload,
        CancellationToken ct)
    {
        // F02-036: Update the existing row by id and set LastModifiedDate to UTC now.
        var existing = await _db.FtpServers
            .Where(x => x.FtpServerId == payload.FtpServerId)
            .FirstOrDefaultAsync(ct);

        if (existing is null)
        {
            throw new NullReferenceException();
        }

        var now = DateTimeOffset.UtcNow;

        var updated = existing with
        {
            FtpDescription = payload.FtpDescription,
            FtpServerHost = payload.FtpServerHost,
            FtpServerName = payload.FtpServerName,
            FtpPasswordSecretRef = payload.FtpPasswordSecretRef,
            FtpPort = payload.FtpPort,
            FtpProxyAddress = payload.FtpProxyAddress,
            FtpProxyPort = payload.FtpProxyPort,
            FtpTransferMode = payload.FtpTransferMode,
            FtpUser = payload.FtpUser,
            UseSshAuth = payload.UseSshAuth,
            LastModifiedTs = now,
            OperatorId = payload.OperatorId,
        };

        _db.Entry(existing).CurrentValues.SetValues(updated);
        await _db.SaveChangesAsync(ct);
    }

    public async Task HandleFtpServerInvalidatedAsync(
        FxferEventEnvelope envelope,
        FtpServerProjectionPayload payload,
        CancellationToken ct)
    {
        // F02-037: Set IsValid, OperatorId, and LastModifiedDate to UTC now.
        var existing = await _db.FtpServers
            .Where(x => x.FtpServerId == payload.FtpServerId)
            .FirstOrDefaultAsync(ct);

        if (existing is null)
        {
            throw new NullReferenceException();
        }

        var now = DateTimeOffset.UtcNow;

        var updated = existing with
        {
            IsValid = payload.IsValid,
            OperatorId = payload.OperatorId,
            LastModifiedTs = now,
        };

        _db.Entry(existing).CurrentValues.SetValues(updated);
        await _db.SaveChangesAsync(ct);
    }
}


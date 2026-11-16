using FxferService.Projections.Infrastructure.Persistence;
using FxferService.Projections.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FxferService.Projections.Infrastructure.Messaging.Idempotency;

public sealed class InboxStore
{
    private readonly FxferProjectionsDbContext _db;

    public InboxStore(FxferProjectionsDbContext db)
    {
        _db = db;
    }

    public async Task<bool> TryMarkProcessedAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        var exists = await _db.InboxEntries.AnyAsync(x => x.EventId == eventId, cancellationToken);
        if (exists)
        {
            return false;
        }

        _db.InboxEntries.Add(new InboxEntry
        {
            EventId = eventId,
            ProcessedAt = DateTimeOffset.UtcNow,
        });

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}

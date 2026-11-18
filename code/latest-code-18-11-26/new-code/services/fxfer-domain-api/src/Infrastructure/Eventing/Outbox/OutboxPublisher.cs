using Microsoft.EntityFrameworkCore;
using Fxfer.DomainApi.Infrastructure.DataAccess;

namespace Fxfer.DomainApi.Infrastructure.Eventing.Outbox;

// T-006: Publish outbox messages to the Event Adapter without losing events.
public sealed class OutboxPublisher
{
    private readonly FxferDbContext _dbContext;
    private readonly EventAdapterClient _eventAdapterClient;

    public OutboxPublisher(FxferDbContext dbContext, EventAdapterClient eventAdapterClient)
    {
        _dbContext = dbContext;
        _eventAdapterClient = eventAdapterClient;
    }

    public async Task<int> PublishPendingAsync(int batchSize, CancellationToken cancellationToken)
    {
        var pending = await _dbContext.OutboxMessages
            .Where(m => m.PublishedAt == null)
            .OrderBy(m => m.OccurredAt)
            .Take(batchSize)
            .ToListAsync(cancellationToken);

        var publishedCount = 0;

        foreach (var message in pending)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _eventAdapterClient.PublishAsync(message, cancellationToken);
            message.PublishedAt = DateTimeOffset.UtcNow;
            publishedCount++;
        }

        if (publishedCount > 0)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return publishedCount;
    }
}


using Fxfer.Bff.Application.LegacyContracts;

namespace Fxfer.Bff.Infrastructure.Events;

public sealed class NoOpEventDistribution : IEventDistribution
{
    public Task DistributeAsync(object payload, CancellationToken cancellationToken = default)
    {
        _ = payload;
        _ = cancellationToken;
        return Task.CompletedTask;
    }
}


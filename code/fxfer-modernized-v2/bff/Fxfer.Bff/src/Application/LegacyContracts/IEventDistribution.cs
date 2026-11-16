namespace Fxfer.Bff.Application.LegacyContracts;

public interface IEventDistribution
{
    // T-002: Event distribution is a downstream side effect and must only
    // occur after the operator JWT and request schema have been validated.
    Task DistributeAsync(object payload, CancellationToken cancellationToken = default);
}


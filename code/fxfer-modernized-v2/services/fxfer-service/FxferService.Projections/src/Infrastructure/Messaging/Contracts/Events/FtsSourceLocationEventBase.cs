namespace FxferService.Projections.Infrastructure.Messaging.Contracts.Events;

public abstract class FtsSourceLocationEventBase
{
    public string? UserID { get; init; }
    public required string FtsSourceLocationsId { get; init; }
}


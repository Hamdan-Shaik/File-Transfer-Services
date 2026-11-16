namespace FxferService.Application.LegacyContracts.ExternalEvents.EventsDistributor.Contracts.Events;

public sealed record FTEMetadataInvalidated
{
    public string? UserID { get; init; }

    public Guid FTEMetadataId { get; init; }
}


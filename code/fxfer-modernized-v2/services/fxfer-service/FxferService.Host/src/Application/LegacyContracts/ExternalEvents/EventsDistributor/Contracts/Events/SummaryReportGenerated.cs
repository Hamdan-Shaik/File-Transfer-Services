namespace FxferService.Application.LegacyContracts.ExternalEvents.EventsDistributor.Contracts.Events;

public sealed record SummaryReportGenerated
{
    public string? UserID { get; init; }

    public Guid FTEMetadataId { get; init; }

    public string? FileName { get; init; }

    public Guid? GroupId { get; init; }

    public int NumberOfRecords { get; init; }
}


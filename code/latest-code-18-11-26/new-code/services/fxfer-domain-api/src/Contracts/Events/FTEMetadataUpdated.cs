namespace Fxfer.DomainApi.Contracts.Events;

public sealed record FTEMetadataUpdated
{
    public string? UserID { get; init; }
    public string? ModuleName { get; init; }
    public string? ImportJobName { get; init; }
    public Guid FTEMetadataId { get; init; }
}

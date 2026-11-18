namespace Fxfer.DomainApi.Contracts.Commands;

public sealed record IntegrationFileProcessingRequest
{
    public int? ConfigId { get; init; }
    public string? FileName { get; init; }
    public required bool IsValid { get; init; }
}

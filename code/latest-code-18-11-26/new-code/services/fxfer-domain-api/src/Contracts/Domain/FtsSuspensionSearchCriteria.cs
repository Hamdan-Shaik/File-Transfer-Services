namespace Fxfer.DomainApi.Contracts.Domain;

public sealed class FtsSuspensionSearchCriteria
{
    public Guid SourceLocationId { get; set; }

    public string? ErrorText { get; set; }

    public string? StackTrace { get; set; }

    public string? OperatorId { get; set; }
}
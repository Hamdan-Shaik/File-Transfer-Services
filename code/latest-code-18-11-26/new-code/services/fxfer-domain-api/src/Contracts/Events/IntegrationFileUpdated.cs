namespace Fxfer.DomainApi.Contracts.Events;

public sealed record IntegrationFileUpdated : IntegrationFileAdded
{
    public DateTime LastModifiedDate { get; init; }
}


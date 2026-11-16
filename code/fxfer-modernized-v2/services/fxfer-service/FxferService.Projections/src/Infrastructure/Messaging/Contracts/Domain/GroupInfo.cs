namespace FxferService.Projections.Infrastructure.Messaging.Contracts.Domain;

public sealed class GroupInfo
{
    public Guid? GroupId { get; init; }
    public int NumberOfRecords { get; init; }
}


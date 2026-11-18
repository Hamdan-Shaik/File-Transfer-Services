using Fxfer.DomainApi.Contracts.Domain;

namespace Fxfer.DomainApi.Contracts.Events;

public sealed record ExtractFileGenerated
{
    public string? UserID { get; init; }
    public Guid MetadataId { get; init; }
    public string? FileDirectory { get; init; }
    public List<GroupInfo>? GroupInfo { get; init; }
}

namespace Fxfer.Bff.Contracts.Common;

public sealed record InboundTargetMetadata
{
    public Guid FteMetadataId { get; init; }
    public required string ImportJobName { get; init; }
}


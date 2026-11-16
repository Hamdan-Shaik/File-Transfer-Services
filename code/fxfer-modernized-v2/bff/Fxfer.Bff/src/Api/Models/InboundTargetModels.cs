namespace Fxfer.Bff.Api.Models;

public sealed record InboundTarget(
    Guid InboundTargetId,
    Guid ExternalFileDefinitionId,
    string ImportJobName,
    string ContractName,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset LastModifiedAt);

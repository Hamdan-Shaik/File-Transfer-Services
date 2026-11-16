namespace Fxfer.Bff.Api.Models;

public sealed record IntegrationFile(
    Guid IntegrationFileId,
    Guid? GroupId,
    int? ConfigNumber,
    string FileName,
    Guid OutboundMetadataDefinitionId,
    bool IsProcessed,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset LastModifiedAt);

public sealed record IntegrationFileUpdate(
    [param: System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)] int ConfigNumber,
    bool IsActive);

public sealed record IntegrationFileIdResponse(Guid IntegrationFileId);

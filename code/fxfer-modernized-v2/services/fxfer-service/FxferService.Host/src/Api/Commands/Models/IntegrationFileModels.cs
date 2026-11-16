namespace FxferService.Api.Commands.Models;

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

public sealed record IntegrationFileUpdate(int ConfigNumber, bool IsActive);

public sealed record IntegrationFileIdResponse(Guid IntegrationFileId);


namespace Fxfer.Bff.Api.Models;

public sealed record FileTransferSearch(
    [param: System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)] int Page,
    [param: System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)] int PageSize,
    string? OrderBy,
    string? OrderDir,
    [param: System.ComponentModel.DataAnnotations.Required] FileTransferSearchFilters Filters);

public sealed record FileTransferSearchFilters(
    Guid? FileId,
    string? FileNameContains,
    string? FileStatus,
    string? FileDirection,
    string? TransferMethod,
    DateTimeOffset? CreatedFrom,
    DateTimeOffset? CreatedTo,
    double? FileSizeMin,
    double? FileSizeMax,
    string? TradingPartner,
    Guid? SourceLocationId);

public sealed record TrackedFileEvent(
    Guid EventId,
    string EventCode,
    DateTimeOffset CreatedAt,
    string Description);

public sealed record TrackedFileException(
    Guid ExceptionId,
    string ExceptionType,
    string Message,
    string StackTrace,
    string MethodName,
    DateTimeOffset CreatedAt);

public sealed record TrackedFile(
    Guid FileId,
    string FileName,
    string FileStatus,
    string FileDirection,
    string FileTransportType,
    DateTimeOffset CreatedAt,
    double FileSize,
    string? TradingPartner,
    Guid? SourceLocationId,
    string? SourceLocationConfigName,
    string? SourceLocationAddress,
    string? OriginatingServerName,
    TrackedFileEvent[] Events,
    TrackedFileException[] Exceptions);

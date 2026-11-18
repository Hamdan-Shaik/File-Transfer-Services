namespace Fxfer.ProjectionWorker.Infrastructure.DataAccess.Entities;

public sealed record FtsFileManagement
{
    public required Guid FileId { get; init; }
    public required Guid FileSourceLocationId { get; init; }
    public required string FileName { get; init; }
    public required string FileStatus { get; init; }
    public required DateTimeOffset FileStatusTs { get; init; }
    public int? SakFileTrack { get; init; }
    public required decimal FileSize { get; init; }
    public string? ChecksumCd { get; init; }
    public required string OperatorId { get; init; }
    public required DateTimeOffset LastModifiedTs { get; init; }
    public required DateTimeOffset CreatedTs { get; init; }
}


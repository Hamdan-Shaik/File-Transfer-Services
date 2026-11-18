namespace Fxfer.Bff.Contracts.Tracking;

public sealed record FileTransferTracking
{
    public Guid FileId { get; init; }
    public Guid FileSourceLocationId { get; init; }
    public required string FileName { get; init; }
    public required string FileStatus { get; init; }
    public required string FileDirection { get; init; }
    public required string FileTransportType { get; init; }
    public DateTimeOffset CreatedDate { get; init; }
    public string? TradingPartner { get; init; }
    public string? CheckSumCode { get; init; }
    public decimal FileSize { get; init; }
    public required string SourceLocAddress { get; init; }
    public required string SourceLocConfigName { get; init; }
    public required string OriginatingServerName { get; init; }
    public IReadOnlyList<FileEvent> Events { get; init; } = Array.Empty<FileEvent>();
    public IReadOnlyList<FileException> Exceptions { get; init; } = Array.Empty<FileException>();
}


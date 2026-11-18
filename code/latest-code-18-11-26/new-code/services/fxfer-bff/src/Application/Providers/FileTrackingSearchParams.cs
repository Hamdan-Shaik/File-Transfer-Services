namespace Fxfer.Bff.Application.Providers;

public sealed record FileTrackingSearchParams
{
    public string? FileId { get; init; }
    public string? FileName { get; init; }
    public string? FileStatus { get; init; }
    public string? FileDirection { get; init; }
    public string? FileTransportType { get; init; }
    public string? FileSourceLocationId { get; init; }
    public string? StartDate { get; init; }
    public string? EndDate { get; init; }
    public string? FileSizeFrom { get; init; }
    public string? FileSizeThrough { get; init; }
    public string? TradingPartner { get; init; }
    public int? Page { get; init; }
    public int? PageSize { get; init; }
    public IReadOnlyList<SortCriteriaParam>? SortCriteria { get; init; }
}


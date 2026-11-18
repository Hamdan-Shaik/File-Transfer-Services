namespace Fxfer.Bff.Application.Providers;

public sealed record FtpServerSearchParams
{
    public bool? IsValid { get; init; }
    public Guid? FtpServerId { get; init; }
    public string? FtpServerName { get; init; }
    public string? FtpDescription { get; init; }
    public string? NameMatch { get; init; }
    public string? DescriptionMatch { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;
    public IReadOnlyList<SortCriteriaParam>? SortCriteria { get; init; }
}


namespace Fxfer.Bff.Application.Providers;

public sealed record TransferConfigurationSearchParams
{
    public int? ConfigId { get; init; }
    public string? ConfigName { get; init; }
    public string? OwnerName { get; init; }
    public Guid? OwnerSubsystem { get; init; }
    public string? Protocol { get; init; }
    public bool? IsActive { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;
    public IReadOnlyList<SortCriteriaParam>? SortCriteria { get; init; }
}


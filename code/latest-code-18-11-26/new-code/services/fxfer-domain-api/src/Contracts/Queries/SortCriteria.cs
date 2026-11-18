namespace Fxfer.DomainApi.Contracts.Queries;

public sealed class SortCriteria
{
    public string? FieldName { get; set; }

    public SortOrder Order { get; set; }
}


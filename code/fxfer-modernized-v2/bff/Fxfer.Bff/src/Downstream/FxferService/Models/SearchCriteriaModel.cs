namespace Fxfer.Bff.Downstream.FxferService.Models;

public sealed class SearchCriteriaModel
{
    public List<SearchFieldModel>? SearchFields { get; set; }
    public List<SortCriteriaModel>? SortCriteria { get; set; }
    public PagingCriteriaModel? PagingCriteria { get; set; }
}


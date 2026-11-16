namespace Fxfer.Bff.Downstream.FxferService.Models;

public sealed class SortCriteriaModel
{
    public string SortField { get; set; } = string.Empty;
    public CoreEnumerations.Messaging.SortDirectionType SortDirection { get; set; }
}


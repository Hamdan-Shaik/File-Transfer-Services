namespace Fxfer.Bff.Downstream.FxferService.Models;

public sealed class SearchFieldModel
{
    public string FieldName { get; set; } = string.Empty;
    public object FieldValue { get; set; } = string.Empty;
    public CoreEnumerations.Messaging.SearchModeType SearchMode { get; set; }
}


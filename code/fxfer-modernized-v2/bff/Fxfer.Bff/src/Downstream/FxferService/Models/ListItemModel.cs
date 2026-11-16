namespace Fxfer.Bff.Downstream.FxferService.Models;

public sealed class ListItemModel
{
    public CoreEnumerations.Views.DataListDisplayFormatType DisplayFormat { get; set; }
    public string? Name { get; set; }
    public string? ID { get; set; }
}


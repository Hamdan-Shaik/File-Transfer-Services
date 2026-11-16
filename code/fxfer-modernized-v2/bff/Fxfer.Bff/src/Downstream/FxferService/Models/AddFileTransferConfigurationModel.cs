namespace Fxfer.Bff.Downstream.FxferService.Models;

public sealed class AddFileTransferConfigurationModel
{
    public ListItemModel OwnerSubsystem { get; set; } = new();
    public ListItemModel ProtocolType { get; set; } = new();
    public ListItemModel RunOnThisDay { get; set; } = new();
    public ListItemModel TimeWindowStart { get; set; } = new();
    public ListItemModel TimeWindowEnd { get; set; } = new();

    public string? SourceAddress { get; set; }
    public string? SourceFileMask { get; set; }

    public bool IsActive { get; set; }
    public string? Status { get; set; }
    public MetadataModel? Metadata { get; set; }
}
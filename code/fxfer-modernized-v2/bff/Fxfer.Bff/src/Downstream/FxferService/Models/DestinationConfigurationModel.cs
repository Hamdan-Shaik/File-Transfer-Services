using System;

namespace Fxfer.Bff.Downstream.FxferService.Models;

public sealed class DestinationConfigurationModel
{
    public string? DestinationId { get; set; }
    public Guid? FTEMetadataId { get; set; }

    public ListItemModel ContractName { get; set; } = new();
    public ListItemModel DestinationType { get; set; } = new();
    public ListItemModel DestinationProtocol { get; set; } = new();
    public ListItemModel FTPServer { get; set; } = new();
    public ListItemModel RenamePattern { get; set; } = new();

    public string? DestinationAddress { get; set; }

    public string? ContactName { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }

    public bool OverwriteAtDestination { get; set; }
    public bool CompressAtDestination { get; set; }
    public bool RenameAtDestination { get; set; }
    public bool DestinationIsArchive { get; set; }

    public string? ImportJobName { get; set; }
    public bool Active { get; set; }
    public string? Status { get; set; }
    public bool IsAddedUpdated { get; set; }
    public int DestinationLineNumber { get; set; }
    public MetadataModel? Metadata { get; set; }
}

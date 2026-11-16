namespace Fxfer.Bff.Downstream.FxferService.Models;

public sealed class FileTransferConfigurationModel
{
    public FileTransferConfigurationModel()
    {
        DestinationDetails = new List<DestinationConfigurationModel>();
    }

    public int ConfigId { get; set; }
    public string? ConfigName { get; set; }
    public string? Description { get; set; }

    public string? FTPServerId { get; set; }
    public string? FTPServerName { get; set; }
    public ListItemModel FTPServer { get; set; } = new();

    public ListItemModel FileDirection { get; set; } = new();
    public string? SourceFileMask { get; set; }
    public string? SourceAddress { get; set; }
    public ListItemModel ProtocolType { get; set; } = new();
    public ListItemModel OwnerSubsystem { get; set; } = new();
    public string? OwnerSubsystemId { get; set; }

    public string? OwnerName { get; set; }
    public string? OwnerEmail { get; set; }
    public string? OwnerPhoneNumber { get; set; }

    public bool RunOnMonday { get; set; }
    public bool RunOnTuesday { get; set; }
    public bool RunOnWednesday { get; set; }
    public bool RunOnThursday { get; set; }
    public bool RunOnFriday { get; set; }
    public bool RunOnSaturday { get; set; }
    public bool RunOnSunday { get; set; }

    public ListItemModel RunOnThisDay { get; set; } = new();
    public ListItemModel TimeWindowStart { get; set; } = new();
    public ListItemModel TimeWindowEnd { get; set; } = new();
    public ListItemModel RunSchedule { get; set; } = new();

    public bool RunOnLastDay { get; set; }
    public bool RunOnFirstDay { get; set; }

    public bool IsActive { get; set; }
    public bool Suspended { get; set; }
    public bool SendNotification { get; set; }
    public bool Suspend { get; set; }

    public string? TradingPartner { get; set; }
    public string? FtsSourceLocationId { get; set; }
    public string? Status { get; set; }
    public MetadataModel? Metadata { get; set; }

    public List<DestinationConfigurationModel> DestinationDetails { get; }
}

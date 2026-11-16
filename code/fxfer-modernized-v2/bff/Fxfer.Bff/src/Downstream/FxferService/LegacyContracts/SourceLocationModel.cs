namespace FileTransferSvcQueryService;

public sealed class SourceLocationModel
{
    public Guid FtsSourceLocationId { get; set; }

    public int ConfigId { get; set; }
    public string? ConfigName { get; set; }
    public string? Description { get; set; }

    public Guid? FtpServerId { get; set; }
    public string? FtpServerName { get; set; }

    public string? Address { get; set; }

    public string? OwnerName { get; set; }
    public string? OwnerEmail { get; set; }
    public Guid OwnerSubsystem { get; set; }
    public string? OwnerPhoneNumber { get; set; }

    public string? Protocol { get; set; }
    public string? FileDirection { get; set; }
    public string? Mask { get; set; }

    public string? TradingPartner { get; set; }
    public string? OperatorId { get; set; }

    public bool RunOnMonday { get; set; }
    public bool RunOnTuesday { get; set; }
    public bool RunOnWednesday { get; set; }
    public bool RunOnThursday { get; set; }
    public bool RunOnFriday { get; set; }
    public bool RunOnSaturday { get; set; }
    public bool RunOnSunday { get; set; }

    public bool RunOnFirstDay { get; set; }
    public bool RunOnLastDay { get; set; }
    public byte RunOnThisDay { get; set; }

    public string? StartWindow { get; set; }
    public string? EndWindow { get; set; }

    public bool SendNotification { get; set; }
    public bool IsActive { get; set; }

    public DateTime ValidChangeDate { get; set; }
    public bool Suspended { get; set; }
    public DateTime SuspendedChangeDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public DateTime CreationDate { get; set; }

    public List<FtsDestinationLocationAdded>? DestinationAddress { get; set; }
}
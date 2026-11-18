namespace Fxfer.DomainApi.Contracts.Domain;

public sealed class FileEventModel
{
    public Guid EventId { get; set; }

    public Guid FileId { get; set; }

    public Guid DestinationId { get; set; }

    public string? EventCode { get; set; }

    public string? EventTrackDescription { get; set; }

    public DateTime CreatedDate { get; set; }
}


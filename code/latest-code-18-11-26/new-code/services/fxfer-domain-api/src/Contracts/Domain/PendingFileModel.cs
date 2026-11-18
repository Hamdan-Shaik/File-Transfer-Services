namespace Fxfer.DomainApi.Contracts.Domain;

public sealed class PendingFileModel
{
    public Guid FileId { get; set; }

    public long ConfigId { get; set; }

    public Guid FileSourceLocationId { get; set; }

    public string? FileName { get; set; }

    public string? FileStatus { get; set; }

    public long? FileTrackId { get; set; }

    public DateTime StatusDate { get; set; }

    public decimal FileSize { get; set; }
}
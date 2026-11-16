namespace FxferService.Infrastructure.Persistence.Batch.Contracts;

public sealed class PendingFileModel
{
    public Guid FileId { get; set; }

    public int ConfigId { get; set; }

    public Guid FileSourceLocationId { get; set; }

    public string FileName { get; set; } = "";

    public string FileStatus { get; set; } = "";

    public int? FileTrackId { get; set; }

    public DateTime StatusDate { get; set; }

    public decimal FileSize { get; set; }
}
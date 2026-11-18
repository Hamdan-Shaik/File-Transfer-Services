namespace Fxfer.TransferWorker.Contracts.Domain;

public sealed class FileToDeliverModel
{
    public SourceLocationModel SourceLocation { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public Guid FileId { get; set; }

    public int? SakFileTrack { get; set; }

    public long FileSize { get; set; }

    public string FileType { get; set; } = null!;

    public Guid GroupId { get; set; }
}
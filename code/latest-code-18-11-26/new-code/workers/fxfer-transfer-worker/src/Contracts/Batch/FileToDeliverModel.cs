namespace Fxfer.TransferWorker.Contracts.Batch;

public sealed class FileToDeliverModel
{
    public SourceLocationModel SourceLocation { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public Guid FileId { get; set; }

    public long? SakFileTrack { get; set; }

    public long FileSize { get; set; }

    public string FileType { get; set; } = string.Empty;

    public Guid GroupId { get; set; }
}

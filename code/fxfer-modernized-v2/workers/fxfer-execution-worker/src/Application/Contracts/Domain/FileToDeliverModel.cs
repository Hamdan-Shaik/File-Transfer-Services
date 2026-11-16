using System;

namespace Fxfer.ExecutionWorker.Application.Contracts.Domain;

public class FileToDeliverModel
{
    public SourceLocationModel SourceLocation { get; set; } = new();

    public string FileName { get; set; } = null!;

    public Guid FileId { get; set; }

    public long? SakFileTrack { get; set; }

    public long FileSize { get; set; }

    public string FileType { get; set; } = null!;

    public Guid GroupId { get; set; }
}
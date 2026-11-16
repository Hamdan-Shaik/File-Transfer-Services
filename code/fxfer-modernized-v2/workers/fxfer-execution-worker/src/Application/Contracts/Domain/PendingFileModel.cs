using System;

namespace Fxfer.ExecutionWorker.Application.Contracts.Domain;

public class PendingFileModel
{
    public Guid FileId { get; set; }

    public int ConfigId { get; set; }

    public Guid FileSourceLocationId { get; set; }

    public string FileName { get; set; } = null!;

    public string FileStatus { get; set; } = null!;

    public int? FileTrackId { get; set; }

    public DateTime StatusDate { get; set; }
}


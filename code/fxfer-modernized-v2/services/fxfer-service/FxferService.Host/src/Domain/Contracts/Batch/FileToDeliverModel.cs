namespace FxferService.Domain.Contracts.Batch;

public class FileToDeliverModel
{
    /// <summary>
    /// The location of the file
    /// </summary>
    public SourceLocationModel? SourceLocation { get; set; }

    /// <summary>
    /// The name of the file at that location
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// The file identifier
    /// </summary>
    public Guid FileId { get; set; }

    /// <summary>
    /// The fts tracking identifier
    /// </summary>
    public long? SakFileTrack { get; set; }
}


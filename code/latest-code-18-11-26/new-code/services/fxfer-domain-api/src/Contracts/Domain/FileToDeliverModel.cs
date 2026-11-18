namespace Fxfer.DomainApi.Contracts.Domain;

public sealed class FileToDeliverModel
{
    public SourceLocationModel? SourceLocation { get; set; }

    public string? FileName { get; set; }

    public Guid FileId { get; set; }

    public int? SakFileTrack { get; set; }

    public long FileSize { get; set; }

    public string? FileType { get; set; }

    public Guid GroupId { get; set; }
}
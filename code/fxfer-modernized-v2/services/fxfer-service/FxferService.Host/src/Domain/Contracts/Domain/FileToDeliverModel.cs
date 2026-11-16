using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Domain;

/// <summary>
/// A file that will be delivered
/// </summary>
[DataContract]
public class FileToDeliverModel
{
    /// <summary>
    /// The location of the file
    /// </summary>
    [DataMember]
    public SourceLocationModel? SourceLocation { get; set; }

    /// <summary>
    /// The name of the file at that location
    /// </summary>
    [DataMember]
    public string? FileName { get; set; }

    /// <summary>
    /// The file identifier
    /// </summary>
    [DataMember]
    public Guid FileId { get; set; }

    /// <summary>
    /// The fts tracking identifier
    /// </summary>
    [DataMember]
    public int? SakFileTrack { get; set; }

    /// <summary>
    /// File Size
    /// </summary>
    [DataMember]
    public long FileSize { get; set; }

    /// <summary>
    /// File Type
    /// </summary>
    [DataMember]
    public string? FileType { get; set; }

    /// <summary>
    /// File Type
    /// </summary>
    [DataMember]
    public Guid GroupId { get; set; }
}


using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Domain;

/// <summary>
/// Information about the file that is pending/waiting to send.
/// </summary>
[DataContract]
public class PendingFileModel
{
    /// <summary>
    /// <para>Indicates the File Id.</para>
    /// </summary>
    [DataMember]
    public Guid FileId { get; set; }

    /// <summary>
    /// <para>Indicates the FTS Configuration Id.</para>
    /// </summary>
    [DataMember]
    public long ConfigId { get; set; }

    /// <summary>
    /// <para>Indicates the File Source Location Id.</para>
    /// </summary>
    [DataMember]
    public Guid FileSourceLocationId { get; set; }

    /// <summary>
    /// <para>Indicates the File Name.</para>
    /// </summary>
    [DataMember]
    public string? FileName { get; set; }

    /// <summary>
    /// <para>Indicates the File Status.</para>
    /// </summary>
    [DataMember]
    public string? FileStatus { get; set; }

    /// <summary>
    /// <para>Indicates the File tracking Id.</para>
    /// </summary>
    [DataMember]
    public long? FileTrackId { get; set; }

    /// <summary>
    /// <para>Indicates the FTS Status Code.</para>
    /// </summary>
    [DataMember]
    public DateTime StatusDate { get; set; }

    /// <summary>
    /// <para>Indicates the File Size.</para>
    /// </summary>
    [DataMember]
    public decimal FileSize { get; set; }
}


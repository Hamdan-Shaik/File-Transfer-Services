using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Queries.Parameters;

/// <summary>
/// <para>Query parameters for File Tracking</para>
/// </summary>
[DataContract]
public class FileTransferTrackingQueryParms : SerializableBase
{
    /// <summary>
    /// <para>Remarks: Unique Number of the particular File</para>
    /// </summary>
    [DataMember]
    public string FileId { get; set; } = null!;

    /// <summary>
    /// <para>File Name</para>
    /// <para> Remarks:The name of the file</para>
    /// </summary>
    [DataMember]
    public string FileName { get; set; } = null!;

    /// <summary>
    /// <para>File Status</para>
    /// <para> Remarks:The status of the file</para>
    /// </summary>
    [DataMember]
    public string FileStatus { get; set; } = null!;

    /// <summary>
    /// <para>File Direction</para>
    /// <para> Remarks:The direction of the File(s) e.g. viz. InBound/OutBound in which they are transferred</para>
    /// </summary>
    [DataMember]
    public string FileDirection { get; set; } = null!;

    /// <summary>
    /// <para>File Transport Type</para>
    /// <para> Remarks:The protocol used to transfer the file(s)</para>
    /// </summary>
    [DataMember]
    public string FileTransportType { get; set; } = null!;

    /// <summary>
    /// <para>File Source Location Id</para>
    /// <para> Remarks:The unique source location Id of the file(s)</para>
    /// </summary>
    [DataMember]
    public string FileSourceLocationId { get; set; } = null!;

    /// <summary>
    /// <para>Start Date</para>
    /// <para>Remarks: Timestamp of when the file transfer started</para>
    /// </summary>
    [DataMember]
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// <para>End Date</para>
    /// <para>Remarks: Timestamp of when the file transfer ended</para>
    /// </summary>
    [DataMember]
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// <para>File Size From</para>
    /// <para> Remarks:The size of the file(s) ranging from</para>
    /// </summary>
    [DataMember]
    public decimal? FileSizeFrom { get; set; }

    /// <summary>
    /// <para>File Size Through</para>
    /// <para> Remarks:The size of the file(s) ranging through</para>
    /// </summary>
    [DataMember]
    public decimal? FileSizeThrough { get; set; }

    /// <summary>
    /// <para>Trading Partner</para>
    /// <para> Remarks:The Trading Partner associated with the file(s)</para>
    /// </summary>
    [DataMember]
    public string TradingPartner { get; set; } = null!;
}


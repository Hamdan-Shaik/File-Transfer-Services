using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable


namespace FxferService.Domain.Contracts.Domain;

/// <summary>
/// <para>File Transfer Tracking</para>
/// <para>Contains results from File Transfer Tracking Query.</para>
/// </summary>
[Serializable]
[DataContract]
public class FileTransferTrackingModel
{
    public FileTransferTrackingModel()
        : base()
    {
    }

    /// <summary>
    /// <para>Remarks: Unique Number of the particular File</para>
    /// </summary>
    [DataMember]
    public Guid FileId { get; set; }

    /// <summary>
    /// <para>File Source Location Id</para>
    /// <para> Remarks:The unique source location Id of the file(s)</para>
    /// </summary>
    [DataMember]
    public Guid FileSourceLocationId { get; set; }

    /// <summary>
    /// <para>File Name</para>
    /// <para> Remarks:The name of the file</para>
    /// </summary>
    [DataMember]
    public string FileName { get; set; }

    /// <summary>
    /// <para>File Status</para>
    /// <para> Remarks:The status of the file</para>
    /// </summary>
    [DataMember]
    public string FileStatus { get; set; }

    /// <summary>
    /// <para>File Status</para>
    /// <para> Remarks:The direction of the File(s) e.g. viz. InBound/OutBound in which they are transferred</para>
    /// </summary>
    [DataMember]
    public string FileDirection { get; set; }

    /// <summary>
    /// <para>File Transport Type</para>
    /// <para> Remarks:The protocol used to transfer the file(s)</para>
    /// </summary>
    [DataMember]
    public string FileTransportType { get; set; }

    /// <summary>
    /// <para>Creation Date shows when the File was created.</para>
    /// </summary>
    [DataMember]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// <para>Trading Partner</para>
    /// <para> Remarks:The Trading Partner associated with the file(s)</para>
    /// </summary>
    [DataMember]
    public string TradingPartner { get; set; }

    [DataMember]
    public string CheckSumCode { get; set; }

    /// <summary>
    /// <para>File Size</para>
    /// <para> Remarks:The size of the file(s)</para>
    /// </summary>
    [DataMember]
    public decimal FileSize { get; set; }

    [DataMember]
    public string SourceLocAddress { get; set; }

    [DataMember]
    public string SourceLocConfigName { get; set; }

    [DataMember]
    public string OriginatingServerName { get; set; }

    [DataMember]
    public List<FileEventModel> Events { get; set; }

    [DataMember]
    public List<FileExceptionModel> Exceptions { get; set; }
}

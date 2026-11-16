using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Domain;

/// <summary>
/// Describes File Management Request
/// </summary>
[DataContract]
public class FileManagementRequestModel
{
    /// <summary>
    /// <para>Indicates the File Id.</para>
    /// </summary>
    [DataMember]
    public Guid FileID { get; set; }

    /// <summary>
    /// <para>Indicates the Source Location Id.</para>
    /// </summary>
    [DataMember]
    public Guid SourceLocationId { get; set; }

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
    /// <para>Indicates the File Size.</para>
    /// </summary>
    [DataMember]
    public decimal FileSize { get; set; }

    /// <summary>
    /// <para>Indicates the Checksum Code.</para>
    /// </summary>
    [DataMember]
    public string? ChecksumCode { get; set; }

    /// <summary>
    /// <para>Indicates the Operator Id.</para>
    /// </summary>
    [DataMember]
    public string? OperatorId { get; set; }

    /// <summary>
    /// <para>Indicates the Transaction Id.</para>
    /// </summary>
    [DataMember]
    public string? TransactionId { get; set; }

    /// <summary>
    /// <para>Indicates the Group Id.</para>
    /// </summary>
    [DataMember]
    public Guid GroupId { get; set; }
}


using System;
using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Events;

/// <summary>
/// <para>File Transferred Event</para>
/// <para>The current state of the Fts Source Location event.</para>
/// </summary>
[DataContract]
public class FileTransferred : FileTransferEvents
{
    #region Properties
    /// <summary>
    /// <para>Group Idr</para>
    /// <para>Remarks: Is of type Global Unique Identifier(GUID). Format XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX</para>
    /// </summary>
    [DataMember]
    public Guid GroupId { get; set; }

    /// <summary>
    /// <para>Metadata Idr</para>
    /// <para>Remarks: Is of type Global Unique Identifier(GUID). Format XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX</para>
    /// </summary>
    [DataMember]
    public Guid FTEMetadataId { get; set; }

    /// <summary>
    /// <para>File Id</para>
    /// <para>Remarks: Is of type Global Unique Identifier(GUID). Format XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX</para>
    /// </summary>
    [DataMember]
    public Guid FileId { get; set; }

    /// <summary>
    /// <para>Source Location Id</para>
    /// <para>Remarks: Is of type Global Unique Identifier(GUID). Format XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX</para>
    /// </summary>
    [DataMember]
    public Guid SourceLocationId { get; set; }

    /// <summary>
    /// <para>File Name</para>
    /// <para>Remarks: Name of the File that was Transferred</para>
    /// </summary>
    [DataMember]
    public string? FileName { get; set; }

    /// <summary>
    /// <para>Configuration Id</para>
    /// <para>Remarks: Is Sequential number used to identify the FTS Configuration</para>
    /// </summary>
    [DataMember]
    public long ConfigurationId { get; set; }

    /// <summary>
    /// <para>Configuration Name</para>
    /// <para>Remarks: Name of the File Transfer Configuration</para>
    /// </summary>
    [DataMember]
    public string? ConfigurationName { get; set; }

    /// <summary>
    /// <para>Source Protocol Code</para>
    /// <para>Remarks: Indicates the code of the protocol used for the file transfer</para>
    /// </summary>
    [DataMember]
    public string? SourceProtocolCode { get; set; }

    /// <summary>
    /// <para>Source Address Path</para>
    /// <para>Remarks: Indicates the Source path for the file that was Transferred</para>
    /// </summary>
    [DataMember]
    public string? SourceAddressPath { get; set; }

    /// <summary>
    /// <para>Source Address Path</para>
    /// <para>Remarks: Indicates the Destination path for the file that was Transferred</para>
    /// </summary>
    [DataMember]
    public string? DestinationAddressPath { get; set; }

    /// <summary>
    /// <para>Source Mask</para>
    /// <para>Remarks: Indicates the mask set up on the file transfer ocnfiguration, used to identify the files that was Transferred</para>
    /// </summary>
    [DataMember]
    public string? SourceMask { get; set; }

    /// <summary>
    /// <para>Owner Module</para>
    /// <para>Remarks: Indicates the ownner module for the file transfer configuration</para>
    /// </summary>
    [DataMember]
    public string? OwnerModule { get; set; }

    /// <summary>
    /// <para>Module Contract</para>
    /// <para>Remarks: Indicates the module contract used for the metadata configuration</para>
    /// </summary>
    [DataMember]
    public string? ModuleContract { get; set; }

    /// <summary>
    /// <para>Configuration Description</para>
    /// <para>Remarks: Description for the file transfer configuration</para>
    /// </summary>
    [DataMember]
    public string? ConfigurationDescription { get; set; }

    /// <summary>
    /// <para>Transfer Date</para>
    /// <para>Remarks: Date when the file was Transferred</para>
    /// </summary>
    [DataMember]
    public DateTime TransferDate { get; set; }

    /// <summary>
    /// <para>File Direction</para>
    /// <para>Remarks: Indicates the direction from where the file was Transferred</para>
    /// </summary>
    [DataMember]
    public string? FileDirection { get; set; }

    /// <summary>
    /// <para>File Size</para>
    /// <para>Remarks: Indicates the size in bytes of the file that was trtansfered</para>
    /// </summary>
    [DataMember]
    public long FileSize { get; set; }

    /// <summary>
    /// <para>File Type</para>
    /// <para>Remarks: Indicates the type of the file that was Transferred</para>
    /// </summary>
    [DataMember]
    public string? FileType { get; set; }
    #endregion
}

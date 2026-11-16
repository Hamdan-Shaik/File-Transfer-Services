using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Domain;

/// <summary>
/// <para>A destination for a FTS file.</para>
/// </summary>
[DataContract]
public class DestinationModel
{
    /// <summary>
    /// <para>Unique identifier for the destination.</para>
    /// </summary>
    [DataMember]
    public Guid DestinationId { get; set; }

    /// <summary>
    /// <para>The address of the destination.</para>
    /// </summary>
    [DataMember]
    public string? DestinationAddress { get; set; }

    /// <summary>
    /// <para>The protocol for this destination.</para>
    /// </summary>
    [DataMember]
    public string? DestinationProtocolCode { get; set; }

    /// <summary>
    /// <para>Contact Name registered for the Destination.</para>
    /// </summary>
    [DataMember]
    public string? ContactName { get; set; }

    /// <summary>
    /// <para>Indicates whether the file is overwrite any pre-existing files at the destination.</para>
    /// </summary>
    [DataMember]
    public bool IsOverwriteAtDestination { get; set; }

    /// <summary>
    /// <para>Indicates whether the file is compressed at the destination.</para>
    /// </summary>
    [DataMember]
    public bool IsCompressAtDestination { get; set; }

    /// <summary>
    /// <para>Indicates whether the file is renamed at the destination.</para>
    /// </summary>
    [DataMember]
    public bool IsRenameAtDestination { get; set; }

    /// <summary>
    /// <para>A pattern used to rename files being xmitted to the destination.</para>
    /// </summary>
    [DataMember]
    public string? RenamePattern { get; set; }

    /// <summary>
    /// <para>Indicates whether the destination is an archived location.</para>
    /// </summary>
    [DataMember]
    public bool IsArchive { get; set; }

    /// <summary>
    /// <para>Contains encrypted code.</para>
    /// </summary>
    [DataMember]
    public string? EncryptionCode { get; set; }

    /// <summary>
    /// <para>Public Key that helps to encrypt the file.</para>
    /// </summary>
    [DataMember]
    public string? EncryptionPublicKey { get; set; }

    /// <summary>
    /// <para>Private Key that helps to decrypt the file.</para>
    /// </summary>
    [DataMember]
    public string? EncryptionPrivateKey { get; set; }

    /// <summary>
    /// <para>Email Id registered for destination notification.</para>
    /// </summary>
    [DataMember]
    public string? EmailId { get; set; }

    /// <summary>
    /// <para>Password associate to a private key.</para>
    /// </summary>
    [DataMember]
    public string? PrivateKeyPassword { get; set; }

    /// <summary>
    /// <para>Represent the FTP server for the destination file.</para>
    /// </summary>
    [DataMember]
    public FtpServerModel? FtpServerDetail { get; set; }

    /// <summary>
    /// <para>The File Name in which the File should be stored in destination folder</para>
    /// </summary>
    [DataMember]
    public string? DestinationFileName { get; set; }

    /// <summary>
    /// <para>Unique Identifier pointing to the related File To Event Metadata if applicable</para>
    /// </summary>
    [DataMember]
    public Guid? FTEMetadataId { get; set; }

    /// <summary>
    /// <para>The Contract Name of the destination folder</para>
    /// </summary>
    [DataMember]
    public string? ContractName { get; set; }

    /// <summary>
    /// <para>Mark as moved when file is in SFPT server</para>
    /// </summary>
    [DataMember]
    public bool IsFullyUploaded { get; set; }
}


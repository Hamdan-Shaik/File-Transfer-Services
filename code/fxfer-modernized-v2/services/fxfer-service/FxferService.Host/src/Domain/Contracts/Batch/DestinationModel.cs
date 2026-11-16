namespace FxferService.Domain.Contracts.Batch;

public class DestinationModel
{
    /// <summary>
    /// <para>Unique identifier for the destination.</para>
    /// </summary>
    public Guid DestinationId { get; set; }

    /// <summary>
    /// <para>The address of the destination.</para>
    /// </summary>
    public string? DestinationAddress { get; set; }

    /// <summary>
    /// <para>The protocol for this destination.</para>
    /// </summary>
    public string? DestinationProtocolCode { get; set; }

    /// <summary>
    /// <para>Indicates whether the file is overwrite any pre-existing files at the destination.</para>
    /// </summary>
    public bool IsOverwriteAtDestination { get; set; }

    /// <summary>
    /// <para>Indicates whether the file is compressed at the destination.</para>
    /// </summary>
    public bool IsCompressAtDestination { get; set; }

    /// <summary>
    /// <para>Indicates whether the file is renamed at the destination.</para>
    /// </summary>
    public bool IsRenameAtDestination { get; set; }

    /// <summary>
    /// <para>A pattern used to rename files being xmitted to the destination.</para>
    /// </summary>
    public string? RenamePattern { get; set; }

    /// <summary>
    /// <para>Indicates whether the destination is an archived location.</para>
    /// </summary>
    public bool IsArchive { get; set; }

    /// <summary>
    /// <para>Contains encrypted code.</para>
    /// </summary>
    public string? EncryptionCode { get; set; }

    /// <summary>
    /// <para>Public Key that helps to encrypt the file.</para>
    /// </summary>
    public string? EncryptionPublicKey { get; set; }

    /// <summary>
    /// <para>Private Key that helps to decrypt the file.</para>
    /// </summary>
    public string? EncryptionPrivateKey { get; set; }

    /// <summary>
    /// <para>Password associate to a private key.</para>
    /// </summary>
    public string? PrivateKeyPassword { get; set; }

    /// <summary>
    /// <para>Represent the FTP server for the destination file.</para>
    /// </summary>
    public FtpServerModel? FtpServerDetail { get; set; }

    /// <summary>
    /// <para>The File Name in which the File should be stored in destination folder</para>
    /// </summary>
    public string? DestinationFileName { get; set; }
}


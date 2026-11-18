namespace Fxfer.DomainApi.Domain.Entities;

// Legacy entity: HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities.FtpServer
// Table: FTP_SERVER
// Columns (legacy casing): FTP_SERVER_ID, FTP_SERVER_NAME, FTP_SERVER_IP, FTP_TRANSFER_MODE,
// FTP_USER, FTP_PASSWORD, FTP_DESCRIPTION, FTP_PORT, FTP_PROXY_ADR, FTP_PROXY_PORT, IS_VALID,
// SSH_AUTHENTICATION, OPERATOR_ID, LAST_MODIFIED_TS, CREATED_TS
// Legacy-to-target column naming (fxfer-db-schema):
// FTP_SERVER_IP -> ftp_server_host, FTP_PASSWORD -> ftp_password_secret_ref,
// FTP_PROXY_ADR -> ftp_proxy_address, SSH_AUTHENTICATION -> use_ssh_auth,
// CREATED_TS -> created_ts, LAST_MODIFIED_TS -> last_modified_ts
public sealed record FtpServer
{
    public Guid FtpServerId { get; init; }
    public required string FtpServerName { get; init; }
    public required string FtpServerHost { get; init; }
    public required string FtpTransferMode { get; init; }
    public required string FtpUser { get; init; }
    public required string FtpPasswordSecretRef { get; init; }
    public required string FtpDescription { get; init; }
    public short FtpPort { get; init; }
    public string? FtpProxyAddress { get; init; }
    public short? FtpProxyPort { get; init; }
    public bool IsValid { get; init; }
    public bool UseSshAuth { get; init; }
    public required string OperatorId { get; init; }
    public DateTimeOffset CreationDate { get; init; }
    public DateTimeOffset LastModifiedDate { get; init; }
}


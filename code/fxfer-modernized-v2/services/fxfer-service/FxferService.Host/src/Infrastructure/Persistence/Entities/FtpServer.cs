using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace FxferService.Infrastructure.Persistence.Entities;

[Table("FTP_SERVER")]
[ExcludeFromCodeCoverage]
public partial class FtpServer
{
    [Key]
    [Column("FTP_SERVER_ID")]
    public Guid FtpServerId { get; set; }

    [Required]
    [Column("FTP_SERVER_NAME")]
    [StringLength(50)]
    public string FtpServerName { get; set; } = null!;

    [Required]
    [Column("FTP_SERVER_IP")]
    [StringLength(50)]
    public string FtpServerIp { get; set; } = null!;

    [Required]
    [Column("FTP_TRANSFER_MODE")]
    public string FtpTransferMode { get; set; } = null!;

    [Required]
    [Column("FTP_USER")]
    [StringLength(50)]
    public string FtpUser { get; set; } = null!;

    [Required]
    [Column("FTP_PASSWORD")]
    [StringLength(50)]
    public string FtpPassword { get; set; } = null!;

    [Required]
    [Column("FTP_DESCRIPTION")]
    [StringLength(100)]
    public string FtpDescription { get; set; } = null!;

    [Required]
    [Column("FTP_PORT")]
    public short FtpPort { get; set; }

    [Column("FTP_PROXY_ADR")]
    [StringLength(200)]
    public string? FtpProxyAddress { get; set; }

    [Column("FTP_PROXY_PORT")]
    public short? FtpProxyPort { get; set; }

    [Required]
    [Column("IS_VALID")]
    public bool IsValid { get; set; }

    [Required]
    [StringLength(200)]
    [Column("OPERATOR_ID")]
    public string OperatorId { get; set; } = null!;

    [Column("LAST_MODIFIED_TS")]
    public DateTime LastModifiedDate { get; set; }

    [Column("CREATED_TS")]
    public DateTime CreationDate { get; set; }

    [Required]
    [Column("SSH_AUTHENTICATION")]
    public bool UseSSHAuth { get; set; }

    public virtual ICollection<FtsSourceLocationEntity> FtsSourceLocation { get; set; } = new HashSet<FtsSourceLocationEntity>();
}

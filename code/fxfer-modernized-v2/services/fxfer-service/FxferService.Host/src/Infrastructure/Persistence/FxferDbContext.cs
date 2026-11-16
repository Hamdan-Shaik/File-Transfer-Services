using FxferService.Infrastructure.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FxferService.Infrastructure.Persistence;

public sealed class FxferDbContext : DbContext, IDbContextBase
{
    public FxferDbContext(DbContextOptions<FxferDbContext> options) : base(options)
    {
    }

    public DbSet<FtpServer> FtpServers => Set<FtpServer>();
    public DbSet<FtsSourceLocationEntity> SourceLocations => Set<FtsSourceLocationEntity>();
    public DbSet<FtsDestinationLocations> Destinations => Set<FtsDestinationLocations>();
    public DbSet<FtsInboundTargetEntity> InboundTargets => Set<FtsInboundTargetEntity>();
    public DbSet<FtsIntegrationFileEntity> IntegrationFiles => Set<FtsIntegrationFileEntity>();
    public DbSet<FtsSuspensionEntity> Suspensions => Set<FtsSuspensionEntity>();
    public DbSet<FtsFileManagementEntity> FileManagement => Set<FtsFileManagementEntity>();
    public DbSet<FtsFileDeliveryEntity> FileDeliveries => Set<FtsFileDeliveryEntity>();
    public DbSet<FtsFileEventEntity> FileEvents => Set<FtsFileEventEntity>();
    public DbSet<FtsFileExceptionEntity> FileExceptions => Set<FtsFileExceptionEntity>();
    public DbSet<FtsNotificationEntity> Notifications => Set<FtsNotificationEntity>();
    public DbSet<OutboxMessageEntity> OutboxMessages => Set<OutboxMessageEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var schema = Environment.GetEnvironmentVariable("Persistence__Schema");
        if (string.IsNullOrWhiteSpace(schema))
        {
            schema = "public";
        }

        modelBuilder.HasDefaultSchema(schema);

        modelBuilder.Entity<FtpServer>(b =>
        {
            // F01-004: When serving an FTP server list, the system reads from the FTP_SERVER table.
            b.ToTable("FTP_SERVER", schema);
            b.HasKey(x => x.FtpServerId);
            b.Property(x => x.FtpServerId).HasColumnName("FTP_SERVER_ID");
            b.Property(x => x.FtpServerName).HasColumnName("FTP_SERVER_NAME").HasMaxLength(50);
            b.Property(x => x.FtpServerIp).HasColumnName("FTP_SERVER_IP").HasMaxLength(50);
            b.Property(x => x.FtpTransferMode).HasColumnName("FTP_TRANSFER_MODE");
            b.Property(x => x.FtpUser).HasColumnName("FTP_USER").HasMaxLength(50);
            b.Property(x => x.FtpPassword).HasColumnName("FTP_PASSWORD").HasMaxLength(50);
            b.Property(x => x.FtpDescription).HasColumnName("FTP_DESCRIPTION").HasMaxLength(100);
            b.Property(x => x.FtpPort).HasColumnName("FTP_PORT");
            b.Property(x => x.FtpProxyAddress).HasColumnName("FTP_PROXY_ADR").HasMaxLength(200);
            b.Property(x => x.FtpProxyPort).HasColumnName("FTP_PROXY_PORT");
            b.Property(x => x.UseSSHAuth).HasColumnName("SSH_AUTHENTICATION");
            b.Property(x => x.IsValid).HasColumnName("IS_VALID");
            b.Property(x => x.OperatorId).HasColumnName("OPERATOR_ID").HasMaxLength(200);
            b.Property(x => x.LastModifiedDate).HasColumnName("LAST_MODIFIED_TS");
            b.Property(x => x.CreationDate).HasColumnName("CREATED_TS");

            b.HasMany(x => x.FtsSourceLocation)
                .WithOne()
                .HasForeignKey(x => x.FtpServerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FtsSourceLocationEntity>(b =>
        {
            b.ToTable("FTS_SOURCE_LOCATION", schema);
            b.HasKey(x => x.FtsSourceLocationId);
            b.Property(x => x.FtsSourceLocationId).HasColumnName("FTS_SOURCE_LOCATION_ID");
            b.Property(x => x.ConfigId).HasColumnName("CONFIG_ID");
            b.Property(x => x.ConfigName).HasColumnName("CONFIG_NAME").HasMaxLength(50);
            b.Property(x => x.SourceProtocolCode).HasColumnName("SOURCE_PROTOCOL_CD").HasMaxLength(4);
            b.Property(x => x.SourceAddress).HasColumnName("SOURCE_ADDRESS").HasMaxLength(248);
            b.Property(x => x.SourceFileMask).HasColumnName("SOURCE_FILE_MASK").HasMaxLength(50);
            b.Property(x => x.FtpServerId).HasColumnName("FTP_SERVER_ID");
            b.Property(x => x.SendNotifications).HasColumnName("SEND_NOTIFICATIONS_IND");
            b.Property(x => x.OwnerName).HasColumnName("OWNER_NAME").HasMaxLength(50);
            b.Property(x => x.ModuleId).HasColumnName("MODULE_ID");
            b.Property(x => x.OwnerPhone).HasColumnName("OWNER_PHONE").HasMaxLength(10);
            b.Property(x => x.OwnerEmail).HasColumnName("OWNER_EMAIL").HasMaxLength(150);
            b.Property(x => x.ConfigDescription).HasColumnName("CONFIG_DESCRIPTION").HasMaxLength(200);
            b.Property(x => x.RunOnMonday).HasColumnName("RUN_ON_MONDAY_IND");
            b.Property(x => x.RunOnTuesday).HasColumnName("RUN_ON_TUESDAY_IND");
            b.Property(x => x.RunOnWednesday).HasColumnName("RUN_ON_WED_IND");
            b.Property(x => x.RunOnThursday).HasColumnName("RUN_ON_THURS_IND");
            b.Property(x => x.RunOnFriday).HasColumnName("RUN_ON_FRI_IND");
            b.Property(x => x.RunOnSaturday).HasColumnName("RUN_ON_SAT_IND");
            b.Property(x => x.RunOnSunday).HasColumnName("RUN_ON_SUN_IND");
            b.Property(x => x.RunOnFirstDayOfMonth).HasColumnName("RUN_ON_FRST_DAY_OF_MONTH_IND");
            b.Property(x => x.RunOnLastDayOfMonth).HasColumnName("RUN_ON_LAST_DAY_OF_MONTH_IND");
            b.Property(x => x.RunOnThisDayOfMonth).HasColumnName("RUN_ON_THIS_DAY_OF_MONTH");
            b.Property(x => x.StartTime24hhmm).HasColumnName("START_TIME_24HHMM").HasMaxLength(4);
            b.Property(x => x.EndTime24hhmm).HasColumnName("END_TIME_24HHMM").HasMaxLength(4);
            b.Property(x => x.FileDirection).HasColumnName("FILE_DIRECTION").HasMaxLength(50);
            b.Property(x => x.TradingPartner).HasColumnName("TRADING_PARTNER").HasMaxLength(10);
            b.Property(x => x.IsValid).HasColumnName("IS_VALID");
            b.Property(x => x.IsSuspended).HasColumnName("IS_SUSPENDED");
            b.Property(x => x.SuspendedChangedAt).HasColumnName("IS_SUSPENDED_CHANGED_TS");
            b.Property(x => x.OperatorId).HasColumnName("OPERATOR_ID").HasMaxLength(200);
            b.Property(x => x.LastModifiedAt).HasColumnName("LAST_MODIFIED_TS");
            b.Property(x => x.CreatedAt).HasColumnName("CREATED_TS");

        });

        modelBuilder.Entity<FtsDestinationLocations>(b =>
        {
            b.ToTable("FTS_DESTINATION", schema);
            b.HasKey(x => x.DestinationId);
            b.Property(x => x.DestinationId).HasColumnName("DESTINATION_ID");
            b.Property(x => x.FtsSourceLocationId).HasColumnName("FTS_SOURCE_LOCATION_ID");
            b.Property(x => x.DestinationAddress).HasColumnName("DESTINATION_ADDRESS").HasMaxLength(248);
            b.Property(x => x.DestProtocolCD).HasColumnName("DEST_PROTOCOL_CD").HasMaxLength(4);
            b.Property(x => x.OverwriteAtDest).HasColumnName("OVERWRITE_AT_DEST_IND");
            b.Property(x => x.CompressAtDest).HasColumnName("COMPRESS_AT_DEST_IND");
            b.Property(x => x.ReNameAtDest).HasColumnName("RENAME_AT_DEST_IND");
            b.Property(x => x.DestIsArchive).HasColumnName("DEST_IS_ARCHIVE_IND");
            b.Property(x => x.IsValid).HasColumnName("IS_VALID");
            b.Property(x => x.FtpServerId).HasColumnName("FTP_SERVER_ID");
            b.Property(x => x.RenamePattern).HasColumnName("RENAME_PATTERN").HasMaxLength(50);
            b.Property(x => x.ContactName).HasColumnName("CONTACT_NAME").HasMaxLength(50);
            b.Property(x => x.ContactPhone).HasColumnName("CONTACT_PHONE").HasMaxLength(15);
            b.Property(x => x.ContactEmail).HasColumnName("CONTACT_EMAIL").HasMaxLength(150);
            b.Property(x => x.OperatorId).HasColumnName("OPERATOR_ID").HasMaxLength(200);
            b.Property(x => x.CreationDate).HasColumnName("CREATION_TS");
            b.Property(x => x.LastModifiedDate).HasColumnName("LAST_MODIFIED_TS");
            b.Property(x => x.CdeEncryption).HasColumnName("CDE_ENCRYPTION").HasMaxLength(8);
            b.Property(x => x.EncryptionPublicKey).HasColumnName("ENCRYPTION_PUBLICKEY");
            b.Property(x => x.EncryptionPrivateKey).HasColumnName("ENCRYPTION_PRIVATEKEY");
            b.Property(x => x.PrivateKeyPassword).HasColumnName("PRIVATEKEY_PASSWORD").HasMaxLength(25);
            b.Property(x => x.FTEMetadataId).HasColumnName("EXTERNAL_FILE_DEFN_ID");
            b.Property(x => x.DestinationType).HasColumnName("DESTINATION_TYPE").HasMaxLength(200);
            b.Property(x => x.ContractName).HasColumnName("CONTRACT_NAME").HasMaxLength(200);
        });

        modelBuilder.Entity<FtsInboundTargetEntity>(b =>
        {
            b.ToTable("FTS_INBOUND_TARGET", schema);
            b.HasKey(x => x.FtsInboundTargetId);
            b.Property(x => x.FtsInboundTargetId).HasColumnName("FTS_INBOUND_TARGET_ID");
            b.Property(x => x.ExternalFileDefnId).HasColumnName("EXTERNAL_FILE_DEFN_ID");
            b.Property(x => x.ImportJobName).HasColumnName("IMPORT_JOB_NAME").HasMaxLength(200);
            b.Property(x => x.ContractName).HasColumnName("CONTRACT_NAME").HasMaxLength(50);
            b.Property(x => x.OperatorId).HasColumnName("OPERATOR_ID").HasMaxLength(200);
            b.Property(x => x.IsValid).HasColumnName("IS_VALID");
            b.Property(x => x.CreatedAt).HasColumnName("CREATED_TS");
            b.Property(x => x.LastModifiedAt).HasColumnName("LAST_MODIFIED_TS");
        });

        modelBuilder.Entity<FtsIntegrationFileEntity>(b =>
        {
            b.ToTable("FTS_INTEGRATION_FILE", schema);
            b.HasKey(x => x.FtsIntegrationFileId);
            b.Property(x => x.FtsIntegrationFileId).HasColumnName("FTS_INTEGRATION_FILE_ID");
            b.Property(x => x.GroupId).HasColumnName("GROUP_ID");
            b.Property(x => x.ConfigId).HasColumnName("CONFIG_ID");
            b.Property(x => x.FileName).HasColumnName("FILE_NAME").HasMaxLength(500);
            b.Property(x => x.OutboundMetadataDefnId).HasColumnName("ETF_OUTBOUND_METADATA_DEFN_ID");
            b.Property(x => x.IsProcessed).HasColumnName("IS_PROCESSED");
            b.Property(x => x.OperatorId).HasColumnName("OPERATOR_ID").HasMaxLength(200);
            b.Property(x => x.IsValid).HasColumnName("IS_VALID");
            b.Property(x => x.CreatedAt).HasColumnName("CREATED_TS");
            b.Property(x => x.LastModifiedAt).HasColumnName("LAST_MODIFIED_TS");
        });

        modelBuilder.Entity<FtsSuspensionEntity>(b =>
        {
            b.ToTable("FTS_SUSPENSION", schema);
            b.HasKey(x => x.SuspensionId);
            b.Property(x => x.SuspensionId).HasColumnName("SUSPENSION_ID");
            b.Property(x => x.FtsSourceLocationId).HasColumnName("FTS_SOURCE_LOCATION_ID");
            b.Property(x => x.ErrorText).HasColumnName("ERROR_TEXT").HasMaxLength(250);
            b.Property(x => x.StackTrace).HasColumnName("STACK_TRACE").HasMaxLength(1000);
            b.Property(x => x.OperatorId).HasColumnName("OPERATOR_ID").HasMaxLength(200);
            b.Property(x => x.LastModifiedAt).HasColumnName("LAST_MODIFIED_TS");
            b.Property(x => x.CreatedAt).HasColumnName("CREATED_TS");
        });

        modelBuilder.Entity<FtsFileManagementEntity>(b =>
        {
            b.ToTable("FTS_FILE_MANAGEMENT", schema);
            b.HasKey(x => x.FileId);
            b.Property(x => x.FileId).HasColumnName("FILE_ID");
            b.Property(x => x.FtsSourceLocationId).HasColumnName("FTS_SOURCE_LOCATION_ID");
            b.Property(x => x.FileName).HasColumnName("FILE_NAME").HasMaxLength(255);
            b.Property(x => x.FileStatus).HasColumnName("FILE_STATUS").HasMaxLength(1).IsFixedLength();
            b.Property(x => x.FileStatusAt).HasColumnName("FILE_STATUS_TS");
            b.Property(x => x.SakFileTrack).HasColumnName("SAK_FILE_TRACK");
            b.Property(x => x.FileSize).HasColumnName("FILE_SIZE");
            b.Property(x => x.ChecksumCode).HasColumnName("CHECKSUM_CD").HasMaxLength(20);
            b.Property(x => x.OperatorId).HasColumnName("OPERATOR_ID").HasMaxLength(200);
            b.Property(x => x.LastModifiedAt).HasColumnName("LAST_MODIFIED_TS");
            b.Property(x => x.CreatedAt).HasColumnName("CREATED_TS");
        });

        modelBuilder.Entity<FtsFileDeliveryEntity>(b =>
        {
            b.ToTable("FTS_FILE_DELIVERY", schema);
            b.HasKey(x => x.DeliveryId);
            b.Property(x => x.DeliveryId).HasColumnName("DELIVERY_ID");
            b.Property(x => x.FileId).HasColumnName("FILE_ID");
            b.Property(x => x.DestinationId).HasColumnName("DESTINATION_ID");
            b.Property(x => x.DeliveryAt).HasColumnName("DELIVERY_TS");
            b.Property(x => x.DestinationFileName).HasColumnName("DESTINATION_FILE_NAME");
            b.Property(x => x.OperatorId).HasColumnName("OPERATOR_ID");
            b.Property(x => x.LastModifiedAt).HasColumnName("LAST_MODIFIED_TS");
            b.Property(x => x.CreatedAt).HasColumnName("CREATED_TS");
        });

        modelBuilder.Entity<FtsFileEventEntity>(b =>
        {
            b.ToTable("FTS_FILE_EVENT", schema);
            b.HasKey(x => x.EventId);
            b.Property(x => x.EventId).HasColumnName("EVENT_ID");
            b.Property(x => x.FileId).HasColumnName("FILE_ID");
            b.Property(x => x.DestinationId).HasColumnName("DESTINATION_ID");
            b.Property(x => x.EventCode).HasColumnName("EVENT_CD");
            b.Property(x => x.TrackDescription).HasColumnName("EVENT_TRACK_DESCRIPTION");
            b.Property(x => x.CreatedAt).HasColumnName("CREATED_TS");
            b.Property(x => x.OperatorId).HasColumnName("OPERATOR_ID");
            b.Property(x => x.LastModifiedAt).HasColumnName("LAST_MODIFIED_TS");
        });

        modelBuilder.Entity<FtsFileExceptionEntity>(b =>
        {
            b.ToTable("FTS_FILE_EXCEPTION", schema);
            b.HasKey(x => x.ExceptionId);
            b.Property(x => x.ExceptionId).HasColumnName("EXCEPTION_ID");
            b.Property(x => x.FileId).HasColumnName("FILE_ID");
            b.Property(x => x.DestinationId).HasColumnName("DESTINATION_ID");
            b.Property(x => x.EventId).HasColumnName("EVENT_ID");
            b.Property(x => x.ExceptionType).HasColumnName("EXCEPTION_TYPE");
            b.Property(x => x.MethodName).HasColumnName("METHOD_NAME");
            b.Property(x => x.Message).HasColumnName("MESSAGE");
            b.Property(x => x.StackTrace).HasColumnName("STACK_TRACE");
            b.Property(x => x.CreatedAt).HasColumnName("CREATED_TS");
            b.Property(x => x.OperatorId).HasColumnName("OPERATOR_ID");
            b.Property(x => x.LastModifiedAt).HasColumnName("LAST_MODIFIED_TS");
        });

        modelBuilder.Entity<FtsNotificationEntity>(b =>
        {
            b.ToTable("FTS_NOTIFICATION", schema);
            b.HasKey(x => x.NotificationId);
            b.Property(x => x.NotificationId).HasColumnName("NOTIFICATION_ID");
            b.Property(x => x.FtsSourceLocationId).HasColumnName("FTS_SOURCE_LOCATION_ID");
            b.Property(x => x.EmailAddress).HasColumnName("EMAIL_ADDRESS").HasMaxLength(150);
            b.Property(x => x.ContactName).HasColumnName("CONTACT_NAME").HasMaxLength(200);
            b.Property(x => x.ContactPhone).HasColumnName("CONTACT_PHONE").HasMaxLength(15);
            b.Property(x => x.NotifyOnError).HasColumnName("NOTIFY_ON_ERROR_IND");
            b.Property(x => x.NotifyOnSuccess).HasColumnName("NOTIFY_ON_SUCCESS_IND");
            b.Property(x => x.OperatorId).HasColumnName("OPERATOR_ID").HasMaxLength(200);
            b.Property(x => x.CreatedAt).HasColumnName("CREATED_TS");
            b.Property(x => x.LastModifiedAt).HasColumnName("LAST_MODIFIED_TS");
        });

        modelBuilder.Entity<FtsDestinationLocations>()
            .HasMany(e => e.FtsFileDeliveryFK)
            .WithOne()
            .HasForeignKey(e => e.DestinationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsDestinationLocations>()
            .HasMany(e => e.FtsFileEventFK)
            .WithOne()
            .HasForeignKey(e => e.DestinationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsDestinationLocations>()
            .HasMany(e => e.FtsFileExceptionFK)
            .WithOne()
            .HasForeignKey(e => e.DestinationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsFileEventEntity>()
            .HasMany<FtsFileExceptionEntity>()
            .WithOne()
            .HasForeignKey(e => e.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsFileManagementEntity>()
            .HasMany<FtsFileDeliveryEntity>()
            .WithOne()
            .HasForeignKey(e => e.FileId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsFileManagementEntity>()
            .HasMany<FtsFileEventEntity>()
            .WithOne()
            .HasForeignKey(e => e.FileId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsFileManagementEntity>()
            .HasMany<FtsFileExceptionEntity>()
            .WithOne()
            .HasForeignKey(e => e.FileId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsSourceLocationEntity>()
            .HasMany<FtsFileManagementEntity>()
            .WithOne()
            .HasForeignKey(e => e.FtsSourceLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OutboxMessageEntity>(b =>
        {
            b.ToTable("FXFER_OUTBOX");
            b.HasKey(x => x.OutboxMessageId);
            b.Property(x => x.OutboxMessageId).HasColumnName("OUTBOX_MESSAGE_ID");
            b.Property(x => x.TenantId).HasColumnName("TENANT_ID").HasMaxLength(200);
            b.Property(x => x.EventName).HasColumnName("EVENT_NAME").HasMaxLength(200);
            b.Property(x => x.EventVersion).HasColumnName("EVENT_VERSION").HasMaxLength(50);
            b.Property(x => x.OccurredAt).HasColumnName("OCCURRED_AT");
            b.Property(x => x.PayloadJson).HasColumnName("PAYLOAD_JSON");
            b.Property(x => x.DispatchedAt).HasColumnName("DISPATCHED_AT");
        });
    }
}


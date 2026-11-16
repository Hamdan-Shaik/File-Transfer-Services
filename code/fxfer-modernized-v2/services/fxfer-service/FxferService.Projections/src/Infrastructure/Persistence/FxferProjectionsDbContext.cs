using FxferService.Projections.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FxferService.Projections.Infrastructure.Persistence;

public sealed class FxferProjectionsDbContext : DbContext
{
    public FxferProjectionsDbContext(DbContextOptions<FxferProjectionsDbContext> options)
        : base(options)
    {
    }

    public DbSet<InboxEntry> InboxEntries => Set<InboxEntry>();

    public DbSet<FtpServerProjection> FtpServers => Set<FtpServerProjection>();
    public DbSet<FtsSourceLocationProjection> FtsSourceLocations => Set<FtsSourceLocationProjection>();
    public DbSet<FtsDestinationProjection> FtsDestinations => Set<FtsDestinationProjection>();
    public DbSet<FtsInboundTargetProjection> FtsInboundTargets => Set<FtsInboundTargetProjection>();
    public DbSet<FtsIntegrationFileProjection> FtsIntegrationFiles => Set<FtsIntegrationFileProjection>();
    public DbSet<FtsSuspensionProjection> FtsSuspensions => Set<FtsSuspensionProjection>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InboxEntry>(b =>
        {
            b.ToTable("FXFER_INBOX");
            b.HasKey(x => x.EventId);
            b.Property(x => x.EventId).HasColumnName("EVENT_ID");
            b.Property(x => x.ProcessedAt).HasColumnName("PROCESSED_TS");
        });

        modelBuilder.Entity<FtpServerProjection>(b =>
        {
            b.ToTable("FTP_SERVER");
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
            b.Property(x => x.UseSshAuth).HasColumnName("SSH_AUTHENTICATION");
            b.Property(x => x.IsValid).HasColumnName("IS_VALID");
            b.Property(x => x.OperatorId).HasColumnName("OPERATOR_ID").HasMaxLength(200);
            b.Property(x => x.LastModifiedTs).HasColumnName("LAST_MODIFIED_TS");
            b.Property(x => x.CreatedTs).HasColumnName("CREATED_TS");
        });

        modelBuilder.Entity<FtsSourceLocationProjection>(b =>
        {
            b.ToTable("FTS_SOURCE_LOCATION");
            b.HasKey(x => x.FtsSourceLocationId);

            b.Property(x => x.FtsSourceLocationId).HasColumnName("FTS_SOURCE_LOCATION_ID");
            b.Property(x => x.ConfigId).HasColumnName("CONFIG_ID");
            b.Property(x => x.ConfigName).HasColumnName("CONFIG_NAME").HasMaxLength(50);
            b.Property(x => x.SourceProtocolCd).HasColumnName("SOURCE_PROTOCOL_CD").HasMaxLength(4);
            b.Property(x => x.SourceAddress).HasColumnName("SOURCE_ADDRESS").HasMaxLength(248);
            b.Property(x => x.SourceFileMask).HasColumnName("SOURCE_FILE_MASK").HasMaxLength(50);
            b.Property(x => x.FtpServerId).HasColumnName("FTP_SERVER_ID");
            b.Property(x => x.SendNotificationsInd).HasColumnName("SEND_NOTIFICATIONS_IND");
            b.Property(x => x.OwnerName).HasColumnName("OWNER_NAME").HasMaxLength(50);
            b.Property(x => x.ModuleId).HasColumnName("MODULE_ID");
            b.Property(x => x.OwnerPhone).HasColumnName("OWNER_PHONE").HasMaxLength(10);
            b.Property(x => x.OwnerEmail).HasColumnName("OWNER_EMAIL").HasMaxLength(150);
            b.Property(x => x.ConfigDescription).HasColumnName("CONFIG_DESCRIPTION").HasMaxLength(200);
            b.Property(x => x.RunOnMondayInd).HasColumnName("RUN_ON_MONDAY_IND");
            b.Property(x => x.RunOnTuesdayInd).HasColumnName("RUN_ON_TUESDAY_IND");
            b.Property(x => x.RunOnWedInd).HasColumnName("RUN_ON_WED_IND");
            b.Property(x => x.RunOnThursInd).HasColumnName("RUN_ON_THURS_IND");
            b.Property(x => x.RunOnFriInd).HasColumnName("RUN_ON_FRI_IND");
            b.Property(x => x.RunOnSatInd).HasColumnName("RUN_ON_SAT_IND");
            b.Property(x => x.RunOnSunInd).HasColumnName("RUN_ON_SUN_IND");
            b.Property(x => x.RunOnFrstDayOfMonthInd).HasColumnName("RUN_ON_FRST_DAY_OF_MONTH_IND");
            b.Property(x => x.RunOnLastDayOfMonthInd).HasColumnName("RUN_ON_LAST_DAY_OF_MONTH_IND");
            b.Property(x => x.RunOnThisDayOfMonth).HasColumnName("RUN_ON_THIS_DAY_OF_MONTH");
            b.Property(x => x.StartTime24hhmm).HasColumnName("START_TIME_24HHMM").HasMaxLength(4);
            b.Property(x => x.EndTime24hhmm).HasColumnName("END_TIME_24HHMM").HasMaxLength(4);
            b.Property(x => x.FileDirection).HasColumnName("FILE_DIRECTION").HasMaxLength(50);
            b.Property(x => x.TradingPartner).HasColumnName("TRADING_PARTNER").HasMaxLength(10);
            b.Property(x => x.IsValid).HasColumnName("IS_VALID");
            b.Property(x => x.IsSuspended).HasColumnName("IS_SUSPENDED");
            b.Property(x => x.IsSuspendedChangedTs).HasColumnName("IS_SUSPENDED_CHANGED_TS");
            b.Property(x => x.OperatorId).HasColumnName("OPERATOR_ID").HasMaxLength(200);
            b.Property(x => x.LastModifiedTs).HasColumnName("LAST_MODIFIED_TS");
            b.Property(x => x.CreatedTs).HasColumnName("CREATED_TS");

            b.HasMany(x => x.Destinations)
                .WithOne()
                .HasForeignKey(x => x.FtsSourceLocationId);
        });

        modelBuilder.Entity<FtsDestinationProjection>(b =>
        {
            b.ToTable("FTS_DESTINATION");
            b.HasKey(x => x.DestinationId);

            b.Property(x => x.DestinationId).HasColumnName("DESTINATION_ID");
            b.Property(x => x.FtsSourceLocationId).HasColumnName("FTS_SOURCE_LOCATION_ID");
            b.Property(x => x.DestinationAddress).HasColumnName("DESTINATION_ADDRESS").HasMaxLength(248);
            b.Property(x => x.DestProtocolCd).HasColumnName("DEST_PROTOCOL_CD").HasMaxLength(4);
            b.Property(x => x.OverwriteAtDestInd).HasColumnName("OVERWRITE_AT_DEST_IND");
            b.Property(x => x.CompressAtDestInd).HasColumnName("COMPRESS_AT_DEST_IND");
            b.Property(x => x.RenameAtDestInd).HasColumnName("RENAME_AT_DEST_IND");
            b.Property(x => x.DestIsArchiveInd).HasColumnName("DEST_IS_ARCHIVE_IND");
            b.Property(x => x.IsValid).HasColumnName("IS_VALID");
            b.Property(x => x.FtpServerId).HasColumnName("FTP_SERVER_ID");
            b.Property(x => x.RenamePattern).HasColumnName("RENAME_PATTERN").HasMaxLength(50);
            b.Property(x => x.ContactName).HasColumnName("CONTACT_NAME").HasMaxLength(50);
            b.Property(x => x.ContactPhone).HasColumnName("CONTACT_PHONE").HasMaxLength(15);
            b.Property(x => x.ContactEmail).HasColumnName("CONTACT_EMAIL").HasMaxLength(150);
            b.Property(x => x.OperatorId).HasColumnName("OPERATOR_ID").HasMaxLength(200);
            b.Property(x => x.CreationTs).HasColumnName("CREATION_TS");
            b.Property(x => x.LastModifiedTs).HasColumnName("LAST_MODIFIED_TS");
            b.Property(x => x.CdeEncryption).HasColumnName("CDE_ENCRYPTION").HasMaxLength(8);
            b.Property(x => x.EncryptionPublicKey).HasColumnName("ENCRYPTION_PUBLICKEY");
            b.Property(x => x.EncryptionPrivateKey).HasColumnName("ENCRYPTION_PRIVATEKEY");
            b.Property(x => x.PrivateKeyPassword).HasColumnName("PRIVATEKEY_PASSWORD").HasMaxLength(25);
            b.Property(x => x.ExternalFileDefnId).HasColumnName("EXTERNAL_FILE_DEFN_ID");
            b.Property(x => x.DestinationType).HasColumnName("DESTINATION_TYPE").HasMaxLength(200);
            b.Property(x => x.ContractName).HasColumnName("CONTRACT_NAME").HasMaxLength(200);
        });

        modelBuilder.Entity<FtsInboundTargetProjection>(b =>
        {
            b.ToTable("FTS_INBOUND_TARGET");
            b.HasKey(x => x.FtsInboundTargetId);

            b.Property(x => x.FtsInboundTargetId).HasColumnName("FTS_INBOUND_TARGET_ID");
            b.Property(x => x.ExternalFileDefnId).HasColumnName("EXTERNAL_FILE_DEFN_ID");
            b.Property(x => x.ImportJobName).HasColumnName("IMPORT_JOB_NAME").HasMaxLength(200);
            b.Property(x => x.ContractName).HasColumnName("CONTRACT_NAME").HasMaxLength(200);
            b.Property(x => x.OperatorId).HasColumnName("OPERATOR_ID").HasMaxLength(200);
            b.Property(x => x.IsValid).HasColumnName("IS_VALID");
            b.Property(x => x.CreatedTs).HasColumnName("CREATED_TS");
            b.Property(x => x.LastModifiedTs).HasColumnName("LAST_MODIFIED_TS");
        });

        modelBuilder.Entity<FtsIntegrationFileProjection>(b =>
        {
            b.ToTable("FTS_INTEGRATION_FILE");
            b.HasKey(x => x.FtsIntegrationFileId);

            b.Property(x => x.FtsIntegrationFileId).HasColumnName("FTS_INTEGRATION_FILE_ID");
            b.Property(x => x.GroupId).HasColumnName("GROUP_ID");
            b.Property(x => x.ConfigId).HasColumnName("CONFIG_ID");
            b.Property(x => x.FileName).HasColumnName("FILE_NAME").HasMaxLength(500);
            b.Property(x => x.EtfOutboundMetadataDefnId).HasColumnName("ETF_OUTBOUND_METADATA_DEFN_ID");
            b.Property(x => x.IsProcessed).HasColumnName("IS_PROCESSED");
            b.Property(x => x.OperatorId).HasColumnName("OPERATOR_ID").HasMaxLength(200);
            b.Property(x => x.IsValid).HasColumnName("IS_VALID");
            b.Property(x => x.CreatedTs).HasColumnName("CREATED_TS");
            b.Property(x => x.LastModifiedTs).HasColumnName("LAST_MODIFIED_TS");
        });

        modelBuilder.Entity<FtsSuspensionProjection>(b =>
        {
            b.ToTable("FTS_SUSPENSION");
            b.HasKey(x => x.SuspensionId);

            b.Property(x => x.SuspensionId).HasColumnName("SUSPENSION_ID");
            b.Property(x => x.FtsSourceLocationId).HasColumnName("FTS_SOURCE_LOCATION_ID");
            b.Property(x => x.ErrorText).HasColumnName("ERROR_TEXT").HasMaxLength(250);
            b.Property(x => x.StackTrace).HasColumnName("STACK_TRACE").HasMaxLength(1000);
            b.Property(x => x.OperatorId).HasColumnName("OPERATOR_ID").HasMaxLength(200);
            b.Property(x => x.LastModifiedTs).HasColumnName("LAST_MODIFIED_TS");
            b.Property(x => x.CreatedTs).HasColumnName("CREATED_TS");
        });

        base.OnModelCreating(modelBuilder);
    }
}

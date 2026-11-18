using Microsoft.EntityFrameworkCore;

using Fxfer.DomainApi.Domain.Entities;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

namespace Fxfer.DomainApi.Infrastructure.DataAccess;

public sealed class FxferDbContext : DbContext
{
    public FxferDbContext(DbContextOptions<FxferDbContext> options)
        : base(options)
    {
    }

    public DbSet<FtpServer> FtpServers => Set<FtpServer>();
    public DbSet<FtsSourceLocation> FtsSourceLocations => Set<FtsSourceLocation>();
    public DbSet<FtsDestinationLocations> FtsDestinationLocations => Set<FtsDestinationLocations>();
    public DbSet<FtsFileManagement> FtsFileManagement => Set<FtsFileManagement>();
    public DbSet<FtsFileDelivery> FtsFileDelivery => Set<FtsFileDelivery>();
    public DbSet<FtsFileEvent> FtsFileEvents => Set<FtsFileEvent>();
    public DbSet<FtsFileException> FtsFileExceptions => Set<FtsFileException>();
    public DbSet<FtsSuspension> FtsSuspension => Set<FtsSuspension>();
    public DbSet<FtsNotification> FtsNotification => Set<FtsNotification>();
    public DbSet<FtsInboundTarget> FtsInboundTarget => Set<FtsInboundTarget>();
    public DbSet<FtsIntegrationFile> FtsIntegrationFile => Set<FtsIntegrationFile>();

    public DbSet<Eventing.Outbox.OutboxMessage> OutboxMessages => Set<Eventing.Outbox.OutboxMessage>();

    private const string cde_encryption = nameof(cde_encryption);
    private const string checksum_cd = nameof(checksum_cd);
    private const string compress_at_dest_ind = nameof(compress_at_dest_ind);
    private const string config_id = nameof(config_id);
    private const string config_name = nameof(config_name);
    private const string contact_email = nameof(contact_email);
    private const string contact_name = nameof(contact_name);
    private const string contact_phone = nameof(contact_phone);
    private const string contract_name = nameof(contract_name);
    private const string created_ts = nameof(created_ts);
    private const string delivery_id = nameof(delivery_id);
    private const string delivery_ts = nameof(delivery_ts);
    private const string description = nameof(description);
    private const string dest_is_archive_ind = nameof(dest_is_archive_ind);
    private const string dest_protocol_cd = nameof(dest_protocol_cd);
    private const string destination_address = nameof(destination_address);
    private const string destination_file_name = nameof(destination_file_name);
    private const string destination_id = nameof(destination_id);
    private const string destination_type = nameof(destination_type);
    private const string email_address = nameof(email_address);
    private const string encryption_private_key_secret_ref = nameof(encryption_private_key_secret_ref);
    private const string encryption_public_key = nameof(encryption_public_key);
    private const string end_time_24hhmm = nameof(end_time_24hhmm);
    private const string error_text = nameof(error_text);
    private const string etf_outbound_metadata_defn_id = nameof(etf_outbound_metadata_defn_id);
    private const string event_cd = nameof(event_cd);
    private const string event_id = nameof(event_id);
    private const string event_track_description = nameof(event_track_description);
    private const string exception_id = nameof(exception_id);
    private const string exception_type = nameof(exception_type);
    private const string external_file_defn_id = nameof(external_file_defn_id);
    private const string file_direction = nameof(file_direction);
    private const string file_id = nameof(file_id);
    private const string file_name = nameof(file_name);
    private const string file_size = nameof(file_size);
    private const string file_source_location_id = nameof(file_source_location_id);
    private const string file_status = nameof(file_status);
    private const string file_status_ts = nameof(file_status_ts);
    private const string fte_metadata_id = nameof(fte_metadata_id);
    private const string ftp_description = nameof(ftp_description);
    private const string ftp_password_secret_ref = nameof(ftp_password_secret_ref);
    private const string ftp_port = nameof(ftp_port);
    private const string ftp_proxy_address = nameof(ftp_proxy_address);
    private const string ftp_proxy_port = nameof(ftp_proxy_port);
    private const string ftp_server = nameof(ftp_server);
    private const string ftp_server_host = nameof(ftp_server_host);
    private const string ftp_server_id = nameof(ftp_server_id);
    private const string ftp_server_name = nameof(ftp_server_name);
    private const string ftp_transfer_mode = nameof(ftp_transfer_mode);
    private const string ftp_user = nameof(ftp_user);
    private const string fts_destination = nameof(fts_destination);
    private const string fts_file_delivery = nameof(fts_file_delivery);
    private const string fts_file_event = nameof(fts_file_event);
    private const string fts_file_exception = nameof(fts_file_exception);
    private const string fts_file_management = nameof(fts_file_management);
    private const string fts_inbound_target = nameof(fts_inbound_target);
    private const string fts_inbound_target_id = nameof(fts_inbound_target_id);
    private const string fts_integration_file = nameof(fts_integration_file);
    private const string fts_integration_file_id = nameof(fts_integration_file_id);
    private const string fts_notification = nameof(fts_notification);
    private const string fts_source_location = nameof(fts_source_location);
    private const string fts_source_location_id = nameof(fts_source_location_id);
    private const string fts_suspension = nameof(fts_suspension);
    private const string group_id = nameof(group_id);
    private const string import_job_name = nameof(import_job_name);
    private const string is_processed = nameof(is_processed);
    private const string is_suspended = nameof(is_suspended);
    private const string is_suspended_changed_ts = nameof(is_suspended_changed_ts);
    private const string is_valid = nameof(is_valid);
    private const string last_modified_ts = nameof(last_modified_ts);
    private const string message = nameof(message);
    private const string method_name = nameof(method_name);
    private const string notification_id = nameof(notification_id);
    private const string notify_on_error_ind = nameof(notify_on_error_ind);
    private const string notify_on_success_ind = nameof(notify_on_success_ind);
    private const string operator_id = nameof(operator_id);
    private const string overwrite_at_dest_ind = nameof(overwrite_at_dest_ind);
    private const string owner_email = nameof(owner_email);
    private const string owner_module = nameof(owner_module);
    private const string owner_name = nameof(owner_name);
    private const string owner_phone = nameof(owner_phone);
    private const string private_key_password_secret_ref = nameof(private_key_password_secret_ref);
    private const string rename_at_dest_ind = nameof(rename_at_dest_ind);
    private const string rename_pattern = nameof(rename_pattern);
    private const string run_on_first_day_of_month_ind = nameof(run_on_first_day_of_month_ind);
    private const string run_on_friday_ind = nameof(run_on_friday_ind);
    private const string run_on_last_day_of_month_ind = nameof(run_on_last_day_of_month_ind);
    private const string run_on_monday_ind = nameof(run_on_monday_ind);
    private const string run_on_saturday_ind = nameof(run_on_saturday_ind);
    private const string run_on_sunday_ind = nameof(run_on_sunday_ind);
    private const string run_on_this_day_of_month = nameof(run_on_this_day_of_month);
    private const string run_on_thursday_ind = nameof(run_on_thursday_ind);
    private const string run_on_tuesday_ind = nameof(run_on_tuesday_ind);
    private const string run_on_wednesday_ind = nameof(run_on_wednesday_ind);
    private const string sak_file_track = nameof(sak_file_track);
    private const string send_notifications_ind = nameof(send_notifications_ind);
    private const string source_address = nameof(source_address);
    private const string source_file_mask = nameof(source_file_mask);
    private const string source_protocol_cd = nameof(source_protocol_cd);
    private const string stack_trace = nameof(stack_trace);
    private const string start_time_24hhmm = nameof(start_time_24hhmm);
    private const string suspension_id = nameof(suspension_id);
    private const string trading_partner = nameof(trading_partner);
    private const string use_ssh_auth = nameof(use_ssh_auth);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Legacy note (FileTransferEntityDbContext.OnModelCreating):
        // MaintainabilityDetails ReviewerName = Josue Chavez.
        // EF6 table names (Oracle casing): FTP_SERVER, FTS_SOURCE_LOCATION, FTS_DESTINATION,
        // FTS_FILE_DELIVERY, FTS_FILE_EVENT, FTS_FILE_EXCEPTION, FTS_FILE_MANAGEMENT,
        // FTS_SUSPENSION, FTS_NOTIFICATION, FTS_INBOUND_TARGET, FTS_INTEGRATION_FILE.
        modelBuilder.Entity<FtpServer>(b =>
        {
            b.ToTable(ftp_server);
            b.HasKey(e => e.FtpServerId);

            b.Property(e => e.FtpServerId).HasColumnName(ftp_server_id);
            b.Property(e => e.FtpServerName).HasColumnName(ftp_server_name).HasMaxLength(50);
            b.Property(e => e.FtpServerHost).HasColumnName(ftp_server_host).HasMaxLength(50);
            b.Property(e => e.FtpTransferMode).HasColumnName(ftp_transfer_mode).HasMaxLength(15);
            b.Property(e => e.FtpUser).HasColumnName(ftp_user).HasMaxLength(50);
            b.Property(e => e.FtpPasswordSecretRef).HasColumnName(ftp_password_secret_ref);
            b.Property(e => e.FtpDescription).HasColumnName(ftp_description).HasMaxLength(100);
            b.Property(e => e.FtpPort).HasColumnName(ftp_port);
            b.Property(e => e.FtpProxyAddress).HasColumnName(ftp_proxy_address).HasMaxLength(200);
            b.Property(e => e.FtpProxyPort).HasColumnName(ftp_proxy_port);
            b.Property(e => e.IsValid).HasColumnName(is_valid);
            b.Property(e => e.UseSshAuth).HasColumnName(use_ssh_auth);
            b.Property(e => e.OperatorId).HasColumnName(operator_id).HasMaxLength(200);
            b.Property(e => e.CreationDate).HasColumnName(created_ts);
            b.Property(e => e.LastModifiedDate).HasColumnName(last_modified_ts);
        });

        modelBuilder.Entity<FtsSourceLocation>(b =>
        {
            b.ToTable(fts_source_location);
            b.HasKey(e => e.FtsSourceLocationId);

            b.Property(e => e.FtsSourceLocationId).HasColumnName(fts_source_location_id);
            b.Property(e => e.ConfigId).HasColumnName(config_id);
            b.Property(e => e.ConfigName).HasColumnName(config_name).HasMaxLength(50);
            b.Property(e => e.SourceProtocolCD).HasColumnName(source_protocol_cd).HasMaxLength(4);
            b.Property(e => e.SourceAddress).HasColumnName(source_address).HasMaxLength(248);
            b.Property(e => e.SourceFileMask).HasColumnName(source_file_mask).HasMaxLength(50);
            b.Property(e => e.FtpServerId).HasColumnName(ftp_server_id);
            b.Property(e => e.SendNotificationsInd).HasColumnName(send_notifications_ind);
            b.Property(e => e.OwnerName).HasColumnName(owner_name).HasMaxLength(50);
            b.Property(e => e.OwnerModule).HasColumnName(owner_module);
            b.Property(e => e.OwnerPhone).HasColumnName(owner_phone).HasMaxLength(50);
            b.Property(e => e.OwnerEmail).HasColumnName(owner_email).HasMaxLength(50);
            b.Property(e => e.Description).HasColumnName(description).HasMaxLength(200);
            b.Property(e => e.RunOnMondayInd).HasColumnName(run_on_monday_ind);
            b.Property(e => e.RunOnTuesdayInd).HasColumnName(run_on_tuesday_ind);
            b.Property(e => e.RunOnWednesdayInd).HasColumnName(run_on_wednesday_ind);
            b.Property(e => e.RunOnThursdayInd).HasColumnName(run_on_thursday_ind);
            b.Property(e => e.RunOnFridayInd).HasColumnName(run_on_friday_ind);
            b.Property(e => e.RunOnSaturdayInd).HasColumnName(run_on_saturday_ind);
            b.Property(e => e.RunOnSundayInd).HasColumnName(run_on_sunday_ind);
            b.Property(e => e.RunOnFirstDayOfMonthInd).HasColumnName(run_on_first_day_of_month_ind);
            b.Property(e => e.RunOnLastDayOfMonthInd).HasColumnName(run_on_last_day_of_month_ind);
            b.Property(e => e.RunOnThisDayOfMonth).HasColumnName(run_on_this_day_of_month);
            b.Property(e => e.StartTime24HHMM).HasColumnName(start_time_24hhmm).HasMaxLength(4);
            b.Property(e => e.EndTime24HHMM).HasColumnName(end_time_24hhmm).HasMaxLength(4);
            b.Property(e => e.FileDirection).HasColumnName(file_direction).HasMaxLength(50);
            b.Property(e => e.TradingPartner).HasColumnName(trading_partner).HasMaxLength(10);
            b.Property(e => e.IsValid).HasColumnName(is_valid);
            b.Property(e => e.IsSuspended).HasColumnName(is_suspended);
            b.Property(e => e.IsSuspendedChangedTs).HasColumnName(is_suspended_changed_ts);
            b.Property(e => e.OperatorId).HasColumnName(operator_id).HasMaxLength(200);
            b.Property(e => e.CreationDate).HasColumnName(created_ts);
            b.Property(e => e.LastModifiedDate).HasColumnName(last_modified_ts);
        });

        modelBuilder.Entity<FtsDestinationLocations>(b =>
        {
            b.ToTable(fts_destination);
            b.HasKey(e => e.DestinationId);

            b.Property(e => e.DestinationId).HasColumnName(destination_id);
            b.Property(e => e.FtsSourceLocationId).HasColumnName(fts_source_location_id);
            b.Property(e => e.DestinationAddress).HasColumnName(destination_address).HasMaxLength(248);
            b.Property(e => e.DestProtocolCD).HasColumnName(dest_protocol_cd).HasMaxLength(4);
            b.Property(e => e.OverwriteAtDest).HasColumnName(overwrite_at_dest_ind);
            b.Property(e => e.CompressAtDest).HasColumnName(compress_at_dest_ind);
            b.Property(e => e.RenameAtDest).HasColumnName(rename_at_dest_ind);
            b.Property(e => e.DestIsArchive).HasColumnName(dest_is_archive_ind);
            b.Property(e => e.IsValid).HasColumnName(is_valid);
            b.Property(e => e.FtpServerId).HasColumnName(ftp_server_id);
            b.Property(e => e.RenamePattern).HasColumnName(rename_pattern).HasMaxLength(50);
            b.Property(e => e.ContactName).HasColumnName(contact_name).HasMaxLength(50);
            b.Property(e => e.ContactPhone).HasColumnName(contact_phone).HasMaxLength(15);
            b.Property(e => e.ContactEmail).HasColumnName(contact_email).HasMaxLength(150);
            b.Property(e => e.OperatorId).HasColumnName(operator_id).HasMaxLength(200);
            b.Property(e => e.CreationDate).HasColumnName(created_ts);
            b.Property(e => e.LastModifiedDate).HasColumnName(last_modified_ts);
            b.Property(e => e.CdeEncryption).HasColumnName(cde_encryption).HasMaxLength(8);
            b.Property(e => e.EncryptionPublicKey).HasColumnName(encryption_public_key);
            b.Property(e => e.EncryptionPrivateKeySecretRef).HasColumnName(encryption_private_key_secret_ref);
            b.Property(e => e.PrivateKeyPasswordSecretRef).HasColumnName(private_key_password_secret_ref);
            b.Property(e => e.FTEMetadataId).HasColumnName(external_file_defn_id);
            b.Property(e => e.DestinationType).HasColumnName(destination_type).HasMaxLength(200);
            b.Property(e => e.ContractName).HasColumnName(contract_name).HasMaxLength(200);
        });

        modelBuilder.Entity<FtsFileManagement>(b =>
        {
            b.ToTable(fts_file_management);
            b.HasKey(e => e.FileId);

            b.Property(e => e.FileId).HasColumnName(file_id);
            b.Property(e => e.FileSourceLocationId).HasColumnName(file_source_location_id);
            b.Property(e => e.FileName).HasColumnName(file_name).HasMaxLength(255);
            b.Property(e => e.FileStatus).HasColumnName(file_status).HasMaxLength(1);
            b.Property(e => e.FileStatusTs).HasColumnName(file_status_ts);
            b.Property(e => e.SakFileTrack).HasColumnName(sak_file_track);
            b.Property(e => e.FileSize).HasColumnName(file_size);
            b.Property(e => e.ChecksumCD).HasColumnName(checksum_cd).HasMaxLength(20);
            b.Property(e => e.OperatorId).HasColumnName(operator_id).HasMaxLength(200);
            b.Property(e => e.LastModifiedTs).HasColumnName(last_modified_ts);
            b.Property(e => e.CreatedDate).HasColumnName(created_ts);
            b.Property(e => e.GroupId).HasColumnName(group_id);
        });

        modelBuilder.Entity<FtsFileEvent>(b =>
        {
            b.ToTable(fts_file_event);
            b.HasKey(e => e.EventId);

            b.Property(e => e.EventId).HasColumnName(event_id);
            b.Property(e => e.FileId).HasColumnName(file_id);
            b.Property(e => e.DestinationId).HasColumnName(destination_id);
            b.Property(e => e.EventCD).HasColumnName(event_cd).HasMaxLength(20);
            b.Property(e => e.EventTrackDescription).HasColumnName(event_track_description).HasMaxLength(500);
            b.Property(e => e.CreatedDate).HasColumnName(created_ts);
            b.Property(e => e.OperatorId).HasColumnName(operator_id).HasMaxLength(200);
            b.Property(e => e.LastModifiedTs).HasColumnName(last_modified_ts);
        });

        modelBuilder.Entity<FtsFileDelivery>(b =>
        {
            b.ToTable(fts_file_delivery);
            b.HasKey(e => e.DeliveryId);

            b.Property(e => e.DeliveryId).HasColumnName(delivery_id);
            b.Property(e => e.FileId).HasColumnName(file_id);
            b.Property(e => e.DestinationId).HasColumnName(destination_id);
            b.Property(e => e.DeliveryTs).HasColumnName(delivery_ts);
            b.Property(e => e.DestinationFileName).HasColumnName(destination_file_name).HasMaxLength(255);
            b.Property(e => e.OperatorId).HasColumnName(operator_id).HasMaxLength(200);
            b.Property(e => e.LastModifiedTs).HasColumnName(last_modified_ts);
            b.Property(e => e.CreatedDate).HasColumnName(created_ts);
        });

        modelBuilder.Entity<FtsFileException>(b =>
        {
            b.ToTable(fts_file_exception);
            b.HasKey(e => e.ExceptionId);

            b.Property(e => e.ExceptionId).HasColumnName(exception_id);
            b.Property(e => e.FileId).HasColumnName(file_id);
            b.Property(e => e.DestinationId).HasColumnName(destination_id);
            b.Property(e => e.EventId).HasColumnName(event_id);
            b.Property(e => e.ExceptionType).HasColumnName(exception_type).HasMaxLength(2000);
            b.Property(e => e.MethodName).HasColumnName(method_name).HasMaxLength(2000);
            b.Property(e => e.Message).HasColumnName(message).HasMaxLength(2000);
            b.Property(e => e.StackTrace).HasColumnName(stack_trace).HasMaxLength(4000);
            b.Property(e => e.CreatedDate).HasColumnName(created_ts);
            b.Property(e => e.OperatorId).HasColumnName(operator_id).HasMaxLength(200);
            b.Property(e => e.LastModifiedTs).HasColumnName(last_modified_ts);
        });

        modelBuilder.Entity<FtsSuspension>(b =>
        {
            b.ToTable(fts_suspension);
            b.HasKey(e => e.SuspensionId);

            b.Property(e => e.SuspensionId).HasColumnName(suspension_id);
            b.Property(e => e.FtsSourceLocationId).HasColumnName(fts_source_location_id);
            b.Property(e => e.ErrorText).HasColumnName(error_text).HasMaxLength(250);
            b.Property(e => e.StackTrace).HasColumnName(stack_trace).HasMaxLength(1000);
            b.Property(e => e.OperatorId).HasColumnName(operator_id).HasMaxLength(200);
            b.Property(e => e.CreatedTs).HasColumnName(created_ts);
            b.Property(e => e.LastModifiedTs).HasColumnName(last_modified_ts);
        });

        modelBuilder.Entity<FtsNotification>(b =>
        {
            b.ToTable(fts_notification);
            b.HasKey(e => e.NotificationId);

            b.Property(e => e.NotificationId).HasColumnName(notification_id);
            b.Property(e => e.FtsSourceLocationId).HasColumnName(fts_source_location_id);
            b.Property(e => e.EmailAddress).HasColumnName(email_address).HasMaxLength(150);
            b.Property(e => e.ContactName).HasColumnName(contact_name).HasMaxLength(200);
            b.Property(e => e.ContactPhone).HasColumnName(contact_phone).HasMaxLength(15);
            b.Property(e => e.NotifyOnErrorInd).HasColumnName(notify_on_error_ind);
            b.Property(e => e.NotifyOnSuccessInd).HasColumnName(notify_on_success_ind);
            b.Property(e => e.OperatorId).HasColumnName(operator_id).HasMaxLength(200);
            b.Property(e => e.CreatedTs).HasColumnName(created_ts);
            b.Property(e => e.LastModifiedTs).HasColumnName(last_modified_ts);
        });

        modelBuilder.Entity<FtsInboundTarget>(b =>
        {
            b.ToTable(fts_inbound_target);
            b.HasKey(e => e.FtsInboundTargetId);

            b.Property(e => e.FtsInboundTargetId).HasColumnName(fts_inbound_target_id);
            b.Property(e => e.FTEMetadataId).HasColumnName(fte_metadata_id);
            b.Property(e => e.ImportJobName).HasColumnName(import_job_name).HasMaxLength(200);
            b.Property(e => e.ContractName).HasColumnName(contract_name).HasMaxLength(200);
            b.Property(e => e.OperatorId).HasColumnName(operator_id).HasMaxLength(200);
            b.Property(e => e.IsValid).HasColumnName(is_valid);
            b.Property(e => e.CreationDate).HasColumnName(created_ts);
            b.Property(e => e.LastModifiedDate).HasColumnName(last_modified_ts);
        });

        modelBuilder.Entity<FtsIntegrationFile>(b =>
        {
            b.ToTable(fts_integration_file);
            b.HasKey(e => e.FtsIntegrationFileId);

            b.Property(e => e.FtsIntegrationFileId).HasColumnName(fts_integration_file_id);
            b.Property(e => e.GroupId).HasColumnName(group_id);
            b.Property(e => e.ConfigId).HasColumnName(config_id);
            b.Property(e => e.FileName).HasColumnName(file_name).HasMaxLength(500);
            b.Property(e => e.ETFOutboundMetadataDefnId).HasColumnName(etf_outbound_metadata_defn_id);
            b.Property(e => e.IsProcessed).HasColumnName(is_processed);
            b.Property(e => e.OperatorId).HasColumnName(operator_id).HasMaxLength(200);
            b.Property(e => e.IsValid).HasColumnName(is_valid);
            b.Property(e => e.CreationDate).HasColumnName(created_ts);
            b.Property(e => e.LastModifiedDate).HasColumnName(last_modified_ts);
        });

        // Legacy parity: EF6 uses WillCascadeOnDelete(false) for all relationships.
        modelBuilder.Entity<FtpServer>()
            .HasMany<FtsSourceLocation>()
            .WithOne()
            .HasForeignKey(e => e.FtpServerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsDestinationLocations>()
            .HasMany(e => e.FtsFileDeliveryFK)
            .WithOne(e => e.FtsDestinationLocationsFK)
            .HasForeignKey(e => e.DestinationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsDestinationLocations>()
            .HasMany(e => e.FtsFileEventFK)
            .WithOne(e => e.FtsDestinationLocationsFK)
            .HasForeignKey(e => e.DestinationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsDestinationLocations>()
            .HasMany(e => e.FtsFileExceptionFK)
            .WithOne(e => e.FtsDestinationLocationsFK)
            .HasForeignKey(e => e.DestinationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsFileEvent>()
            .HasMany(e => e.FtsFileExceptionFK)
            .WithOne(e => e.FtsFileEventFK)
            .HasForeignKey(e => e.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsFileManagement>()
            .HasMany(e => e.FtsFileDeliveryFK)
            .WithOne(e => e.FtsFileManagementFK)
            .HasForeignKey(e => e.FileId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsFileManagement>()
            .HasMany(e => e.FtsFileEventFK)
            .WithOne(e => e.FtsFileManagementFK)
            .HasForeignKey(e => e.FileId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsFileManagement>()
            .HasMany(e => e.FtsFileExceptionFK)
            .WithOne(e => e.FtsFileManagementFK)
            .HasForeignKey(e => e.FileId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsSourceLocation>()
            .HasMany<FtsFileManagement>()
            .WithOne(e => e.FtsSourceLocationFK)
            .HasForeignKey(e => e.FileSourceLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Eventing.Outbox.OutboxMessage>(b =>
        {
            b.ToTable("outbox_messages");
            b.HasKey(x => x.Id);

            b.Property(e => e.Id).HasColumnName("id");
            b.Property(e => e.TenantId).HasColumnName("tenant_id").HasMaxLength(200);
            b.Property(e => e.EventName).HasColumnName("event_name").HasMaxLength(200);
            b.Property(e => e.EventVersion).HasColumnName("event_version").HasMaxLength(20);
            b.Property(e => e.EventId).HasColumnName("event_id");
            b.Property(e => e.OccurredAt).HasColumnName("occurred_at");
            b.Property(e => e.CorrelationId).HasColumnName("correlation_id").HasMaxLength(200);
            b.Property(e => e.TraceId).HasColumnName("trace_id").HasMaxLength(64);
            b.Property(e => e.Producer).HasColumnName("producer").HasMaxLength(200);
            b.Property(e => e.PayloadJson).HasColumnName("payload_json");
            b.Property(e => e.PublishedAt).HasColumnName("published_at");
        });
    }
}

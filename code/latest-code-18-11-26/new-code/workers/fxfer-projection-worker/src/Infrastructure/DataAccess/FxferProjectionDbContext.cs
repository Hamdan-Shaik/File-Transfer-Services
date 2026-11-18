using Fxfer.ProjectionWorker.Infrastructure.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.ProjectionWorker.Infrastructure.DataAccess;

public sealed class FxferProjectionDbContext : DbContext
{
    public FxferProjectionDbContext(DbContextOptions<FxferProjectionDbContext> options)
        : base(options)
    {
    }

    public DbSet<FtpServer> FtpServers => Set<FtpServer>();
    public DbSet<FtsSourceLocation> SourceLocations => Set<FtsSourceLocation>();
    public DbSet<FtsDestination> Destinations => Set<FtsDestination>();
    public DbSet<FtsSuspension> Suspensions => Set<FtsSuspension>();
    public DbSet<FtsInboundTarget> InboundTargets => Set<FtsInboundTarget>();
    public DbSet<FtsIntegrationFile> IntegrationFiles => Set<FtsIntegrationFile>();
    public DbSet<FtsFileManagement> FileManagementRecords => Set<FtsFileManagement>();
    public DbSet<FtsFileEvent> FileEvents => Set<FtsFileEvent>();
    public DbSet<FtsFileDelivery> FileDeliveries => Set<FtsFileDelivery>();
    public DbSet<FtsFileException> FileExceptions => Set<FtsFileException>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FtpServer>(entity =>
        {
            entity.ToTable("ftp_server");
            entity.HasKey(x => x.FtpServerId);
        });

        modelBuilder.Entity<FtsSourceLocation>(entity =>
        {
            entity.ToTable("fts_source_location");
            entity.HasKey(x => x.FtsSourceLocationId);
        });

        modelBuilder.Entity<FtsDestination>(entity =>
        {
            entity.ToTable("fts_destination");
            entity.HasKey(x => x.DestinationId);
        });

        modelBuilder.Entity<FtsSuspension>(entity =>
        {
            entity.ToTable("fts_suspension");
            entity.HasKey(x => x.SuspensionId);
        });

        modelBuilder.Entity<FtsInboundTarget>(entity =>
        {
            entity.ToTable("fts_inbound_target");
            entity.HasKey(x => x.FtsInboundTargetId);
        });

        modelBuilder.Entity<FtsIntegrationFile>(entity =>
        {
            entity.ToTable("fts_integration_file");
            entity.HasKey(x => x.FtsIntegrationFileId);
        });

        modelBuilder.Entity<FtsFileManagement>(entity =>
        {
            entity.ToTable("fts_file_management");
            entity.HasKey(x => x.FileId);
        });

        modelBuilder.Entity<FtsFileEvent>(entity =>
        {
            entity.ToTable("fts_file_event");
            entity.HasKey(x => x.EventId);
        });

        modelBuilder.Entity<FtsFileDelivery>(entity =>
        {
            entity.ToTable("fts_file_delivery");
            entity.HasKey(x => x.DeliveryId);
        });

        modelBuilder.Entity<FtsFileException>(entity =>
        {
            entity.ToTable("fts_file_exception");
            entity.HasKey(x => x.ExceptionId);
        });
    }
}


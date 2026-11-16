using FxferService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FxferService.Infrastructure.Persistence.Batch;

public sealed class FileTransferEntityDbContext : IDbContextBase, IDisposable
{
    private readonly FxferDbContext _dbContext;

    public FileTransferEntityDbContext(FxferDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public FileTransferEntityDbContext(IDbSession dbsession, bool isTransactionRequired)
    {
        _ = isTransactionRequired;
        _dbContext = dbsession.DbContext;
    }

    public DbSet<TEntity> Set<TEntity>() where TEntity : class => _dbContext.Set<TEntity>();

    public int SaveChanges() => _dbContext.SaveChanges();
    public void Dispose()
    {
    }


    public FileTransferEntityDbContext Create()
    {
        const string defaultConnectionName = "DefaultConnection";
        const string defaultConnectionProvider = "DefaultConnectionProvider";
        const string defaultConnectionString = "DefaultConnectionString";

        // Legacy behavior:
        // - prefer ConnectionStrings[DefaultConnection] (provider + connection string)
        // - else fall back to AppSettings[DefaultConnectionProvider/DefaultConnectionString]
        var csConnectionString = Environment.GetEnvironmentVariable($"ConnectionStrings__{defaultConnectionName}");
        var csProviderName = Environment.GetEnvironmentVariable($"ConnectionStrings__{defaultConnectionName}Provider");
        IDbSession session;

        if (csConnectionString != null)
        {
            session = DbSession.Create(csProviderName, csConnectionString);
        }
        else
        {
            session = DbSession.Create(
                Environment.GetEnvironmentVariable(defaultConnectionProvider),
                Environment.GetEnvironmentVariable(defaultConnectionString));
        }

        return new FileTransferEntityDbContext(session, true);
    }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        var schema = Environment.GetEnvironmentVariable("FileTransferEntityDbContext.SchemaName");

        modelBuilder.Entity<FtpServer>()
            .ToTable("FTP_SERVER", schema);

        modelBuilder.Entity<FtpServer>()
            .Property(e => e.FtpServerId);

        modelBuilder.Entity<FtsSourceLocationEntity>()
            .ToTable("FTS_SOURCE_LOCATION", schema);

        modelBuilder.Entity<FtsDestinationLocations>()
            .ToTable("FTS_DESTINATION", schema);

        modelBuilder.Entity<FtpServer>()
            .ToTable("FTP_SERVER", schema);

        modelBuilder.Entity<FtpServer>()
            .HasMany(e => e.FtsSourceLocation)
            .WithOne()
            .HasForeignKey(e => e.FtpServerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsFileDeliveryEntity>()
            .ToTable("FTS_FILE_DELIVERY", schema);

        modelBuilder.Entity<FtsFileEventEntity>()
            .ToTable("FTS_FILE_EVENT", schema);

        modelBuilder.Entity<FtsFileExceptionEntity>()
            .ToTable("FTS_FILE_EXCEPTION", schema);

        modelBuilder.Entity<FtsFileManagementEntity>()
            .ToTable("FTS_FILE_MANAGEMENT", schema);

        modelBuilder.Entity<FtsDestinationLocations>()
            .HasMany(e => e.FtsFileDeliveryFK)
            .WithOne()
            .HasForeignKey(e => e.DestinationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsDestinationLocations>()
            .HasMany(e => e.FtsFileEventFK)
            .WithOne()
            .HasForeignKey(e => e.DestinationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsFileManagementEntity>()
            .Property(e => e.FileStatus)
            .IsFixedLength();

        modelBuilder.Entity<FtsFileManagementEntity>()
            .HasMany<FtsFileDeliveryEntity>()
            .WithOne()
            .HasForeignKey(e => e.FileId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsFileManagementEntity>()
            .HasMany<FtsFileEventEntity>()
            .WithOne()
            .HasForeignKey(e => e.FileId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsFileManagementEntity>()
            .HasMany<FtsFileExceptionEntity>()
            .WithOne()
            .HasForeignKey(e => e.FileId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsSourceLocationEntity>()
            .HasMany<FtsFileManagementEntity>()
            .WithOne()
            .HasForeignKey(e => e.FtsSourceLocationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FtsSuspensionEntity>()
            .ToTable("FTS_SUSPENSION", schema);

        modelBuilder.Entity<FtsNotificationEntity>()
            .ToTable("FTS_NOTIFICATION", schema);
    }
}

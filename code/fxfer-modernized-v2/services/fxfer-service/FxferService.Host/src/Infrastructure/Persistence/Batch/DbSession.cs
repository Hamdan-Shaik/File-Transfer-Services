using System.Data;
using FxferService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FxferService.Infrastructure.Persistence.Batch;

public sealed class DbSession : IDbSession
{
    private readonly FxferDbContext _dbContext;

    private DbSession(FxferDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public FxferDbContext DbContext => _dbContext;

    public static IDbSession Create()
    {
        // ConfigurationManager/app.config is not used in the target service.
        // Connection strings flow via environment/.NET configuration (and may be tenant-specific).
        var connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__TenantDb")
            ?? Environment.GetEnvironmentVariable("FXFER_TENANTDB_CONNECTION_STRING");

        var options = new DbContextOptionsBuilder<FxferDbContext>();

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            options.UseNpgsql(connectionString);
        }
        else
        {
            options.UseInMemoryDatabase($"fxfer_{Guid.NewGuid():N}");
        }

        return new DbSession(new FxferDbContext(options.Options));
    }

    public static IDbSession Create(string? providerName, string? connectionString)
    {
        _ = providerName;
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return Create();
        }

        var options = new DbContextOptionsBuilder<FxferDbContext>();
        options.UseNpgsql(connectionString);
        return new DbSession(new FxferDbContext(options.Options));
    }

    public IDbTransaction BeginTransaction()
    {
        var transaction = _dbContext.Database.BeginTransaction();
        return new CommittingDbTransaction(transaction);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    private sealed class CommittingDbTransaction : IDbTransaction
    {
        private readonly IDbContextTransaction _transaction;
        private bool _committed;

        public CommittingDbTransaction(IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public IDbConnection Connection => _transaction.GetDbTransaction().Connection!;

        public IsolationLevel IsolationLevel => _transaction.GetDbTransaction().IsolationLevel;

        public void Commit()
        {
            _transaction.Commit();
            _committed = true;
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _committed = true;
        }

        public void Dispose()
        {
            if (!_committed)
            {
                // Legacy code scopes a transaction but never explicitly commits it.
                // Preserve the observed behavior by committing on dispose.
                _transaction.Commit();
            }

            _transaction.Dispose();
        }
    }
}



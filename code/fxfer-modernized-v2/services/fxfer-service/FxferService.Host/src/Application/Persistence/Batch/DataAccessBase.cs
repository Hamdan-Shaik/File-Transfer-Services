using System.Data;
using FxferService.Infrastructure.Persistence.Batch;

namespace FxferService.Application.Persistence.Batch;

/// <summary>
/// Abstracts the DataAccess logic. Creates DbContext and executes the query using the DbContext.
/// </summary>
public abstract class DataAccessBase<TdacOutput> : IDataAccess<TdacOutput>
{
    public TdacOutput Execute()
    {
        TdacOutput output;

        using (IDbSession dbsession = CreateDbSession())
        {
            using (IDbTransaction transaction = dbsession.BeginTransaction())
            {
                _ = transaction;
                IDbContextBase dbcontext = CreateDbContext(dbsession);
                output = ExecuteQuery(dbcontext);
            }
        }

        return output;
    }

    public TdacOutput ExecuteWithNoTransaction()
    {
        TdacOutput output;

        using (IDbSession dbsession = CreateDbSession())
        {
            IDbContextBase dbcontext = CreateDbContext(dbsession);
            output = ExecuteQuery(dbcontext);
        }

        return output;
    }

    public virtual IDbSession CreateDbSession()
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

        return session;
    }

    public virtual IDbContextBase CreateDbContext(IDbSession dbsession)
    {
        return new FileTransferEntityDbContext(dbsession, true);
    }

    protected abstract TdacOutput ExecuteQuery(IDbContextBase dbcontext);
}


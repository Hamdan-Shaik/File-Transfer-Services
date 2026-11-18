using Fxfer.ProjectionWorker.Worker.Configuration;
using Fxfer.ProjectionWorker.Worker.Processing;
using Microsoft.Extensions.Options;

namespace Fxfer.ProjectionWorker.Worker.Services;

public sealed class TenantConnectionStringProvider
{
    private readonly IOptions<TenantDatabaseOptions> _options;
    private readonly ProcessingContextAccessor _contextAccessor;

    public TenantConnectionStringProvider(IOptions<TenantDatabaseOptions> options, ProcessingContextAccessor contextAccessor)
    {
        _options = options;
        _contextAccessor = contextAccessor;
    }

    public string? TryGetConnectionString()
    {
        // skeleton: tenant-scoped credentials are resolved from a secret store later.
        // For now, allow a single default connection string for local development.
        _ = _contextAccessor.Current?.TenantId;
        return _options.Value.DefaultConnectionString;
    }
}


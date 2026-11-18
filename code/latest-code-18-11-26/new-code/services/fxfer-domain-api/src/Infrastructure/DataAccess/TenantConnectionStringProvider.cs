using Fxfer.DomainApi.Contracts.Utilities;

namespace Fxfer.DomainApi.Infrastructure.DataAccess;

public sealed class TenantConnectionStringProvider
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantConnectionStringProvider(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public string? TryGetConnectionString()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var tenantId = httpContext is null
            ? null
            : httpContext.Items.TryGetValue(ContextKeys.TenantIdItemKey, out var raw)
                ? raw as string
                : null;

        if (string.IsNullOrWhiteSpace(tenantId))
        {
            return null;
        }

        return _configuration[$"Tenants:{tenantId}:ConnectionString"];
    }
}

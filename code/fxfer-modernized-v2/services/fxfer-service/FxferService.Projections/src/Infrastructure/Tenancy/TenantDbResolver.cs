namespace FxferService.Projections.Infrastructure.Tenancy;

public sealed class RequestContext
{
    public string? TenantId { get; set; }
    public string? CorrelationId { get; set; }
}

public interface ITenantDbResolver
{
    string? ResolveConnectionString();
}

public sealed class TenantDbResolver : ITenantDbResolver
{
    private readonly IConfiguration _configuration;
    private readonly RequestContext _requestContext;

    public TenantDbResolver(IConfiguration configuration, RequestContext requestContext)
    {
        _configuration = configuration;
        _requestContext = requestContext;
    }

    public string? ResolveConnectionString()
    {
        var tenantId = _requestContext.TenantId;
        if (!string.IsNullOrWhiteSpace(tenantId))
        {
            var perTenant = _configuration[$"Tenancy:Tenants:{tenantId}:ConnectionString"];
            if (!string.IsNullOrWhiteSpace(perTenant))
            {
                return perTenant;
            }
        }

        var fallback = _configuration.GetConnectionString("TenantDb");
        return string.IsNullOrWhiteSpace(fallback) ? null : fallback;
    }
}

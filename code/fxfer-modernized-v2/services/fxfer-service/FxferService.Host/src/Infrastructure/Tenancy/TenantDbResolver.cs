namespace FxferService.Infrastructure.Tenancy;

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
        if (string.IsNullOrWhiteSpace(tenantId))
        {
            return null;
        }

        var perTenant = _configuration[$"Tenancy:Tenants:{tenantId}:ConnectionString"];
        return string.IsNullOrWhiteSpace(perTenant) ? null : perTenant;
    }
}

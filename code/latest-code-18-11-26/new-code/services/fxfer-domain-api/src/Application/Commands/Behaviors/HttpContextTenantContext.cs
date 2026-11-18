using Fxfer.DomainApi.Contracts.Utilities;

namespace Fxfer.DomainApi.Application.Commands.Behaviors;

public sealed class HttpContextTenantContext : ITenantContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextTenantContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? TenantId
    {
        get
        {
            var context = _httpContextAccessor.HttpContext;
            return context is null
                ? null
                : context.Items.TryGetValue(ContextKeys.TenantIdItemKey, out var raw)
                    ? raw as string
                    : null;
        }
    }
}


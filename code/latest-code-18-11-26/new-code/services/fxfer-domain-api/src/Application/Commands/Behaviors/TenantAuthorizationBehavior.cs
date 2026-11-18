using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Fxfer.DomainApi.Application.Commands.Behaviors;

public sealed class TenantAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ITenantContext _tenantContext;
    private readonly IAuthorizationService _authorizationService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantAuthorizationBehavior(
        ITenantContext tenantContext,
        IAuthorizationService authorizationService,
        IHttpContextAccessor httpContextAccessor)
    {
        _tenantContext = tenantContext;
        _authorizationService = authorizationService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not FileTransferCommand)
        {
            return await next();
        }

        // T-003: Enforce tenant-scoped authorization and invariants at the domain boundary.
        if (string.IsNullOrWhiteSpace(_tenantContext.TenantId))
        {
            throw new FxferAuthorizationException("Missing tenant context");
        }

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            throw new FxferAuthorizationException("Missing request context");
        }

        var principal = httpContext.User;
        if (principal.Identity?.IsAuthenticated != true)
        {
            throw new FxferAuthorizationException("Unauthorized");
        }

        // Defense-in-depth: re-evaluate endpoint authorization metadata within the service boundary.
        var endpoint = httpContext.GetEndpoint();
        if (endpoint is not null && endpoint.Metadata.GetMetadata<IAllowAnonymous>() is null)
        {
            var authorizeData = endpoint.Metadata.GetOrderedMetadata<IAuthorizeData>();
            foreach (var item in authorizeData)
            {
                if (!string.IsNullOrWhiteSpace(item.Roles))
                {
                    var allowedRoles = item.Roles.Split(',').Select(role => role.Trim()).Where(role => role.Length > 0);
                    if (!allowedRoles.Any(principal.IsInRole))
                    {
                        throw new FxferAuthorizationException("Forbidden");
                    }
                }

                if (!string.IsNullOrWhiteSpace(item.Policy))
                {
                    var result = await _authorizationService.AuthorizeAsync(principal, resource: null, item.Policy);
                    if (!result.Succeeded)
                    {
                        throw new FxferAuthorizationException("Forbidden");
                    }
                }
            }

            if (authorizeData.Count == 0)
            {
                var defaultCheck = await _authorizationService.AuthorizeAsync(principal, resource: null, new[] { new DenyAnonymousAuthorizationRequirement() });
                if (!defaultCheck.Succeeded)
                {
                    throw new FxferAuthorizationException("Forbidden");
                }
            }
        }

        return await next();
    }
}


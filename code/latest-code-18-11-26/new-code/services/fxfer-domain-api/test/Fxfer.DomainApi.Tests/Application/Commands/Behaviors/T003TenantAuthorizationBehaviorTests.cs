using System.Security.Claims;
using Fxfer.DomainApi.Application.Commands.Behaviors;
using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Fxfer.DomainApi.Tests.Application.Commands.Behaviors;

public sealed class T003TenantAuthorizationBehaviorTests
{
    [Fact]
    public async Task T_003_command_execution_requires_tenant_context()
    {
        var httpContext = CreateHttpContext(user: CreateAuthenticatedUser());
        var behavior = CreateBehavior(httpContext, tenantId: null, authorizationService: new AllowAuthorizationService());

        await Assert.ThrowsAsync<FxferAuthorizationException>(() =>
            behavior.Handle(new TestCommand(), Next("ok"), CancellationToken.None));
    }

    [Fact]
    public async Task T_003_command_execution_is_denied_when_endpoint_authorization_fails()
    {
        var httpContext = CreateHttpContext(user: CreateAuthenticatedUser(), policy: "policy");
        var behavior = CreateBehavior(httpContext, tenantId: "tenant", authorizationService: new DenyAuthorizationService());

        await Assert.ThrowsAsync<FxferAuthorizationException>(() =>
            behavior.Handle(new TestCommand(), Next("ok"), CancellationToken.None));
    }

    [Fact]
    public async Task T_003_command_execution_continues_when_authorized_and_tenant_scoped()
    {
        var httpContext = CreateHttpContext(user: CreateAuthenticatedUser(), policy: "policy");
        var behavior = CreateBehavior(httpContext, tenantId: "tenant", authorizationService: new AllowAuthorizationService());

        var result = await behavior.Handle(new TestCommand(), Next("ok"), CancellationToken.None);
        Assert.Equal("ok", result);
    }

    private static TenantAuthorizationBehavior<IRequest<string>, string> CreateBehavior(
        HttpContext httpContext,
        string? tenantId,
        IAuthorizationService authorizationService)
    {
        var tenantContext = new TestTenantContext(tenantId);
        var httpContextAccessor = new HttpContextAccessor { HttpContext = httpContext };
        return new TenantAuthorizationBehavior<IRequest<string>, string>(tenantContext, authorizationService, httpContextAccessor);
    }

    private static HttpContext CreateHttpContext(ClaimsPrincipal user, string? policy = null)
    {
        var context = new DefaultHttpContext
        {
            User = user
        };

        if (!string.IsNullOrWhiteSpace(policy))
        {
            var endpoint = new Endpoint(
                _ => Task.CompletedTask,
                new EndpointMetadataCollection(new Microsoft.AspNetCore.Authorization.AuthorizeAttribute(policy)),
                "test");
            context.SetEndpoint(endpoint);
        }

        return context;
    }

    private static ClaimsPrincipal CreateAuthenticatedUser()
    {
        var identity = new ClaimsIdentity(new[] { new Claim("sub", "user") }, authenticationType: "Test");
        return new ClaimsPrincipal(identity);
    }

    private static RequestHandlerDelegate<string> Next(string value) => () => Task.FromResult(value);

    private sealed record TestTenantContext(string? TenantId) : ITenantContext;

    private sealed class TestCommand : FileTransferCommand, IRequest<string>
    {
        public override void SetCommandValidations()
        {
        }
    }

    private sealed class AllowAuthorizationService : IAuthorizationService
    {
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            return Task.FromResult(AuthorizationResult.Success());
        }

        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
        {
            return Task.FromResult(AuthorizationResult.Success());
        }
    }

    private sealed class DenyAuthorizationService : IAuthorizationService
    {
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            return Task.FromResult(AuthorizationResult.Failed());
        }

        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
        {
            return Task.FromResult(AuthorizationResult.Failed());
        }
    }
}


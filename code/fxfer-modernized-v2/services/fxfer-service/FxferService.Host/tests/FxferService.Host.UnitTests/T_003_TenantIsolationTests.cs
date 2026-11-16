using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using FxferService.Host.Composition;
using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Persistence.Entities;
using FxferService.Infrastructure.Tenancy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace FxferService.Host.UnitTests;

public sealed class T_003_TenantIsolationTests
{
    [Fact]
    public async Task T_003_routes_inmemory_database_per_tenant_preventing_cross_tenant_reads()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Tenancy:UseInMemoryPerTenant"] = "true",
            })
            .Build();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddBasFileTransferApiHost(configuration);

        await using var serviceProvider = services.BuildServiceProvider();

        var ftpServerId = System.Guid.NewGuid();

        await using (var scope = serviceProvider.CreateAsyncScope())
        {
            var requestContext = scope.ServiceProvider.GetRequiredService<RequestContext>();
            requestContext.TenantId = "tenant-a";

            var db = scope.ServiceProvider.GetRequiredService<FxferDbContext>();
            db.FtpServers.Add(new FtpServer
            {
                FtpServerId = ftpServerId,
                FtpServerName = "s1",
                FtpServerIp = "127.0.0.1",
                FtpTransferMode = "P",
                FtpUser = "u",
                FtpPassword = "p",
                FtpDescription = "d",
                FtpPort = (short)21,
                IsValid = true,
                OperatorId = "op",
                LastModifiedDate = System.DateTime.UtcNow,
                CreationDate = System.DateTime.UtcNow,
                UseSSHAuth = false,
            });
            await db.SaveChangesAsync();
        }

        await using (var scope = serviceProvider.CreateAsyncScope())
        {
            var requestContext = scope.ServiceProvider.GetRequiredService<RequestContext>();
            requestContext.TenantId = "tenant-a";

            var db = scope.ServiceProvider.GetRequiredService<FxferDbContext>();
            Assert.Single(db.FtpServers);
        }

        await using (var scope = serviceProvider.CreateAsyncScope())
        {
            var requestContext = scope.ServiceProvider.GetRequiredService<RequestContext>();
            requestContext.TenantId = "tenant-b";

            var db = scope.ServiceProvider.GetRequiredService<FxferDbContext>();
            Assert.Empty(db.FtpServers);
        }
    }

    [Fact]
    public async Task T_003_extracts_tenant_id_from_authenticated_jwt_claim()
    {
        var middleware = new RequestContextMiddleware(_ => Task.CompletedTask, NullLogger<RequestContextMiddleware>.Instance);

        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("tenant_id", "t1"),
                new Claim("sub", "user"),
            }, authenticationType: "jwt")),
        };

        var requestContext = new RequestContext();
        await middleware.Invoke(httpContext, requestContext);

        Assert.Equal("t1", requestContext.TenantId);
    }

    [Fact]
    public async Task T_003_returns_400_when_authenticated_but_tenant_id_claim_missing()
    {
        var nextCalled = false;
        var middleware = new RequestContextMiddleware(_ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        }, NullLogger<RequestContextMiddleware>.Instance);

        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("sub", "user"),
            }, authenticationType: "jwt")),
        };

        httpContext.Response.Body = new MemoryStream();

        var requestContext = new RequestContext();
        await middleware.Invoke(httpContext, requestContext);

        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status400BadRequest, httpContext.Response.StatusCode);

        httpContext.Response.Body.Position = 0;
        var body = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
        Assert.Contains("TENANT_MISSING", body);
    }
}
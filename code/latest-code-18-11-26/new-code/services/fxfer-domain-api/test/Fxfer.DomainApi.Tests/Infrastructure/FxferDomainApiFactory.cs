using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Fxfer.DomainApi.Infrastructure.DataAccess;

namespace Fxfer.DomainApi.Tests.Infrastructure;

internal sealed class FxferDomainApiFactory : WebApplicationFactory<global::Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var databaseName = Guid.NewGuid().ToString();

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Tenants:tenant-test:ConnectionString"] = "Host=localhost;Database=fxfer_tenant_test;Username=fxfer;Password=fxfer"
            });
        });

        builder.ConfigureServices(services =>
        {
            // Use EF InMemory for API-level integration tests.
            services.RemoveAll<DbContextOptions<FxferDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<FxferDbContext>>();
            services.RemoveAll<FxferDbContext>();
            services.AddDbContext<FxferDbContext>(options => options.UseInMemoryDatabase(databaseName));

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                    options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });
        });
    }
}


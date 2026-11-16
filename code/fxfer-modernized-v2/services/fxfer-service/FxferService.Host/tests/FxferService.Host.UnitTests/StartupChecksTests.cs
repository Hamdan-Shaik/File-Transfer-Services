using System.Collections.Generic;
using FxferService.Host.Hosting;
using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FxferService.Host.UnitTests;

public sealed class StartupChecksTests
{
    [Fact]
    public async Task RunFileTransferSvcMainStartupChecks_logs_warning_and_continues_on_exception()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Tenancy:Tenants:t1:ConnectionString"] = "bad",
            })
            .Build();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IConfiguration>(configuration);

        services.AddScoped<RequestContext>();
        services.AddScoped<ITenantDbResolver, TenantDbResolver>();

        services.AddDbContext<FxferDbContext>((sp, options) =>
        {
            var resolver = sp.GetRequiredService<ITenantDbResolver>();
            var connectionString = resolver.ResolveConnectionString();
            options.UseNpgsql(connectionString!);
        });

        using var serviceProvider = services.BuildServiceProvider();
        var logger = new ListLogger();

        await StartupChecks.RunFileTransferSvcMainStartupChecks(serviceProvider, configuration, logger);

        Assert.Contains(logger.Messages, m => m == "Service startup partially succeeded");
    }

    private sealed class ListLogger : ILogger
    {
        public List<string> Messages { get; } = new();

        public IDisposable BeginScope<TState>(TState state) where TState : notnull => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            _ = logLevel;
            _ = eventId;
            Messages.Add(formatter(state, exception));
        }

        private sealed class NullScope : IDisposable
        {
            public static readonly NullScope Instance = new();

            public void Dispose()
            {
            }
        }
    }
}


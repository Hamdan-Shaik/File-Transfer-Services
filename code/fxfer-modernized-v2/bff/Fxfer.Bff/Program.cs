using System.Diagnostics;
using Fxfer.Bff.Host.Composition;
using Fxfer.Bff.Host.Hosting;
using Microsoft.Extensions.Logging;

namespace Fxfer.Bff;

public partial class Program
{
    private const string ServiceName = "fxfer-bff";

    public static async Task Main(string[] args)
    {
        using var bootstrapLoggerFactory = LoggerFactory.Create(b => b.AddSimpleConsole());
        var bootstrapLogger = bootstrapLoggerFactory.CreateLogger(ServiceName);

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddFxferBff(builder.Configuration, builder.Environment);

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; // 10 MB
            });

            var app = builder.Build();

            var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger(ServiceName);
            logger.LogInformation("Service starting (pid={pid})", Process.GetCurrentProcess().Id);

            await StartupChecks.RunFxferBffStartupChecks(
                services: app.Services,
                configuration: app.Configuration,
                logger: logger,
                cancellationToken: app.Lifetime.ApplicationStopping);

            app.UseFxferBff();

            app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live"),
            });

            app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("ready"),
            });

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            bootstrapLogger.LogError(ex, "Service host initialization failed");
            throw;
        }
    }
}

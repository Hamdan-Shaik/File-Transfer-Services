using System.Diagnostics;
using FxferService.Application.Hosting;
using FxferService.Host.Composition;
using FxferService.Host.Hosting;
using Microsoft.Extensions.Logging;

namespace FxferService.Host;

public partial class Program
{
    private const string LegacyRegisteredServiceType = "Main";

    public static async Task Main(string[] args)
    {
        var loggerName = typeof(Program).Namespace ?? nameof(Program);

        using var bootstrapLoggerFactory = LoggerFactory.Create(b => b.AddSimpleConsole());
        var bootstrapLogger = bootstrapLoggerFactory.CreateLogger(loggerName);

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            // drop: Service Fabric WCF listeners (legacy Main.CreateServiceInstanceListeners). ASP.NET Core endpoints are owned by API components.
            builder.Services.AddFileTransferSvcMainHost(builder.Configuration);
            builder.Services.AddBasFileTransferApiHost(builder.Configuration);

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; // 10 MB
            });

            var app = builder.Build();

            var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger(loggerName);

            await StartupChecks.RunFileTransferSvcMainStartupChecks(
                app.Services,
                app.Configuration,
                logger,
                app.Lifetime.ApplicationStopping);

            var serviceEventSource = new ServiceEventSource(logger);

            // drop: Service Fabric service type registration (legacy: ServiceRuntime.RegisterServiceAsync(HPP.FileTransfer.BAS.FileTransferSvc.MainType, ...)).
            // Legacy: ServiceEventSource.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(Main).Name)
            serviceEventSource.ServiceTypeRegistered(
                Process.GetCurrentProcess().Id,
                LegacyRegisteredServiceType);

            app.UseBasFileTransferApiHost();

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
            // Legacy: ServiceEventSource.ServiceHostInitializationFailed(e.ToString())
            new ServiceEventSource(bootstrapLogger).ServiceHostInitializationFailed(ex.ToString());
            throw;
        }
    }
}


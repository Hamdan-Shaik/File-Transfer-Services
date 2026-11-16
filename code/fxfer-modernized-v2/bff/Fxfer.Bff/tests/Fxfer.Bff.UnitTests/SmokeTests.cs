using System;
using Fxfer.Bff.Host.Composition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Fxfer.Bff.UnitTests;

public sealed class SmokeTests
{
    [Fact]
    public void DependencyInjection_Composes()
    {
        var config = new ConfigurationBuilder().AddInMemoryCollection().Build();
        var env = new HostEnvironment { EnvironmentName = Environments.Development };

        var services = new ServiceCollection();
        services.AddFxferBff(config, env);

        using var provider = services.BuildServiceProvider();
        Assert.NotNull(provider.GetRequiredService<Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckService>());
    }

    private sealed class HostEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = Environments.Development;
        public string ApplicationName { get; set; } = "Fxfer.Bff";
        public string ContentRootPath { get; set; } = AppContext.BaseDirectory;
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}


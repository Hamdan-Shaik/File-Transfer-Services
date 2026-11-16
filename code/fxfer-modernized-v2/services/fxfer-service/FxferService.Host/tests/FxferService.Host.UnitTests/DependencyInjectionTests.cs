using System.Collections.Generic;
using System.Reflection;
using FxferService.Application.LegacyContracts;
using FxferService.Application.Profiles;
using FxferService.Host.Composition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter;
using Xunit;

namespace FxferService.Host.UnitTests;

public sealed class DependencyInjectionTests
{
    [Fact]
    public void AddFileTransferSvcMainHost_registers_profiles_and_sets_instrumentation_key_header()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["InstrumentationKey"] = "abc",
            })
            .Build();

        var services = new ServiceCollection();
        services.AddFileTransferSvcMainHost(configuration);

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<OtlpExporterOptions>>().Value;

        Assert.Contains("InstrumentationKey=abc", options.Headers);

        var registryType = typeof(Profile).Assembly.GetType(
            "FxferService.Application.LegacyContracts.ProfilesRegistry",
            throwOnError: true);

        var assembliesField = registryType!.GetField("_assemblies", BindingFlags.NonPublic | BindingFlags.Static);
        var assemblies = (HashSet<Assembly>)assembliesField!.GetValue(null)!;

        Assert.Contains(typeof(InboundTargetProfile).Assembly, assemblies);
        Assert.Contains(typeof(IntegrationFileProfile).Assembly, assemblies);
    }
}


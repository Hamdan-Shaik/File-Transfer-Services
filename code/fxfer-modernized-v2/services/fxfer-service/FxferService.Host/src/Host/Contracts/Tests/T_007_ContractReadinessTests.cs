using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using FxferService.Host.Composition;
using Microsoft.OpenApi.Writers;
using Swashbuckle.AspNetCore.Swagger;
using Xunit;

namespace FxferService.Tests;

public sealed class T_007_ContractReadinessTests
{
    private const string OpenApiDeployOutputPath = "contracts/openapi/fxfer-service.openapi.json";
    private const string OpenApiDocsOutputPath = "docs/design/seam-contracts/http/fxfer-service.openapi.json";

    private const string AsyncApiDeployOutputPath = "contracts/asyncapi/fxfer-service.asyncapi.yaml";
    private const string AsyncApiDocsOutputPath = "docs/design/seam-contracts/events/fxfer-service.asyncapi.yaml";

    private static string ReadOutputFile(string relativePath)
    {
        var path = Path.Combine(AppContext.BaseDirectory, relativePath);
        return File.ReadAllText(path, Encoding.UTF8);
    }

    private static string GenerateRuntimeSwaggerJson()
    {
        var options = new WebApplicationOptions
        {
            EnvironmentName = Environments.Production,
            ApplicationName = typeof(FxferService.Host.Program).Assembly.FullName,
        };

        var builder = WebApplication.CreateBuilder(options);
        builder.Services.AddFileTransferSvcMainHost(builder.Configuration);
        builder.Services.AddBasFileTransferApiHost(builder.Configuration);

        var app = builder.Build();
        var provider = app.Services.GetRequiredService<ISwaggerProvider>();
        var document = provider.GetSwagger("v1");

        using var stringWriter = new StringWriter();
        var writer = new OpenApiJsonWriter(stringWriter);
        document.SerializeAsV3(writer);
        writer.Flush();

        return stringWriter.ToString();
    }

    private static IReadOnlySet<string> GetOpenApiOperationKeys(string openApiJson)
    {
        using var doc = JsonDocument.Parse(openApiJson);

        var paths = doc.RootElement.GetProperty("paths");
        var keys = new HashSet<string>(StringComparer.Ordinal);

        foreach (var pathProperty in paths.EnumerateObject())
        {
            foreach (var methodProperty in pathProperty.Value.EnumerateObject())
            {
                keys.Add($"{methodProperty.Name}:{pathProperty.Name}");
            }
        }

        return keys;
    }

    private static IReadOnlySet<string> GetOpenApiSchemaKeys(string openApiJson)
    {
        using var doc = JsonDocument.Parse(openApiJson);

        if (!doc.RootElement.TryGetProperty("components", out var components))
        {
            return new HashSet<string>(StringComparer.Ordinal);
        }

        if (!components.TryGetProperty("schemas", out var schemas))
        {
            return new HashSet<string>(StringComparer.Ordinal);
        }

        return schemas.EnumerateObject().Select(p => p.Name).ToHashSet(StringComparer.Ordinal);
    }

    [Fact]
    public void T_007_every_http_seam_has_an_openapi_contract_and_it_matches_the_runtime_surface()
    {
        // T-007: Every HTTP seam is specified as an OpenAPI contract and diff-gated before deploy.
        var runtimeSwagger = GenerateRuntimeSwaggerJson();

        var deployContract = ReadOutputFile(OpenApiDeployOutputPath);
        var docsContract = ReadOutputFile(OpenApiDocsOutputPath);

        var swaggerOperations = GetOpenApiOperationKeys(runtimeSwagger);
        var deployOperations = GetOpenApiOperationKeys(deployContract);
        var docsOperations = GetOpenApiOperationKeys(docsContract);

        Assert.Equal(swaggerOperations.OrderBy(x => x), deployOperations.OrderBy(x => x));
        Assert.Equal(deployOperations.OrderBy(x => x), docsOperations.OrderBy(x => x));

        Assert.Contains("get:/api/v1/ftp-servers", deployOperations);
        Assert.Contains("get:/api/v1/transfer-configurations", deployOperations);
        Assert.Contains("get:/api/v1/integration-files", deployOperations);
        Assert.Contains("get:/api/v1/inbound-targets/search", deployOperations);
        Assert.Contains("post:/api/v1/file-transfers/search", deployOperations);
    }

    [Fact]
    public void T_007_every_published_event_family_has_an_asyncapi_contract_with_consumers_declared()
    {
        // T-007: Every published event family is specified as an AsyncAPI contract.
        var deploy = ReadOutputFile(AsyncApiDeployOutputPath);
        var docs = ReadOutputFile(AsyncApiDocsOutputPath);

        Assert.Equal(deploy.Replace("\r\n", "\n"), docs.Replace("\r\n", "\n"));

        Assert.Contains("asyncapi: 3.0.0", deploy);
        Assert.Contains("channels:", deploy);
        Assert.Contains("fxfer.configuration-changed", deploy);
        Assert.Contains("fxfer.run-state-changed", deploy);
        Assert.Contains("fxfer.delivery-outcome-recorded", deploy);
        Assert.Contains("fxfer.suspension-state-changed", deploy);

        Assert.Contains("operations:", deploy);
        Assert.Contains("x-producer: fxfer-service", deploy);
        Assert.Contains("x-consumers: [fxfer-projections]", deploy);
    }

    [Fact]
    public void T_007_contract_changes_are_compatibility_gated_when_a_baseline_is_available()
    {
        var repoRoot = FindRepoRoot();
        var baseRef = Environment.GetEnvironmentVariable("CONTRACT_BASE_REF") ?? "origin/main";

        var openApiRelative = "services/fxfer-service/FxferService.Host/deploy/contracts/openapi/fxfer-service.openapi.json";
        var asyncApiRelative = "services/fxfer-service/FxferService.Host/deploy/contracts/asyncapi/fxfer-service.asyncapi.yaml";

        var baselineOpenApi = TryGitShow(repoRoot, baseRef, openApiRelative);
        if (baselineOpenApi is not null)
        {
            var currentOpenApi = File.ReadAllText(Path.Combine(repoRoot, openApiRelative), Encoding.UTF8);
            AssertOpenApiBackwardCompatible(baselineOpenApi, currentOpenApi);
        }

        var baselineAsyncApi = TryGitShow(repoRoot, baseRef, asyncApiRelative);
        if (baselineAsyncApi is not null)
        {
            var currentAsyncApi = File.ReadAllText(Path.Combine(repoRoot, asyncApiRelative), Encoding.UTF8);
            AssertNoAsyncApiChannelRemovals(baselineAsyncApi, currentAsyncApi);
        }
    }

    private static void AssertOpenApiBackwardCompatible(string baselineJson, string currentJson)
    {
        var baselineOps = GetOpenApiOperationKeys(baselineJson);
        var currentOps = GetOpenApiOperationKeys(currentJson);

        var removed = baselineOps.Except(currentOps).OrderBy(x => x).ToArray();
        Assert.True(removed.Length == 0, $"OpenAPI breaking change (operations removed): {string.Join(", ", removed)}");

        var baselineSchemas = GetOpenApiSchemaKeys(baselineJson);
        var currentSchemas = GetOpenApiSchemaKeys(currentJson);

        var removedSchemas = baselineSchemas.Except(currentSchemas).OrderBy(x => x).ToArray();
        Assert.True(removedSchemas.Length == 0, $"OpenAPI breaking change (schemas removed): {string.Join(", ", removedSchemas)}");
    }

    private static void AssertNoAsyncApiChannelRemovals(string baselineYaml, string currentYaml)
    {
        var baselineChannels = ExtractAsyncApiChannels(baselineYaml);
        var currentChannels = ExtractAsyncApiChannels(currentYaml);

        var removed = baselineChannels.Except(currentChannels).OrderBy(x => x).ToArray();
        Assert.True(removed.Length == 0, $"AsyncAPI breaking change (channels removed): {string.Join(", ", removed)}");
    }

    private static IReadOnlySet<string> ExtractAsyncApiChannels(string yaml)
    {
        var lines = yaml.Replace("\r\n", "\n").Split('\n');
        var inChannels = false;
        var channelNames = new HashSet<string>(StringComparer.Ordinal);

        var channelLine = new Regex("^  (?<name>[A-Za-z0-9_.-]+):\\s*$", RegexOptions.Compiled);

        foreach (var line in lines)
        {
            if (!inChannels)
            {
                if (line.Trim() == "channels:")
                {
                    inChannels = true;
                }

                continue;
            }

            if (line.Length > 0 && !line.StartsWith("  "))
            {
                break;
            }

            var match = channelLine.Match(line);
            if (match.Success)
            {
                channelNames.Add(match.Groups["name"].Value);
            }
        }

        return channelNames;
    }

    private static string? TryGitShow(string repoRoot, string gitRef, string relativePath)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "git",
            WorkingDirectory = repoRoot,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };

        psi.ArgumentList.Add("show");
        psi.ArgumentList.Add($"{gitRef}:{relativePath.Replace('\\', '/')}");

        using var process = Process.Start(psi);
        Assert.NotNull(process);

        var output = process!.StandardOutput.ReadToEnd();
        process.WaitForExit(10_000);

        return process.ExitCode == 0 ? output : null;
    }

    private static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (Directory.Exists(Path.Combine(dir.FullName, ".git")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new InvalidOperationException("Unable to locate git repo root from test base directory");
    }
}

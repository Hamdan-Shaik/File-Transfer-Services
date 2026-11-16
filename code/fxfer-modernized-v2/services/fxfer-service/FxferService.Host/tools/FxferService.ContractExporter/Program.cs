using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using FxferService.Host.Composition;
using Microsoft.OpenApi.Writers;
using Swashbuckle.AspNetCore.Swagger;

namespace FxferService.ContractExporter;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var outputRoot = args.Length > 0 ? args[0] : "../..";
        var deployPath = Path.GetFullPath(Path.Combine(outputRoot, "deploy/contracts/openapi/fxfer-service.openapi.json"));
        var docsPath = Path.GetFullPath(Path.Combine(outputRoot, "docs/design/seam-contracts/http/fxfer-service.openapi.json"));

        Directory.CreateDirectory(Path.GetDirectoryName(deployPath)!);
        Directory.CreateDirectory(Path.GetDirectoryName(docsPath)!);

        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = Environments.Production,
            ApplicationName = typeof(FxferService.Host.Program).Assembly.FullName,
        });
        builder.Services.AddFileTransferSvcMainHost(builder.Configuration);
        builder.Services.AddBasFileTransferApiHost(builder.Configuration);

        var app = builder.Build();
        var provider = app.Services.GetRequiredService<ISwaggerProvider>();
        var document = provider.GetSwagger("v1");

        using var stringWriter = new StringWriter();
        var writer = new OpenApiJsonWriter(stringWriter);
        document.SerializeAsV3(writer);
        writer.Flush();

        var swaggerJson = stringWriter.ToString();

        var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
        await File.WriteAllTextAsync(deployPath, swaggerJson, encoding);
        await File.WriteAllTextAsync(docsPath, swaggerJson, encoding);

        Console.WriteLine($"Wrote OpenAPI contract to: {deployPath}");
        Console.WriteLine($"Wrote OpenAPI contract to: {docsPath}");

        return 0;
    }
}


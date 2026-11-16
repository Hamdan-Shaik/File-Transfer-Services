using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace FxferService.Host.ContractTests;

public sealed class ContractSmokeTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ContractSmokeTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder => builder.UseEnvironment("Development"));
    }

    [Fact]
    public async Task OpenApi_ExposesExpectedRoutes()
    {
        using var client = _factory.CreateClient();
        var swagger = await client.GetStringAsync("/swagger/v1/swagger.json");

        Assert.Contains("/api/v1/ftp-servers", swagger);
        Assert.Contains("/api/v1/transfer-configurations", swagger);
        Assert.Contains("/api/v1/integration-files", swagger);
        Assert.Contains("/api/v1/inbound-targets/search", swagger);
        Assert.Contains("/api/v1/file-transfers/search", swagger);
    }

    [Fact]
    public void AsyncApi_FileExistsAndLooksValid()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "fxfer-service.asyncapi.yaml");
        Assert.True(File.Exists(path), $"Missing AsyncAPI contract file at {path}");

        var text = File.ReadAllText(path, Encoding.UTF8);
        Assert.Contains("asyncapi: 3.0.0", text);
        Assert.Contains("fxfer.configuration-changed", text);
    }
}

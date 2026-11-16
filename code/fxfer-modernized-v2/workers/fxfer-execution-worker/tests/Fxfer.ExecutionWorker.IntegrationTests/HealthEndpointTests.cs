using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Fxfer.ExecutionWorker.IntegrationTests;

public sealed class HealthEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HealthEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder => builder.UseEnvironment("Development"));
    }

    [Fact]
    public async Task LiveHealth_ReturnsOk()
    {
        using var client = _factory.CreateClient();
        var response = await client.GetAsync("/health/live");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}

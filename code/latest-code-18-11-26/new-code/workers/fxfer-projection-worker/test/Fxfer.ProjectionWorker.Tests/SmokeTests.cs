using System.Net;
using System.Net.Http.Json;
using Fxfer.ProjectionWorker.Worker.Health;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Fxfer.ProjectionWorker.Tests;

public sealed class SmokeTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public SmokeTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetLiveHealth_ReturnsOk()
    {
        using var client = _factory.CreateClient();

        using var response = await client.GetAsync("/api/health/live");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<HealthResponse>();
        Assert.NotNull(payload);
        Assert.Equal("ok", payload!.Status);
    }
}


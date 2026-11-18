using System.Net;
using System.Net.Http.Json;
using Fxfer.Bff.Contracts.Common;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Fxfer.Bff.Tests;

public sealed class SmokeTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public SmokeTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetLiveHealth_ReturnsOkAndCorrelationId()
    {
        using var client = _factory.CreateClient();

        using var response = await client.GetAsync("/api/health/live");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        Assert.True(response.Headers.TryGetValues("X-Correlation-ID", out var values));
        Assert.Single(values);
        Assert.True(Guid.TryParse(values.Single(), out _));

        var payload = await response.Content.ReadFromJsonAsync<HealthResponse>();
        Assert.NotNull(payload);
        Assert.Equal("ok", payload!.Status);
    }
}


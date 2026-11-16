using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Fxfer.Bff.IntegrationTests;

public sealed class T_002_AuthenticationAndValidationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string Issuer = "fxfer-bff-tests";
    private const string Audience = "fxfer-bff-tests";
    private const string SigningKey = "fxfer-bff-tests-signing-key-32-chars-min";

    private readonly WebApplicationFactory<Program> _factory;

    public T_002_AuthenticationAndValidationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development");
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Authentication:Jwt:Authority"] = "",
                    ["Authentication:Jwt:Issuer"] = Issuer,
                    ["Authentication:Jwt:Audience"] = Audience,
                    ["Authentication:Jwt:SigningKey"] = SigningKey,
                    ["Authentication:Jwt:RequireHttpsMetadata"] = "false",
                });
            });
        });
    }

    [Fact]
    public async Task T_002_requires_a_valid_jwt_to_access_ui_api_endpoints()
    {
        using var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/v1/ftp-servers");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task T_002_rejects_a_jwt_missing_tenant_context()
    {
        using var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(tenantId: null));

        var response = await client.GetAsync("/api/v1/ftp-servers");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task T_002_performs_request_schema_validation_before_forwarding()
    {
        using var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(tenantId: "tenant-123"));

        using var response = await client.PostAsJsonAsync("/api/v1/ftp-servers", new
        {
            name = "",
            ipAddress = "",
            transferMode = "",
            user = "",
            description = "",
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("VALIDATION_ERROR", body);
    }

    private static string CreateToken(string? tenantId)
    {
        var claims = new List<Claim>
        {
            new("sub", Guid.NewGuid().ToString()),
        };

        if (!string.IsNullOrWhiteSpace(tenantId))
        {
            claims.Add(new Claim("tenant_id", tenantId));
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            notBefore: DateTime.UtcNow.AddMinutes(-1),
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

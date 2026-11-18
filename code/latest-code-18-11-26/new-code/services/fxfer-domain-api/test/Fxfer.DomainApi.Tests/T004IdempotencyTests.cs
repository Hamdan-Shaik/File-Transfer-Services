using System.Net;
using System.Net.Http.Json;
using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Fxfer.DomainApi.Tests;

public sealed class T004IdempotencyTests
{
    [Fact]
    public async Task T_004_repeated_command_submission_returns_the_same_result()
    {
        await using var factory = new FxferDomainApiFactory();
        var client = factory.CreateClient();

        var idempotencyKey = Guid.NewGuid().ToString();
        var request = CreateRequest();

        using var first = new HttpRequestMessage(HttpMethod.Post, "/api/fxfer-domain/v1/ftp-servers")
        {
            Content = JsonContent.Create(request)
        };
        first.Headers.Add("Idempotency-Key", idempotencyKey);

        using var firstResponse = await client.SendAsync(first);
        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);

        var firstPayload = await firstResponse.Content.ReadFromJsonAsync<FtpServer>();
        Assert.NotNull(firstPayload);
        var firstLocation = firstResponse.Headers.Location;
        Assert.NotNull(firstLocation);

        using var second = new HttpRequestMessage(HttpMethod.Post, "/api/fxfer-domain/v1/ftp-servers")
        {
            Content = JsonContent.Create(request)
        };
        second.Headers.Add("Idempotency-Key", idempotencyKey);

        using var secondResponse = await client.SendAsync(second);
        Assert.Equal(HttpStatusCode.Created, secondResponse.StatusCode);

        var secondPayload = await secondResponse.Content.ReadFromJsonAsync<FtpServer>();
        Assert.NotNull(secondPayload);

        Assert.Equal(firstPayload!.FtpServerId, secondPayload!.FtpServerId);
        Assert.Equal(firstLocation, secondResponse.Headers.Location);
    }

    [Fact]
    public async Task T_004_IdempotencyKey_rejects_reuse_with_different_payload()
    {
        await using var factory = new FxferDomainApiFactory();
        var client = factory.CreateClient();

        var idempotencyKey = Guid.NewGuid().ToString();

        using var first = new HttpRequestMessage(HttpMethod.Post, "/api/fxfer-domain/v1/ftp-servers")
        {
            Content = JsonContent.Create(CreateRequest())
        };
        first.Headers.Add("Idempotency-Key", idempotencyKey);

        using var firstResponse = await client.SendAsync(first);
        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);

        using var second = new HttpRequestMessage(HttpMethod.Post, "/api/fxfer-domain/v1/ftp-servers")
        {
            Content = JsonContent.Create(CreateRequest(ftpServerName: "other"))
        };
        second.Headers.Add("Idempotency-Key", idempotencyKey);

        using var secondResponse = await client.SendAsync(second);
        Assert.Equal(HttpStatusCode.Conflict, secondResponse.StatusCode);

        var problem = await secondResponse.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("Idempotency-Key conflict", problem!.Title);
    }

    private static FtpServerUpsertRequest CreateRequest(string ftpServerName = "server")
    {
        return new FtpServerUpsertRequest
        {
            FtpServerName = ftpServerName,
            FtpServerIp = "127.0.0.1",
            FtpTransferMode = "1",
            FtpUser = "user",
            CredentialSecretRef = "secret-ref",
            FtpDescription = "desc",
            FtpPort = 21,
            UseSshAuth = false,
            IsValid = true,
            OperatorId = "operator"
        };
    }
}


using System.Net;
using System.Net.Http.Json;
using Fxfer.Bff.Api.Configuration;
using Fxfer.Bff.Application.Providers;
using Fxfer.Bff.Contracts.Common;
using Fxfer.Bff.Contracts.FtpServers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Xunit;

namespace Fxfer.Bff.Tests.Application.Providers;

public sealed class FileTransferQueryProviderFtpServersOptionsTests
{
    [Fact]
    public async Task GetFtpServersOptionsAsync_sorts_by_name_case_insensitive()
    {
        var responsePayload = new PagedResult<FtpServer>
        {
            Page = 1,
            PageSize = 5000,
            Total = 3,
            Items =
            [
                CreateServer("bravo"),
                CreateServer("Alpha"),
                CreateServer("charlie")
            ]
        };

        var handler = new StubHttpMessageHandler(async request =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);
            var json = System.Text.Json.JsonSerializer.Serialize(responsePayload);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        });

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var httpContextAccessor = new HttpContextAccessor { HttpContext = new DefaultHttpContext() };
        var options = Options.Create(new DownstreamOptions());

        var domainApiClient = new FxferDomainApiClient(httpClient, httpContextAccessor, options);
        var provider = new FileTransferQueryProvider(domainApiClient, new DataListsClient());

        var result = await provider.GetFtpServersOptionsAsync(isValid: true, ct: CancellationToken.None);

        Assert.Collection(
            result,
            first => Assert.Equal("Alpha", first.FtpServerName),
            second => Assert.Equal("bravo", second.FtpServerName),
            third => Assert.Equal("charlie", third.FtpServerName));
    }

    private static FtpServer CreateServer(string name) => new()
    {
        FtpServerId = Guid.NewGuid(),
        FtpServerName = name,
        FtpDescription = "desc",
        FtpServerIp = "127.0.0.1",
        FtpTransferMode = "passive",
        FtpUser = "user",
        FtpPort = 21,
        IsValid = true,
        UseSshAuth = false,
        OperatorId = "op",
        CreationDate = DateTimeOffset.UtcNow,
        LastModifiedDate = DateTimeOffset.UtcNow
    };

    private sealed class StubHttpMessageHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> handler) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            handler(request);
    }
}
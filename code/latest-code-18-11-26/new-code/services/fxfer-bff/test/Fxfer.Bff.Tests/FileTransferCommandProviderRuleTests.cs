using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Fxfer.Bff.Api.Configuration;
using Fxfer.Bff.Application.Providers;
using Fxfer.Bff.Contracts.FtpServers;
using Fxfer.Bff.Contracts.TransferConfigurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Fxfer.Bff.Tests;

public sealed class FileTransferCommandProviderRuleTests
{
    [Fact]
    public async Task F01_027_removes_hyphens_from_destination_contact_phone()
    {
        var handler = new RecordingHandler((_) => OkJsonResponse(BuildDownstreamTransferConfigurationResponse()));
        var provider = CreateProvider(handler);

        var request = BuildUpsertRequest(new DestinationUpsert
        {
            DestinationId = null,
            DestinationAddress = "\\\\dest\\\\a",
            DestinationProtocol = "SFTP",
            FtpServerId = Guid.NewGuid(),
            ContactPhone = "555-111-2222",
            IsValid = true,
            OperatorId = null
        });

        await provider.AddFileTransferBaseConfigurationAsync(request, CancellationToken.None);

        var body = handler.Requests.Single().Body!;
        using var doc = JsonDocument.Parse(body);
        var contactPhone = doc.RootElement.GetProperty("destinations")[0].GetProperty("contactPhone").GetString();
        Assert.Equal("5551112222", contactPhone);
    }

    [Fact]
    public async Task F01_028_file_to_event_overwrites_destination_address_with_external_fte_common_folder()
    {
        var handler = new RecordingHandler((_) => OkJsonResponse(BuildDownstreamTransferConfigurationResponse()));
        var provider = CreateProvider(handler, externalFteCommonFolder: "\\\\server\\\\share\\\\fte");

        var request = BuildUpsertRequest(new DestinationUpsert
        {
            DestinationId = null,
            DestinationAddress = "\\\\dest\\\\ignored",
            DestinationProtocol = "SFTP",
            FtpServerId = Guid.NewGuid(),
            DestinationType = "FileToEvent",
            IsValid = true,
            OperatorId = null
        });

        await provider.AddFileTransferBaseConfigurationAsync(request, CancellationToken.None);

        var body = handler.Requests.Single().Body!;
        using var doc = JsonDocument.Parse(body);
        var address = doc.RootElement.GetProperty("destinations")[0].GetProperty("destinationAddress").GetString();
        Assert.Equal("\\\\server\\\\share\\\\fte", address);
    }

    [Fact]
    public async Task F01_029_destination_ftp_server_id_empty_maps_to_guid_empty()
    {
        var handler = new RecordingHandler((_) => OkJsonResponse(BuildDownstreamTransferConfigurationResponse()));
        var provider = CreateProvider(handler);

        var request = BuildUpsertRequest(new DestinationUpsert
        {
            DestinationId = null,
            DestinationAddress = "\\\\dest\\\\a",
            DestinationProtocol = "SFTP",
            FtpServerId = null,
            IsValid = true,
            OperatorId = null
        });

        await provider.AddFileTransferBaseConfigurationAsync(request, CancellationToken.None);

        var body = handler.Requests.Single().Body!;
        using var doc = JsonDocument.Parse(body);
        var ftpServerId = doc.RootElement.GetProperty("destinations")[0].GetProperty("ftpServerId").GetString();
        Assert.Equal(Guid.Empty.ToString(), ftpServerId);
    }

    [Fact]
    public async Task F01_030_empty_run_on_this_day_maps_to_zero()
    {
        var handler = new RecordingHandler((_) => OkJsonResponse(BuildDownstreamTransferConfigurationResponse()));
        var provider = CreateProvider(handler);

        var request = BuildUpsertRequest(BuildMinimalDestination()) with { RunOnThisDay = null };

        await provider.AddFileTransferBaseConfigurationAsync(request, CancellationToken.None);

        var body = handler.Requests.Single().Body!;
        using var doc = JsonDocument.Parse(body);
        var runOnThisDay = doc.RootElement.GetProperty("runOnThisDay").GetInt32();
        Assert.Equal(0, runOnThisDay);
    }

    [Fact]
    public async Task F01_031_sets_is_suspended_changed_date_only_when_suspended()
    {
        var handler = new RecordingHandler((_) => OkJsonResponse(BuildDownstreamTransferConfigurationResponse()));
        var provider = CreateProvider(handler);

        var request = BuildUpsertRequest(BuildMinimalDestination()) with { Suspended = true };
        var before = DateTimeOffset.UtcNow;

        await provider.AddFileTransferBaseConfigurationAsync(request, CancellationToken.None);

        var after = DateTimeOffset.UtcNow;
        var body = handler.Requests.Single().Body!;
        using var doc = JsonDocument.Parse(body);

        var suspendedChangeDate = doc.RootElement.GetProperty("suspendedChangeDate").GetDateTimeOffset();
        Assert.True(suspendedChangeDate >= before && suspendedChangeDate <= after);

        handler.Requests.Clear();

        var notSuspended = BuildUpsertRequest(BuildMinimalDestination()) with { Suspended = false };
        await provider.AddFileTransferBaseConfigurationAsync(notSuspended, CancellationToken.None);

        body = handler.Requests.Single().Body!;
        using var doc2 = JsonDocument.Parse(body);
        var hasProperty = doc2.RootElement.TryGetProperty("suspendedChangeDate", out var prop);
        Assert.True(hasProperty);
        Assert.Equal(JsonValueKind.Null, prop.ValueKind);
    }

    [Fact]
    public async Task F01_032_calls_domain_operation_for_add_transfer_configuration()
    {
        var handler = new RecordingHandler((_) => OkJsonResponse(BuildDownstreamTransferConfigurationResponse()));
        var provider = CreateProvider(handler);

        await provider.AddFileTransferBaseConfigurationAsync(BuildUpsertRequest(BuildMinimalDestination()), CancellationToken.None);

        var request = handler.Requests.Single();
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("/api/fxfer-domain/v1/transfer-configurations", request.PathAndQuery);
    }

    [Fact]
    public async Task F01_033_propagates_downstream_faults_for_problem_details_mapping()
    {
        var handler = new RecordingHandler((_) => new HttpResponseMessage(HttpStatusCode.BadRequest));
        var provider = CreateProvider(handler);

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            provider.AddFileTransferBaseConfigurationAsync(BuildUpsertRequest(BuildMinimalDestination()), CancellationToken.None));
    }

    [Fact]
    public async Task F02_051_add_ftp_server_assigns_new_id_and_uses_requestor_operator_id()
    {
        var handler = new RecordingHandler((_) => OkJsonResponse(BuildDownstreamFtpServerResponse()));
        var provider = CreateProvider(handler);

        var request = BuildFtpServerUpsertRequest() with { OperatorId = "operator-123" };
        await provider.AddFtpServerAsync(request, CancellationToken.None);

        var body = handler.Requests.Single().Body!;
        using var doc = JsonDocument.Parse(body);

        Assert.True(Guid.TryParse(doc.RootElement.GetProperty("ftpServerId").GetString(), out var generated));
        Assert.NotEqual(Guid.Empty, generated);
        Assert.Equal("operator-123", doc.RootElement.GetProperty("operatorId").GetString());
    }

    [Fact]
    public async Task F02_052_add_ftp_server_non_null_response_maps_id_and_status()
    {
        var ftpServerId = Guid.NewGuid();
        var handler = new RecordingHandler((_) => OkJsonResponse(BuildDownstreamFtpServerResponse(ftpServerId: ftpServerId)));
        var provider = CreateProvider(handler);

        var result = await provider.AddFtpServerAsync(BuildFtpServerUpsertRequest(), CancellationToken.None);

        Assert.Equal(ftpServerId, result.FtpServerId);
    }

    [Fact]
    public async Task F02_056_update_ftp_server_reuses_existing_password_when_omitted()
    {
        var ftpServerId = Guid.NewGuid();
        var secret = "existing-secret";
        var handler = new RecordingHandler(async (r) =>
        {
            if (r.Method == HttpMethod.Get)
            {
                var body = JsonSerializer.Serialize(new
                {
                    items = new[] { new { ftpServerId, credentialSecretRef = secret } },
                    total = 1,
                    page = 1,
                    pageSize = 50
                });
                return OkJsonResponse(body);
            }

            if (r.Method == HttpMethod.Put)
            {
                return OkJsonResponse(BuildDownstreamFtpServerResponse(ftpServerId: ftpServerId));
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        });

        var provider = CreateProvider(handler);

        var request = BuildFtpServerUpsertRequest() with { FtpPassword = null };
        await provider.UpdateFtpServerAsync(ftpServerId, request, CancellationToken.None);

        var putRequest = handler.Requests.Single(x => x.Method == HttpMethod.Put);
        using var doc = JsonDocument.Parse(putRequest.Body!);
        Assert.Equal(secret, doc.RootElement.GetProperty("credentialSecretRef").GetString());
    }

    [Fact]
    public async Task F02_058_update_ftp_server_non_null_response_maps_id_and_is_valid()
    {
        var ftpServerId = Guid.NewGuid();
        var handler = new RecordingHandler((_) => OkJsonResponse(BuildDownstreamFtpServerResponse(ftpServerId: ftpServerId, isValid: false)));
        var provider = CreateProvider(handler);

        var result = await provider.UpdateFtpServerAsync(ftpServerId, BuildFtpServerUpsertRequest(), CancellationToken.None);

        Assert.Equal(ftpServerId, result.FtpServerId);
        Assert.False(result.IsValid);
    }

    [Fact]
    public async Task F02_059_invalidate_ftp_server_non_null_response_maps_id_and_is_valid()
    {
        var ftpServerId = Guid.NewGuid();
        var handler = new RecordingHandler((_) => OkJsonResponse(BuildDownstreamFtpServerResponse(ftpServerId: ftpServerId, isValid: false)));
        var provider = CreateProvider(handler);

        var result = await provider.InvalidateFtpServerAsync(ftpServerId, BuildFtpServerPatchRequest(), CancellationToken.None);

        Assert.Equal(ftpServerId, result.FtpServerId);
        Assert.False(result.IsValid);
    }

    [Fact]
    public async Task F02_060_invalidate_builds_command_by_parsing_ftp_server_id_as_guid()
    {
        var ftpServerId = Guid.NewGuid();
        var handler = new RecordingHandler((_) => OkJsonResponse(BuildDownstreamFtpServerResponse(ftpServerId: ftpServerId)));
        var provider = CreateProvider(handler);

        await provider.InvalidateFtpServerAsync(ftpServerId, BuildFtpServerPatchRequest(), CancellationToken.None);

        var request = handler.Requests.Single();
        Assert.Equal(HttpMethod.Patch, request.Method);
        Assert.Equal($"/api/fxfer-domain/v1/ftp-servers/{ftpServerId}", request.PathAndQuery);
    }

    [Fact]
    public async Task F02_063_password_lookup_ok_with_empty_list_throws_unhandled_exception()
    {
        var ftpServerId = Guid.NewGuid();
        var handler = new RecordingHandler((r) =>
        {
            if (r.Method == HttpMethod.Get)
            {
                var body = JsonSerializer.Serialize(new { items = Array.Empty<object>(), total = 0, page = 1, pageSize = 50 });
                return OkJsonResponse(body);
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        });

        var provider = CreateProvider(handler);

        var request = BuildFtpServerUpsertRequest() with { FtpPassword = null };
        await Assert.ThrowsAsync<InvalidOperationException>(() => provider.UpdateFtpServerAsync(ftpServerId, request, CancellationToken.None));
    }

    private static TransferConfigurationUpsertRequest BuildUpsertRequest(DestinationUpsert destination)
    {
        return new TransferConfigurationUpsertRequest
        {
            ConfigName = "cfg",
            Description = "desc",
            FtpServerId = Guid.NewGuid(),
            Address = "\\\\src\\\\address",
            Mask = "*.txt",
            Protocol = "SFTP",
            FileDirection = "Outbound",
            OwnerName = "owner",
            OwnerEmail = "owner@example.com",
            OwnerPhoneNumber = "5551112222",
            OwnerSubsystem = Guid.NewGuid(),
            TradingPartner = null,
            SendNotification = true,
            RunOnMonday = true,
            RunOnTuesday = false,
            RunOnWednesday = false,
            RunOnThursday = false,
            RunOnFriday = false,
            RunOnSaturday = false,
            RunOnSunday = false,
            RunOnFirstDay = false,
            RunOnLastDay = false,
            RunOnThisDay = 5,
            StartWindow = "0000",
            EndWindow = "2359",
            IsActive = true,
            Suspended = false,
            Destinations = new[] { destination },
            OperatorId = "operator"
        };
    }

    private static DestinationUpsert BuildMinimalDestination()
    {
        return new DestinationUpsert
        {
            DestinationId = null,
            DestinationAddress = "\\\\dest\\\\a",
            DestinationProtocol = "SFTP",
            FtpServerId = Guid.NewGuid(),
            IsValid = true,
            OperatorId = null
        };
    }

    private static FtpServerUpsertRequest BuildFtpServerUpsertRequest()
    {
        return new FtpServerUpsertRequest
        {
            FtpServerName = "s1",
            FtpDescription = "d1",
            FtpServerIp = "127.0.0.1",
            FtpTransferMode = "1",
            FtpUser = "user",
            FtpPassword = "pw",
            FtpPasswordConfirmation = "pw",
            FtpPort = 22,
            FtpProxyAddress = null,
            FtpProxyPort = null,
            UseSshAuth = false,
            IsValid = true,
            OperatorId = "operator"
        };
    }

    private static FtpServerPatchRequest BuildFtpServerPatchRequest()
    {
        return new FtpServerPatchRequest
        {
            IsValid = false,
            OperatorId = "operator"
        };
    }

    private static FileTransferCommandProvider CreateProvider(RecordingHandler handler, string? externalFteCommonFolder = null, ClaimsPrincipal? user = null)
    {
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://downstream")
        };

        var context = new DefaultHttpContext();
        if (user is not null)
        {
            context.User = user;
        }

        var accessor = new HttpContextAccessor { HttpContext = context };
        var domainClient = new FxferDomainApiClient(httpClient, accessor, Options.Create(new DownstreamOptions { MaxRetries = 0 }));

        var options = Options.Create(new FileTransferManagementOptions { ExternalFTECommonFolder = externalFteCommonFolder });
        return new FileTransferCommandProvider(domainClient, accessor, options);
    }

    private static string BuildDownstreamTransferConfigurationResponse()
    {
        return JsonSerializer.Serialize(new
        {
            ftsSourceLocationId = Guid.NewGuid(),
            configId = 0,
            configName = "cfg",
            description = "desc",
            ftpServerId = Guid.NewGuid(),
            ftpServerName = "server",
            address = "\\\\src\\\\address",
            mask = "*.txt",
            protocol = "SFTP",
            fileDirection = "Outbound",
            ownerName = "owner",
            ownerEmail = "owner@example.com",
            ownerSubsystem = Guid.NewGuid(),
            ownerPhoneNumber = "5551112222",
            tradingPartner = (string?)null,
            operatorId = "operator",
            runOnMonday = true,
            runOnTuesday = false,
            runOnWednesday = false,
            runOnThursday = false,
            runOnFriday = false,
            runOnSaturday = false,
            runOnSunday = false,
            runOnFirstDay = false,
            runOnLastDay = false,
            runOnThisDay = 0,
            startWindow = "0000",
            endWindow = "2359",
            sendNotification = true,
            isActive = true,
            validChangeDate = DateTimeOffset.UtcNow,
            suspended = false,
            suspendedChangeDate = DateTimeOffset.UtcNow,
            lastModifiedDate = DateTimeOffset.UtcNow,
            creationDate = DateTimeOffset.UtcNow,
            destinations = Array.Empty<object>()
        });
    }

    private static string BuildDownstreamFtpServerResponse(Guid? ftpServerId = null, bool isValid = true)
    {
        return JsonSerializer.Serialize(new
        {
            ftpServerId = ftpServerId ?? Guid.NewGuid(),
            ftpServerName = "s1",
            ftpDescription = "d1",
            ftpServerIp = "127.0.0.1",
            ftpTransferMode = "1",
            ftpUser = "user",
            credentialSecretRef = "pw",
            ftpPort = 22,
            ftpProxyAddress = (string?)null,
            ftpProxyPort = (int?)null,
            useSshAuth = false,
            isValid,
            operatorId = "operator",
            creationDate = DateTimeOffset.UtcNow,
            lastModifiedDate = DateTimeOffset.UtcNow
        });
    }

    private static HttpResponseMessage OkJsonResponse(string json)
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
    }

    private sealed class RecordingHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _sendAsync;

        public RecordingHandler(Func<HttpRequestMessage, HttpResponseMessage> send)
            : this(request => Task.FromResult(send(request)))
        {
        }

        public RecordingHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> sendAsync)
        {
            _sendAsync = sendAsync;
        }

        public List<RecordedRequest> Requests { get; } = new();

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var body = request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken);
            Requests.Add(new RecordedRequest(request.Method, request.RequestUri!.PathAndQuery, body));
            return await _sendAsync(request);
        }
    }

    private sealed record RecordedRequest(HttpMethod Method, string PathAndQuery, string? Body);
}


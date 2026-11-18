using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Fxfer.TransferWorker.Application.Constants;
using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Contracts.Batch;
using Fxfer.TransferWorker.Infrastructure.DataAccess;

namespace Fxfer.TransferWorker.Tests;

public sealed class BackendDataAccessBatchDataAccessPart1Tests
{
    [Fact]
    public async Task CreateErrorLogDataAccess_truncates_fields_and_posts_exception()
    {
        var fileId = Guid.NewGuid();

        var errorLog = new ErrorLogDetailModel
        {
            FileId = fileId,
            ErrorType = new string('t', 2500),
            ErrorText = new string('m', 2500),
            MethodName = new string('n', 2500),
            StackTrace = new string('s', 4500),
            OperatorId = "op"
        };

        var api = new FakeDomainApiClient(async request =>
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("/api/fxfer-domain/v1/file-tracking/" + fileId + "/exceptions", request.RequestUri!.ToString());

            var body = await request.Content!.ReadFromJsonAsync<JsonElement>();
            Assert.Equal(2000, ReadString(body, "ExceptionType", "exceptionType")!.Length);
            Assert.Equal(2000, ReadString(body, "Message", "message")!.Length);
            Assert.Equal(2000, ReadString(body, "MethodName", "methodName")!.Length);
            Assert.Equal(4000, ReadString(body, "StackTrace", "stackTrace")!.Length);
            Assert.Equal("op", ReadString(body, "OperatorId", "operatorId"));

            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var sut = new CreateErrorLogDataAccess(api, new NullRequestContextAccessor(), errorLog);

        var result = await sut.ExecuteAsync(CancellationToken.None);

        Assert.Equal(1, result);
    }

    [Fact]
    public async Task CreateFileManagementDataAccess_generates_file_id_sets_transfering_status_and_posts_tracking_header()
    {
        var api = new FakeDomainApiClient(async request =>
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("/api/fxfer-domain/v1/file-tracking", request.RequestUri!.ToString());

            var body = await request.Content!.ReadFromJsonAsync<JsonElement>();
            var fileId = ReadGuid(body, "FileID", "fileID", "fileId");

            Assert.NotEqual(Guid.Empty, fileId);
            Assert.Equal(FileStatusConstant.Transfering, ReadString(body, "FileStatus", "fileStatus"));

            var response = new FileManagementResponseModel { FileId = fileId, FileTrackId = Guid.NewGuid() };
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = JsonContent.Create(response) };
        });

        var input = new FileManagementRequestModel
        {
            FileName = "a.txt",
            FileSize = 12,
            SourceLocationId = Guid.NewGuid(),
            ChecksumCode = "sum",
            OperatorId = "op"
        };

        var sut = new CreateFileManagementDataAccess(api, new NullRequestContextAccessor(), input);

        var result = await sut.ExecuteAsync(CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result.FileId);
    }

    [Fact]
    public async Task CreateFtsFileDeliveryDataAccess_posts_delivery_and_returns_delivery_id()
    {
        var fileId = Guid.NewGuid();
        var destinationId = Guid.NewGuid();
        Guid? postedDeliveryId = null;

        var api = new FakeDomainApiClient(async request =>
        {
            var uri = request.RequestUri!.ToString();

            if (request.Method == HttpMethod.Post && uri == "/api/fxfer-domain/v1/file-tracking/" + fileId + "/deliveries")
            {
                var body = await request.Content!.ReadFromJsonAsync<JsonElement>();
                postedDeliveryId = ReadGuid(body, "DeliveryId", "deliveryId");
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            throw new InvalidOperationException($"Unexpected request: {request.Method} {uri}");
        });

        var sut = new CreateFtsFileDeliveryDataAccess(
            api,
            new NullRequestContextAccessor(),
            fileId,
            destinationId,
            destinationFileName: "renamed.txt",
            operatorId: "op");

        var deliveryId = await sut.ExecuteAsync(CancellationToken.None);

        Assert.NotNull(postedDeliveryId);
        Assert.NotEqual(Guid.Empty, deliveryId);
        Assert.Equal(postedDeliveryId.Value, deliveryId);
    }

    [Fact]
    public async Task FtsSuspensionDataAccess_posts_suspension_even_when_source_not_found()
    {
        var sourceLocationId = Guid.NewGuid();
        Guid? postedSuspensionId = null;

        var api = new FakeDomainApiClient(async request =>
        {
            var uri = request.RequestUri!.ToString();

            if (request.Method == HttpMethod.Get && uri == "/api/fxfer-domain/v1/transfer-configurations/" + sourceLocationId)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            if (request.Method == HttpMethod.Post && uri == "/api/fxfer-domain/v1/transfer-configurations/" + sourceLocationId + "/suspensions")
            {
                var body = await request.Content!.ReadFromJsonAsync<JsonElement>();
                postedSuspensionId = ReadGuid(body, "SuspensionId", "suspensionId");
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            if (request.Method == HttpMethod.Patch && uri == "/api/fxfer-domain/v1/transfer-configurations/" + sourceLocationId)
            {
                throw new InvalidOperationException("PATCH should not be called when source is not found");
            }

            throw new InvalidOperationException($"Unexpected request: {request.Method} {uri}");
        });

        var query = new FtsSuspensionSearchCriteria { SourceLocationId = sourceLocationId };

        var sut = new FtsSuspensionDataAccess(api, new NullRequestContextAccessor(), query);

        var result = await sut.ExecuteAsync(CancellationToken.None);

        Assert.NotNull(postedSuspensionId);
        Assert.Equal(postedSuspensionId.Value, result.SuspensionId);
    }

    [Fact]
    public async Task FtsSuspensionDataAccess_posts_stack_trace_when_source_exists()
    {
        var sourceLocationId = Guid.NewGuid();
        Guid? postedSuspensionId = null;
        string? postedStackTrace = null;

        var api = new FakeDomainApiClient(async request =>
        {
            var uri = request.RequestUri!.ToString();

            if (request.Method == HttpMethod.Get && uri == "/api/fxfer-domain/v1/transfer-configurations/" + sourceLocationId)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            if (request.Method == HttpMethod.Patch && uri == "/api/fxfer-domain/v1/transfer-configurations/" + sourceLocationId)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            if (request.Method == HttpMethod.Post && uri == "/api/fxfer-domain/v1/transfer-configurations/" + sourceLocationId + "/suspensions")
            {
                var body = await request.Content!.ReadFromJsonAsync<JsonElement>();
                postedSuspensionId = ReadGuid(body, "SuspensionId", "suspensionId");
                postedStackTrace = ReadString(body, "StackTrace", "stackTrace");
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            throw new InvalidOperationException($"Unexpected request: {request.Method} {uri}");
        });

        var query = new FtsSuspensionSearchCriteria
        {
            SourceLocationId = sourceLocationId,
            ErrorText = "err",
            StackTrace = new string('x', 1005)
        };

        var sut = new FtsSuspensionDataAccess(api, new NullRequestContextAccessor(), query);

        var result = await sut.ExecuteAsync(CancellationToken.None);

        Assert.NotNull(postedSuspensionId);
        Assert.Equal(postedSuspensionId.Value, result.SuspensionId);
        Assert.NotNull(postedStackTrace);
        Assert.Equal(1005, postedStackTrace!.Length);
    }

    [Fact]
    public async Task FtsSuspensionDataAccess_throws_on_unexpected_transfer_configuration_failure()
    {
        var sourceLocationId = Guid.NewGuid();

        var api = new FakeDomainApiClient(request =>
        {
            var uri = request.RequestUri!.ToString();

            if (request.Method == HttpMethod.Get && uri == "/api/fxfer-domain/v1/transfer-configurations/" + sourceLocationId)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            }

            if (request.Method == HttpMethod.Patch && uri == "/api/fxfer-domain/v1/transfer-configurations/" + sourceLocationId)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }

            throw new InvalidOperationException($"Unexpected request: {request.Method} {uri}");
        });

        var query = new FtsSuspensionSearchCriteria { SourceLocationId = sourceLocationId };
        var sut = new FtsSuspensionDataAccess(api, new NullRequestContextAccessor(), query);

        await Assert.ThrowsAsync<HttpRequestException>(() => sut.ExecuteAsync(CancellationToken.None));
    }

    private static string? ReadString(JsonElement element, params string[] candidates)
    {
        foreach (var candidate in candidates)
        {
            if (element.TryGetProperty(candidate, out var value))
            {
                return value.GetString();
            }
        }

        return null;
    }

    private static Guid ReadGuid(JsonElement element, params string[] candidates)
    {
        foreach (var candidate in candidates)
        {
            if (element.TryGetProperty(candidate, out var value))
            {
                return value.GetGuid();
            }
        }

        return Guid.Empty;
    }

    private sealed class FakeDomainApiClient : IFxferDomainApiClient
    {
        private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _handler;

        public FakeDomainApiClient(Func<HttpRequestMessage, Task<HttpResponseMessage>> handler)
        {
            _handler = handler;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _handler(request);
        }
    }

    private sealed class NullRequestContextAccessor : IRequestContextAccessor
    {
        public string? CorrelationId => null;

        public string? TenantId => null;
    }
}

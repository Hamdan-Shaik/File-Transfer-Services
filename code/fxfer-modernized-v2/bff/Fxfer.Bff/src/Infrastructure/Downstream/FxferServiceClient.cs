using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

using Fxfer.Bff.Api.Models;
using Fxfer.Bff.Host.Composition;
using Fxfer.Bff.Infrastructure.Tenancy;

namespace Fxfer.Bff.Infrastructure.Downstream;

public sealed class FxferServiceClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
    };

    private readonly HttpClient _http;
    private readonly TenantContext _tenantContext;
    private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;

    public FxferServiceClient(
        HttpClient http,
        TenantContext tenantContext,
        Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
    {
        _http = http;
        _tenantContext = tenantContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> CanReachHealthEndpoint(CancellationToken cancellationToken)
    {
        if (_http.BaseAddress is null)
        {
            // shortcut: treat missing downstream base URL as "not wired yet" in skeleton mode, upgrade when platform config is available.
            return true;
        }

        using var request = CreateRequest(HttpMethod.Get, "/health/live", content: null, idempotencyKey: null);

        var response = await _http.SendAsync(request, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<(HttpStatusCode StatusCode, PagedResult<FtpServer>? Body, string? RawBody, string? ContentType)>
        SearchFtpServersAsync(string queryString, CancellationToken cancellationToken)
    {
        if (_http.BaseAddress is null)
        {
            return (HttpStatusCode.OK, new PagedResult<FtpServer>(Array.Empty<FtpServer>(), 0, 1, 50), null, null);
        }

        using var request = CreateRequest(HttpMethod.Get, $"/api/v1/ftp-servers{queryString}", content: null, idempotencyKey: null);
        using var response = await _http.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return await ReadRawAsync<PagedResult<FtpServer>>(response, cancellationToken);
        }

        var service = await response.Content.ReadFromJsonAsync<FxferServicePagedResult<FtpServer>>(JsonOptions, cancellationToken);
        if (service is null)
        {
            return (HttpStatusCode.InternalServerError, null, null, null);
        }

        var mapped = new PagedResult<FtpServer>(
            Items: service.Items ?? Array.Empty<FtpServer>(),
            TotalCount: service.RowCount,
            Page: service.CurrentPage,
            PageSize: service.PageSize);

        return (HttpStatusCode.OK, mapped, null, response.Content.Headers.ContentType?.ToString());
    }

    public async Task<(HttpStatusCode StatusCode, PagedResult<TransferConfigurationSummary>? Body, string? RawBody, string? ContentType)>
        SearchTransferConfigurationsAsync(string queryString, CancellationToken cancellationToken)
    {
        if (_http.BaseAddress is null)
        {
            return (HttpStatusCode.OK, new PagedResult<TransferConfigurationSummary>(Array.Empty<TransferConfigurationSummary>(), 0, 1, 50), null, null);
        }

        using var request = CreateRequest(HttpMethod.Get, $"/api/v1/transfer-configurations{queryString}", content: null, idempotencyKey: null);
        using var response = await _http.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return await ReadRawAsync<PagedResult<TransferConfigurationSummary>>(response, cancellationToken);
        }

        var service = await response.Content.ReadFromJsonAsync<FxferServicePagedResult<TransferConfigurationSummary>>(JsonOptions, cancellationToken);
        if (service is null)
        {
            return (HttpStatusCode.InternalServerError, null, null, null);
        }

        var mapped = new PagedResult<TransferConfigurationSummary>(
            Items: service.Items ?? Array.Empty<TransferConfigurationSummary>(),
            TotalCount: service.RowCount,
            Page: service.CurrentPage,
            PageSize: service.PageSize);

        return (HttpStatusCode.OK, mapped, null, response.Content.Headers.ContentType?.ToString());
    }

    public async Task<(HttpStatusCode StatusCode, TransferConfiguration? Body, string? RawBody, string? ContentType)>
        GetTransferConfigurationAsync(Guid transferConfigurationId, CancellationToken cancellationToken)
    {
        if (_http.BaseAddress is null)
        {
            return (HttpStatusCode.OK, new TransferConfiguration(
                TransferConfigurationId: transferConfigurationId,
                ConfigNumber: 0,
                Name: string.Empty,
                Description: null,
                ProtocolCode: string.Empty,
                Address: string.Empty,
                FileMask: string.Empty,
                FtpServerId: null,
                FtpServerName: null,
                SendNotifications: false,
                Owner: new OwnerInfo(string.Empty, Guid.Empty, string.Empty, string.Empty),
                Schedule: new ScheduleInfo(
                    RunOnMonday: false,
                    RunOnTuesday: false,
                    RunOnWednesday: false,
                    RunOnThursday: false,
                    RunOnFriday: false,
                    RunOnSaturday: false,
                    RunOnSunday: false,
                    RunOnFirstDayOfMonth: false,
                    RunOnLastDayOfMonth: false,
                    RunOnThisDayOfMonth: 0,
                    StartTime24hhmm: "0000",
                    EndTime24hhmm: "0000"),
                FileDirection: null,
                TradingPartner: null,
                IsActive: true,
                IsSuspended: false,
                SuspendedChangedAt: null,
                Destinations: Array.Empty<Destination>()), null, null);
        }

        using var request = CreateRequest(HttpMethod.Get, $"/api/v1/transfer-configurations/{transferConfigurationId}", content: null, idempotencyKey: null);
        using var response = await _http.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return await ReadRawAsync<TransferConfiguration>(response, cancellationToken);
        }

        var cfg = await response.Content.ReadFromJsonAsync<TransferConfiguration>(JsonOptions, cancellationToken);
        return (response.StatusCode, cfg, null, response.Content.Headers.ContentType?.ToString());
    }

    public async Task<(HttpStatusCode StatusCode, TransferConfigurationIdResponse? Body, string? RawBody, string? ContentType)>
        UpsertTransferConfigurationAsync(
            TransferConfigurationUpsert body,
            string? idempotencyKey,
            CancellationToken cancellationToken)
    {
        if (_http.BaseAddress is null)
        {
            return (HttpStatusCode.OK, new TransferConfigurationIdResponse(body.TransferConfigurationId ?? Guid.NewGuid()), null, null);
        }

        using var content = new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json");
        using var request = CreateRequest(HttpMethod.Post, "/api/v1/transfer-configurations", content, idempotencyKey);
        using var response = await _http.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return await ReadRawAsync<TransferConfigurationIdResponse>(response, cancellationToken);
        }

        var result = await response.Content.ReadFromJsonAsync<TransferConfigurationIdResponse>(JsonOptions, cancellationToken);
        return (response.StatusCode, result, null, response.Content.Headers.ContentType?.ToString());
    }

    public async Task<(HttpStatusCode StatusCode, TransferConfigurationStateResponse? Body, string? RawBody, string? ContentType)>
        InvalidateTransferConfigurationAsync(Guid transferConfigurationId, string? idempotencyKey, CancellationToken cancellationToken)
    {
        if (_http.BaseAddress is null)
        {
            return (HttpStatusCode.OK, new TransferConfigurationStateResponse(transferConfigurationId, IsActive: false), null, null);
        }

        using var request = CreateRequest(HttpMethod.Delete, $"/api/v1/transfer-configurations/{transferConfigurationId}", content: null, idempotencyKey);
        using var response = await _http.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return await ReadRawAsync<TransferConfigurationStateResponse>(response, cancellationToken);
        }

        var deleted = await response.Content.ReadFromJsonAsync<TransferConfigurationStateResponse>(JsonOptions, cancellationToken);
        return (response.StatusCode, deleted, null, response.Content.Headers.ContentType?.ToString());
    }

    public async Task<(HttpStatusCode StatusCode, PagedResult<IntegrationFile>? Body, string? RawBody, string? ContentType)>
        SearchIntegrationFilesAsync(string queryString, CancellationToken cancellationToken)
    {
        if (_http.BaseAddress is null)
        {
            return (HttpStatusCode.OK, new PagedResult<IntegrationFile>(Array.Empty<IntegrationFile>(), 0, 1, 50), null, null);
        }

        using var request = CreateRequest(HttpMethod.Get, $"/api/v1/integration-files{queryString}", content: null, idempotencyKey: null);
        using var response = await _http.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return await ReadRawAsync<PagedResult<IntegrationFile>>(response, cancellationToken);
        }

        var service = await response.Content.ReadFromJsonAsync<FxferServicePagedResult<IntegrationFile>>(JsonOptions, cancellationToken);
        if (service is null)
        {
            return (HttpStatusCode.InternalServerError, null, null, null);
        }

        var mapped = new PagedResult<IntegrationFile>(
            Items: service.Items ?? Array.Empty<IntegrationFile>(),
            TotalCount: service.RowCount,
            Page: service.CurrentPage,
            PageSize: service.PageSize);

        return (HttpStatusCode.OK, mapped, null, response.Content.Headers.ContentType?.ToString());
    }

    public async Task<(HttpStatusCode StatusCode, IntegrationFileIdResponse? Body, string? RawBody, string? ContentType)>
        UpdateIntegrationFileAsync(Guid integrationFileId, IntegrationFileUpdate body, string? idempotencyKey, CancellationToken cancellationToken)
    {
        if (_http.BaseAddress is null)
        {
            return (HttpStatusCode.OK, new IntegrationFileIdResponse(integrationFileId), null, null);
        }

        using var content = new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json");
        using var request = CreateRequest(HttpMethod.Put, $"/api/v1/integration-files/{integrationFileId}", content, idempotencyKey);
        using var response = await _http.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return await ReadRawAsync<IntegrationFileIdResponse>(response, cancellationToken);
        }

        var updated = await response.Content.ReadFromJsonAsync<IntegrationFileIdResponse>(JsonOptions, cancellationToken);
        return (response.StatusCode, updated, null, response.Content.Headers.ContentType?.ToString());
    }

    public async Task<(HttpStatusCode StatusCode, InboundTarget[]? Body, string? RawBody, string? ContentType)>
        SearchInboundTargetsAsync(string queryString, CancellationToken cancellationToken)
    {
        if (_http.BaseAddress is null)
        {
            return (HttpStatusCode.OK, Array.Empty<InboundTarget>(), null, null);
        }

        using var request = CreateRequest(HttpMethod.Get, $"/api/v1/inbound-targets/search{queryString}", content: null, idempotencyKey: null);
        using var response = await _http.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return await ReadRawAsync<InboundTarget[]>(response, cancellationToken);
        }

        var items = await response.Content.ReadFromJsonAsync<InboundTarget[]>(JsonOptions, cancellationToken);
        return (response.StatusCode, items, null, response.Content.Headers.ContentType?.ToString());
    }

    public async Task<(HttpStatusCode StatusCode, PagedResult<TrackedFile>? Body, string? RawBody, string? ContentType)>
        SearchFileTransfersAsync(FxferServiceFileTransferSearch body, CancellationToken cancellationToken)
    {
        if (_http.BaseAddress is null)
        {
            return (HttpStatusCode.OK, new PagedResult<TrackedFile>(Array.Empty<TrackedFile>(), 0, body.Page, body.PageSize), null, null);
        }

        using var content = new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json");
        using var request = CreateRequest(HttpMethod.Post, "/api/v1/file-transfers/search", content, idempotencyKey: null);
        using var response = await _http.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return await ReadRawAsync<PagedResult<TrackedFile>>(response, cancellationToken);
        }

        var service = await response.Content.ReadFromJsonAsync<FxferServicePagedResult<TrackedFile>>(JsonOptions, cancellationToken);
        if (service is null)
        {
            return (HttpStatusCode.InternalServerError, null, null, null);
        }

        var mapped = new PagedResult<TrackedFile>(
            Items: service.Items ?? Array.Empty<TrackedFile>(),
            TotalCount: service.RowCount,
            Page: service.CurrentPage,
            PageSize: service.PageSize);

        return (HttpStatusCode.OK, mapped, null, response.Content.Headers.ContentType?.ToString());
    }

    public async Task<(HttpStatusCode StatusCode, FtpServerIdResponse? Body, string? RawBody, string? ContentType)>
        CreateFtpServerAsync(FtpServerUpsert body, string? idempotencyKey, CancellationToken cancellationToken)
    {
        if (_http.BaseAddress is null)
        {
            return (HttpStatusCode.Created, new FtpServerIdResponse(Guid.NewGuid()), null, null);
        }

        using var content = new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json");
        using var request = CreateRequest(HttpMethod.Post, "/api/v1/ftp-servers", content, idempotencyKey);
        using var response = await _http.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return await ReadRawAsync<FtpServerIdResponse>(response, cancellationToken);
        }

        var created = await response.Content.ReadFromJsonAsync<FtpServerIdResponse>(JsonOptions, cancellationToken);
        return (response.StatusCode, created, null, response.Content.Headers.ContentType?.ToString());
    }

    public async Task<(HttpStatusCode StatusCode, FtpServerStateResponse? Body, string? RawBody, string? ContentType)>
        UpdateFtpServerAsync(Guid ftpServerId, FtpServerUpsert body, string? idempotencyKey, CancellationToken cancellationToken)
    {
        if (_http.BaseAddress is null)
        {
            return (HttpStatusCode.OK, new FtpServerStateResponse(ftpServerId, body.IsActive), null, null);
        }

        using var content = new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json");
        using var request = CreateRequest(HttpMethod.Put, $"/api/v1/ftp-servers/{ftpServerId}", content, idempotencyKey);
        using var response = await _http.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return await ReadRawAsync<FtpServerStateResponse>(response, cancellationToken);
        }

        var updated = await response.Content.ReadFromJsonAsync<FtpServerStateResponse>(JsonOptions, cancellationToken);
        return (response.StatusCode, updated, null, response.Content.Headers.ContentType?.ToString());
    }

    public async Task<(HttpStatusCode StatusCode, FtpServerStateResponse? Body, string? RawBody, string? ContentType)>
        InvalidateFtpServerAsync(Guid ftpServerId, string? idempotencyKey, CancellationToken cancellationToken)
    {
        if (_http.BaseAddress is null)
        {
            return (HttpStatusCode.OK, new FtpServerStateResponse(ftpServerId, IsActive: false), null, null);
        }

        using var request = CreateRequest(HttpMethod.Delete, $"/api/v1/ftp-servers/{ftpServerId}", content: null, idempotencyKey);
        using var response = await _http.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return await ReadRawAsync<FtpServerStateResponse>(response, cancellationToken);
        }

        var deleted = await response.Content.ReadFromJsonAsync<FtpServerStateResponse>(JsonOptions, cancellationToken);
        return (response.StatusCode, deleted, null, response.Content.Headers.ContentType?.ToString());
    }

    private HttpRequestMessage CreateRequest(HttpMethod method, string relativeUrl, HttpContent? content, string? idempotencyKey)
    {
        var request = new HttpRequestMessage(method, relativeUrl);

        if (content is not null)
        {
            request.Content = content;
        }

        if (!string.IsNullOrWhiteSpace(_tenantContext.CorrelationId))
        {
            request.Headers.TryAddWithoutValidation(RequestContextHeaderNames.CorrelationId, _tenantContext.CorrelationId);
        }

        var httpContext = _httpContextAccessor.HttpContext;

        var authorization = httpContext?.Request.Headers.Authorization.ToString();
        if (!string.IsNullOrWhiteSpace(authorization))
        {
            request.Headers.TryAddWithoutValidation("Authorization", authorization);
        }

        if (httpContext?.Request.Headers.TryGetValue("traceparent", out var traceparent) == true && !string.IsNullOrWhiteSpace(traceparent))
        {
            request.Headers.TryAddWithoutValidation("traceparent", traceparent.ToString());
        }

        if (httpContext?.Request.Headers.TryGetValue("tracestate", out var tracestate) == true && !string.IsNullOrWhiteSpace(tracestate))
        {
            request.Headers.TryAddWithoutValidation("tracestate", tracestate.ToString());
        }

        if (!string.IsNullOrWhiteSpace(idempotencyKey))
        {
            request.Headers.TryAddWithoutValidation("Idempotency-Key", idempotencyKey);
        }

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return request;
    }

    private static async Task<(HttpStatusCode StatusCode, T? Body, string? RawBody, string? ContentType)> ReadRawAsync<T>(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        var raw = await response.Content.ReadAsStringAsync(cancellationToken);
        return (response.StatusCode, default, raw, response.Content.Headers.ContentType?.ToString());
    }

    public sealed record FxferServiceFileTransferSearch(
        int Page,
        int PageSize,
        string? OrderBy,
        string? OrderDir,
        FxferServiceFileTransferSearchFilters? Filters);

    public sealed record FxferServiceFileTransferSearchFilters(
        Guid? FileId,
        string? FileNameContains,
        string? FileStatus,
        string? FileDirection,
        string? TransferMethod,
        DateTimeOffset? CreatedFrom,
        DateTimeOffset? CreatedTo,
        double? FileSizeMin,
        double? FileSizeMax,
        string? TradingPartner,
        Guid? SourceLocationId);

    private sealed record FxferServicePagedResult<T>(T[] Items, int RowCount, int CurrentPage, int PageSize);
}

using System.Security.Cryptography;
using System.Text;
using Fxfer.DomainApi.Api;
using Fxfer.DomainApi.Contracts.Utilities;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;

namespace Fxfer.DomainApi.Api.Middleware;

public sealed class IdempotencyMiddleware
{
    // T-004: Support idempotent command submission for operator retries.

    private const string IdempotencyKeyHeaderName = "Idempotency-Key";
    private static readonly TimeSpan RetentionWindow = TimeSpan.FromHours(24);

    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;

    public IdempotencyMiddleware(RequestDelegate next, IMemoryCache cache)
    {
        _next = next;
        _cache = cache;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/api/fxfer-domain/v1", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        if (!HttpMethods.IsPost(context.Request.Method)
            && !HttpMethods.IsPut(context.Request.Method)
            && !HttpMethods.IsPatch(context.Request.Method)
            && !HttpMethods.IsDelete(context.Request.Method))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(IdempotencyKeyHeaderName, out var values))
        {
            await _next(context);
            return;
        }

        var idempotencyKey = values.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            await _next(context);
            return;
        }

        var tenantId = RequestContext.GetTenantId(context);
        if (string.IsNullOrWhiteSpace(tenantId))
        {
            await _next(context);
            return;
        }

        var commandKind = GetCommandKind(context);
        var cacheKey = new IdempotencyCacheKey(tenantId, commandKind, idempotencyKey);
        var payloadHash = await ComputePayloadHashAsync(context);

        // shortcut: cache is in-memory/per-instance; upgrade to distributed storage for multi-node deployments.
        var entry = _cache.GetOrCreate(cacheKey, cacheEntry =>
        {
            cacheEntry.AbsoluteExpirationRelativeToNow = RetentionWindow;
            return new IdempotencyEntry(payloadHash);
        });

        if (!string.Equals(entry.PayloadHash, payloadHash, StringComparison.Ordinal))
        {
            await FxferProblemDetails.WriteAsync(
                context,
                statusCode: StatusCodes.Status409Conflict,
                title: "Idempotency-Key conflict",
                detail: "Idempotency-Key has already been used with a different request payload.");
            return;
        }

        await entry.Mutex.WaitAsync(context.RequestAborted);
        try
        {
            if (entry.CachedResponse is not null)
            {
                await WriteCachedResponseAsync(context, entry.CachedResponse);
                return;
            }

            var response = await CaptureResponseAsync(context);
            if (IsCacheable(response.StatusCode))
            {
                entry.CachedResponse = response;
            }
        }
        finally
        {
            entry.Mutex.Release();
        }
    }

    private static bool IsCacheable(int statusCode) => statusCode is >= 200 and <= 299;

    private static string GetCommandKind(HttpContext context)
    {
        var route = context.GetEndpoint() as RouteEndpoint;
        var rawPattern = route?.RoutePattern.RawText;
        var path = context.Request.Path.Value;

        var selector = rawPattern ?? path ?? string.Empty;
        return $"{context.Request.Method} {selector}";
    }

    private static async Task<string> ComputePayloadHashAsync(HttpContext context)
    {
        context.Request.EnableBuffering();

        byte[] bodyBytes;
        using (var buffer = new MemoryStream())
        {
            await context.Request.Body.CopyToAsync(buffer, context.RequestAborted);
            bodyBytes = buffer.ToArray();
        }

        context.Request.Body.Position = 0;

        var prefix = Encoding.UTF8.GetBytes($"{context.Request.Method}\n{context.Request.Path}{context.Request.QueryString}\n");

        using var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        hash.AppendData(prefix);
        hash.AppendData(bodyBytes);
        return Convert.ToHexString(hash.GetHashAndReset());
    }

    private async Task<IdempotencyResponse> CaptureResponseAsync(HttpContext context)
    {
        var originalBody = context.Response.Body;
        await using var buffer = new MemoryStream();

        context.Response.Body = buffer;
        try
        {
            await _next(context);
        }
        finally
        {
            context.Response.Body = originalBody;
        }

        var body = buffer.ToArray();
        await originalBody.WriteAsync(body, context.RequestAborted);

        context.Response.Headers.TryGetValue("Location", out var location);

        return new IdempotencyResponse(
            StatusCode: context.Response.StatusCode,
            ContentType: context.Response.ContentType,
            Location: location.FirstOrDefault(),
            Body: body);
    }

    private static async Task WriteCachedResponseAsync(HttpContext context, IdempotencyResponse response)
    {
        if (!string.IsNullOrWhiteSpace(response.ContentType))
        {
            context.Response.ContentType = response.ContentType;
        }

        if (!string.IsNullOrWhiteSpace(response.Location))
        {
            context.Response.Headers["Location"] = response.Location;
        }

        context.Response.StatusCode = response.StatusCode;
        await context.Response.Body.WriteAsync(response.Body, context.RequestAborted);
    }

    private readonly record struct IdempotencyCacheKey(string TenantId, string CommandKind, string IdempotencyKey);

    private sealed class IdempotencyEntry
    {
        public IdempotencyEntry(string payloadHash)
        {
            PayloadHash = payloadHash;
        }

        public string PayloadHash { get; }

        public SemaphoreSlim Mutex { get; } = new(1, 1);

        public IdempotencyResponse? CachedResponse { get; set; }
    }

    private sealed record IdempotencyResponse(int StatusCode, string? ContentType, string? Location, byte[] Body);
}


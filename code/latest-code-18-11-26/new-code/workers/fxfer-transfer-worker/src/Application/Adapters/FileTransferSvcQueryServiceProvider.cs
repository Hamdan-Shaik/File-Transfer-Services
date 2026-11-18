using System.Net;
using System.Net.Http.Json;

using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Contracts.Batch;
using Fxfer.TransferWorker.Domain.Adapters;

namespace Fxfer.TransferWorker.Application.Adapters;

public sealed class FileTransferSvcQueryServiceProvider : IFileTransferSvcQueryServiceProvider
{
    private readonly IFxferDomainApiClient _domainApiClient;

    public FileTransferSvcQueryServiceProvider(
        IFxferDomainApiClient domainApiClient,
        IRequestContextAccessor requestContextAccessor)
    {
        _domainApiClient = domainApiClient;
        _ = requestContextAccessor;
    }

    public FtsIntegrationFileModel? GetIntegrationFile(GetIntegrationFileQuery query)
    {
        var where = query.Where;
        var queryParameters = new List<string> { "isValid=true" };

        if (where is not null)
        {
            if (where.FtsIntegrationFileId != Guid.Empty)
            {
                queryParameters.Add($"integrationFileId={Uri.EscapeDataString(where.FtsIntegrationFileId.ToString())}");
            }

            if (!string.IsNullOrWhiteSpace(where.FileName))
            {
                queryParameters.Add($"fileName={Uri.EscapeDataString(where.FileName)}");
            }
        }

        var url = "/api/fxfer-domain/v1/integration-files";
        if (queryParameters.Count > 0)
        {
            url += "?" + string.Join("&", queryParameters);
        }

        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
        using var response = _domainApiClient.SendAsync(httpRequest, CancellationToken.None).GetAwaiter().GetResult();

        if (response.StatusCode is HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var payload = response.Content.ReadFromJsonAsync<IntegrationFileDefinition[]>(CancellationToken.None)
            .GetAwaiter()
            .GetResult();

        var first = payload?.FirstOrDefault();
        if (first is null)
        {
            return null;
        }

        return new FtsIntegrationFileModel
        {
            FtsIntegrationFileId = first.FtsIntegrationFileId,
            GroupId = first.GroupId,
            ConfigId = first.ConfigId,
            FileName = first.FileName,
            ETFOutboundMetadataDefnId = first.ETFOutboundMetadataDefnId,
            IsProcessed = first.IsProcessed,
            IsValid = first.IsValid,
            CreationDate = first.CreationDate.UtcDateTime,
            LastModifiedDate = first.LastModifiedDate.UtcDateTime,
        };
    }

    private sealed record IntegrationFileDefinition
    {
        public Guid FtsIntegrationFileId { get; init; }

        public Guid? GroupId { get; init; }

        public int? ConfigId { get; init; }

        public string? FileName { get; init; }

        public Guid ETFOutboundMetadataDefnId { get; init; }

        public bool IsProcessed { get; init; }

        public bool IsValid { get; init; }

        public DateTimeOffset CreationDate { get; init; }

        public DateTimeOffset LastModifiedDate { get; init; }
    }
}


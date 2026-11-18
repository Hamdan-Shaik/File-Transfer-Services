using System.Net;
using System.Net.Http.Json;

using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Contracts.Batch;

namespace Fxfer.TransferWorker.Infrastructure.DataAccess;

public sealed class GetIntegrationFileDataAccess : DataAccessBase<FtsIntegrationFileModel?>
{
    private readonly string _fileName;

    public GetIntegrationFileDataAccess(
        IFxferDomainApiClient domainApiClient,
        IRequestContextAccessor requestContextAccessor,
        string fileName)
        : base(domainApiClient, requestContextAccessor)
    {
        _fileName = fileName;
    }

    protected override async Task<FtsIntegrationFileModel?> ExecuteQueryAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_fileName))
        {
            return null;
        }

        var url = $"/api/fxfer-domain/v1/integration-files?fileName={Uri.EscapeDataString(_fileName)}";
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
        using var response = await DomainApiClient.SendAsync(httpRequest, cancellationToken);

        if (response.StatusCode is HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<IntegrationFileDefinition[]>(cancellationToken);
        var first = payload?.FirstOrDefault();
        if (first is null)
        {
            return null;
        }

        return new FtsIntegrationFileModel
        {
            FtsIntegrationFileId = first.FtsIntegrationFileId,
            GroupId = first.GroupId,
        };
    }

    private sealed record IntegrationFileDefinition
    {
        public Guid FtsIntegrationFileId { get; init; }

        public Guid? GroupId { get; init; }
    }
}


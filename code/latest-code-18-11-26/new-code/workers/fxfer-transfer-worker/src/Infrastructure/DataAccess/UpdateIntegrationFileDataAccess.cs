using System.Net.Http.Json;

using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Contracts.Batch;

namespace Fxfer.TransferWorker.Infrastructure.DataAccess;

public sealed class UpdateIntegrationFileDataAccess : DataAccessBase<bool>
{
    private readonly UpdateIntegrationFileCommand _command;

    public UpdateIntegrationFileDataAccess(
        IFxferDomainApiClient domainApiClient,
        IRequestContextAccessor requestContextAccessor,
        UpdateIntegrationFileCommand command)
        : base(domainApiClient, requestContextAccessor)
    {
        _command = command;
    }

    protected override async Task<bool> ExecuteQueryAsync(CancellationToken cancellationToken)
    {
        var requestBody = new IntegrationFileProcessingRequest
        {
            ConfigId = _command.ConfigId,
            IsValid = _command.IsValid,
        };

        using var httpRequest = new HttpRequestMessage(HttpMethod.Patch, $"/api/fxfer-domain/v1/integration-files/{_command.FtsIntegrationFileId}")
        {
            Content = JsonContent.Create(requestBody),
        };

        using var response = await DomainApiClient.SendAsync(httpRequest, cancellationToken);
        response.EnsureSuccessStatusCode();
        return true;
    }

    private sealed record IntegrationFileProcessingRequest
    {
        public int? ConfigId { get; init; }

        public bool IsValid { get; init; }
    }
}


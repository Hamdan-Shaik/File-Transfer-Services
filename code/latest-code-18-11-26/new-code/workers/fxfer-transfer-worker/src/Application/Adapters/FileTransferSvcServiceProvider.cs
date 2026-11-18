using System.Net.Http.Json;

using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Contracts.Batch;
using Fxfer.TransferWorker.Domain.Adapters;

namespace Fxfer.TransferWorker.Application.Adapters;

public sealed class FileTransferSvcServiceProvider : IFileTransferSvcServiceProvider
{
    private readonly IFxferDomainApiClient _domainApiClient;

    public FileTransferSvcServiceProvider(
        IFxferDomainApiClient domainApiClient,
        IRequestContextAccessor requestContextAccessor)
    {
        _domainApiClient = domainApiClient;
        _ = requestContextAccessor;
    }

    public IntegrationFileUpdated UpdateIntegrationFile(UpdateIntegrationFileCommand command)
    {
        command.Validate();

        var requestBody = new IntegrationFileProcessingRequest
        {
            ConfigId = command.ConfigId!.Value,
            IsValid = command.IsValid,
        };

        using var httpRequest = new HttpRequestMessage(HttpMethod.Patch, $"/api/fxfer-domain/v1/integration-files/{command.FtsIntegrationFileId}")
        {
            Content = JsonContent.Create(requestBody),
        };

        using var response = _domainApiClient.SendAsync(httpRequest, CancellationToken.None).GetAwaiter().GetResult();
        response.EnsureSuccessStatusCode();

        var definition = response.Content.ReadFromJsonAsync<IntegrationFileDefinition>(CancellationToken.None)
            .GetAwaiter()
            .GetResult();

        if (definition is null)
        {
            throw new InvalidOperationException("Domain API returned an empty integration file response.");
        }

        return new IntegrationFileUpdated
        {
            FtsIntegrationFileId = definition.FtsIntegrationFileId,
            ConfigId = definition.ConfigId ?? command.ConfigId,
            FileName = !string.IsNullOrWhiteSpace(definition.FileName) ? definition.FileName : command.FileName ?? string.Empty,
            GroupInfo = command.GroupInfo ?? [],
            ETFOutboundMetadataId = definition.ETFOutboundMetadataDefnId != Guid.Empty
                ? definition.ETFOutboundMetadataDefnId
                : command.ETFOutboundMetadataDefnId,
            IsProcessed = definition.IsProcessed,
            IsValid = definition.IsValid,
            LastModifiedDate = definition.LastModifiedDate.UtcDateTime,
            OperatorId = command.Requestor?.IdentifierId ?? string.Empty,
        };
    }

    private sealed record IntegrationFileProcessingRequest
    {
        public required int ConfigId { get; init; }

        public bool IsValid { get; init; }
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

        public DateTimeOffset LastModifiedDate { get; init; }
    }
}


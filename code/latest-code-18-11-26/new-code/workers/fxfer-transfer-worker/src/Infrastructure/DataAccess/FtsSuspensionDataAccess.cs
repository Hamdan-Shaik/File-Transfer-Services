using System.Net;
using System.Net.Http.Json;

using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Contracts.Batch;

namespace Fxfer.TransferWorker.Infrastructure.DataAccess;

public sealed class FtsSuspensionDataAccess : DataAccessBase<FtsSuspensionModel>
{
    private readonly FtsSuspensionSearchCriteria _query;

    public FtsSuspensionDataAccess(
        IFxferDomainApiClient domainApiClient,
        IRequestContextAccessor requestContextAccessor,
        FtsSuspensionSearchCriteria searchQuery)
        : base(domainApiClient, requestContextAccessor)
    {
        _query = searchQuery;
    }

    protected override async Task<FtsSuspensionModel> ExecuteQueryAsync(CancellationToken cancellationToken)
    {
        var suspensionModel = new FtsSuspensionModel();

        var hasSourceLocation = await GetSourceLocation(cancellationToken);
        if (hasSourceLocation)
        {
            await UpdateSourceLocation(cancellationToken);
        }

        var entitySourceLocation = CreateSourceLocationEntityData();
        await AddEntitySourceLocation(entitySourceLocation, cancellationToken);

        suspensionModel.SuspensionId = entitySourceLocation.SuspensionId;
        return suspensionModel;
    }

    private async Task<bool> GetSourceLocation(CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            "/api/fxfer-domain/v1/transfer-configurations/" + this._query.SourceLocationId);

        using var response = await DomainApiClient.SendAsync(request, cancellationToken);

        if (response.StatusCode is HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();
        return true;
    }

    private async Task UpdateSourceLocation(CancellationToken cancellationToken)
    {
        TransferConfigurationPatchRequest updateRequestBody = new TransferConfigurationPatchRequest
        {
            Suspended = true,
            SuspendedChangeDate = DateTimeOffset.UtcNow,
        };

        using var request = new HttpRequestMessage(
            HttpMethod.Patch,
            "/api/fxfer-domain/v1/transfer-configurations/" + this._query.SourceLocationId)
        {
            Content = JsonContent.Create(updateRequestBody)
        };

        using var response = await DomainApiClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    private async Task AddEntitySourceLocation(SuspensionCreateRequest entitySourceLocation, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/fxfer-domain/v1/transfer-configurations/" + this._query.SourceLocationId + "/suspensions")
        {
            Content = JsonContent.Create(entitySourceLocation)
        };

        using var response = await DomainApiClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    private SuspensionCreateRequest CreateSourceLocationEntityData()
    {
        return new SuspensionCreateRequest
        {
            SuspensionId = Guid.NewGuid(),
            SourceLocationId = _query.SourceLocationId,
            ErrorText = _query.ErrorText,
            StackTrace = _query.StackTrace,
            OperatorId = null,
            LastModifiedTs = DateTime.UtcNow,
            CreatedTs = DateTime.UtcNow,
        };
    }

    private sealed class SuspensionCreateRequest
    {
        public Guid SuspensionId { get; init; }
        public Guid SourceLocationId { get; init; }
        public string? ErrorText { get; init; }
        public string? StackTrace { get; init; }
        public string? OperatorId { get; init; }
        public DateTime LastModifiedTs { get; init; }
        public DateTime CreatedTs { get; init; }
    }

    private sealed class TransferConfigurationPatchRequest
    {
        public bool Suspended { get; init; }
        public DateTimeOffset SuspendedChangeDate { get; init; }
    }
}


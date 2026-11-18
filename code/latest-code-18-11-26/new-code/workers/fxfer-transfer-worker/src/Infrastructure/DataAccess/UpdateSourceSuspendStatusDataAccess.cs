using System.Net;
using System.Net.Http.Json;

using Fxfer.TransferWorker.Application.DataAccess;

namespace Fxfer.TransferWorker.Infrastructure.DataAccess;

public sealed class UpdateSourceSuspendStatusDataAccess : DataAccessBase<bool>
{
    private readonly Guid _sourceLocationId;

    public UpdateSourceSuspendStatusDataAccess(
        IFxferDomainApiClient domainApiClient,
        IRequestContextAccessor requestContextAccessor,
        Guid sourceLocationId)
        : base(domainApiClient, requestContextAccessor)
    {
        _sourceLocationId = sourceLocationId;
    }

    protected override async Task<bool> ExecuteQueryAsync(CancellationToken cancellationToken)
    {
        var requestBody = new TransferConfigurationPatchRequest
        {
            Suspended = false,
            SuspendedChangeDate = DateTimeOffset.UtcNow,
        };

        using var httpRequest = new HttpRequestMessage(HttpMethod.Patch, $"/api/fxfer-domain/v1/transfer-configurations/{_sourceLocationId}")
        {
            Content = JsonContent.Create(requestBody),
        };

        using var response = await DomainApiClient.SendAsync(httpRequest, cancellationToken);

        if (response.StatusCode is HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();

        return true;
    }

    private sealed class TransferConfigurationPatchRequest
    {
        public bool Suspended { get; init; }
        public DateTimeOffset SuspendedChangeDate { get; init; }
    }
}


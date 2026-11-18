using System.Net;
using System.Net.Http.Json;

using Fxfer.TransferWorker.Application.DataAccess;

namespace Fxfer.TransferWorker.Infrastructure.DataAccess;

public sealed class UpdateFileManagementStatusDataAccess : DataAccessBase<Guid>
{
    private readonly Guid _fileId;
    private readonly string _fileStatus;

    public UpdateFileManagementStatusDataAccess(
        IFxferDomainApiClient domainApiClient,
        IRequestContextAccessor requestContextAccessor,
        Guid fileId,
        string fileStatus)
        : base(domainApiClient, requestContextAccessor)
    {
        _fileId = fileId;
        _fileStatus = fileStatus;
    }

    protected override async Task<Guid> ExecuteQueryAsync(CancellationToken cancellationToken)
    {
        var requestBody = new FileTrackingPatchRequest
        {
            FileStatus = _fileStatus,
            FileStatusTs = DateTime.UtcNow,
            LastModifiedTs = DateTime.UtcNow,
        };

        using var httpRequest = new HttpRequestMessage(HttpMethod.Patch, $"/api/fxfer-domain/v1/file-tracking/{_fileId}")
        {
            Content = JsonContent.Create(requestBody),
        };

        using var response = await DomainApiClient.SendAsync(httpRequest, cancellationToken);

        if (response.StatusCode is HttpStatusCode.NotFound)
        {
            return _fileId;
        }

        response.EnsureSuccessStatusCode();

        return _fileId;
    }

    private sealed class FileTrackingPatchRequest
    {
        public string? FileStatus { get; init; }
        public DateTime FileStatusTs { get; init; }
        public DateTime LastModifiedTs { get; init; }
    }
}


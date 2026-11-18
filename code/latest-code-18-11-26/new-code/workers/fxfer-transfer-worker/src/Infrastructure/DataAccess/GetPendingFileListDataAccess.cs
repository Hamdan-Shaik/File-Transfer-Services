using System.Net.Http.Json;

using Fxfer.TransferWorker.Application.Constants;
using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Contracts.Batch;

namespace Fxfer.TransferWorker.Infrastructure.DataAccess;

public sealed class GetPendingFileListDataAccess : DataAccessBase<List<PendingFileModel>>
{
    private const int FileTrackingPageSize = 2000;

    public GetPendingFileListDataAccess(IFxferDomainApiClient domainApiClient, IRequestContextAccessor requestContextAccessor)
        : base(domainApiClient, requestContextAccessor)
    {
    }

    protected override async Task<List<PendingFileModel>> ExecuteQueryAsync(CancellationToken cancellationToken)
    {
        using var httpRequest = new HttpRequestMessage(
            HttpMethod.Get,
            $"/api/fxfer-domain/v1/file-tracking?page=1&pageSize={FileTrackingPageSize}");

        using var response = await DomainApiClient.SendAsync(httpRequest, cancellationToken);
        response.EnsureSuccessStatusCode();

        var fileTrackingPage = await response.Content.ReadFromJsonAsync<PagedResult<FileTrackingItem>>(cancellationToken);

        var pendingFiles = new List<PendingFileModel>();
        if (fileTrackingPage?.Items is null)
        {
            return pendingFiles;
        }

        DateTime currentDate = DateTime.UtcNow.AddMinutes(-1);
        foreach (var item in fileTrackingPage.Items)
        {
            if (item.FileId == Guid.Empty)
            {
                continue;
            }

            var compareCompleted = string.Compare(item.FileStatus, FileStatusConstant.Completed, StringComparison.InvariantCultureIgnoreCase);
            var statusDateUtc = item.FileStatusTs.UtcDateTime;

            if (compareCompleted != 0 || (compareCompleted == 0 && statusDateUtc > currentDate))
            {
                pendingFiles.Add(
                    new PendingFileModel
                    {
                        FileId = item.FileId,
                        FileSourceLocationId = item.FileSourceLocationId,
                        FileName = item.FileName,
                        FileStatus = item.FileStatus,
                        FileTrackId = item.FileTrackId,
                        StatusDate = statusDateUtc,
                    });
            }
        }

        return pendingFiles;
    }

    private sealed record PagedResult<T>
    {
        public List<T>? Items { get; init; }
    }

    private sealed record FileTrackingItem
    {
        public Guid FileId { get; init; }
        public Guid FileSourceLocationId { get; init; }
        public string? FileName { get; init; }
        public string? FileStatus { get; init; }
        public int? FileTrackId { get; init; }
        public DateTimeOffset FileStatusTs { get; init; }
    }
}


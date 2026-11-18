using System.Net;
using System.Net.Http.Json;

using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Contracts.Batch;
using Fxfer.TransferWorker.Infrastructure.DataAccess;

namespace Fxfer.TransferWorker.Tests;

public sealed class BackendDataAccessBatchDataAccessPart2Tests
{
    [Fact]
    public async Task GetConfigMetaDataDataAccess_filters_out_delivered_destinations_except_archives()
    {
        var fileId = Guid.NewGuid();
        var sourceLocationId = Guid.NewGuid();
        var deliveredDestinationId = Guid.NewGuid();
        var deliveredArchiveDestinationId = Guid.NewGuid();
        var pendingDestinationId = Guid.NewGuid();
        var invalidDestinationId = Guid.NewGuid();

        var sourceFtpServerId = Guid.NewGuid();
        var destinationFtpServerId = Guid.NewGuid();

        var api = new FakeDomainApiClient(request =>
        {
            var uri = request.RequestUri!.ToString();

            if (request.Method == HttpMethod.Get && uri == "/api/fxfer-domain/v1/file-tracking/" + fileId)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(new
                    {
                        fileSourceLocationId = sourceLocationId,
                        events = new[]
                        {
                            new { destinationId = deliveredDestinationId },
                            new { destinationId = deliveredArchiveDestinationId },
                        }
                    })
                });
            }

            if (request.Method == HttpMethod.Get && uri == "/api/fxfer-domain/v1/transfer-configurations/" + sourceLocationId)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(new
                    {
                        ftsSourceLocationId = sourceLocationId,
                        configId = 101,
                        protocol = "1",
                        address = "source",
                        sendNotification = true,
                        ftpServerId = sourceFtpServerId,
                        destinations = new object[]
                        {
                            new
                            {
                                destinationId = deliveredDestinationId,
                                destinationAddress = "delivered",
                                destinationProtocolCode = "1",
                                isValid = true,
                                destIsArchive = false,
                            },
                            new
                            {
                                destinationId = deliveredArchiveDestinationId,
                                destinationAddress = "archive",
                                destinationProtocolCode = "1",
                                isValid = true,
                                destIsArchive = true,
                            },
                            new
                            {
                                destinationId = pendingDestinationId,
                                destinationAddress = "pending",
                                destinationProtocolCode = "1",
                                isValid = true,
                                destIsArchive = false,
                                ftpServerId = destinationFtpServerId,
                            },
                            new
                            {
                                destinationId = invalidDestinationId,
                                destinationAddress = "invalid",
                                destinationProtocolCode = "1",
                                isValid = false,
                                destIsArchive = false,
                            }
                        }
                    })
                });
            }

            if (request.Method == HttpMethod.Get && uri == "/api/fxfer-domain/v1/ftp-servers/" + sourceFtpServerId)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(new
                    {
                        ftpServerId = sourceFtpServerId,
                        ftpServerName = "source-ftp",
                        ftpServerIp = "1.2.3.4",
                        ftpTransferMode = "Passive",
                        ftpUser = "u",
                        ftpPassword = "p"
                    })
                });
            }

            if (request.Method == HttpMethod.Get && uri == "/api/fxfer-domain/v1/ftp-servers/" + destinationFtpServerId)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(new
                    {
                        ftpServerId = destinationFtpServerId,
                        ftpServerName = "dest-ftp",
                        ftpServerIp = "5.6.7.8",
                        ftpTransferMode = "Passive",
                        ftpUser = "du",
                        ftpPassword = "dp"
                    })
                });
            }

            throw new InvalidOperationException("Unexpected request: " + request.Method + " " + uri);
        });

        var sut = new GetConfigMetaDataDataAccess(api, new NullRequestContextAccessor(), fileId);
        var result = await sut.ExecuteAsync(CancellationToken.None);

        Assert.NotNull(result);
        var source = result!;

        Assert.Equal(sourceLocationId, source.SourceLocationId);
        Assert.Equal(101, source.ConfigurationId);
        Assert.Equal("1", source.SourceProtocolCode);
        Assert.Equal("source", source.SourceAddress);
        Assert.True(source.SendNotificationsIndicator);
        Assert.NotNull(source.FtpServerDetail);
        Assert.Equal(sourceFtpServerId, source.FtpServerDetail!.FtpServerId);

        var destinationIds = source.Destinations.Select(x => x.DestinationId).ToList();
        Assert.Equal(2, destinationIds.Count);
        Assert.Contains(deliveredArchiveDestinationId, destinationIds);
        Assert.Contains(pendingDestinationId, destinationIds);

        var pendingDestination = source.Destinations.Single(x => x.DestinationId == pendingDestinationId);
        Assert.NotNull(pendingDestination.FtpServerDetail);
        Assert.Equal(destinationFtpServerId, pendingDestination.FtpServerDetail!.FtpServerId);
    }

    [Fact]
    public async Task GetPendingFileListDataAccess_filters_completed_files_using_status_timestamp_cutoff()
    {
        var pendingId = Guid.NewGuid();
        var completedOldId = Guid.NewGuid();
        var completedRecentId = Guid.NewGuid();

        var api = new FakeDomainApiClient(request =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Equal("/api/fxfer-domain/v1/file-tracking?page=1&pageSize=2000", request.RequestUri!.ToString());

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new
                {
                    items = new object[]
                    {
                        new
                        {
                            fileId = pendingId,
                            fileSourceLocationId = Guid.NewGuid(),
                            fileName = "pending.txt",
                            fileStatus = "3",
                            fileTrackId = 10,
                            fileStatusTs = DateTimeOffset.UtcNow.AddMinutes(-20),
                        },
                        new
                        {
                            fileId = completedOldId,
                            fileSourceLocationId = Guid.NewGuid(),
                            fileName = "completed-old.txt",
                            fileStatus = "1",
                            fileTrackId = 20,
                            fileStatusTs = DateTimeOffset.UtcNow.AddMinutes(-20),
                        },
                        new
                        {
                            fileId = completedRecentId,
                            fileSourceLocationId = Guid.NewGuid(),
                            fileName = "completed-recent.txt",
                            fileStatus = "1",
                            fileTrackId = 30,
                            fileStatusTs = DateTimeOffset.UtcNow.AddSeconds(10),
                        }
                    }
                })
            });
        });

        var sut = new GetPendingFileListDataAccess(api, new NullRequestContextAccessor());
        var result = await sut.ExecuteAsync(CancellationToken.None);

        var fileIds = result.Select(x => x.FileId).ToList();
        Assert.Equal(2, fileIds.Count);
        Assert.Contains(pendingId, fileIds);
        Assert.Contains(completedRecentId, fileIds);
        Assert.DoesNotContain(completedOldId, fileIds);
    }

    [Fact]
    public async Task GetSuspendedLocationDataAccess_returns_null_when_transfer_configuration_not_found()
    {
        var sourceLocationId = Guid.NewGuid();

        var api = new FakeDomainApiClient(request =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Equal("/api/fxfer-domain/v1/transfer-configurations/" + sourceLocationId, request.RequestUri!.ToString());
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        });

        var sut = new GetSuspendedLocationDataAccess(api, new NullRequestContextAccessor(), sourceLocationId);
        var result = await sut.ExecuteAsync(CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateFileManagementStatusDataAccess_returns_file_id_when_tracking_not_found()
    {
        var fileId = Guid.NewGuid();
        var api = new FakeDomainApiClient(request =>
        {
            Assert.Equal(HttpMethod.Patch, request.Method);
            Assert.Equal("/api/fxfer-domain/v1/file-tracking/" + fileId, request.RequestUri!.ToString());
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        });

        var sut = new UpdateFileManagementStatusDataAccess(api, new NullRequestContextAccessor(), fileId, fileStatus: "1");
        var result = await sut.ExecuteAsync(CancellationToken.None);

        Assert.Equal(fileId, result);
    }

    [Fact]
    public async Task UpdateSourceSuspendStatusDataAccess_returns_false_when_source_not_found()
    {
        var sourceLocationId = Guid.NewGuid();
        var api = new FakeDomainApiClient(request =>
        {
            Assert.Equal(HttpMethod.Patch, request.Method);
            Assert.Equal("/api/fxfer-domain/v1/transfer-configurations/" + sourceLocationId, request.RequestUri!.ToString());
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        });

        var sut = new UpdateSourceSuspendStatusDataAccess(api, new NullRequestContextAccessor(), sourceLocationId);
        var result = await sut.ExecuteAsync(CancellationToken.None);

        Assert.False(result);
    }

    [Fact]
    public async Task QueryFtsNotificationDataAccess_filters_notifications_using_legacy_branches()
    {
        var configId = Guid.NewGuid();

        var api = new FakeDomainApiClient(request =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Equal($"/api/fxfer-domain/v1/transfer-configurations/{configId}/notifications", request.RequestUri!.ToString());

            var payload = new List<FtsNotificationModel>
            {
                new() { ConfigId = configId, EmailAddress = "a", NotifyOnErrorInd = true, NotifyOnSuccessInd = false },
                new() { ConfigId = configId, EmailAddress = "b", NotifyOnErrorInd = false, NotifyOnSuccessInd = true },
                new() { ConfigId = configId, EmailAddress = "c", NotifyOnErrorInd = true, NotifyOnSuccessInd = true },
            };

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = JsonContent.Create(payload) });
        });

        var sut = new QueryFtsNotificationDataAccess(
            api,
            new NullRequestContextAccessor(),
            new GetNotificationSearchCriteria { ConfigId = configId, NotifyOnErrorInd = true, NotifyOnSuccessInd = true });

        var result = await sut.ExecuteAsync(CancellationToken.None);

        Assert.Single(result);
        Assert.Equal("c", result[0].EmailAddress);
    }

    private sealed class FakeDomainApiClient : IFxferDomainApiClient
    {
        private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _handler;

        public FakeDomainApiClient(Func<HttpRequestMessage, Task<HttpResponseMessage>> handler)
        {
            _handler = handler;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _handler(request);
        }
    }

    private sealed class NullRequestContextAccessor : IRequestContextAccessor
    {
        public string? CorrelationId => null;

        public string? TenantId => null;
    }
}


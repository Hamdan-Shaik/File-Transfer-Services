using System.Net;
using System.Net.Http.Json;

using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Contracts.Batch;

namespace Fxfer.TransferWorker.Infrastructure.DataAccess;

public sealed class GetConfigMetaDataDataAccess : DataAccessBase<SourceLocationModel?>
{
    private readonly Guid _fileId;

    public GetConfigMetaDataDataAccess(
        IFxferDomainApiClient domainApiClient,
        IRequestContextAccessor requestContextAccessor,
        Guid fileId)
        : base(domainApiClient, requestContextAccessor)
    {
        _fileId = fileId;
    }

    protected override async Task<SourceLocationModel?> ExecuteQueryAsync(CancellationToken cancellationToken)
    {
        var domainApiClient = DomainApiClient;

        var fileTracking = await GetFileTrackingAsync(domainApiClient, cancellationToken);
        if (fileTracking is null || fileTracking.FileSourceLocationId == Guid.Empty)
        {
            // Legacy parity: return null when the file management record is missing.
            return null;
        }

        var configuration = await GetTransferConfigurationAsync(domainApiClient, fileTracking.FileSourceLocationId, cancellationToken);
        if (configuration is null)
        {
            return null;
        }

        var ftpServer = new FtpServer();
        if (configuration.FtpServerId is Guid ftpServerId && ftpServerId != Guid.Empty)
        {
            ftpServer = await GetFtpServerAsync(domainApiClient, ftpServerId, cancellationToken) ?? ftpServer;
        }

        var source = new SourceLocationModel
        {
            SourceLocationId = configuration.FtsSourceLocationId,
            ConfigurationId = configuration.ConfigId,
            SourceProtocolCode = configuration.Protocol,
            SourceAddress = configuration.Address,
            SendNotificationsIndicator = configuration.SendNotification ?? false,
            FtpServerDetail = new FtpServerModel
            {
                FtpServerId = ftpServer.FtpServerId,
                FtpServerName = ftpServer.FtpServerName,
                FtpServerIp = ftpServer.FtpServerIp,
                FtpTransferMode = ftpServer.FtpTransferMode,
                FtpUser = ftpServer.FtpUser,
                FtpPassword = ftpServer.FtpPassword,
                FtpPort = ftpServer.FtpPort,
                SSHAuth = ftpServer.SSHAuth
            }
        };

        var destinations = await GetDestinationsAsync(domainApiClient, fileTracking, configuration, cancellationToken);
        source.Destinations.AddRange(destinations);

        return source;
    }

    private async Task<FileTrackingResponse?> GetFileTrackingAsync(IFxferDomainApiClient domainApiClient, CancellationToken cancellationToken)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/fxfer-domain/v1/file-tracking/{_fileId}");
        using var response = await domainApiClient.SendAsync(httpRequest, cancellationToken);

        if (response.StatusCode is HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<FileTrackingResponse>(cancellationToken);
    }

    private static async Task<TransferConfiguration?> GetTransferConfigurationAsync(
        IFxferDomainApiClient domainApiClient,
        Guid sourceLocationId,
        CancellationToken cancellationToken)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/fxfer-domain/v1/transfer-configurations/{sourceLocationId}");
        using var response = await domainApiClient.SendAsync(httpRequest, cancellationToken);

        if (response.StatusCode is HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TransferConfiguration>(cancellationToken);
    }

    private static async Task<FtpServer?> GetFtpServerAsync(IFxferDomainApiClient domainApiClient, Guid ftpServerId, CancellationToken cancellationToken)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/fxfer-domain/v1/ftp-servers/{ftpServerId}");
        using var response = await domainApiClient.SendAsync(httpRequest, cancellationToken);

        if (response.StatusCode is HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<FtpServer>(cancellationToken);
    }

    private static async Task<List<DestinationModel>> GetDestinationsAsync(
        IFxferDomainApiClient domainApiClient,
        FileTrackingResponse fileTracking,
        TransferConfiguration configuration,
        CancellationToken cancellationToken)
    {
        var deliveredDestinationIds = fileTracking.Events
            .Where(x => x.DestinationId != Guid.Empty)
            .Select(x => x.DestinationId)
            .Distinct()
            .ToHashSet();

        var destinations = new List<DestinationModel>();
        var ftpServerCache = new Dictionary<Guid, FtpServerModel?>();

        foreach (var item in configuration.Destinations)
        {
            if (item.IsValid != true)
            {
                continue;
            }

            if (deliveredDestinationIds.Contains(item.DestinationId) && item.DestIsArchive != true)
            {
                continue;
            }

            var destination = new DestinationModel
            {
                DestinationId = item.DestinationId,
                DestinationAddress = item.DestinationAddress,
                DestinationProtocolCode = item.DestinationProtocolCode,
                IsOverwriteAtDestination = item.IsOverwriteAtDestination ?? false,
                IsCompressAtDestination = item.IsCompressAtDestination ?? false,
                IsRenameAtDestination = item.IsRenameAtDestination ?? false,
                IsArchive = item.DestIsArchive ?? false,
                RenamePattern = item.RenamePattern,
                EncryptionCode = item.EncryptionCode,
                EncryptionPublicKey = item.EncryptionPublicKey,
                EncryptionPrivateKey = item.EncryptionPrivateKey,
                PrivateKeyPassword = item.PrivateKeyPassword
            };

            if (item.FtpServerId is Guid ftpServerId && ftpServerId != Guid.Empty)
            {
                if (!ftpServerCache.TryGetValue(ftpServerId, out var ftpServer))
                {
                    var ftpServerEntity = await GetFtpServerAsync(domainApiClient, ftpServerId, cancellationToken);
                    ftpServer = ftpServerEntity is null
                        ? null
                        : new FtpServerModel
                        {
                            FtpServerId = ftpServerEntity.FtpServerId,
                            FtpServerName = ftpServerEntity.FtpServerName,
                            FtpServerIp = ftpServerEntity.FtpServerIp,
                            FtpTransferMode = ftpServerEntity.FtpTransferMode,
                            FtpUser = ftpServerEntity.FtpUser,
                            FtpPassword = ftpServerEntity.FtpPassword,
                            FtpPort = ftpServerEntity.FtpPort,
                            SSHAuth = ftpServerEntity.SSHAuth
                        };

                    ftpServerCache[ftpServerId] = ftpServer;
                }

                destination.FtpServerDetail = ftpServer;
            }

            destinations.Add(destination);
        }

        return destinations;
    }

    private sealed record FileTrackingResponse
    {
        public Guid FileSourceLocationId { get; init; }

        public IReadOnlyList<FileEvent> Events { get; init; } = Array.Empty<FileEvent>();
    }

    private sealed record FileEvent
    {
        public Guid DestinationId { get; init; }
    }

    private sealed record TransferConfiguration
    {
        public Guid FtsSourceLocationId { get; init; }
        public long ConfigId { get; init; }
        public Guid? FtpServerId { get; init; }
        public string? Protocol { get; init; }
        public string? Address { get; init; }
        public bool? SendNotification { get; init; }
        public IReadOnlyList<DestinationLocation> Destinations { get; init; } = Array.Empty<DestinationLocation>();
    }

    private sealed record DestinationLocation
    {
        public Guid DestinationId { get; init; }
        public string? DestinationAddress { get; init; }
        public string? DestinationProtocolCode { get; init; }
        public bool? IsOverwriteAtDestination { get; init; }
        public bool? IsCompressAtDestination { get; init; }
        public bool? IsRenameAtDestination { get; init; }
        public bool? DestIsArchive { get; init; }
        public string? RenamePattern { get; init; }
        public string? EncryptionCode { get; init; }
        public string? EncryptionPublicKey { get; init; }
        public string? EncryptionPrivateKey { get; init; }
        public string? PrivateKeyPassword { get; init; }
        public Guid? FtpServerId { get; init; }
        public bool? IsValid { get; init; }
    }

    private sealed record FtpServer
    {
        public Guid FtpServerId { get; init; }
        public string? FtpServerName { get; init; }
        public string? FtpServerIp { get; init; }
        public string? FtpTransferMode { get; init; }
        public string? FtpUser { get; init; }
        public string? FtpPassword { get; init; }

        public int FtpPort { get; init; }

        public bool SSHAuth { get; init; }
    }
}

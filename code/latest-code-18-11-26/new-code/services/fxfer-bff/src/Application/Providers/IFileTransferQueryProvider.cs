using Fxfer.Bff.Contracts.Common;
using Fxfer.Bff.Contracts.FtpServers;
using Fxfer.Bff.Contracts.Tracking;
using Fxfer.Bff.Contracts.TransferConfigurations;

namespace Fxfer.Bff.Application.Providers;

public interface IFileTransferQueryProvider
{
    Task<PagedResult<TransferConfigurationSummary>> SearchFileTransferAsync(TransferConfigurationSearchParams search, CancellationToken ct);
    Task<TransferConfiguration?> GetFileTransferConfigurationInformationAsync(Guid sourceLocationId, CancellationToken ct);
    Task<IReadOnlyList<FtpServerOption>> GetFtpServersOptionsAsync(bool isValid, CancellationToken ct);
    Task<PagedResult<FtpServer>> GetFtpServersAsync(FtpServerSearchParams search, bool removePassword, CancellationToken ct);
    Task<IReadOnlyList<InboundTargetMetadata>> GetMetadataIdsAsync(string? contractName, CancellationToken ct);
    Task<IReadOnlyList<SourceLocationOption>> GetSourceLocationsAsync(bool isActive, CancellationToken ct);
    Task<PagedResult<FileTransferTracking>> SearchFileTransferTrackingAsync(FileTrackingSearchParams search, CancellationToken ct);
    Task<FileTransferTracking?> GetFileTransferTrackingDetailAsync(Guid fileId, CancellationToken ct);
}


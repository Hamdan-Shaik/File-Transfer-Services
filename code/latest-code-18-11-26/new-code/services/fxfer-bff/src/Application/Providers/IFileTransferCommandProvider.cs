using Fxfer.Bff.Contracts.FtpServers;
using Fxfer.Bff.Contracts.TransferConfigurations;

namespace Fxfer.Bff.Application.Providers;

public interface IFileTransferCommandProvider
{
    Task<TransferConfiguration> AddFileTransferBaseConfigurationAsync(TransferConfigurationUpsertRequest request, CancellationToken ct);
    Task<TransferConfiguration> UpdateFileTransferBaseConfigurationAsync(Guid sourceLocationId, TransferConfigurationUpsertRequest request, CancellationToken ct);
    Task InvalidateFileTransferBaseConfigurationAsync(Guid sourceLocationId, CancellationToken ct);
    Task<FtpServer> AddFtpServerAsync(FtpServerUpsertRequest request, CancellationToken ct);
    Task<FtpServer> UpdateFtpServerAsync(Guid ftpServerId, FtpServerUpsertRequest request, CancellationToken ct);
    Task<FtpServer> InvalidateFtpServerAsync(Guid ftpServerId, FtpServerPatchRequest request, CancellationToken ct);
}


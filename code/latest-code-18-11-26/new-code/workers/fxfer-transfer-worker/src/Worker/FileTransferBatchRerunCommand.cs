using System.Diagnostics;
using Fxfer.TransferWorker.Domain.Batch;
using Fxfer.TransferWorker.Domain.Constants;
using Fxfer.TransferWorker.Worker.Services;
using Microsoft.Extensions.Logging;

namespace Fxfer.TransferWorker.Worker;

public sealed class FileTransferBatchRerunCommand
{
    private static readonly ActivitySource ActivitySource = new("Fxfer.TransferWorker.FileTransferBatchRerunCommand");

    private readonly ILogger<FileTransferBatchRerunCommand> _logger;
    private readonly IWorkerBatchRuntimeFactory _runtimeFactory;

    public FileTransferBatchRerunCommand(
        ILogger<FileTransferBatchRerunCommand> logger,
        IWorkerBatchRuntimeFactory runtimeFactory)
    {
        _logger = logger;
        _runtimeFactory = runtimeFactory;
    }

    public static FileTransferBatchRerunCommand CreateDefault()
    {
        var loggerFactory = LoggerFactory.Create(logging =>
        {
            logging.AddSimpleConsole();
            logging.SetMinimumLevel(LogLevel.Information);
        });

        return new FileTransferBatchRerunCommand(
            loggerFactory.CreateLogger<FileTransferBatchRerunCommand>(),
            new WorkerBatchRuntimeFactory());
    }

    public int Run(Guid tenantId, Guid trackedFileId)
    {
        // T-007: Provide privileged rerun execution via a CLI mode targeting a specific tenant and tracked file.
        using var activity = ActivitySource.StartActivity("file_transfer_batch.rerun", ActivityKind.Internal);

        if (tenantId == Guid.Empty || trackedFileId == Guid.Empty)
        {
            Console.Error.WriteLine("Invalid args. Usage: fxfer-transfer-worker rerun-tracked-file --tenant-id <uuid> --tracked-file-id <uuid>");
            return -1;
        }

        try
        {
            using var runtime = _runtimeFactory.Create();
            runtime.Initialize();

            var correlationId = Guid.NewGuid().ToString();
            runtime.Services.GetService<CorrelationIdContext>()?.Set(correlationId);
            runtime.Services.GetService<TenantIdContext>()?.Set(tenantId.ToString());

            Activity.Current?.SetTag("gainwell.correlation_id", correlationId);
            Activity.Current?.SetTag("gainwell.tenant.id", tenantId.ToString());

            var args = new[]
            {
                BatchJobConstants.CommandSakTrack,
                trackedFileId.ToString(),
                tenantId.ToString()
            };

            var executor = runtime.ResolveBatchExecutor(IocConstants.FileTransferExecutor);
            return executor.Execute(args);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{BatchSource}: {Message}", ExceptionConstants.BatchSource, ex.Message);
            return -1;
        }
    }
}

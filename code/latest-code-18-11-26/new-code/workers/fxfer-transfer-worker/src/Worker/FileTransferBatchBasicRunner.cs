using System.Diagnostics;
using Fxfer.TransferWorker.Domain.Constants;
using Fxfer.TransferWorker.Worker.Services;
using Microsoft.Extensions.Logging;

namespace Fxfer.TransferWorker.Worker;

public sealed class FileTransferBatchBasicRunner
{
    private static readonly ActivitySource ActivitySource = new("Fxfer.TransferWorker.FileTransferBatchBasicRunner");

    private readonly ILogger<FileTransferBatchBasicRunner> _logger;
    private readonly IWorkerBatchRuntimeFactory _runtimeFactory;

    public FileTransferBatchBasicRunner(
        ILogger<FileTransferBatchBasicRunner> logger,
        IWorkerBatchRuntimeFactory runtimeFactory)
    {
        _logger = logger;
        _runtimeFactory = runtimeFactory;
    }

    public static FileTransferBatchBasicRunner CreateDefault(ILoggerFactory loggerFactory)
    {
        return new FileTransferBatchBasicRunner(
            loggerFactory.CreateLogger<FileTransferBatchBasicRunner>(),
            new WorkerBatchRuntimeFactory());
    }

    public int Run(string[] args)
    {
        using var activity = ActivitySource.StartActivity("file_transfer_batch.basic", ActivityKind.Internal);

        try
        {
            // F04-001: initialize container, resolve FileTransferExecutor, execute.
            using var runtime = _runtimeFactory.Create();
            runtime.Initialize();

            var correlationId = Guid.NewGuid().ToString();
            runtime.Services.GetService<CorrelationIdContext>()?.Set(correlationId);
            Activity.Current?.SetTag("gainwell.correlation_id", correlationId);

            var ftsBatchExecutor = runtime.ResolveBatchExecutor(IocConstants.FileTransferExecutor);
            return ftsBatchExecutor.Execute(args);
        }
        catch (Exception ex)
        {
            // F04-002: record message under source and return -1.
            _logger.LogError("{BatchSource}: {Message}", ExceptionConstants.BatchSource, ex.Message);
            return -1;
        }
    }
}

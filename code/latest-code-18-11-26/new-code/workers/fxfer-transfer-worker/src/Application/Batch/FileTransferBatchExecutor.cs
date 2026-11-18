using System.Collections.Concurrent;
using System.Text;

using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Contracts.Batch;
using Fxfer.TransferWorker.Domain.Batch;
using Fxfer.TransferWorker.Worker.Configuration;

using Microsoft.Extensions.Logging;

namespace Fxfer.TransferWorker.Application.Batch;

public sealed class FileTransferBatchExecutor : IBatchExecutor<int>
{
    private readonly IFTSProducer ftsProducer;
    private readonly IConsumerFactory consumerFactory;
    private readonly ITenantAppSettingsManager configurationManager;
    private readonly IRequestContextAccessor requestContextAccessor;
    private readonly ILogger<FileTransferBatchExecutor> logger;
    private string tenantId = string.Empty;

    public FileTransferBatchExecutor(
        IFTSProducer producer,
        IConsumerFactory consumerFactory,
        ITenantAppSettingsManager configurationManager,
        IRequestContextAccessor requestContextAccessor,
        ILogger<FileTransferBatchExecutor> logger)
    {
        this.ftsProducer = producer;
        this.consumerFactory = consumerFactory;
        this.configurationManager = configurationManager;
        this.requestContextAccessor = requestContextAccessor;
        this.logger = logger;
    }

    public int Execute(string[] inputArgs)
    {
        this.AssignTenantId(inputArgs);

        var effectiveTenantId = this.requestContextAccessor.TenantId ?? this.tenantId;
        if (this.configurationManager.AppSettings is null && !string.IsNullOrWhiteSpace(effectiveTenantId))
        {
            this.configurationManager.LoadApplicationConfiguration(effectiveTenantId);
        }

        using (BlockingCollection<FileToDeliverModel> blockingCollection = new BlockingCollection<FileToDeliverModel>())
        {
            BlockingCollection<FileToDeliverModel> filesToDeliverQueue = this.AddFilesToDeliveryQueue(inputArgs, blockingCollection, effectiveTenantId);

            this.ProcessFilesInDeliverQueue(filesToDeliverQueue, effectiveTenantId);
        }

        return 0;
    }

    private void AssignTenantId(string[] inputArgs)
    {
        if (inputArgs.Length == 1)
        {
            this.tenantId = inputArgs[0];
        }
        else
        {
            this.tenantId = inputArgs[2];
        }
    }

    private BlockingCollection<FileToDeliverModel> AddFilesToDeliveryQueue(string[] arguments, BlockingCollection<FileToDeliverModel> filesToDeliverQueue, string tenantId)
    {
        Guid? fileId = null;
        int? topXFailures = null;
        int? configId = null;

        if (arguments.Length > 1)
        {
            this.ParseInputArguments(arguments, out fileId, out topXFailures, out configId);
        }

        // Queue files to be processed
        if (fileId == null && topXFailures == null && configId == null)
        {
            this.logger.LogInformation("INFO: Running for all locations");
            filesToDeliverQueue = this.ftsProducer.ProcessAllFilesSources(tenantId);
        }
        else
        {
            this.logger.LogInformation("INFO: Running in a special mode to re-run failed transfers using the specified criteria");
            filesToDeliverQueue = this.ftsProducer.ProcessFailedTransfers(topXFailures, fileId, configId, tenantId);
        }

        return filesToDeliverQueue;
    }

    private void ProcessFilesInDeliverQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, string tenantId)
    {
        Task[] consumerTasks = this.CreateConsumerTasks(filesToDeliverQueue, tenantId);
        if (filesToDeliverQueue != null && filesToDeliverQueue.Count > 0)
        {
            filesToDeliverQueue.CompleteAdding();
        }

        this.WaitForCompletion(consumerTasks);
    }

    private Task[] CreateConsumerTasks(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, string tenantId)
    {
        int maxConcurrentDeliveryProcesses = 0;
        int.TryParse(this.configurationManager.AppSettings?[AppSettingsConstants.MaxConcurrentDeliveryProcesses], out maxConcurrentDeliveryProcesses);
        Task[] consumers = new Task[maxConcurrentDeliveryProcesses];
        for (int processId = 0; processId < maxConcurrentDeliveryProcesses; processId++)
        {
            IFTSConsumer consumer = this.consumerFactory.CreateConsumer(processId);
            consumers[processId] = Task.Run(() => consumer.ProcessDeliveryQueue(filesToDeliverQueue, tenantId));
        }

        return consumers;
    }

    private void WaitForCompletion(Task[] consumers)
    {
        try
        {
            Task.WaitAll(consumers);
        }
        catch (AggregateException aex)
        {
            foreach (Exception ex in aex.InnerExceptions)
            {
                this.logger.LogError(ex.Message);
            }
        }
    }

    private void ParseInputArguments(string[] arguments, out Guid? sakTrack, out int? topXFailures, out int? configId)
    {
        sakTrack = null;
        topXFailures = null;
        configId = null;

        if (string.Compare(arguments[0], BatchJobConstants.CommandSakTrack, StringComparison.InvariantCultureIgnoreCase) == 0)
        {
            Guid fileIdGuid;
            if (Guid.TryParse(arguments[1], out fileIdGuid))
            {
                sakTrack = fileIdGuid;
            }
        }
        else if (string.Compare(arguments[0], BatchJobConstants.CommandConfigId, StringComparison.InvariantCultureIgnoreCase) == 0)
        {
            configId = this.GetCommandValue(arguments[1]);
        }
        else if (string.Compare(arguments[0], BatchJobConstants.CommandTop, StringComparison.InvariantCultureIgnoreCase) == 0)
        {
            topXFailures = this.GetCommandValue(arguments[1]);
        }
        else
        {
            this.PrintUsage();
        }
    }

    private int GetCommandValue(string commandDataString)
    {
        int commandValue;

        int.TryParse(commandDataString, out commandValue);

        return commandValue;
    }

    private void PrintUsage()
    {
        StringBuilder msg = new StringBuilder();
        msg.AppendLine("Usage: FileTransferService <command> <command_value>")
            .AppendLine("  command = sakTrack X (X is the sak_file_track to be processed)")
            .AppendLine("            configId X (X is the config_id of the location to be processed")
            .AppendLine("                     top X (X is the number of failed entries to be processed")
            .Append("  To process all files do not specify the <command> and <command_value> arguments");
        this.logger.LogInformation(msg.ToString());
    }
}


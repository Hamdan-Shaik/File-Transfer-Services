using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using Fxfer.ExecutionWorker.Application.Contracts.Domain;
using Fxfer.ExecutionWorker.BatchProcess;
using Fxfer.ExecutionWorker.Domain.Batch.Business;

namespace Fxfer.ExecutionWorker.Batch.Business;

public sealed class FileTransferBatchExecutor : IBatchExecutor<int>
{
    private readonly IConsumerFactory consumerFactory;
    private readonly IFTSProducer ftsProducer;
    private readonly IAppSettings appSettings;
    private string tenantId = string.Empty;

    public FileTransferBatchExecutor()
    {
        consumerFactory = BatchServiceLocator.GetRequiredService<IConsumerFactory>();
        ftsProducer = BatchServiceLocator.GetRequiredService<IFTSProducer>();
        appSettings = BatchServiceLocator.GetRequiredService<IAppSettings>();
    }

    public FileTransferBatchExecutor(IConsumerFactory consumerFactory, IFTSProducer ftsProducer, IAppSettings appSettings)
    {
        this.consumerFactory = consumerFactory;
        this.ftsProducer = ftsProducer;
        this.appSettings = appSettings;
    }

    public int Execute(string[] inputArgs)
    {
        // F03-012: when the batch executor completes without throwing, it reports success as 0.
        var isManagementScheduledRun = TryGetTenantIdFromManagementArgs(inputArgs, out var managementTenantId);
        tenantId = managementTenantId ?? string.Empty;

        if (!isManagementScheduledRun)
        {
            // F03-021: require mandatory settings before any delivery work begins.
            ValidateMandatorySettingsForStandardScheduledRun();

            // F03-013: validate standard scheduled run details.
            if (!TryValidateStandardScheduledRunDetails(inputArgs))
            {
                PrintUsage();
                return -1;
            }
        }

        using (BlockingCollection<FileToDeliverModel> blockingCollection = new BlockingCollection<FileToDeliverModel>())
        {
            BlockingCollection<FileToDeliverModel> filesToDeliverQueue = AddFilesToDeliveryQueue(inputArgs, blockingCollection);
            ProcessFilesInDeliverQueue(filesToDeliverQueue);
        }

        return 0;
    }

    private static bool TryGetTenantIdFromManagementArgs(string[] inputArgs, out string? managementTenantId)
    {
        managementTenantId = null;

        if (inputArgs.Length == 1 && Guid.TryParse(inputArgs[0], out var tenantIdFromSingleArg))
        {
            managementTenantId = tenantIdFromSingleArg.ToString("D");
            return true;
        }

        if (inputArgs.Length == 3
            && Guid.TryParse(inputArgs[2], out var tenantIdFromThreeArgs)
            && (int.TryParse(inputArgs[1], out _) || Guid.TryParse(inputArgs[1], out _)))
        {
            managementTenantId = tenantIdFromThreeArgs.ToString("D");
            return true;
        }

        return false;
    }

    private static bool TryValidateStandardScheduledRunDetails(string[] inputArgs)
    {
        if (inputArgs.Length == 0)
        {
            return true;
        }

        if (inputArgs.Length != 2)
        {
            return false;
        }

        return int.TryParse(inputArgs[1], out _) || Guid.TryParse(inputArgs[1], out _);
    }

    private void ValidateMandatorySettingsForStandardScheduledRun()
    {
        var requiredKeys = new[]
        {
            AppSettingsConstants.WorkingDirectory,
            AppSettingsConstants.MaxConcurrentDeliveryProcesses,
            AppSettingsConstants.MaxFilesPerCycle,
            AppSettingsConstants.MinsToWaitBeforeRetryFailedFile,
            AppSettingsConstants.MinsToWaitBeforeRetrySuspendedConfig,
            AppSettingsConstants.DisableNotifications,
            AppSettingsConstants.EnvironmentName,
            AppSettingsConstants.DeleteArchiveIfExists,
            AppSettingsConstants.DuplicateFileExtension,
            AppSettingsConstants.ActivateDuplicateRenameLogic,
        };

        foreach (var key in requiredKeys)
        {
            if (string.IsNullOrEmpty(appSettings[key]))
            {
                throw new ConfigurationErrorsException($"{key} is not specified in app.config");
            }
        }
    }

    private BlockingCollection<FileToDeliverModel> AddFilesToDeliveryQueue(string[] arguments, BlockingCollection<FileToDeliverModel> filesToDeliverQueue)
    {
        // F03-014: run for all locations or re-run failed transfers depending on selection.
        Guid? fileId = null;
        int? topXFailures = null;
        int? configId = null;

        if (arguments.Length > 1)
        {
            ParseInputArguments(arguments, out fileId, out topXFailures, out configId);
        }

        // Queue files to be processed
        if (fileId == null && topXFailures == null && configId == null)
        {
            Console.WriteLine("INFO: Running for all locations");
            filesToDeliverQueue = ftsProducer.ProcessAllFilesSources(tenantId);
        }
        else
        {
            Console.WriteLine("INFO: Running in a special mode to re-run failed transfers using the specified criteria");
            filesToDeliverQueue = ftsProducer.ProcessFailedTransfers(topXFailures, fileId, configId, tenantId);
        }

        return filesToDeliverQueue;
    }

    private void ProcessFilesInDeliverQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue)
    {
        Task[] consumerTasks = CreateConsumerTasks(filesToDeliverQueue);
        if (filesToDeliverQueue != null && filesToDeliverQueue.Count > 0)
        {
            filesToDeliverQueue.CompleteAdding();
        }

        WaitForCompletion(consumerTasks);
    }

    private Task[] CreateConsumerTasks(BlockingCollection<FileToDeliverModel> filesToDeliverQueue)
    {
        // F03-019: start up to MaxConcurrentDeliveryProcesses parallel workers.
        int maxConcurrentDeliveryProcesses = 0;
        int.TryParse(appSettings[AppSettingsConstants.MaxConcurrentDeliveryProcesses], out maxConcurrentDeliveryProcesses);
        Task[] consumers = new Task[maxConcurrentDeliveryProcesses];
        for (int processId = 0; processId < maxConcurrentDeliveryProcesses; processId++)
        {
            IFTSConsumer consumer = consumerFactory.CreateConsumer(processId);
            consumers[processId] = Task.Run(() => consumer.ProcessDeliveryQueue(filesToDeliverQueue, tenantId));
        }

        return consumers;
    }

    private void WaitForCompletion(Task[] consumers)
    {
        // F03-019: aggregate worker failures are logged but do not fail the batch run.
        try
        {
            Task.WaitAll(consumers);
        }
        catch (AggregateException aex)
        {
            foreach (Exception ex in aex.InnerExceptions)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    private void ParseInputArguments(string[] arguments, out Guid? sakTrack, out int? topXFailures, out int? configId)
    {
        // F03-007/F03-014/F03-023: unrecognised commands log usage but still default to all-locations run.
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
            configId = GetCommandValue(arguments[1]);
        }
        else if (string.Compare(arguments[0], BatchJobConstants.CommandTop, StringComparison.InvariantCultureIgnoreCase) == 0)
        {
            topXFailures = GetCommandValue(arguments[1]);
        }
        else
        {
            PrintUsage();
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
        Console.WriteLine(msg.ToString());
    }
}
using System.Diagnostics;
using System.Text;
using Fxfer.TransferWorker.Application.Batch;
using Fxfer.TransferWorker.Domain.Constants;
using Fxfer.TransferWorker.Worker.Configuration;
using Fxfer.TransferWorker.Worker.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Fxfer.TransferWorker.Worker;

public sealed class FileTransferBatchManagedRunner
{
    private static readonly ActivitySource ActivitySource = new("Fxfer.TransferWorker.FileTransferBatchManagedRunner");

    private readonly ILogger<FileTransferBatchManagedRunner> _logger;
    private readonly ITenantAppSettingsManager _configurationManager;
    private readonly IWorkerBatchRuntimeFactory _runtimeFactory;

    private bool _processingAllFiles;
    private string? _tenantId;

    public FileTransferBatchManagedRunner(
        ILogger<FileTransferBatchManagedRunner> logger,
        ITenantAppSettingsManager configurationManager,
        IWorkerBatchRuntimeFactory runtimeFactory)
    {
        _logger = logger;
        _configurationManager = configurationManager;
        _runtimeFactory = runtimeFactory;
    }

    public static FileTransferBatchManagedRunner CreateDefault()
    {
        var configuration = BuildConfiguration();

        var loggerFactory = LoggerFactory.Create(logging =>
        {
            logging.AddSimpleConsole();
            logging.SetMinimumLevel(LogLevel.Information);
        });

        return new FileTransferBatchManagedRunner(
            loggerFactory.CreateLogger<FileTransferBatchManagedRunner>(),
            new ConfigurationTenantAppSettingsManager(configuration),
            new WorkerBatchRuntimeFactory());
    }

    public int Run(string[] args)
    {
        // F04-003: start record.
        _logger.LogInformation("Starting FileTransfer Batch Process");

        using var activity = ActivitySource.StartActivity("file_transfer_batch.managed", ActivityKind.Internal);

        try
        {
            _logger.LogInformation("Validating batch arguments.");

            // F04-004: gate execution on validation.
            if (Validate(args) == false)
            {
                return -1;
            }

            _logger.LogInformation("Retrieving app settings for tenant");

            // F04-011: load once.
            if (_configurationManager.AppSettings is null)
            {
                _configurationManager.LoadApplicationConfiguration(_tenantId!);
            }

            _logger.LogInformation("Validating app settings.");

            // F04-012: mandatory keys.
            ValidateAppConfig();

            _logger.LogInformation("Initialize Unit Container.");

            // F04-013: initialize container + resolve executor.
            using var runtime = _runtimeFactory.Create();
            runtime.Initialize();

            SetCorrelationContext(runtime.Services, _tenantId);

            _logger.LogInformation("Resolve the batch executor.");

            var ftsBatchExecutor = runtime.ResolveBatchExecutor(IocConstants.FileTransferExecutor);

            _logger.LogInformation("Begin the process.");

            return ftsBatchExecutor.Execute(args);
        }
        catch (Exception ex)
        {
            // F04-014: fatal + record under batch source.
            _logger.LogCritical(ex, "Exception occurred: ");
            _logger.LogError("{BatchSource}: {Message}", ExceptionConstants.BatchSource, ex.Message);
            return -1;
        }
        finally
        {
            // F04-003: always record end.
            _logger.LogInformation("Ending FileTransfer Batch Process");
        }
    }

    public bool Validate(string[] args)
    {
        // F04-004: return ValidateInputArguments(args).
        if (!ValidateInputArguments(args))
        {
            return false;
        }

        return true;
    }

    public bool ValidateInputArguments(string[] args)
    {
        _processingAllFiles = false;
        _tenantId = null;

        // F04-004: accept only 1-value or 3-value forms.
        var badArgFound = false;
        Guid tenantGuid;

        if (args.Length != 1 && args.Length != 3)
        {
            PrintUsage();
            return false;
        }

        // F04-005 / F04-006: 1-value form.
        if (args.Length == 1)
        {
            _processingAllFiles = true;
            _tenantId = args[0];
            if (Guid.TryParseExact(args[0], "D", out tenantGuid) == false)
            {
                badArgFound = true;
            }
        }

        // F04-007 / F04-008 / F04-009 / F04-010: 3-value form (args[0] is not validated).
        int commandValue;
        Guid fileId;
        if (!_processingAllFiles)
        {
            _tenantId = args[2];
            if (int.TryParse(args[1], out commandValue) == false && Guid.TryParseExact(args[1], "D", out fileId) == false)
            {
                badArgFound = true;
            }

            if (Guid.TryParseExact(args[2], "D", out tenantGuid) == false)
            {
                badArgFound = true;
            }
        }

        if (badArgFound)
        {
            PrintUsage();
            return false;
        }

        return true;
    }

    public void ValidateAppConfig()
    {
        CheckAndThrowConfigurationException(FileTransferManagementAppSettingsConstants.ExternalFTECommonFolder);
        CheckAndThrowConfigurationException(FileTransferManagementAppSettingsConstants.WorkingDirectory);
        CheckAndThrowConfigurationException(FileTransferManagementAppSettingsConstants.MaxConcurrentDeliveryProcesses);
        CheckAndThrowConfigurationException(FileTransferManagementAppSettingsConstants.MaxFilesPerCycle);
        CheckAndThrowConfigurationException(FileTransferManagementAppSettingsConstants.MinsToWaitBeforeRetryFailedFile);
        CheckAndThrowConfigurationException(FileTransferManagementAppSettingsConstants.MinsToWaitBeforeRetrySuspendedConfig);
        CheckAndThrowConfigurationException(FileTransferManagementAppSettingsConstants.DisableNotifications);
        CheckAndThrowConfigurationException(FileTransferManagementAppSettingsConstants.EnvironmentName);
        CheckAndThrowConfigurationException(FileTransferManagementAppSettingsConstants.DeleteArchiveIfExists);
        CheckAndThrowConfigurationException(FileTransferManagementAppSettingsConstants.DuplicateFileExtension);
        CheckAndThrowConfigurationException(FileTransferManagementAppSettingsConstants.ActivateDuplicateRenameLogic);
    }

    public void CheckAndThrowConfigurationException(string key)
    {
        // F04-012: missing key fails the run with the exact message.
        if (_configurationManager.AppSettings?[key] is null)
        {
            var message = string.Format(FileTransferManagementAppSettingsConstants.AppsettingErrorMessageTemplate, key);
            _logger.LogError(message);
            throw new InvalidOperationException(message);
        }
    }

    public void PrintUsage()
    {
        var msg = new StringBuilder();
        msg.AppendLine("Usage: FileTransferService <command> <command_value>")
            .AppendLine("  command = sakTrack X (X is the sak_file_track to be processed)")
            .AppendLine("            configId X (X is the config_id of the location to be processed")
            .AppendLine("                     top X (X is the number of failed entries to be processed")
            .Append("  To process all files do not specify the <command> and <command_value> arguments");

        _logger.LogInformation(msg.ToString());
    }

    private static void SetCorrelationContext(IServiceProvider services, string? tenantId)
    {
        var correlationId = Guid.NewGuid().ToString();

        services.GetService<CorrelationIdContext>()?.Set(correlationId);
        if (!string.IsNullOrWhiteSpace(tenantId))
        {
            services.GetService<TenantIdContext>()?.Set(tenantId);
        }

        Activity.Current?.SetTag("gainwell.correlation_id", correlationId);

        if (!string.IsNullOrWhiteSpace(tenantId))
        {
            Activity.Current?.SetTag("gainwell.tenant.id", tenantId);
        }
    }

    private static IConfiguration BuildConfiguration()
    {
        var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? "Production";

        return new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }
}

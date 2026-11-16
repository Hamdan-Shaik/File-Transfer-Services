using System.Diagnostics;
using Fxfer.ExecutionWorker.Domain.Batch.Business;
using Fxfer.ExecutionWorker.Infrastructure.Tenancy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Fxfer.ExecutionWorker.BatchProcess;

public sealed class RunFileTransferBatchEntrypoint
{
    private readonly ILogger<RunFileTransferBatchEntrypoint> _logger;
    private readonly TenantContext _tenantContext;
    private readonly RunFileTransferBatchConfigurationValidation _configurationValidation;
    private readonly IBatchExecutor<int> _batchExecutor;
    private Func<Guid, IReadOnlyDictionary<string, string?>> _loadTenantSettings;

    private IReadOnlyDictionary<string, string?>? _tenantSettings;

    public RunFileTransferBatchEntrypoint(
        ILogger<RunFileTransferBatchEntrypoint> logger,
        TenantContext tenantContext,
        RunFileTransferBatchConfigurationValidation configurationValidation,
        IBatchExecutor<int> batchExecutor,
        IConfiguration configuration)
    {
        _logger = logger;
        _tenantContext = tenantContext;
        _configurationValidation = configurationValidation;
        _batchExecutor = batchExecutor;

        _loadTenantSettings = tenantId =>
        {
            var settings = new Dictionary<string, string?>(StringComparer.Ordinal)
            {
                ["ExternalFTECommonFolder"] = configuration["ExternalFTECommonFolder"],
                [AppSettingsConstants.WorkingDirectory] = configuration[AppSettingsConstants.WorkingDirectory],
                [AppSettingsConstants.MaxConcurrentDeliveryProcesses] = configuration[AppSettingsConstants.MaxConcurrentDeliveryProcesses],
                [AppSettingsConstants.MaxFilesPerCycle] = configuration[AppSettingsConstants.MaxFilesPerCycle],
                [AppSettingsConstants.MinsToWaitBeforeRetryFailedFile] = configuration[AppSettingsConstants.MinsToWaitBeforeRetryFailedFile],
                [AppSettingsConstants.MinsToWaitBeforeRetrySuspendedConfig] = configuration[AppSettingsConstants.MinsToWaitBeforeRetrySuspendedConfig],
                [AppSettingsConstants.DisableNotifications] = configuration[AppSettingsConstants.DisableNotifications],
                [AppSettingsConstants.EnvironmentName] = configuration[AppSettingsConstants.EnvironmentName],
                [AppSettingsConstants.DeleteArchiveIfExists] = configuration[AppSettingsConstants.DeleteArchiveIfExists],
                [AppSettingsConstants.DuplicateFileExtension] = configuration[AppSettingsConstants.DuplicateFileExtension],
                [AppSettingsConstants.ActivateDuplicateRenameLogic] = configuration[AppSettingsConstants.ActivateDuplicateRenameLogic],
            };

            return settings;
        };
    }

    public int RunStandardScheduled(string[] runDetails)
    {
        // F03-001: resolve and execute the batch using the provided run details, returning its numeric result.
        try
        {
            return _batchExecutor.Execute(runDetails);
        }
        catch (Exception ex)
        {
            // F03-002: record error and return -1.
            _logger.LogError("{event_source}: {error_message}", ExceptionConstants.BatchSource, ex.Message);
            return -1;
        }
    }

    public int RunManagementScheduled(string[] runDetails)
    {
        var traceId = Activity.Current?.TraceId.ToString();
        var spanId = Activity.Current?.SpanId.ToString();

        using var scope = _logger.BeginScope(new Dictionary<string, object?>
        {
            ["trace_id"] = traceId,
            ["span_id"] = spanId,
        });

        try
        {
            // F03-003: always log start and end records.
            _logger.LogInformation("Starting FileTransfer Batch Process");

            _logger.LogInformation("Validating batch arguments.");

            if (!RunFileTransferBatchValidation.TryValidateRunDetails(runDetails, out var tenantId, out _))
            {
                // F03-004: log usage guidance and do not start.
                _logger.LogInformation(RunFileTransferBatchValidation.GetUsageMessage());
                return -1;
            }

            using var __ = _logger.BeginScope(new Dictionary<string, object?>
            {
                ["tenant_id"] = tenantId,
            });

            // F03-008: ensure tenant settings are loaded before validating mandatory settings.
            _logger.LogInformation("Retrieving app settings for tenant");

            // Tenant identifier is a trust boundary; set tenant context before reading tenant settings.
            _tenantContext.TenantId = tenantId.ToString("D");
            _tenantContext.TraceId = traceId;

            if (_tenantSettings is null)
            {
                _tenantSettings = _loadTenantSettings(tenantId);
            }

            _logger.LogInformation("Validating app settings.");
            _configurationValidation.ValidateMandatorySettings(_tenantSettings);

            _logger.LogInformation("Initialize Unit Container.");
            _logger.LogInformation("Resolve the batch executor.");
            _logger.LogInformation("Begin the process.");

            // F03-010: execute with run details unchanged.
            return _batchExecutor.Execute(runDetails);
        }
        catch (Exception ex)
        {
            // F03-011: log fatal-equivalent record, record error, and return -1.
            _logger.LogCritical(ex, "Exception occurred: ");
            _logger.LogError("{event_source}: {error_message}", ExceptionConstants.BatchSource, ex.Message);
            return -1;
        }
        finally
        {
            _logger.LogInformation("Ending FileTransfer Batch Process");
        }
    }

    internal void OverrideTenantSettingsLoaderForTests(Func<Guid, IReadOnlyDictionary<string, string?>> loader)
    {
        _loadTenantSettings = loader;
    }
}



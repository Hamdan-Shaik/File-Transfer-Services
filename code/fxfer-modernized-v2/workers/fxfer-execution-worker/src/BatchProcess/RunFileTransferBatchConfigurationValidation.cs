using Fxfer.ExecutionWorker.Domain.Batch.Business;
using Microsoft.Extensions.Logging;

namespace Fxfer.ExecutionWorker.BatchProcess;

public sealed class RunFileTransferBatchConfigurationValidation
{
    private static readonly string[] MandatorySettingsKeys =
    [
        "ExternalFTECommonFolder",
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
    ];

    private readonly ILogger<RunFileTransferBatchConfigurationValidation> _logger;

    public RunFileTransferBatchConfigurationValidation(ILogger<RunFileTransferBatchConfigurationValidation> logger)
    {
        _logger = logger;
    }

    public void ValidateMandatorySettings(IReadOnlyDictionary<string, string?> settings)
    {
        // F03-009: mandatory settings must exist; missing key is logged and raises a configuration error.
        foreach (var key in MandatorySettingsKeys)
        {
            if (!settings.TryGetValue(key, out var value) || value is null)
            {
                var message = string.Format(AppSettingsConstants.AppsettingErrorMessageTemplate, key);
                _logger.LogError(message);
                throw new ConfigurationErrorsException(message);
            }
        }
    }
}
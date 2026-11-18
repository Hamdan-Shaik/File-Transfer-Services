namespace Fxfer.TransferWorker.Domain.Constants;

public static class FileTransferManagementAppSettingsConstants
{
    public const string ActivateDuplicateRenameLogic = "ActivateDuplicateRenameLogic";
    public const string AppsettingErrorMessageTemplate = "{0} is not specified in app.config";
    public const string DeleteArchiveIfExists = "DeleteArchiveIfExists";
    public const string DisableNotifications = "DisableNotifications";
    public const string DuplicateFileExtension = "DuplicateFileExtension";
    public const string EnvironmentName = "EnvironmentName";
    public const string ExternalFTECommonFolder = "ExternalFTECommonFolder";
    public const string FtpsConnectionProtocol = "FtpsConnectionProtocol";
    public const string FTPConnectMode = "FTPConnectMode";
    public const string HostName = "#RootUrl.Tenant";
    public const string MaxConcurrentDeliveryProcesses = "MaxConcurrentDeliveryProcesses";
    public const string MaxFilesPerCycle = "MaxFilesPerCycle";
    public const string MinsToWaitBeforeRetryFailedFile = "MinsToWaitBeforeRetryFailedFile";
    public const string MinsToWaitBeforeRetrySuspendedConfig = "MinsToWaitBeforeRetrySuspendedConfig";
    public const string NotificationService = "NotificationServiceUrl";
    public const string NotificationServiceConfiguration = "NotificationServiceConfiguration";
    public const string WorkingDirectory = "WorkingDirectory";

    public static class AppSettingsKey
    {
        public const string SPTURLPath = "/SystemProcessTracking/SystemProcessTrackingExternalHistory/?&groupid=";
    }
}


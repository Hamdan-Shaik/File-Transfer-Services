namespace Fxfer.ExecutionWorker.Application.Helpers;

internal static class FileTransferManagementConstants
{
    internal static class ExceptionMessages
    {
        public const string AutoCreateDirectoryFailed = "Remote directory does not exist and it cannot be auto created because it does not start with a / character";
        public const string DestinationPathIsNotValid = "Destination: '{0}' path is not valid";
        public const string ErrorTryingToDeleteFile = "An error occurred when trying to delete the file, exception thrown: {0}";
        public const string ErrorRenamingFile = "An error occurred when trying to rename the file, exception thrown: {0}";
        public const string FileExixstAtDestination = "File exists at destination and overwrite was not specified in config";
    }

    internal static class Information
    {
        public const string AttemptingToConnect = "Attempting to connect to SFTP server {0}";
        public const string ConnectionSuccessful = "Connection established successfully";
        public const string DestinationFileName = "Destination file name: {0}";
        public const string Encrypted = "ENCRYPTED";
        public const string FileFullyMoved = "File is moved to destination";
        public const string FileWasUnabletoMove = "File was unable to destination";
        public const string FullFilePath = "Full file path: {0}";
        public const string SFTPConnectionClosed = "SFTP connection closed";
        public const string SFTPConnectionEstablished = "SFTP connection established";
    }

    internal static class EventDistribution
    {
        public const string AttemptingToDistributeEvent = "Attempting to distibute {0} event, CommitId: {1}";
        public const string DistributionOfEventCompleted = "Distribution of {0} event completed, CommitId: {1}";
    }

    internal static class Paths
    {
        public const string PathToPrivateKeyFile = "\\\\SSHPrivateKey\\\\SSHPrivateKeyFile";
    }
}


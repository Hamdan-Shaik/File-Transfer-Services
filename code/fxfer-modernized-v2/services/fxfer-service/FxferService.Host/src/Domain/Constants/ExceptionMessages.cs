namespace FxferService.Domain.Constants;

public static class ExceptionMessages
{
    public const string AutoCreateDirectoryFailed = "Remote directory does not exist and it cannot be auto created because it does not start with a / character";
    public const string DestinationPathIsNotValid = "Destination: '{0}' path is not valid";
    public const string ErrorTryingToDeleteFile = "An error occurred when trying to delete the file, exception thrown: {0}";
    public const string ErrorRenamingFile = "An error occurred when trying to rename the file, exception thrown: {0}";
    public const string FileExixstAtDestination = "File exists at destination and overwrite was not specified in config";
}

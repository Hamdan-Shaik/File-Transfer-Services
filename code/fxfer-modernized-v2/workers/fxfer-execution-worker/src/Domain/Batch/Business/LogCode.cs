namespace Fxfer.ExecutionWorker.Domain.Batch.Business;

public enum LogCode
{
    BeginFile,
    BeginFileRestart,
    DownloadWorkingFile,
    CreateFileTrackingRecord,
    GetListOfDestinations,
    UploadToDestination,
    CreateEventRecord,
    DeleteSourceFile,
    UpdateFileTrackingRecord,
    CleanupWorkDirectory,
    ExceptionEncountered,
    DuplicateFileFound,
    Info,
    Finished,
}
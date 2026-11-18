using Fxfer.TransferWorker.Contracts.Batch;

namespace Fxfer.TransferWorker.Application.DataAccess;

public interface IDataAccessFactory
{
    IDataAccess<FtsSuspensionModel> CreateFtsSuspensionDataAccess(FtsSuspensionSearchCriteria searchQuery);

    IDataAccess<int> CreateErrorLogDataAccess(ErrorLogDetailModel errorLog);

    IDataAccess<Guid> CreateFtsFileDeliveryDataAccess(
        Guid fileId,
        Guid destinationId,
        string destinationFileName,
        string operatorId);

    IDataAccess<Guid> CreateFtsFileExceptionEventDataAccess(
        Guid fileId,
        Guid destinationId,
        string destinationFileName,
        string operatorId,
        ErrorLogDetailModel errorLog);

    IDataAccess<FileManagementResponseModel> CreateFileManagementDataAccess(FileManagementRequestModel requestModel);

    IDataAccess<SourceLocationModel?> CreateGetConfigMetaDataDataAccess(Guid fileId);

    IDataAccess<List<PendingFileModel>> CreateGetPendingFileListDataAccess();

    IDataAccess<List<SourceLocationModel>> CreateGetSourceLocationsDataAccess();

    IDataAccess<List<FtsNotificationModel>> CreateQueryFtsNotificationDataAccess(GetNotificationSearchCriteria searchQuery);

    IDataAccess<FtsIntegrationFileModel?> CreateGetIntegrationFileDataAccess(string fileName);

    IDataAccess<bool> CreateUpdateIntegrationFileDataAccess(UpdateIntegrationFileCommand command);

    IDataAccess<Guid> CreateUpdateFileManagementStatusDataAccess(Guid fileId, string fileStatus);

    IDataAccess<bool> CreateUpdateSourceSuspendStatusDataAccess(Guid sourceLocationId);

    IDataAccess<SourceLocationModel?> GetSuspendedLocation(Guid sourceLocationId);
}


using FxferService.Infrastructure.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Batch.Contracts;
using FxferService.Infrastructure.Persistence.Entities;

namespace FxferService.Application.Persistence.Batch;

public interface IDataAccessFactory
{
    IDataAccess<FtsSuspensionModel> CreateFtsSuspensionDataAccess(FtsSuspensionSearchCriteria searchQuery);

    IDataAccess<List<PendingFileModel>> CreateGetPendingFileListDataAccess();

    IDataAccess<List<FtsNotificationModel>> CreateQueryFtsNotificationDataAccess(GetNotificationSearchCriteria searchQuery);

    IDataAccess<List<SourceLocationModel>> CreateGetSourceLocationsDataAccess();

    IDataAccess<int> CreateErrorLogDataAccess(ErrorLogDetailModel errorLog);

    IDataAccess<SourceLocationModel?> CreateGetConfigMetaDataDataAccess(Guid fileId);

    IDataAccess<Guid> CreateUpdateFileManagementStatusDataAccess(Guid fileId, string fileStatus);

    IDataAccess<bool> CreateUpdateSourceSuspendStatusDataAccess(FtsSourceLocationEntity ftsFile);

    IDataAccess<FtsSourceLocationEntity?> GetSuspendedLocation(Guid sourceLocationId);

    IDataAccess<Guid> CreateFtsFileDeliveryDataAccess(
        Guid fileId,
        Guid destinationId,
        string destinationFileName,
        string operatorId,
        List<DestinationModel> destinations);

    IDataAccess<Guid> CreateFtsFileExceptionEventDataAccess(
        Guid fileId,
        Guid destinationId,
        string destinationFileName,
        string operatorId,
        ErrorLogDetailModel errorLog);

    IDataAccess<FileManagementResponseModel> CreateFileManagementDataAccess(FileManagementRequestModel requestModel);
}

using FxferService.Infrastructure.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Batch.Contracts;
using FxferService.Infrastructure.Persistence.Entities;

namespace FxferService.Application.Persistence.Batch;

public sealed class DataAccessFactory : IDataAccessFactory
{
    public IDataAccess<FtsSuspensionModel> CreateFtsSuspensionDataAccess(FtsSuspensionSearchCriteria searchQuery)
    {
        return new FtsSuspensionDataAccess(searchQuery);
    }

    public IDataAccess<List<PendingFileModel>> CreateGetPendingFileListDataAccess()
    {
        return new GetPendingFileListDataAccess();
    }

    public IDataAccess<List<FtsNotificationModel>> CreateQueryFtsNotificationDataAccess(GetNotificationSearchCriteria searchQuery)
    {
        return new QueryFtsNotificationDataAccess(searchQuery);
    }

    public IDataAccess<List<SourceLocationModel>> CreateGetSourceLocationsDataAccess()
    {
        return new GetSourceLocationsDataAccess();
    }

    public IDataAccess<int> CreateErrorLogDataAccess(ErrorLogDetailModel errorLog)
    {
        return new CreateErrorLogDataAccess(errorLog);
    }

    public IDataAccess<SourceLocationModel?> CreateGetConfigMetaDataDataAccess(Guid fileId)
    {
        return new GetConfigMetaDataDataAccess(fileId);
    }

    public IDataAccess<Guid> CreateUpdateFileManagementStatusDataAccess(Guid fileId, string fileStatus)
    {
        return new UpdateFileManagementStatusDataAccess(fileId, fileStatus);
    }

    public IDataAccess<bool> CreateUpdateSourceSuspendStatusDataAccess(FtsSourceLocationEntity ftsFile)
    {
        return new UpdateSourceSuspendStatusDataAccess(ftsFile);
    }

    public IDataAccess<FtsSourceLocationEntity?> GetSuspendedLocation(Guid sourceLocationId)
    {
        return new GetSuspendedLocation(sourceLocationId);
    }

    public IDataAccess<Guid> CreateFtsFileDeliveryDataAccess(
        Guid fileId,
        Guid destinationId,
        string destinationFileName,
        string operatorId,
        List<DestinationModel> destinations)
    {
        return new CreateFtsFileDeliveryDataAccess(fileId, destinationId, destinationFileName, operatorId, destinations);
    }

    public IDataAccess<Guid> CreateFtsFileExceptionEventDataAccess(
        Guid fileId,
        Guid destinationId,
        string destinationFileName,
        string operatorId,
        ErrorLogDetailModel errorLog)
    {
        return new CreateFtsFileExceptionEventDataAccess(fileId, destinationId, destinationFileName, operatorId, errorLog);
    }

    public IDataAccess<FileManagementResponseModel> CreateFileManagementDataAccess(FileManagementRequestModel requestModel)
    {
        return new CreateFileManagementDataAccess(requestModel);
    }
}
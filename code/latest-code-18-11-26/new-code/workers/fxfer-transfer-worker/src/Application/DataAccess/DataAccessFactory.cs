using Fxfer.TransferWorker.Contracts.Batch;
using Fxfer.TransferWorker.Infrastructure.DataAccess;

namespace Fxfer.TransferWorker.Application.DataAccess;

public sealed class DataAccessFactory : IDataAccessFactory
{
    private readonly IFxferDomainApiClient _domainApiClient;
    private readonly IRequestContextAccessor _requestContextAccessor;

    public DataAccessFactory(IFxferDomainApiClient domainApiClient, IRequestContextAccessor requestContextAccessor)
    {
        _domainApiClient = domainApiClient;
        _requestContextAccessor = requestContextAccessor;
    }

    public IDataAccess<FtsSuspensionModel> CreateFtsSuspensionDataAccess(FtsSuspensionSearchCriteria searchQuery)
    {
        return new FtsSuspensionDataAccess(_domainApiClient, _requestContextAccessor, searchQuery);
    }

    public IDataAccess<int> CreateErrorLogDataAccess(ErrorLogDetailModel errorLog)
    {
        return new CreateErrorLogDataAccess(_domainApiClient, _requestContextAccessor, errorLog);
    }

    public IDataAccess<Guid> CreateFtsFileDeliveryDataAccess(
        Guid fileId,
        Guid destinationId,
        string destinationFileName,
        string operatorId)
    {
        return new CreateFtsFileDeliveryDataAccess(
            _domainApiClient,
            _requestContextAccessor,
            fileId,
            destinationId,
            destinationFileName,
            operatorId);
    }

    public IDataAccess<Guid> CreateFtsFileExceptionEventDataAccess(
        Guid fileId,
        Guid destinationId,
        string destinationFileName,
        string operatorId,
        ErrorLogDetailModel errorLog)
    {
        return new CreateFtsFileExceptionEventDataAccess(
            _domainApiClient,
            _requestContextAccessor,
            fileId,
            destinationId,
            destinationFileName,
            operatorId,
            errorLog);
    }

    public IDataAccess<FileManagementResponseModel> CreateFileManagementDataAccess(FileManagementRequestModel requestModel)
    {
        return new CreateFileManagementDataAccess(_domainApiClient, _requestContextAccessor, requestModel);
    }

    public IDataAccess<SourceLocationModel?> CreateGetConfigMetaDataDataAccess(Guid fileId)
    {
        return new GetConfigMetaDataDataAccess(_domainApiClient, _requestContextAccessor, fileId);
    }

    public IDataAccess<List<PendingFileModel>> CreateGetPendingFileListDataAccess()
    {
        return new GetPendingFileListDataAccess(_domainApiClient, _requestContextAccessor);
    }

    public IDataAccess<List<SourceLocationModel>> CreateGetSourceLocationsDataAccess()
    {
        return new GetSourceLocationsDataAccess(_domainApiClient, _requestContextAccessor);
    }

    public IDataAccess<List<FtsNotificationModel>> CreateQueryFtsNotificationDataAccess(GetNotificationSearchCriteria searchQuery)
    {
        return new QueryFtsNotificationDataAccess(_domainApiClient, _requestContextAccessor, searchQuery);
    }

    public IDataAccess<FtsIntegrationFileModel?> CreateGetIntegrationFileDataAccess(string fileName)
    {
        return new GetIntegrationFileDataAccess(_domainApiClient, _requestContextAccessor, fileName);
    }

    public IDataAccess<bool> CreateUpdateIntegrationFileDataAccess(UpdateIntegrationFileCommand command)
    {
        return new UpdateIntegrationFileDataAccess(_domainApiClient, _requestContextAccessor, command);
    }

    public IDataAccess<Guid> CreateUpdateFileManagementStatusDataAccess(Guid fileId, string fileStatus)
    {
        return new UpdateFileManagementStatusDataAccess(_domainApiClient, _requestContextAccessor, fileId, fileStatus);
    }

    public IDataAccess<bool> CreateUpdateSourceSuspendStatusDataAccess(Guid sourceLocationId)
    {
        return new UpdateSourceSuspendStatusDataAccess(_domainApiClient, _requestContextAccessor, sourceLocationId);
    }

    public IDataAccess<SourceLocationModel?> GetSuspendedLocation(Guid sourceLocationId)
    {
        return new GetSuspendedLocationDataAccess(_domainApiClient, _requestContextAccessor, sourceLocationId);
    }
}


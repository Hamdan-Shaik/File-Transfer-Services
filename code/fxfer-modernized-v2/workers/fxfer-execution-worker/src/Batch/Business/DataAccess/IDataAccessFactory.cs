using System;
using System.Collections.Generic;
using Fxfer.ExecutionWorker.Application.Contracts.Domain;

namespace Fxfer.ExecutionWorker.Batch.Business.DataAccess;

public interface IDataAccessFactory
{
    IDataAccess<List<FtsNotificationModel>> CreateQueryFtsNotificationDataAccess(GetNotificationSearchCriteria searchQuery);

    IDataAccess<List<PendingFileModel>> CreateGetPendingFileListDataAccess();

    IDataAccess<List<SourceLocationModel>> CreateGetSourceLocationsDataAccess();

    IDataAccess<FileManagementResponseModel> CreateFileManagementDataAccess(FileManagementRequestModel request);

    IDataAccess<int> CreateErrorLogDataAccess(ErrorLogDetailModel errorLog);

    IDataAccess<SourceLocationModel>? CreateGetConfigMetaDataDataAccess(Guid fileId);

    IDataAccess<Guid> CreateUpdateFileManagementStatusDataAccess(Guid fileId, string fileStatus);

    IDataAccess<Guid> CreateUpdateSourceSuspendStatusDataAccess(Guid sourceLocationId);

    IDataAccess<int> CreateFtsSuspensionDataAccess(FtsSuspensionSearchCriteria criteria);

    IDataAccess<Guid> CreateFtsFileDeliveryDataAccess(Guid fileId, Guid destinationId, string destinationFileName, string operatorId, List<DestinationModel> destinations);

    IDataAccess<Guid> CreateFtsFileExceptionEventDataAccess(Guid fileId, Guid destinationId, string destinationFileName, string operatorId, ErrorLogDetailModel errorLog);
}

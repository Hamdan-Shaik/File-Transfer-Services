using FxferService.Application.Persistence.Batch;
using FxferService.Domain.Constants;
using FxferService.Infrastructure.Persistence.Batch.Contracts;
using FxferService.Infrastructure.Persistence.Entities;

namespace FxferService.Infrastructure.Persistence.Batch;

public sealed class CreateFileManagementDataAccess : DataAccessBase<FileManagementResponseModel>
{
    private readonly FileManagementRequestModel _requestModel;

    public CreateFileManagementDataAccess(FileManagementRequestModel requestModel)
    {
        _requestModel = requestModel;
    }

    // F03-020: create tracking record for a queued file.
    protected override FileManagementResponseModel ExecuteQuery(IDbContextBase dbcontext)
    {
        FileManagementResponseModel response = new();

        FtsFileManagementEntity entityFileManagement = CreateFileManagementEntityData(dbcontext);
        dbcontext.Set<FtsFileManagementEntity>().Add(entityFileManagement);
        dbcontext.SaveChanges();

        response.FileId = entityFileManagement.FileId;
        return response;
    }

    private FtsFileManagementEntity CreateFileManagementEntityData(IDbContextBase dbcontext)
    {
        _ = dbcontext;
        var fileId = Guid.NewGuid();
        FtsFileManagementEntity entityFileMgt = new()
        {
            FileId = fileId,
            FtsSourceLocationId = _requestModel.SourceLocationId,
            FileName = _requestModel.FileName,
            FileStatus = DataAccessConstants.FileStatusConstant.Transfering,
            FileSize = _requestModel.FileSize,
            ChecksumCode = _requestModel.ChecksumCode,
            OperatorId = _requestModel.OperatorId,
            LastModifiedAt = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow,
            FileStatusAt = DateTimeOffset.UtcNow,
            // shortcut: legacy persisted GroupId; target schema/table no longer has it.
        };

        return entityFileMgt;
    }
}

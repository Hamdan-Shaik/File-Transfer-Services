using FxferService.Infrastructure.Persistence.Entities;

using FxferService.Application.Persistence.Batch;

namespace FxferService.Infrastructure.Persistence.Batch;

public sealed class UpdateFileManagementStatusDataAccess : DataAccessBase<Guid>
{
    private readonly Guid _fileId;
    private readonly string _fileStatus;

    public UpdateFileManagementStatusDataAccess(Guid fileId, string fileStatus)
    {
        _fileId = fileId;
        _fileStatus = fileStatus;
    }

    protected override Guid ExecuteQuery(IDbContextBase dbcontext)
    {
        var fileManagement = dbcontext.Set<FtsFileManagementEntity>().FirstOrDefault(f => f.FileId == _fileId);
        if (fileManagement != null)
        {
            fileManagement.FileStatus = _fileStatus;
            fileManagement.FileStatusAt = DateTimeOffset.UtcNow;
            fileManagement.LastModifiedAt = DateTimeOffset.UtcNow;

            dbcontext.SaveChanges();
        }

        return _fileId;
    }
}

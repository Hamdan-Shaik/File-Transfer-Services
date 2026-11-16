using FxferService.Application.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Batch.Contracts;
using FxferService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FxferService.Infrastructure.Persistence.Batch;

public sealed class GetPendingFileListDataAccess : DataAccessBase<List<PendingFileModel>>
{
    protected override List<PendingFileModel> ExecuteQuery(IDbContextBase dbcontext)
    {
        DateTime currentDate = DateTime.UtcNow.AddMinutes(-1);

        return (from fileMgmt in dbcontext.Set<FtsFileManagementEntity>().AsNoTracking()
                where (string.Compare(fileMgmt.FileStatus, FileStatusConstant.Completed, StringComparison.InvariantCultureIgnoreCase) != 0)
                      || (string.Compare(fileMgmt.FileStatus, FileStatusConstant.Completed, StringComparison.InvariantCultureIgnoreCase) == 0
                          && fileMgmt.FileStatusAt.UtcDateTime > currentDate)
                select new PendingFileModel
                {
                    FileId = fileMgmt.FileId,
                    FileName = fileMgmt.FileName,
                    FileSourceLocationId = fileMgmt.FtsSourceLocationId,
                    FileStatus = fileMgmt.FileStatus,
                    FileTrackId = fileMgmt.SakFileTrack,
                    StatusDate = fileMgmt.FileStatusAt.UtcDateTime,
                    FileSize = fileMgmt.FileSize,
                }).ToList();
    }
}
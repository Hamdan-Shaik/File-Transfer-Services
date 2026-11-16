using FxferService.Application.Contracts.Queries;
using FxferService.Domain.Contracts.Domain;
using FxferService.Domain.Contracts.Queries.Parameters;
using FxferService.Domain.Utilities;
using FxferService.Infrastructure.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Entities;
using FxferService.Infrastructure.Queries;

namespace FxferService.Application.QueryHandlers;

public class FileTransferTrackingQueryExecutor : QueryExecutorBase<FileTransferTrackingModel, FileTransferTrackingQuery>
{
    private IDbContextBase? _databaseContext;

    protected override IQueryable<FileTransferTrackingModel> ApplyPaging(IQueryable<FileTransferTrackingModel> baseQuery, FileTransferTrackingQuery query)
    {
        return base.ApplyPaging(baseQuery, query);
    }

    [MaintainabilityDetails(ApprovedIndex = 59, ReviewedOnSprint = 94, ReviewerName = "Josue Chavez")]
    protected override IQueryable<FileTransferTrackingModel> ApplySorting(IQueryable<FileTransferTrackingModel> baseQuery, FileTransferTrackingQuery query)
    {
        var dynamicSort = DynamicSort()
            .SortUsing(f => f.FileId).ForValue("FileId")
            .SortUsing(f => f.FileName).ForValue("FileName")
            .SortUsing(f => f.FileStatus).ForValue("FileStatus")
            .SortUsing(f => f.FileDirection).ForValue("FileDirection")
            .SortUsing(f => f.FileSize).ForValue("FileSize")
            .SortUsing(f => f.CreatedDate).ForValue("CreatedDate")
            .SortUsing(f => f.TradingPartner).ForValue("TradingPartnerId")
            .WithDefaults(def =>
            {
                def.ByAscending(f => f.FileName);
            }).Build();

        return baseQuery.OrderBy(dynamicSort, query.SortCriteria);
    }
    [MaintainabilityDetails(ApprovedIndex = 46, ReviewedOnSprint = 94, ReviewerName = "Josue Chavez")]
    protected override IQueryable<FileTransferTrackingModel> ApplyWhereClause(IQueryable<FileTransferTrackingModel> baseQuery, FileTransferTrackingQuery query)
    {
        Guid fileId = Guid.Empty;
        Guid.TryParse(query.Where.FileId, out fileId);
        Guid sourceLocationId = Guid.Empty;
        Guid.TryParse(query.Where.FileSourceLocationId, out sourceLocationId);


        var whereClause = DynamicWhere()
            .When(q => !string.IsNullOrEmpty(q.Where.FileId))
                .AddWhere(p => p.FileId == fileId)
            .When(q => !string.IsNullOrEmpty(q.Where.FileName))
                .AddWhere(p => p.FileName == query.Where.FileName)
            .When(q => !string.IsNullOrEmpty(q.Where.FileStatus))
                .AddWhere(p => p.FileStatus == query.Where.FileStatus)
            .When(q => !string.IsNullOrEmpty(q.Where.FileDirection))
                .AddWhere(p => p.FileDirection == query.Where.FileDirection)
            .When(q => !string.IsNullOrEmpty(q.Where.FileTransportType))
                .AddWhere(p => p.FileTransportType == query.Where.FileTransportType)
            .When(q => !string.IsNullOrEmpty(q.Where.FileSourceLocationId))
                .AddWhere(p => p.FileSourceLocationId == sourceLocationId)
            .When(q => !string.IsNullOrEmpty(q.Where.TradingPartner))
                .AddWhere(p => p.TradingPartner == query.Where.TradingPartner)
            .When(q => q.Where.StartDate != null)
                .AddWhere(p => p.CreatedDate >= query.Where.StartDate!.Value)
            .When(q => q.Where.EndDate != null)
                .AddWhere(p => p.CreatedDate <= query.Where.EndDate!.Value)
            .When(q => q.Where.FileSizeFrom > 0M)
                .AddWhere(p => p.FileSize >= query.Where.FileSizeFrom)
            .When(q => q.Where.FileSizeThrough > 0M)
                .AddWhere(p => p.FileSize <= query.Where.FileSizeThrough)
                    .Build();
        return baseQuery.Where(whereClause, query);
    }

    protected override void AssignResults(IQueryable<FileTransferTrackingModel> baseQuery, FileTransferTrackingQuery query)
    {
        query.Results = baseQuery.ToList();

        foreach (var file in query.Results)
        {
            FileEventQuery fileEventQuery = new FileEventQuery()
            {
                Where = new FileEventQueryParms()
                {
                    FileId = file.FileId.ToString()
                }
            };

            FileExceptionQuery fileExceptionQuery = new FileExceptionQuery()
            {
                Where = new FileExceptionQueryParms()
                {
                    FileId = file.FileId.ToString()
                }
            };

            file.Events = new FileEventQueryExecutor().Execute(_databaseContext!, fileEventQuery).Results;
            file.Exceptions = new FileExceptionQueryExecutor().Execute(_databaseContext!, fileExceptionQuery).Results;
        }
    }

    protected override IQueryable<FileTransferTrackingModel> GetBaseQuery(IDbContextBase databaseContext, FileTransferTrackingQuery query)
    {
        _databaseContext = databaseContext;

        var result = GetFileTransferTrackingUNC(databaseContext).Union(GetFileTransferTrackingFTPProtocls(databaseContext));
        return result.AsQueryable();
    }

    private List<FileTransferTrackingModel> GetFileTransferTrackingFTPProtocls(IDbContextBase databaseContext)
    {
        var result = from fm in databaseContext.Set<FtsFileManagementEntity>()
               join sl in databaseContext.Set<FtsSourceLocationEntity>().Where(x => x.FtpServerId != null)
                 on fm.FtsSourceLocationId equals sl.FtsSourceLocationId
               join fs in databaseContext.Set<FxferService.Infrastructure.Persistence.Entities.FtpServer>()
                 on sl.FtpServerId equals (Guid?)fs.FtpServerId
               select new FileTransferTrackingModel()
               {
                   FileId = fm.FileId,
                   FileName = fm.FileName,
                   FileStatus = fm.FileStatus,
                   FileSize = fm.FileSize,
                   CreatedDate = fm.CreatedAt.DateTime,
                   FileDirection = sl.FileDirection,
                   FileTransportType = sl.SourceProtocolCode,
                   TradingPartner = sl.TradingPartner,
                   FileSourceLocationId = sl.FtsSourceLocationId,
                   CheckSumCode = fm.ChecksumCode,
                   SourceLocConfigName = sl.ConfigName,
                   SourceLocAddress = sl.SourceAddress,
                   OriginatingServerName = fs.FtpServerName
               };

        return result.ToList();
    }

    private List<FileTransferTrackingModel> GetFileTransferTrackingUNC(IDbContextBase databaseContext)
    {
        var result = from fm in databaseContext.Set<FtsFileManagementEntity>()
               join sl in databaseContext.Set<FtsSourceLocationEntity>().Where(x => x.SourceProtocolCode == FxferService.Domain.Constants.DataAccessConstants.FileStatusConstant.UNCProtocolCD)
                 on fm.FtsSourceLocationId equals sl.FtsSourceLocationId
               select new FileTransferTrackingModel()
               {
                   CheckSumCode = fm.ChecksumCode,
                   CreatedDate = fm.CreatedAt.DateTime,
                   FileDirection = sl.FileDirection,
                   FileId = fm.FileId,
                   FileName = fm.FileName,
                   FileSize = fm.FileSize,
                   FileSourceLocationId = sl.FtsSourceLocationId,
                   FileStatus = fm.FileStatus,
                   FileTransportType = sl.SourceProtocolCode,
                   OriginatingServerName = string.Empty,
                   SourceLocAddress = sl.SourceAddress,
                   SourceLocConfigName = sl.ConfigName,
                   TradingPartner = sl.TradingPartner
               };

        return result.ToList();
    }
}




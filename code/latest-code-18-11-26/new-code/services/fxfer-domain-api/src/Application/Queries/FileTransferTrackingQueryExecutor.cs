using Microsoft.EntityFrameworkCore;

using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Domain.Constants;
using FtpServerEntity = Fxfer.DomainApi.Domain.Entities.FtpServer;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

namespace Fxfer.DomainApi.Application.Queries;

public sealed class FileTransferTrackingQueryExecutor
{
    public async Task<IReadOnlyList<FileTransferTrackingModel>> ExecuteAsync(
        FxferDbContext dbContext,
        FileTransferTrackingQuery query,
        CancellationToken ct)
    {
        var uncResults = await GetFileTransferTrackingUncAsync(dbContext, ct);
        var ftpResults = await GetFileTransferTrackingFtpProtocolsAsync(dbContext, ct);

        var baseQuery = uncResults.Union(ftpResults).AsQueryable();

        baseQuery = ApplyWhereClause(baseQuery, query);
        baseQuery = ApplySorting(baseQuery, query);
        baseQuery = ApplyPaging(baseQuery, query);

        var results = baseQuery.ToList();

        var fileEventQueryExecutor = new FileEventQueryExecutor();
        var fileExceptionQueryExecutor = new FileExceptionQueryExecutor();

        foreach (var file in results)
        {
            var fileEventQuery = new FileEventQuery
            {
                Where = new Fxfer.DomainApi.Contracts.Queries.Parameters.FileEventQueryParms
                {
                    FileId = file.FileId.ToString()
                }
            };

            var fileExceptionQuery = new FileExceptionQuery
            {
                Where = new Fxfer.DomainApi.Contracts.Queries.Parameters.FileExceptionQueryParms
                {
                    FileId = file.FileId.ToString()
                }
            };

            file.Events = (await fileEventQueryExecutor.ExecuteAsync(dbContext, fileEventQuery, ct)).ToList();
            file.Exceptions = (await fileExceptionQueryExecutor.ExecuteAsync(dbContext, fileExceptionQuery, ct)).ToList();
        }

        return results;
    }

    private static IQueryable<FileTransferTrackingModel> ApplyWhereClause(IQueryable<FileTransferTrackingModel> baseQuery, FileTransferTrackingQuery query)
    {
        // MaintainabilityDetails(ApprovedIndex = 46, ReviewedOnSprint = 94, ReviewerName = "Josue Chavez")
        // F05-023: When a Where field is provided, filter by exact match or inclusive range.
        if (!string.IsNullOrWhiteSpace(query.Where!.FileId) && Guid.TryParse(query.Where.FileId, out var fileId))
        {
            baseQuery = baseQuery.Where(p => p.FileId == fileId);
        }

        if (!string.IsNullOrEmpty(query.Where.FileName))
        {
            baseQuery = baseQuery.Where(p => p.FileName == query.Where.FileName);
        }

        if (!string.IsNullOrEmpty(query.Where.FileStatus))
        {
            baseQuery = baseQuery.Where(p => p.FileStatus == query.Where.FileStatus);
        }

        if (!string.IsNullOrEmpty(query.Where.FileDirection))
        {
            baseQuery = baseQuery.Where(p => p.FileDirection == query.Where.FileDirection);
        }

        if (!string.IsNullOrEmpty(query.Where.FileTransportType))
        {
            baseQuery = baseQuery.Where(p => p.FileTransportType == query.Where.FileTransportType);
        }

        if (!string.IsNullOrWhiteSpace(query.Where.FileSourceLocationId) && Guid.TryParse(query.Where.FileSourceLocationId, out var sourceLocationId))
        {
            baseQuery = baseQuery.Where(p => p.FileSourceLocationId == sourceLocationId);
        }

        if (!string.IsNullOrEmpty(query.Where.TradingPartner))
        {
            baseQuery = baseQuery.Where(p => p.TradingPartner == query.Where.TradingPartner);
        }

        if (query.Where.StartDate != null)
        {
            baseQuery = baseQuery.Where(p => p.CreatedDate >= query.Where.StartDate);
        }

        if (query.Where.EndDate != null)
        {
            baseQuery = baseQuery.Where(p => p.CreatedDate <= query.Where.EndDate);
        }

        if (query.Where.FileSizeFrom > 0M)
        {
            baseQuery = baseQuery.Where(p => p.FileSize >= query.Where.FileSizeFrom);
        }

        if (query.Where.FileSizeThrough > 0M)
        {
            baseQuery = baseQuery.Where(p => p.FileSize <= query.Where.FileSizeThrough);
        }

        return baseQuery;
    }

    private static IQueryable<FileTransferTrackingModel> ApplySorting(IQueryable<FileTransferTrackingModel> baseQuery, FileTransferTrackingQuery query)
    {
        // MaintainabilityDetails(ApprovedIndex = 59, ReviewedOnSprint = 94, ReviewerName = "Josue Chavez")
        // F05-024: Supports specific sort keys and defaults to FileName ASC.
        if (query.SortCriteria is null || query.SortCriteria.Count == 0)
        {
            return baseQuery.OrderBy(f => f.FileName);
        }

        IOrderedQueryable<FileTransferTrackingModel>? ordered = null;

        for (var i = 0; i < query.SortCriteria.Count; i++)
        {
            var sort = query.SortCriteria[i];

            ordered = (ordered, sort.FieldName, sort.Order) switch
            {
                (null, "FileId", SortOrder.Desc) => baseQuery.OrderByDescending(f => f.FileId),
                (null, "FileId", _) => baseQuery.OrderBy(f => f.FileId),
                (null, "FileName", SortOrder.Desc) => baseQuery.OrderByDescending(f => f.FileName),
                (null, "FileName", _) => baseQuery.OrderBy(f => f.FileName),
                (null, "FileStatus", SortOrder.Desc) => baseQuery.OrderByDescending(f => f.FileStatus),
                (null, "FileStatus", _) => baseQuery.OrderBy(f => f.FileStatus),
                (null, "FileDirection", SortOrder.Desc) => baseQuery.OrderByDescending(f => f.FileDirection),
                (null, "FileDirection", _) => baseQuery.OrderBy(f => f.FileDirection),
                (null, "FileSize", SortOrder.Desc) => baseQuery.OrderByDescending(f => f.FileSize),
                (null, "FileSize", _) => baseQuery.OrderBy(f => f.FileSize),
                (null, "CreatedDate", SortOrder.Desc) => baseQuery.OrderByDescending(f => f.CreatedDate),
                (null, "CreatedDate", _) => baseQuery.OrderBy(f => f.CreatedDate),
                (null, "TradingPartnerId", SortOrder.Desc) => baseQuery.OrderByDescending(f => f.TradingPartner),
                (null, "TradingPartnerId", _) => baseQuery.OrderBy(f => f.TradingPartner),
                (not null, "FileId", SortOrder.Desc) => ordered.ThenByDescending(f => f.FileId),
                (not null, "FileId", _) => ordered.ThenBy(f => f.FileId),
                (not null, "FileName", SortOrder.Desc) => ordered.ThenByDescending(f => f.FileName),
                (not null, "FileName", _) => ordered.ThenBy(f => f.FileName),
                (not null, "FileStatus", SortOrder.Desc) => ordered.ThenByDescending(f => f.FileStatus),
                (not null, "FileStatus", _) => ordered.ThenBy(f => f.FileStatus),
                (not null, "FileDirection", SortOrder.Desc) => ordered.ThenByDescending(f => f.FileDirection),
                (not null, "FileDirection", _) => ordered.ThenBy(f => f.FileDirection),
                (not null, "FileSize", SortOrder.Desc) => ordered.ThenByDescending(f => f.FileSize),
                (not null, "FileSize", _) => ordered.ThenBy(f => f.FileSize),
                (not null, "CreatedDate", SortOrder.Desc) => ordered.ThenByDescending(f => f.CreatedDate),
                (not null, "CreatedDate", _) => ordered.ThenBy(f => f.CreatedDate),
                (not null, "TradingPartnerId", SortOrder.Desc) => ordered.ThenByDescending(f => f.TradingPartner),
                (not null, "TradingPartnerId", _) => ordered.ThenBy(f => f.TradingPartner),
                _ => throw new ArgumentOutOfRangeException(nameof(query.SortCriteria), sort.FieldName, "Unknown sort field")
            };
        }

        return ordered ?? baseQuery;
    }

    private static IQueryable<FileTransferTrackingModel> ApplyPaging(IQueryable<FileTransferTrackingModel> baseQuery, FileTransferTrackingQuery query)
    {
        if (query.Page is null || query.PageSize is null)
        {
            return baseQuery;
        }

        if (query.Page <= 0 || query.PageSize <= 0)
        {
            return baseQuery;
        }

        var skip = (query.Page.Value - 1) * query.PageSize.Value;
        return baseQuery.Skip(skip).Take(query.PageSize.Value);
    }

    private static Task<List<FileTransferTrackingModel>> GetFileTransferTrackingUncAsync(
        FxferDbContext dbContext,
        CancellationToken ct)
    {
        var result =
            from fm in dbContext.Set<FtsFileManagement>()
            join sl in dbContext.Set<FtsSourceLocation>()
                    .Where(x => x.SourceProtocolCD == DataAccessConstants.FileStatusConstant.UNCProtocolCD && x.IsValid == true)
                on fm.FileSourceLocationId equals sl.FtsSourceLocationId
            select new FileTransferTrackingModel
            {
                CheckSumCode = fm.ChecksumCD,
                CreatedDate = fm.CreatedDate,
                FileDirection = sl.FileDirection,
                FileId = fm.FileId,
                FileName = fm.FileName,
                FileSize = fm.FileSize,
                FileSourceLocationId = sl.FtsSourceLocationId,
                FileStatus = fm.FileStatus,
                FileTransportType = sl.SourceProtocolCD,
                OriginatingServerName = string.Empty,
                SourceLocAddress = sl.SourceAddress,
                SourceLocConfigName = sl.ConfigName,
                TradingPartner = sl.TradingPartner
            };

        return result.ToListAsync(ct);
    }

    private static Task<List<FileTransferTrackingModel>> GetFileTransferTrackingFtpProtocolsAsync(
        FxferDbContext dbContext,
        CancellationToken ct)
    {
        var result =
            from fm in dbContext.Set<FtsFileManagement>()
            join sl in dbContext.Set<FtsSourceLocation>()
                    .Where(x => x.FtpServerId != null && x.IsValid == true)
                on fm.FileSourceLocationId equals sl.FtsSourceLocationId
            join fs in dbContext.Set<FtpServerEntity>() on sl.FtpServerId equals fs.FtpServerId
            select new FileTransferTrackingModel
            {
                FileId = fm.FileId,
                FileName = fm.FileName,
                FileStatus = fm.FileStatus,
                FileSize = fm.FileSize,
                CreatedDate = fm.CreatedDate,
                FileDirection = sl.FileDirection,
                FileTransportType = sl.SourceProtocolCD,
                TradingPartner = sl.TradingPartner,
                FileSourceLocationId = sl.FtsSourceLocationId,
                CheckSumCode = fm.ChecksumCD,
                SourceLocConfigName = sl.ConfigName,
                SourceLocAddress = sl.SourceAddress,
                OriginatingServerName = fs.FtpServerName
            };

        return result.ToListAsync(ct);
    }
}


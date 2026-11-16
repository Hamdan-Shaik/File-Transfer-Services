using FxferService.Application.Contracts.Queries;
using FxferService.Domain.Contracts.Domain;
using FxferService.Infrastructure.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Entities;
using FxferService.Infrastructure.Queries;

namespace FxferService.Application.QueryHandlers;

public class FileExceptionQueryExecutor : QueryExecutorBase<FileExceptionModel, FileExceptionQuery>
{
    protected override IQueryable<FileExceptionModel> ApplyPaging(IQueryable<FileExceptionModel> baseQuery, FileExceptionQuery query)
    {
        return base.ApplyPaging(baseQuery, query);
    }

    protected override IQueryable<FileExceptionModel> ApplySorting(IQueryable<FileExceptionModel> baseQuery, FileExceptionQuery query)
    {
        var dynamicSort = DynamicSort()
            .SortUsing(p => p.ExceptionId).ForValue("EXCEPTION_ID")
            .WithDefaults(def =>
            {
                def.ByAscending(p => p.ExceptionId)
                    .ThenByAscending(p => p.ExceptionType);
            }).Build();

        return baseQuery.OrderBy(dynamicSort, query.SortCriteria);
    }

    protected override IQueryable<FileExceptionModel> ApplyWhereClause(IQueryable<FileExceptionModel> baseQuery, FileExceptionQuery query)
    {
        Guid fileId = Guid.Empty;
        Guid.TryParse(query.Where.FileId, out fileId);
        var whereClause = DynamicWhere()
            .When(q => q.Where.FileId != null)
                .AddWhere(p => p.FileId == fileId)
           .Build();

        return baseQuery.Where(whereClause, query);
    }

    protected override void AssignResults(IQueryable<FileExceptionModel> baseQuery, FileExceptionQuery query)
    {
        query.Results = baseQuery.ToList();
    }

    protected override IQueryable<FileExceptionModel> GetBaseQuery(IDbContextBase databaseContext, FileExceptionQuery query)
    {
        return from fm in databaseContext.Set<FtsFileManagementEntity>()
               join fe in databaseContext.Set<FtsFileExceptionEntity>()
               on fm.FileId equals fe.FileId
               select new FileExceptionModel()
               {
                   EventId = fe.EventId,
                   FileId = fe.FileId,
                   ExceptionType = fe.ExceptionType,
                   Message = fe.Message,
                   StackTrace = fe.StackTrace,
                   MethodName = fe.MethodName,
                   CreatedDate = fm.CreatedAt.DateTime,
                   ExceptionId = fe.ExceptionId
               };
    }
}


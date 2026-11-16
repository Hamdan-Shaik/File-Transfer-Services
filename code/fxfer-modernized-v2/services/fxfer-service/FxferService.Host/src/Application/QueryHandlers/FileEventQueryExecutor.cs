using FxferService.Application.Contracts.Queries;
using FxferService.Domain.Contracts.Domain;
using FxferService.Infrastructure.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Entities;
using FxferService.Infrastructure.Queries;

namespace FxferService.Application.QueryHandlers;

public class FileEventQueryExecutor : QueryExecutorBase<FileEventModel, FileEventQuery>
{
    protected override IQueryable<FileEventModel> ApplyPaging(IQueryable<FileEventModel> baseQuery, FileEventQuery query)
    {
        return base.ApplyPaging(baseQuery, query);
    }

    protected override IQueryable<FileEventModel> ApplySorting(IQueryable<FileEventModel> baseQuery, FileEventQuery query)
    {
        var dynamicSort = DynamicSort()
            .SortUsing(p => p.EventId).ForValue("EVENT_ID")
            .WithDefaults(def =>
            {
                def.ByAscending(p => p.EventId)
                    .ThenByAscending(p => p.EventCode);
            }).Build();

        return baseQuery.OrderBy(dynamicSort, query.SortCriteria);
    }

    protected override IQueryable<FileEventModel> ApplyWhereClause(IQueryable<FileEventModel> baseQuery, FileEventQuery query)
    {
        Guid fileId = Guid.Empty;
        Guid.TryParse(query.Where.FileId, out fileId);
        var whereClause = DynamicWhere()
            .When(q => q.Where.FileId != null)
                .AddWhere(p => p.FileId == fileId)
           .Build();

        return baseQuery.Where(whereClause, query);
    }

    protected override void AssignResults(IQueryable<FileEventModel> baseQuery, FileEventQuery query)
    {
        query.Results = baseQuery.ToList();
    }

    protected override IQueryable<FileEventModel> GetBaseQuery(IDbContextBase databaseContext, FileEventQuery query)
    {
        return from fm in databaseContext.Set<FtsFileManagementEntity>()
               join fe in databaseContext.Set<FtsFileEventEntity>()
               on fm.FileId equals fe.FileId
               select new FileEventModel()
               {
                   EventId = fe.EventId,
                   FileId = fe.FileId,
                   EventCode = fe.EventCode,
                   CreatedDate = fm.CreatedAt.DateTime,
                   EventTrackDescription = fe.TrackDescription
               };
    }
}


using FxferService.Application.Contracts.Queries;
using FxferService.Domain.Contracts.Domain;
using FxferService.Domain.Utilities;
using FxferService.Infrastructure.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Entities;
using FxferService.Infrastructure.Queries;

namespace FxferService.Application.QueryHandlers;

public class SearchSourceLocationQueryExecutor : QueryExecutorBase<SourceLocationModel, SourceLocationQuery>
{
    // F02-011: Supports requested sort keys with a default of ConfigId asc then IsActive desc.
    [MaintainabilityDetails(ApprovedIndex = 58, ReviewedOnSprint = 94, ReviewerName = "Josue Chavez")]
    protected override IQueryable<SourceLocationModel> ApplySorting(IQueryable<SourceLocationModel> baseQuery, SourceLocationQuery query)
    {
        var dynamicSort = DynamicSort()
            .SortUsing(p => p.ConfigId).ForValue("ConfigId")
            .SortUsing(p => p.ConfigName).ForValue("ConfigName")
            .SortUsing(p => p.Address).ForValue("SourceAddress")
            .SortUsing(p => p.Mask).ForValue("SourceFileMask")
            .SortUsing(p => p.OwnerName).ForValue("OwnerName")
            .SortUsing(p => p.OwnerSubsystem).ForValue("ModuleName")
            .SortUsing(p => p.IsActive).ForValue("Status")
            .WithDefaults(def =>
            {
                def.ByAscending(l => l.ConfigId)
                    .ThenByDescending(l => l.IsActive);
            }).Build();

        return baseQuery.OrderBy(dynamicSort, query.SortCriteria);
    }

    // F02-010: Conditionally applies exact-match filters, including case-insensitive comparisons.
    // F02-053/F02-055: Applies ConfigId filtering so update/invalidate flows can detect missing ConfigId via empty results.
    [MaintainabilityDetails(ApprovedIndex = 55, ReviewedOnSprint = 94, ReviewerName = "Josue Chavez")]
    protected override IQueryable<SourceLocationModel> ApplyWhereClause(IQueryable<SourceLocationModel> basequery, SourceLocationQuery query)
    {
        var whereClause = DynamicWhere()
            .When(q => q.Where.ConfigId != null && q.Where.ConfigId > 0)
                .AddWhere(p => p.ConfigId == query.Where.ConfigId!.Value)
            .When(q => !string.IsNullOrEmpty(q.Where.OwnerName))
                .AddWhere(p => p.OwnerName.ToUpper() == query.Where.OwnerName.ToUpper())
            .When(q => !string.IsNullOrEmpty(q.Where.ConfigName))
                .AddWhere(p => p.ConfigName.ToUpper() == query.Where.ConfigName.ToUpper())
            .When(q => !string.IsNullOrEmpty(q.Where.OwnerSubsystem))
                .AddWhere(p => p.OwnerSubsystem == query.Where.OwnerModuleGuid)
            .When(q => !string.IsNullOrEmpty(q.Where.Protocol))
                .AddWhere(p => p.Protocol == query.Where.Protocol)
            .When(q => q.Where.IsActive)
                .AddWhere(p => p.IsActive == query.Where.IsActive)
            .Build();

        return basequery.Where(whereClause, query);
    }

    protected override void AssignResults(IQueryable<SourceLocationModel> baseQuery, SourceLocationQuery query)
    {
        query.Results = baseQuery.ToList();
    }

    // F02-009: Projects each source location into the query result model, with IsActive = IsValid.
    protected override IQueryable<SourceLocationModel> GetBaseQuery(IDbContextBase context, SourceLocationQuery query)
    {
        return from identifier in context.Set<FtsSourceLocationEntity>()
               select new SourceLocationModel()
               {
                   FtsSourceLocationId = identifier.FtsSourceLocationId,
                   ConfigId = identifier.ConfigId,
                   ConfigName = identifier.ConfigName,
                   OwnerName = identifier.OwnerName,
                   OwnerSubsystem = identifier.ModuleId,
                   Protocol = identifier.SourceProtocolCode,
                   Address = identifier.SourceAddress,
                   Mask = identifier.SourceFileMask,
                   IsActive = identifier.IsValid,
               };
    }
}


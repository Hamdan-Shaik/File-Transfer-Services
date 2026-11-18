using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.DomainApi.Api.Endpoints.Queries;

public static class SourceLocationsQueries
{
    public static RouteGroupBuilder MapSourceLocationsQueries(this RouteGroupBuilder group)
    {
        group.MapGet("/source-locations", async (
            FxferDbContext dbContext,
            bool isActive = true,
            CancellationToken ct = default) =>
        {
            var results = await dbContext.Set<FtsSourceLocation>()
                .AsNoTracking()
                .Where(x => x.IsValid == isActive)
                .OrderBy(x => x.ConfigName)
                .Select(x => new SourceLocationOption
                {
                    FtsSourceLocationId = x.FtsSourceLocationId,
                    ConfigName = x.ConfigName ?? string.Empty,
                    Address = x.SourceAddress ?? string.Empty
                })
                .ToListAsync(ct);

            return Results.Ok(results);
        });

        return group;
    }
}


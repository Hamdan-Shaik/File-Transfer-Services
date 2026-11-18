using Fxfer.Bff.Contracts.Common;
using Fxfer.Bff.Application.Providers;

namespace Fxfer.Bff.Api.Endpoints.V1;

public static class SourceLocationsEndpoints
{
    public static RouteGroupBuilder MapSourceLocationsEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/source-locations", async (
            IFileTransferQueryProvider provider,
            bool isActive = true,
            CancellationToken ct = default) =>
        {
            var result = await provider.GetSourceLocationsAsync(isActive, ct);
            return Results.Ok(result);
        });

        return group;
    }
}


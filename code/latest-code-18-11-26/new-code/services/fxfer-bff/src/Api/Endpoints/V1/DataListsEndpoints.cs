using Fxfer.Bff.Application.Providers;

namespace Fxfer.Bff.Api.Endpoints.V1;

public static class DataListsEndpoints
{
    public static RouteGroupBuilder MapDataListsEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/datalists", async (
            string contentId,
            string? startsWith,
            string? localeId,
            DataListsClient client,
            CancellationToken cancellationToken) =>
        {
            var items = await client.GetItemsAsync(contentId, startsWith, localeId, cancellationToken);
            return Results.Ok(items);
        });

        return group;
    }
}


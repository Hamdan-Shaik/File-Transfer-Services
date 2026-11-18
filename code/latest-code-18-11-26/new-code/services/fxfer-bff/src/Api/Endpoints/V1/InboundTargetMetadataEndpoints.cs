using Fxfer.Bff.Contracts.Common;
using Fxfer.Bff.Application.Providers;

namespace Fxfer.Bff.Api.Endpoints.V1;

public static class InboundTargetMetadataEndpoints
{
    public static RouteGroupBuilder MapInboundTargetMetadataEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/inbound-target-metadata", async (
            IFileTransferQueryProvider provider,
            string? contractName,
            CancellationToken ct = default) =>
        {
            var result = await provider.GetMetadataIdsAsync(contractName, ct);
            return Results.Ok(result);
        });

        return group;
    }
}


using Fxfer.Bff.Contracts.Common;
using Fxfer.Bff.Contracts.FtpServers;
using Fxfer.Bff.Application.Providers;

namespace Fxfer.Bff.Api.Endpoints.V1;

public static class FtpServersEndpoints
{
    public static RouteGroupBuilder MapFtpServersEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/ftp-servers", async (
            IFileTransferQueryProvider provider,
            bool? isValid,
            Guid? ftpServerId,
            string? ftpServerName,
            string? ftpDescription,
            string? nameMatch,
            string? descriptionMatch,
            int page = 1,
            int pageSize = 50,
            CancellationToken ct = default) =>
        {
            var result = await provider.GetFtpServersAsync(
                new FtpServerSearchParams
                {
                    IsValid = isValid,
                    FtpServerId = ftpServerId,
                    FtpServerName = ftpServerName,
                    FtpDescription = ftpDescription,
                    NameMatch = nameMatch,
                    DescriptionMatch = descriptionMatch,
                    Page = page,
                    PageSize = pageSize
                },
                removePassword: true,
                ct);

            return Results.Ok(result);
        });

        group.MapPost("/ftp-servers", async (
            IFileTransferCommandProvider provider,
            FtpServerUpsertRequest request,
            CancellationToken ct = default) =>
        {
            var created = await provider.AddFtpServerAsync(request, ct);
            return Results.Created($"/api/file-transfer/v1/ftp-servers/{created.FtpServerId}", created);
        });

        group.MapPut("/ftp-servers/{ftpServerId:guid}", async (
            IFileTransferCommandProvider provider,
            Guid ftpServerId,
            FtpServerUpsertRequest request,
            CancellationToken ct = default) =>
        {
            var updated = await provider.UpdateFtpServerAsync(ftpServerId, request, ct);
            return Results.Ok(updated);
        });

        group.MapPatch("/ftp-servers/{ftpServerId:guid}", async (
            IFileTransferCommandProvider provider,
            Guid ftpServerId,
            FtpServerPatchRequest request,
            CancellationToken ct = default) =>
        {
            var updated = await provider.InvalidateFtpServerAsync(ftpServerId, request, ct);
            return Results.Ok(updated);
        });

        group.MapGet("/ftp-servers/options", async (
            IFileTransferQueryProvider provider,
            bool isValid = true,
            CancellationToken ct = default) =>
        {
            var options = await provider.GetFtpServersOptionsAsync(isValid, ct);
            return Results.Ok(options);
        });

        return group;
    }
}


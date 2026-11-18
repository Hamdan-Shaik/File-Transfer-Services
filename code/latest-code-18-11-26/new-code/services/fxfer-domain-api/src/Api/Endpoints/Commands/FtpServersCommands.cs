using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using HPP.Core.BAS.CQRS.UserMeta;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.DomainApi.Api.Endpoints.Commands;

public static class FtpServersCommands
{
    public static RouteGroupBuilder MapFtpServersCommands(this RouteGroupBuilder group)
    {
        group.MapPost("/ftp-servers", async (
            ISender sender,
            FxferDbContext dbContext,
            FtpServerUpsertRequest request,
            CancellationToken ct = default) =>
        {
            // F02-017: route the incoming command through the command router.
            var command = new AddFtpServerCommand
            {
                Requestor = new RequestorModel { IdentifierId = request.OperatorId },
                OperatorId = request.OperatorId,
                FtpServerName = request.FtpServerName,
                FtpDescription = request.FtpDescription,
                FtpServerIp = request.FtpServerIp,
                FtpTransferMode = request.FtpTransferMode,
                FtpUser = request.FtpUser,
                FtpPassword = request.CredentialSecretRef,
                FtpPort = checked((short)request.FtpPort),
                FtpProxyAddress = request.FtpProxyAddress,
                FtpProxyPort = request.FtpProxyPort.HasValue ? checked((short)request.FtpProxyPort.Value) : null,
                UseSSHAuth = request.UseSshAuth,
                IsValid = request.IsValid
            };

            await sender.Send(command, ct);

            var created = await dbContext.FtpServers
                .AsNoTracking()
                .SingleAsync(x => x.FtpServerId == command.FtpServerId, ct);

            var response = Map(created);
            var location = $"/api/fxfer-domain/v1/ftp-servers/{created.FtpServerId}";
            return Results.Created(location, response);
        });

        group.MapPut("/ftp-servers/{ftpServerId:guid}", async (
            ISender sender,
            FxferDbContext dbContext,
            Guid ftpServerId,
            FtpServerUpsertRequest request,
            CancellationToken ct = default) =>
        {
            // F02-019: route the incoming command through the command router.
            var command = new UpdateFtpServerCommand
            {
                Requestor = new RequestorModel { IdentifierId = request.OperatorId },
                OperatorId = request.OperatorId,
                FtpServerId = ftpServerId,
                FtpServerName = request.FtpServerName,
                FtpDescription = request.FtpDescription,
                FtpServerIp = request.FtpServerIp,
                FtpTransferMode = request.FtpTransferMode,
                FtpUser = request.FtpUser,
                FtpPassword = request.CredentialSecretRef,
                FtpPort = checked((short)request.FtpPort),
                FtpProxyAddress = request.FtpProxyAddress,
                FtpProxyPort = request.FtpProxyPort.HasValue ? checked((short)request.FtpProxyPort.Value) : null,
                UseSSHAuth = request.UseSshAuth,
                IsValid = request.IsValid
            };

            await sender.Send(command, ct);

            var updated = await dbContext.FtpServers
                .AsNoTracking()
                .SingleAsync(x => x.FtpServerId == ftpServerId, ct);

            return Results.Ok(Map(updated));
        });

        group.MapPatch("/ftp-servers/{ftpServerId:guid}", async (
            ISender sender,
            FxferDbContext dbContext,
            Guid ftpServerId,
            FtpServerPatchRequest request,
            CancellationToken ct = default) =>
        {
            // F02-021, F03-004: route the incoming invalidation command through the command router.
            var command = new InvalidateFtpServerCommand
            {
                Requestor = new RequestorModel { IdentifierId = request.OperatorId },
                OperatorId = request.OperatorId,
                FtpServerId = ftpServerId
            };

            await sender.Send(command, ct);

            var invalidated = await dbContext.FtpServers
                .AsNoTracking()
                .SingleAsync(x => x.FtpServerId == ftpServerId, ct);

            return Results.Ok(Map(invalidated));
        });

        return group;
    }

    private static FtpServer Map(Fxfer.DomainApi.Domain.Entities.FtpServer entity)
    {
        return new FtpServer
        {
            FtpServerId = entity.FtpServerId,
            FtpServerName = entity.FtpServerName,
            FtpServerIp = entity.FtpServerHost,
            FtpTransferMode = entity.FtpTransferMode,
            FtpUser = entity.FtpUser,
            CredentialSecretRef = entity.FtpPasswordSecretRef,
            FtpDescription = entity.FtpDescription,
            FtpPort = entity.FtpPort,
            FtpProxyAddress = entity.FtpProxyAddress,
            FtpProxyPort = entity.FtpProxyPort,
            UseSshAuth = entity.UseSshAuth,
            IsValid = entity.IsValid,
            LastModifiedDate = entity.LastModifiedDate,
            CreationDate = entity.CreationDate,
            OperatorId = entity.OperatorId
        };
    }
}


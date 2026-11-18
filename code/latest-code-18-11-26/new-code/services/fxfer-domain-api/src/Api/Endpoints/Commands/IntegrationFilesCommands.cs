using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using HPP.Core.BAS.CQRS.UserMeta;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Fxfer.DomainApi.Api.Endpoints.Commands;

public static class IntegrationFilesCommands
{
    public static RouteGroupBuilder MapIntegrationFilesCommands(this RouteGroupBuilder group)
    {
        group.MapPatch("/integration-files/{integrationFileId:guid}", async (
            HttpContext context,
            ISender sender,
            FxferDbContext dbContext,
            Guid integrationFileId,
            IntegrationFileProcessingRequest request,
            CancellationToken ct = default) =>
        {
            // F03-004: route the incoming command through the command router.
            var operatorId = context.User.FindFirstValue("sub") ?? string.Empty;

            var command = new UpdateIntegrationFileCommand
            {
                Requestor = new RequestorModel { IdentifierId = operatorId },
                FtsIntegrationFileId = integrationFileId,
                ConfigId = request.ConfigId,
                FileName = request.FileName,
                IsValid = request.IsValid
            };

            await sender.Send(command, ct);

            var updated = await dbContext.FtsIntegrationFile
                .AsNoTracking()
                .SingleAsync(x => x.FtsIntegrationFileId == integrationFileId, ct);

            return Results.Ok(new IntegrationFileDefinition
            {
                FtsIntegrationFileId = updated.FtsIntegrationFileId,
                GroupId = updated.GroupId,
                ConfigId = updated.ConfigId,
                FileName = updated.FileName,
                ETFOutboundMetadataDefnId = updated.ETFOutboundMetadataDefnId,
                IsProcessed = updated.IsProcessed,
                IsValid = updated.IsValid,
                CreationDate = updated.CreationDate,
                LastModifiedDate = updated.LastModifiedDate
            });
        });

        return group;
    }
}


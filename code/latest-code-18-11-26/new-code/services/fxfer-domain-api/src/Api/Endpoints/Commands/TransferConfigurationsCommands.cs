using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;
using HPP.Core.BAS.CQRS.UserMeta;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.DomainApi.Api.Endpoints.Commands;

public static class TransferConfigurationsCommands
{
    public static RouteGroupBuilder MapTransferConfigurationsCommands(this RouteGroupBuilder group)
    {
        group.MapPost("/transfer-configurations", async (
            ISender sender,
            FxferDbContext dbContext,
            TransferConfigurationUpsertRequest request,
            CancellationToken ct = default) =>
        {
            // F01-034, F01-035, F01-036: route the incoming add command through the command router.
            var command = BuildAddCommand(request);
            var domainEvent = await sender.Send(command, ct);
            var sourceLocationId = domainEvent.Payload.AggregateId;

            var response = await LoadTransferConfigurationAsync(dbContext, sourceLocationId, ct);
            var location = $"/api/fxfer-domain/v1/transfer-configurations/{sourceLocationId}";
            return Results.Created(location, response);
        });

        group.MapPut("/transfer-configurations/{sourceLocationId:guid}", async (
            ISender sender,
            FxferDbContext dbContext,
            Guid sourceLocationId,
            TransferConfigurationUpsertRequest request,
            CancellationToken ct = default) =>
        {
            var configId = await dbContext.Set<FtsSourceLocation>()
                .AsNoTracking()
                .Where(x => x.FtsSourceLocationId == sourceLocationId)
                .Select(x => (int?)x.ConfigId)
                .SingleOrDefaultAsync(ct);

            if (configId is null)
            {
                return Results.NotFound();
            }

            // F01-055: route the incoming update command through the command router.
            var command = BuildUpdateCommand(sourceLocationId, configId.Value, request);
            await sender.Send(command, ct);

            var response = await LoadTransferConfigurationAsync(dbContext, sourceLocationId, ct);
            return Results.Ok(response);
        });

        group.MapPatch("/transfer-configurations/{sourceLocationId:guid}", async (
            ISender sender,
            FxferDbContext dbContext,
            Guid sourceLocationId,
            TransferConfigurationPatchRequest request,
            CancellationToken ct = default) =>
        {
            var configId = await dbContext.Set<FtsSourceLocation>()
                .AsNoTracking()
                .Where(x => x.FtsSourceLocationId == sourceLocationId)
                .Select(x => (int?)x.ConfigId)
                .SingleOrDefaultAsync(ct);

            if (configId is null)
            {
                return Results.NotFound();
            }

            if (request.IsActive == false)
            {
                // F01-057: route the incoming invalidation command through the command router.
                var command = new FtsSourceLocationInvalidateCommand
                {
                    OperatorId = request.OperatorId,
                    FtsSourceLocationId = sourceLocationId.ToString(),
                    ConfigId = configId.Value,
                    DestinationAddress = []
                };

                await sender.Send(command, ct);
            }

            var response = await LoadTransferConfigurationAsync(dbContext, sourceLocationId, ct);
            return Results.Ok(response);
        });

        return group;
    }

    private static FtsSourceLocationAddCommand BuildAddCommand(TransferConfigurationUpsertRequest request)
    {
        var ownerPhone = request.OwnerPhoneNumber.Replace("-", string.Empty, StringComparison.Ordinal);

        var destinations = request.Destinations.Select(dest =>
        {
            var destContactPhone = dest.ContactPhone;
            if (destContactPhone is not null)
            {
                destContactPhone = destContactPhone.Replace("-", string.Empty, StringComparison.Ordinal);
            }

            return new FtsDestinationLocationAdded
            {
                DestinationId = dest.DestinationId ?? Guid.Empty,
                DestinationAddress = dest.DestinationAddress,
                DestinationProtocol = dest.DestinationProtocol,
                FtpServerId = dest.FtpServerId ?? Guid.Empty,
                RenamePattern = dest.RenamePattern,
                OverWriteAtDestination = dest.OverWriteAtDestination ?? false,
                CompressAtDestination = dest.CompressAtDestination ?? false,
                RenameAtDestination = dest.RenameAtDestination ?? false,
                DestinationArchive = dest.DestinationArchive ?? false,
                ContactName = dest.ContactName,
                ContactPhone = destContactPhone,
                ContactEmail = dest.ContactEmail,
                IsValid = dest.IsValid ?? true,
                FTEMetadataId = dest.FteMetadataId,
                DestinationType = dest.DestinationType,
                ContractName = dest.ContractName,
                ImportJobName = dest.ImportJobName,
                OperatorId = dest.OperatorId
            };
        }).ToList();

        return new FtsSourceLocationAddCommand
        {
            Requestor = new RequestorModel { IdentifierId = request.OperatorId },
            OperatorId = request.OperatorId,
            ConfigId = 0,
            ConfigName = request.ConfigName,
            Description = request.Description,
            FtpServerId = request.FtpServerId?.ToString(),
            Address = request.Address,
            Mask = request.Mask,
            Protocol = request.Protocol,
            FileDirection = request.FileDirection,
            OwnerName = request.OwnerName,
            OwnerEmail = request.OwnerEmail,
            OwnerModule = request.OwnerSubsystem.ToString(),
            OwnerPhone = ownerPhone,
            TradingPartner = request.TradingPartner,
            DestinationAddress = destinations,
            RunOnMonday = request.RunOnMonday ?? false,
            RunOnTuesday = request.RunOnTuesday ?? false,
            RunOnWed = request.RunOnWednesday ?? false,
            RunOnThrus = request.RunOnThursday ?? false,
            RunOnFri = request.RunOnFriday ?? false,
            RunOnSat = request.RunOnSaturday ?? false,
            RunOnSun = request.RunOnSunday ?? false,
            RunOnFrstDayOfMonthInd = request.RunOnFirstDay ?? false,
            RunOnLastDayOfMonthInd = request.RunOnLastDay ?? false,
            RunOnThisDayOfMonth = checked((byte)(request.RunOnThisDay ?? 0)),
            StartTime = request.StartWindow,
            EndTime = request.EndWindow,
            SendNotifications = request.SendNotification ?? false,
            IsValid = request.IsActive ?? true,
            IsSuspended = request.Suspended ?? false,
            IsSuspendedChangedDate = request.Suspended == true ? DateTime.UtcNow : null
        };
    }

    private static FtsSourceLocationUpdateCommand BuildUpdateCommand(Guid sourceLocationId, int configId, TransferConfigurationUpsertRequest request)
    {
        var ownerPhone = request.OwnerPhoneNumber.Replace("-", string.Empty, StringComparison.Ordinal);

        var destinations = request.Destinations.Select(dest =>
        {
            var destContactPhone = dest.ContactPhone;
            if (destContactPhone is not null)
            {
                destContactPhone = destContactPhone.Replace("-", string.Empty, StringComparison.Ordinal);
            }

            return new FtsDestinationLocationAdded
            {
                DestinationId = dest.DestinationId ?? Guid.Empty,
                DestinationAddress = dest.DestinationAddress,
                DestinationProtocol = dest.DestinationProtocol,
                FtpServerId = dest.FtpServerId ?? Guid.Empty,
                RenamePattern = dest.RenamePattern,
                OverWriteAtDestination = dest.OverWriteAtDestination ?? false,
                CompressAtDestination = dest.CompressAtDestination ?? false,
                RenameAtDestination = dest.RenameAtDestination ?? false,
                DestinationArchive = dest.DestinationArchive ?? false,
                ContactName = dest.ContactName,
                ContactPhone = destContactPhone,
                ContactEmail = dest.ContactEmail,
                IsValid = dest.IsValid ?? true,
                FTEMetadataId = dest.FteMetadataId,
                DestinationType = dest.DestinationType,
                ContractName = dest.ContractName,
                ImportJobName = dest.ImportJobName,
                OperatorId = dest.OperatorId
            };
        }).ToList();

        return new FtsSourceLocationUpdateCommand
        {
            Requestor = new RequestorModel { IdentifierId = request.OperatorId },
            OperatorId = request.OperatorId,
            FtsSourceLocationId = sourceLocationId.ToString(),
            ConfigId = configId,
            ConfigName = request.ConfigName,
            Description = request.Description,
            FtpServerId = request.FtpServerId?.ToString(),
            Address = request.Address,
            Mask = request.Mask,
            Protocol = request.Protocol,
            FileDirection = request.FileDirection,
            OwnerName = request.OwnerName,
            OwnerEmail = request.OwnerEmail,
            OwnerModule = request.OwnerSubsystem.ToString(),
            OwnerPhone = ownerPhone,
            TradingPartner = request.TradingPartner,
            DestinationAddress = destinations,
            RunOnMonday = request.RunOnMonday ?? false,
            RunOnTuesday = request.RunOnTuesday ?? false,
            RunOnWed = request.RunOnWednesday ?? false,
            RunOnThrus = request.RunOnThursday ?? false,
            RunOnFri = request.RunOnFriday ?? false,
            RunOnSat = request.RunOnSaturday ?? false,
            RunOnSun = request.RunOnSunday ?? false,
            RunOnFrstDayOfMonthInd = request.RunOnFirstDay ?? false,
            RunOnLastDayOfMonthInd = request.RunOnLastDay ?? false,
            RunOnThisDayOfMonth = checked((byte)(request.RunOnThisDay ?? 0)),
            StartTime = request.StartWindow,
            EndTime = request.EndWindow,
            SendNotifications = request.SendNotification ?? false,
            IsValid = request.IsActive ?? true,
            IsSuspended = request.Suspended ?? false,
            IsSuspendedChangedDate = request.Suspended == true ? DateTime.UtcNow : null
        };
    }

    private static async Task<TransferConfiguration> LoadTransferConfigurationAsync(
        FxferDbContext dbContext,
        Guid sourceLocationId,
        CancellationToken ct)
    {
        var sourceLocation = await dbContext.Set<FtsSourceLocation>()
            .AsNoTracking()
            .SingleAsync(x => x.FtsSourceLocationId == sourceLocationId, ct);

        var ftpServerName = sourceLocation.FtpServerId is null
            ? null
            : await dbContext.Set<Fxfer.DomainApi.Domain.Entities.FtpServer>()
                .AsNoTracking()
                .Where(x => x.FtpServerId == sourceLocation.FtpServerId)
                .Select(x => x.FtpServerName)
                .SingleOrDefaultAsync(ct);

        var destinations = await dbContext.Set<FtsDestinationLocations>()
            .AsNoTracking()
            .Where(x => x.FtsSourceLocationId == sourceLocationId)
            .OrderByDescending(x => x.CreationDate)
            .ToListAsync(ct);

        var inboundTargets = await dbContext.Set<FtsInboundTarget>()
            .AsNoTracking()
            .ToListAsync(ct);

        var mappedDestinations = destinations.Select(d => new Destination
        {
            DestinationId = d.DestinationId,
            DestinationAddress = d.DestinationAddress,
            DestinationProtocol = d.DestProtocolCD,
            FtpServerId = d.FtpServerId,
            RenamePattern = d.RenamePattern,
            OverWriteAtDestination = d.OverwriteAtDest,
            CompressAtDestination = d.CompressAtDest,
            RenameAtDestination = d.RenameAtDest,
            DestinationArchive = d.DestIsArchive,
            ContactName = d.ContactName,
            ContactPhone = d.ContactPhone,
            ContactEmail = d.ContactEmail,
            IsValid = d.IsValid,
            FteMetadataId = d.FTEMetadataId,
            DestinationType = d.DestinationType,
            ContractName = d.ContractName,
            ImportJobName = d.FTEMetadataId is null
                ? null
                : inboundTargets.FirstOrDefault(f => f.FTEMetadataId == d.FTEMetadataId)!.ImportJobName,
            OperatorId = d.OperatorId,
            CreationDate = d.CreationDate,
            LastModifiedDate = d.LastModifiedDate
        }).ToList();

        var lastModified = sourceLocation.LastModifiedDate;

        return new TransferConfiguration
        {
            FtsSourceLocationId = sourceLocation.FtsSourceLocationId,
            ConfigId = sourceLocation.ConfigId,
            ConfigName = sourceLocation.ConfigName,
            Description = sourceLocation.Description,
            FtpServerId = sourceLocation.FtpServerId,
            FtpServerName = ftpServerName,
            Address = sourceLocation.SourceAddress,
            Mask = sourceLocation.SourceFileMask,
            Protocol = sourceLocation.SourceProtocolCD,
            FileDirection = sourceLocation.FileDirection,
            OwnerName = sourceLocation.OwnerName,
            OwnerEmail = sourceLocation.OwnerEmail ?? string.Empty,
            OwnerSubsystem = sourceLocation.OwnerModule,
            OwnerPhoneNumber = sourceLocation.OwnerPhone ?? string.Empty,
            TradingPartner = sourceLocation.TradingPartner,
            OperatorId = sourceLocation.OperatorId,
            RunOnMonday = sourceLocation.RunOnMondayInd,
            RunOnTuesday = sourceLocation.RunOnTuesdayInd,
            RunOnWednesday = sourceLocation.RunOnWednesdayInd,
            RunOnThursday = sourceLocation.RunOnThursdayInd,
            RunOnFriday = sourceLocation.RunOnFridayInd,
            RunOnSaturday = sourceLocation.RunOnSaturdayInd,
            RunOnSunday = sourceLocation.RunOnSundayInd,
            RunOnFirstDay = sourceLocation.RunOnFirstDayOfMonthInd,
            RunOnLastDay = sourceLocation.RunOnLastDayOfMonthInd,
            RunOnThisDay = sourceLocation.RunOnThisDayOfMonth ?? 0,
            StartWindow = sourceLocation.StartTime24HHMM,
            EndWindow = sourceLocation.EndTime24HHMM,
            SendNotification = sourceLocation.SendNotificationsInd,
            IsActive = sourceLocation.IsValid,
            ValidChangeDate = lastModified,
            Suspended = sourceLocation.IsSuspended,
            SuspendedChangeDate = sourceLocation.IsSuspendedChangedTs ?? lastModified,
            LastModifiedDate = lastModified,
            CreationDate = sourceLocation.CreationDate,
            Destinations = mappedDestinations
        };
    }
}


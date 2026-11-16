using System.Security.Claims;

using FxferService.Api.Commands.Models;
using FxferService.Application.CommandHandlers;
using FxferService.Domain.Commands;
using FxferService.Domain.Contracts.Events;
using FxferService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FxferService.Api.Commands.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/transfer-configurations")]
public sealed class TransferConfigurationsCommandsController : ControllerBase
{
    private readonly FtsSourceLocationAddCommandHandler _sourceLocationCommandHandler;
    private readonly FxferDbContext _dbContext;

    public TransferConfigurationsCommandsController(
        FtsSourceLocationAddCommandHandler sourceLocationCommandHandler,
        FxferDbContext dbContext)
    {
        _sourceLocationCommandHandler = sourceLocationCommandHandler;
        _dbContext = dbContext;
    }

    [HttpPost]
    [ProducesResponseType(typeof(TransferConfigurationIdResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<TransferConfigurationIdResponse>> UpsertTransferConfiguration(
        [FromBody] TransferConfigurationUpsert body,
        CancellationToken ct)
    {
        var operatorId = GetOperatorIdOrThrow();

        var transferConfigurationId = body.TransferConfigurationId;
        var isCreate = !transferConfigurationId.HasValue || transferConfigurationId.Value == Guid.Empty;
        var id = isCreate ? Guid.NewGuid() : transferConfigurationId!.Value;

        if (isCreate)
        {
            var command = new FtsSourceLocationAddCommand
            {
                FtsSourceLocationId = id.ToString(),
                ConfigName = body.Name,
                Description = body.Description,
                Protocol = body.ProtocolCode,
                Address = body.Address,
                Mask = body.FileMask,
                FtpServerId = body.FtpServerId?.ToString(),
                SendNotifications = body.SendNotifications,
                OwnerName = body.Owner.Name,
                OwnerModule = body.Owner.ModuleId.ToString(),
                OwnerPhone = body.Owner.Phone,
                OwnerEmail = body.Owner.Email,
                RunOnMonday = body.Schedule.RunOnMonday,
                RunOnTuesday = body.Schedule.RunOnTuesday,
                RunOnWed = body.Schedule.RunOnWednesday,
                RunOnThrus = body.Schedule.RunOnThursday,
                RunOnFri = body.Schedule.RunOnFriday,
                RunOnSat = body.Schedule.RunOnSaturday,
                RunOnSun = body.Schedule.RunOnSunday,
                RunOnFrstDayOfMonthInd = body.Schedule.RunOnFirstDayOfMonth,
                RunOnLastDayOfMonthInd = body.Schedule.RunOnLastDayOfMonth,
                RunOnThisDayOfMonth = checked((byte)body.Schedule.RunOnThisDayOfMonth),
                StartTime = body.Schedule.StartTime24hhmm,
                EndTime = body.Schedule.EndTime24hhmm,
                FileDirection = body.FileDirection,
                TradingPartner = body.TradingPartner,
                IsValid = body.IsActive,
                IsSuspended = body.IsSuspended,
                IsSuspendedChangedDate = body.SuspendedChangedAt?.UtcDateTime,
                DestinationAddress = BuildDestinationAddress(id, body.Destinations, operatorId),
                Requestor = new FxferService.Application.LegacyContracts.RequestorModel { IdentifierId = operatorId },
            };

            // F02-027: route the add command and cast the routed event result.
            Event routed = await _sourceLocationCommandHandler.HandleAsync(command, ct);
            _ = routed as FtsSourceLocationAdded;

            return Ok(new TransferConfigurationIdResponse(id));
        }

        var updateCommand = new FtsSourceLocationUpdateCommand
        {
            FtsSourceLocationId = id.ToString(),
            ConfigId = body.ConfigNumber ?? 0,
            ConfigName = body.Name,
            Description = body.Description,
            Protocol = body.ProtocolCode,
            Address = body.Address,
            Mask = body.FileMask,
            FtpServerId = body.FtpServerId?.ToString(),
            SendNotifications = body.SendNotifications,
            OwnerName = body.Owner.Name,
            OwnerModule = body.Owner.ModuleId.ToString(),
            OwnerPhone = body.Owner.Phone,
            OwnerEmail = body.Owner.Email,
            RunOnMonday = body.Schedule.RunOnMonday,
            RunOnTuesday = body.Schedule.RunOnTuesday,
            RunOnWed = body.Schedule.RunOnWednesday,
            RunOnThrus = body.Schedule.RunOnThursday,
            RunOnFri = body.Schedule.RunOnFriday,
            RunOnSat = body.Schedule.RunOnSaturday,
            RunOnSun = body.Schedule.RunOnSunday,
            RunOnFrstDayOfMonthInd = body.Schedule.RunOnFirstDayOfMonth,
            RunOnLastDayOfMonthInd = body.Schedule.RunOnLastDayOfMonth,
            RunOnThisDayOfMonth = checked((byte)body.Schedule.RunOnThisDayOfMonth),
            StartTime = body.Schedule.StartTime24hhmm,
            EndTime = body.Schedule.EndTime24hhmm,
            FileDirection = body.FileDirection,
            TradingPartner = body.TradingPartner,
            IsValid = body.IsActive,
            IsSuspended = body.IsSuspended,
            IsSuspendedChangedDate = body.SuspendedChangedAt?.UtcDateTime,
            DestinationAddress = BuildDestinationAddress(id, body.Destinations, operatorId),
            OperatorId = operatorId,
            Requestor = new FxferService.Application.LegacyContracts.RequestorModel { IdentifierId = operatorId },
        };

        // F02-039: route the update command and cast the routed event result.
        Event routedUpdate = await _sourceLocationCommandHandler.HandleAsync(updateCommand, ct);
        _ = routedUpdate as FtsSourceLocationUpdated;

        return Ok(new TransferConfigurationIdResponse(id));
    }

    [HttpDelete("{transferConfigurationId:guid}")]
    [ProducesResponseType(typeof(TransferConfigurationStateResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<TransferConfigurationStateResponse>> InvalidateTransferConfiguration(
        [FromRoute] Guid transferConfigurationId,
        CancellationToken ct)
    {
        var operatorId = GetOperatorIdOrThrow();

        var configId = await _dbContext.SourceLocations
            .AsNoTracking()
            .Where(x => x.FtsSourceLocationId == transferConfigurationId)
            .Select(x => x.ConfigId)
            .FirstOrDefaultAsync(ct);

        var command = new FtsSourceLocationInvalidateCommand
        {
            FtsSourceLocationId = transferConfigurationId.ToString(),
            ConfigId = configId,
            OperatorId = operatorId,
        };

        // F02-041: route the invalidate command and cast the routed event result.
        Event routed = await _sourceLocationCommandHandler.HandleAsync(command, ct);
        _ = routed as FtsSourceLocationInvalidated;

        return Ok(new TransferConfigurationStateResponse(transferConfigurationId, IsActive: false));
    }

    private static List<FtsDestinationLocationAdded> BuildDestinationAddress(
        Guid transferConfigurationId,
        DestinationUpsert[] destinations,
        string operatorId)
    {
        return destinations
            .Select(d => new FtsDestinationLocationAdded
            {
                DestinationId = d.DestinationId ?? Guid.NewGuid(),
                SourceLocationId = transferConfigurationId,
                DestinationAddress = d.Address,
                DestinationProtocol = d.ProtocolCode,
                FtpServerId = d.FtpServerId,
                RenamePattern = d.RenamePattern,
                OverWriteAtDestination = d.OverwriteAtDestination,
                CompressAtDestination = d.CompressAtDestination,
                RenameAtDestination = d.RenameAtDestination,
                DestinationArchive = d.DestinationIsArchive,
                ContactName = d.Contact?.Name,
                ContactPhone = d.Contact?.Phone,
                ContactEmail = d.Contact?.Email,
                IsValid = d.IsActive,
                CreationDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
                OperatorId = operatorId,
                FTEMetadataId = d.ExternalFileDefinitionId,
                DestinationType = d.DestinationType,
                ContractName = d.ContractName,
            })
            .ToList();
    }

    private string GetOperatorIdOrThrow()
    {
        var operatorId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Identity?.Name;
        if (string.IsNullOrWhiteSpace(operatorId))
        {
            throw new InvalidOperationException("Missing authenticated operator id");
        }

        return operatorId;
    }
}

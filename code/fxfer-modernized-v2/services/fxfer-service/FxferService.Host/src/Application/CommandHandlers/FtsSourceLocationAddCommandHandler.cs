using System.Data;

using FxferService.Domain.Commands;
using FxferService.Domain.Contracts.Domain;
using FxferService.Domain.Contracts.Events;
using FxferService.Domain.Contracts.Exceptions;
using FxferService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FxferService.Application.CommandHandlers;

public sealed class FtsSourceLocationAddCommandHandler
{
    private readonly FxferDbContext _dbContext;

    public FtsSourceLocationAddCommandHandler(FxferDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FtsSourceLocationAdded> HandleAsync(FtsSourceLocationAddCommand command, CancellationToken ct)
    {
        // F02-035: validate before building the add event.
        command.Validate();

        // F02-035: allocate the new ConfigId from FTS_SOURCE_LOCATION_SEQ.
        var configId = await GetNextConfigIdAsync(ct);

        return new FtsSourceLocationAdded()
        {
            ConfigId = configId,
            ConfigName = command.ConfigName,
            Description = command.Description,
            FtpServerId = command.FtpServerId,
            Protocol = command.Protocol,
            Address = command.Address,
            Mask = command.Mask,
            FileDirection = command.FileDirection,
            SendNotifications = command.SendNotifications,
            IsValid = command.IsValid,
            IsSuspended = command.IsSuspended,
            IsSuspendedChangedDate = command.IsSuspendedChangedDate,
            OwnerName = command.OwnerName,
            OwnerModule = command.OwnerModule,
            OwnerPhone = command.OwnerPhone,
            OwnerEmail = command.OwnerEmail,
            RunOnMonday = command.RunOnMonday,
            RunOnTuesday = command.RunOnTuesday,
            RunOnWed = command.RunOnWed,
            RunOnThrus = command.RunOnThrus,
            RunOnFri = command.RunOnFri,
            RunOnSat = command.RunOnSat,
            RunOnSun = command.RunOnSun,
            RunOnFrstDayOfMonthInd = command.RunOnFrstDayOfMonthInd,
            RunOnLastDayOfMonthInd = command.RunOnLastDayOfMonthInd,
            RunOnThisDayOfMonth = command.RunOnThisDayOfMonth,
            StartTime = command.StartTime,
            EndTime = command.EndTime,
            TradingPartner = command.TradingPartner,
            OperatorId = command.Requestor!.IdentifierId,
            DestinationAddress = command.DestinationAddress,
        };
    }

    public async Task<FtsSourceLocationUpdated> HandleAsync(FtsSourceLocationUpdateCommand command, CancellationToken ct)
    {
        command.Validate();

        // F02-053, F02-054: select the source location by ConfigId.
        var sourceLocation = await FindSourceLocationByConfigIdAsync(command.ConfigId, ct);
        if (sourceLocation == null)
        {
            throw BuildConfigIdNotFoundBusinessException();
        }

        return new FtsSourceLocationUpdated()
        {
            // F02-054: ignore the command's FtsSourceLocationId; use the record found by ConfigId.
            FtsSourceLocationsId = sourceLocation.FtsSourceLocationId.ToString(),
            ConfigId = sourceLocation.ConfigId,
            ConfigName = command.ConfigName,
            Description = command.Description,
            FtpServerId = command.FtpServerId,
            Protocol = command.Protocol,
            Address = command.Address,
            Mask = command.Mask,
            FileDirection = command.FileDirection,
            SendNotifications = command.SendNotifications,
            IsValid = command.IsValid,
            IsSuspended = command.IsSuspended,
            IsSuspendedChangedDate = command.IsSuspendedChangedDate,
            OwnerName = command.OwnerName,
            OwnerModule = command.OwnerModule,
            OwnerPhone = command.OwnerPhone,
            OwnerEmail = command.OwnerEmail,
            RunOnMonday = command.RunOnMonday,
            RunOnTuesday = command.RunOnTuesday,
            RunOnWed = command.RunOnWed,
            RunOnThrus = command.RunOnThrus,
            RunOnFri = command.RunOnFri,
            RunOnSat = command.RunOnSat,
            RunOnSun = command.RunOnSun,
            RunOnFrstDayOfMonthInd = command.RunOnFrstDayOfMonthInd,
            RunOnLastDayOfMonthInd = command.RunOnLastDayOfMonthInd,
            RunOnThisDayOfMonth = command.RunOnThisDayOfMonth,
            StartTime = command.StartTime,
            EndTime = command.EndTime,
            TradingPartner = command.TradingPartner,
            OperatorId = command.OperatorId,
            DestinationAddress = command.DestinationAddress,
        };
    }

    public async Task<FtsSourceLocationInvalidated> HandleAsync(FtsSourceLocationInvalidateCommand command, CancellationToken ct)
    {
        command.Validate();

        // F02-055, F02-056: select the source location by ConfigId.
        var sourceLocation = await FindSourceLocationByConfigIdAsync(command.ConfigId, ct);
        if (sourceLocation == null)
        {
            throw BuildConfigIdNotFoundBusinessException();
        }

        return new FtsSourceLocationInvalidated()
        {
            // F02-056: ignore the command's FtsSourceLocationId; use the record found by ConfigId.
            FtsSourceLocationsId = sourceLocation.FtsSourceLocationId.ToString(),
            ConfigId = sourceLocation.ConfigId,
            OperatorId = command.OperatorId,
            DestinationAddress = command.DestinationAddress,
        };
    }

    private static BusinessException BuildConfigIdNotFoundBusinessException()
    {
        List<BusinessExceptionMessage> exceptions = new List<BusinessExceptionMessage>();
        exceptions.Add(new BusinessExceptionMessage(
            FileTransferExceptionConstants.Validation.ConfigId,
            FileTransferExceptionConstants.Validation.ConfigIdNotFound,
            FileTransferExceptionConstants.Validation.ProcessingSystem));

        return new BusinessException(
            exceptions,
            FileTransferExceptionConstants.Validation.Message,
            FileTransferExceptionConstants.Validation.ProcessingSystem);
    }

    private async Task<int> GetNextConfigIdAsync(CancellationToken ct)
    {
        if (_dbContext.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
        {
            // shortcut: InMemory provider cannot run nextval(); tests use max(ConfigId)+1, upgrade when moving tests to a relational provider.
            var max = await _dbContext.SourceLocations.MaxAsync(x => (int?)x.ConfigId, ct);
            return (max ?? 0) + 1;
        }

        var connection = _dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await _dbContext.Database.OpenConnectionAsync(ct);
        }

        await using var command = connection.CreateCommand();
        command.CommandText = "select nextval('FTS_SOURCE_LOCATION_SEQ')";
        var value = await command.ExecuteScalarAsync(ct);
        return Convert.ToInt32(value);
    }

    private Task<SourceLocationModel?> FindSourceLocationByConfigIdAsync(int configId, CancellationToken ct)
    {
        return _dbContext.SourceLocations
            .AsNoTracking()
            .Where(x => x.ConfigId == configId)
            .Select(x => new SourceLocationModel
            {
                FtsSourceLocationId = x.FtsSourceLocationId,
                ConfigId = x.ConfigId,
            })
            .FirstOrDefaultAsync(ct);
    }
}

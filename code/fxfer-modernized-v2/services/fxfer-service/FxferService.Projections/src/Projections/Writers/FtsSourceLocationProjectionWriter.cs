using FxferService.Projections.Infrastructure.Messaging.Contracts.Events;
using FxferService.Projections.Infrastructure.Persistence;
using FxferService.Projections.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FxferService.Projections.Projections.Writers;

public sealed class FtsSourceLocationProjectionWriter
{
    private readonly FxferProjectionsDbContext _dbContext;
    private readonly TimeProvider _timeProvider;
    private Guid _ftsCommonSourceLocationId;

    public FtsSourceLocationProjectionWriter(FxferProjectionsDbContext dbContext, TimeProvider timeProvider)
    {
        _dbContext = dbContext;
        _timeProvider = timeProvider;
    }

    public async Task HandleAsync(FtsSourceLocationAdded @event, CancellationToken ct)
    {
        // F02-037: generate a new GUID, insert source with IsValid from the event, stamp UTC timestamps.
        // F02-038: insert destinations with new GUIDs using the new source GUID; force invalid when source invalid.
        var utcNow = _timeProvider.GetUtcNow();
        var utcNowDateTime = utcNow.UtcDateTime;

        _ftsCommonSourceLocationId = Guid.NewGuid();
        _dbContext.FtsSourceLocations.Add(new FtsSourceLocationProjection
        {
            FtsSourceLocationId = _ftsCommonSourceLocationId,
            ConfigId = @event.ConfigId,
            ConfigName = @event.ConfigName,
            ConfigDescription = @event.Description,
            FtpServerId = string.IsNullOrEmpty(@event.FtpServerId) ? (Guid?)null : Guid.Parse(@event.FtpServerId),
            SourceProtocolCd = @event.Protocol,
            SourceAddress = @event.Address,
            SourceFileMask = @event.Mask,
            FileDirection = @event.FileDirection,
            SendNotificationsInd = @event.SendNotifications,
            IsValid = @event.IsValid,
            IsSuspended = @event.IsSuspended,
            IsSuspendedChangedTs = @event.IsSuspendedChangedDate.HasValue
                ? new DateTimeOffset(@event.IsSuspendedChangedDate.Value)
                : null,
            OwnerName = @event.OwnerName,
            ModuleId = Guid.Parse(@event.OwnerModule),
            OwnerPhone = @event.OwnerPhone,
            OwnerEmail = @event.OwnerEmail,
            RunOnMondayInd = @event.RunOnMonday,
            RunOnTuesdayInd = @event.RunOnTuesday,
            RunOnWedInd = @event.RunOnWed,
            RunOnThursInd = @event.RunOnThrus,
            RunOnFriInd = @event.RunOnFri,
            RunOnSatInd = @event.RunOnSat,
            RunOnSunInd = @event.RunOnSun,
            RunOnFrstDayOfMonthInd = @event.RunOnFrstDayOfMonthInd,
            RunOnLastDayOfMonthInd = @event.RunOnLastDayOfMonthInd,
            RunOnThisDayOfMonth = @event.RunOnThisDayOfMonth,
            StartTime24hhmm = @event.StartTime,
            EndTime24hhmm = @event.EndTime,
            TradingPartner = @event.TradingPartner,
            CreatedTs = utcNow,
            LastModifiedTs = utcNow,
            OperatorId = @event.OperatorId,
        });

        foreach (FtsDestinationLocationAdded dest in @event.DestinationAddress)
        {
            if (!@event.IsValid)
            {
                dest.IsValid = false;
            }

            AddDestination(dest, userId: null, utcNowDateTime);
        }

        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(FtsSourceLocationUpdated @event, CancellationToken ct)
    {
        var utcNow = _timeProvider.GetUtcNow();
        var utcNowDateTime = utcNow.UtcDateTime;

        _ftsCommonSourceLocationId = new Guid(@event.FtsSourceLocationsId);

        var entity = await _dbContext.FtsSourceLocations.SingleAsync(
            x => x.FtsSourceLocationId == Guid.Parse(@event.FtsSourceLocationsId),
            ct);

        entity.ConfigId = @event.ConfigId;
        entity.FtsSourceLocationId = _ftsCommonSourceLocationId;
        entity.ConfigName = @event.ConfigName;
        entity.ConfigDescription = @event.Description;
        entity.FtpServerId = string.IsNullOrEmpty(@event.FtpServerId) ? (Guid?)null : Guid.Parse(@event.FtpServerId);
        entity.SourceProtocolCd = @event.Protocol;
        entity.SourceAddress = @event.Address;
        entity.SourceFileMask = @event.Mask;
        entity.FileDirection = @event.FileDirection;
        entity.SendNotificationsInd = @event.SendNotifications;
        entity.IsSuspended = @event.IsSuspended;
        entity.IsSuspendedChangedTs = @event.IsSuspendedChangedDate.HasValue
            ? new DateTimeOffset(@event.IsSuspendedChangedDate.Value)
            : null;
        entity.IsValid = @event.IsValid;
        entity.OwnerName = @event.OwnerName;
        entity.ModuleId = Guid.Parse(@event.OwnerModule);
        entity.OwnerPhone = @event.OwnerPhone;
        entity.OwnerEmail = @event.OwnerEmail;
        entity.RunOnMondayInd = @event.RunOnMonday;
        entity.RunOnTuesdayInd = @event.RunOnTuesday;
        entity.RunOnWedInd = @event.RunOnWed;
        entity.RunOnThursInd = @event.RunOnThrus;
        entity.RunOnFriInd = @event.RunOnFri;
        entity.RunOnSatInd = @event.RunOnSat;
        entity.RunOnSunInd = @event.RunOnSun;
        entity.RunOnFrstDayOfMonthInd = @event.RunOnFrstDayOfMonthInd;
        entity.RunOnLastDayOfMonthInd = @event.RunOnLastDayOfMonthInd;
        entity.RunOnThisDayOfMonth = @event.RunOnThisDayOfMonth;
        entity.StartTime24hhmm = @event.StartTime;
        entity.EndTime24hhmm = @event.EndTime;
        entity.TradingPartner = @event.TradingPartner;
        entity.LastModifiedTs = utcNow;
        entity.OperatorId = @event.OperatorId;
        entity.IsValid = @event.IsValid;

        if (@event.FtsSourceLocationsId.ToString() != null)
        {
            await AddDestinationAddress(@event.DestinationAddress, @event.UserID, utcNowDateTime, ct);

            if (!@event.IsValid)
            {
                var destinations = await _dbContext.FtsDestinations
                    .Where(f => f.FtsSourceLocationId == _ftsCommonSourceLocationId)
                    .ToListAsync(ct);

                destinations.ForEach(a =>
                {
                    a.IsValid = false;
                    a.OperatorId = @event.OperatorId;
                });

                await _dbContext.SaveChangesAsync(ct);
            }
        }
    }

    public async Task HandleAsync(FtsSourceLocationInvalidated @event, CancellationToken ct)
    {
        // MaintainabilityDetails(ApprovedIndex = 58, ReviewedOnSprint = 94, ReviewerName = Josue Chavez)
        var utcNowDateTime = _timeProvider.GetUtcNow().UtcDateTime;

        _ftsCommonSourceLocationId = new Guid(@event.FtsSourceLocationsId);

        var entity = await _dbContext.FtsSourceLocations.SingleAsync(
            x => x.FtsSourceLocationId == Guid.Parse(@event.FtsSourceLocationsId),
            ct);

        entity.OperatorId = @event.OperatorId;
        entity.IsValid = false;

        var sourceLocationId = Guid.Parse(@event.FtsSourceLocationsId);

        if (@event.DestinationAddress != null)
        {
            await AddDestinationAddress(@event.DestinationAddress, @event.UserID, utcNowDateTime, ct);
        }

        var destinations = await _dbContext.FtsDestinations
            .Where(f => f.FtsSourceLocationId == sourceLocationId)
            .ToListAsync(ct);

        destinations.ForEach(a =>
        {
            a.IsValid = false;
            a.OperatorId = @event.OperatorId;
        });

        await _dbContext.SaveChangesAsync(ct);
    }

    private async Task AddDestinationAddress(
        List<FtsDestinationLocationAdded> destinationAddress,
        string? userId,
        DateTime utcNow,
        CancellationToken ct)
    {
        var existingDestinationIds = await _dbContext.FtsDestinations
            .Select(x => x.DestinationId)
            .ToListAsync(ct);

        var existingSet = existingDestinationIds.ToHashSet();

        foreach (FtsDestinationLocationAdded destAddress in destinationAddress)
        {
            if (existingSet.Contains(destAddress.DestinationId))
            {
                UpdateDestination(destAddress, userId, utcNow);
            }
            else
            {
                AddDestination(destAddress, userId, utcNow);
            }
        }

        await _dbContext.SaveChangesAsync(ct);
    }

    private void AddDestination(FtsDestinationLocationAdded dest, string? userId, DateTime utcNow)
    {
        var utcNowOffset = new DateTimeOffset(utcNow);

        _dbContext.FtsDestinations.Add(new FtsDestinationProjection
        {
            DestinationId = Guid.NewGuid(),
            FtsSourceLocationId = _ftsCommonSourceLocationId,
            DestinationAddress = dest.DestinationAddress,
            DestProtocolCd = dest.DestinationProtocol,
            OverwriteAtDestInd = dest.OverWriteAtDestination,
            CompressAtDestInd = dest.CompressAtDestination,
            RenameAtDestInd = dest.RenameAtDestination,
            DestIsArchiveInd = dest.DestinationArchive,
            CreationTs = utcNowOffset,
            LastModifiedTs = utcNowOffset,
            IsValid = dest.IsValid,
            FtpServerId = dest.FtpServerId,
            OperatorId = dest.OperatorId,
            RenamePattern = dest.RenamePattern,
            ContactName = dest.ContactName,
            ContactPhone = dest.ContactPhone,
            ContactEmail = dest.ContactEmail,
            ExternalFileDefnId = dest.FTEMetadataId,
            DestinationType = dest.DestinationType,
            ContractName = dest.ContractName,
        });
    }

    private void UpdateDestination(FtsDestinationLocationAdded dest, string? userId, DateTime utcNow)
    {
        // MaintainabilityDetails(ApprovedIndex = 54, ReviewedOnSprint = 94, ReviewerName = Josue Chavez)
        var destAddress = _dbContext.FtsDestinations.FirstOrDefault(x => x.DestinationId == dest.DestinationId);
        var utcNowOffset = new DateTimeOffset(utcNow);

        destAddress!.DestinationAddress = dest.DestinationAddress;
        destAddress.DestProtocolCd = dest.DestinationProtocol;
        destAddress.OverwriteAtDestInd = dest.OverWriteAtDestination;
        destAddress.CompressAtDestInd = dest.CompressAtDestination;
        destAddress.RenameAtDestInd = dest.RenameAtDestination;
        destAddress.DestIsArchiveInd = dest.DestinationArchive;
        destAddress.LastModifiedTs = utcNowOffset;
        destAddress.IsValid = dest.IsValid;
        destAddress.FtpServerId = dest.FtpServerId;
        destAddress.OperatorId = dest.OperatorId;
        destAddress.RenamePattern = dest.RenamePattern;
        destAddress.ContactName = dest.ContactName;
        destAddress.ContactPhone = dest.ContactPhone;
        destAddress.ContactEmail = dest.ContactEmail;
        destAddress.ExternalFileDefnId = dest.FTEMetadataId;
        destAddress.DestinationType = dest.DestinationType;
        destAddress.ContractName = dest.ContractName;
    }
}


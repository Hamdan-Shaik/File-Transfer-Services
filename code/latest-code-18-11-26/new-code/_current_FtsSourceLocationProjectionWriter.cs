using Fxfer.ProjectionWorker.Infrastructure.DataAccess;
using Fxfer.ProjectionWorker.Infrastructure.DataAccess.Entities;
using Fxfer.ProjectionWorker.Worker.Eventing;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.ProjectionWorker.Infrastructure.Projections;

public sealed class FtsSourceLocationProjectionWriter
{
    private readonly FxferProjectionDbContext _db;
    private Guid _ftsCommonSourceLocationId;

    public FtsSourceLocationProjectionWriter(FxferProjectionDbContext db)
    {
        _db = db;
    }

    public async Task HandleSourceLocationCreatedAsync(
        FxferEventEnvelope envelope,
        SourceLocationProjectionPayload payload,
        CancellationToken ct)
    {
        _ftsCommonSourceLocationId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        _db.SourceLocations.Add(new FtsSourceLocation
        {
            FtsSourceLocationId = _ftsCommonSourceLocationId,
            ConfigId = payload.ConfigId,
            ConfigName = payload.ConfigName,
            Description = payload.Description,
            FtpServerId = string.IsNullOrEmpty(payload.FtpServerId) ? null : Guid.Parse(payload.FtpServerId),
            SourceProtocolCd = payload.Protocol,
            SourceAddress = payload.Address,
            SourceFileMask = payload.Mask,
            FileDirection = payload.FileDirection,
            SendNotificationsInd = payload.SendNotifications,
            IsValid = payload.IsValid,
            IsSuspended = payload.IsSuspended,
            IsSuspendedChangedTs = payload.IsSuspendedChangedDate,
            OwnerName = payload.OwnerName,
            OwnerModule = Guid.Parse(payload.OwnerModule),
            OwnerPhone = payload.OwnerPhone,
            OwnerEmail = payload.OwnerEmail,
            RunOnMondayInd = payload.RunOnMonday,
            RunOnTuesdayInd = payload.RunOnTuesday,
            RunOnWednesdayInd = payload.RunOnWed,
            RunOnThursdayInd = payload.RunOnThrus,
            RunOnFridayInd = payload.RunOnFri,
            RunOnSaturdayInd = payload.RunOnSat,
            RunOnSundayInd = payload.RunOnSun,
            RunOnFirstDayOfMonthInd = payload.RunOnFrstDayOfMonthInd,
            RunOnLastDayOfMonthInd = payload.RunOnLastDayOfMonthInd,
            RunOnThisDayOfMonth = payload.RunOnThisDayOfMonth,
            StartTime24hhmm = payload.StartTime,
            EndTime24hhmm = payload.EndTime,
            TradingPartner = payload.TradingPartner,
            CreatedTs = now,
            LastModifiedTs = now,
            OperatorId = payload.OperatorId,
        });

        foreach (var dest in payload.Destinations)
        {
            var destToAdd = payload.IsValid ? dest : dest with { IsValid = false };
            AddDestination(destToAdd, string.Empty);
        }

        await _db.SaveChangesAsync(ct);
    }

    public async Task HandleSourceLocationUpdatedAsync(
        FxferEventEnvelope envelope,
        SourceLocationProjectionPayload payload,
        CancellationToken ct)
    {
        // F01-073: update the persisted source location identified by SourceLocationId.
        _ftsCommonSourceLocationId = new Guid(payload.SourceLocationId);

        var sourceLocationId = Guid.Parse(payload.SourceLocationId);

        var existing = await _db.SourceLocations
            .Where(x => x.FtsSourceLocationId == sourceLocationId)
            .FirstOrDefaultAsync(ct);

        if (existing is null)
        {
            throw new NullReferenceException();
        }

        var now = DateTimeOffset.UtcNow;

        var updated = existing with
        {
            ConfigId = payload.ConfigId,
            FtsSourceLocationId = _ftsCommonSourceLocationId,
            ConfigName = payload.ConfigName,
            Description = payload.Description,
            FtpServerId = string.IsNullOrEmpty(payload.FtpServerId) ? null : Guid.Parse(payload.FtpServerId),
            SourceProtocolCd = payload.Protocol,
            SourceAddress = payload.Address,
            SourceFileMask = payload.Mask,
            FileDirection = payload.FileDirection,
            SendNotificationsInd = payload.SendNotifications,
            IsSuspended = payload.IsSuspended,
            IsSuspendedChangedTs = payload.IsSuspendedChangedDate,
            IsValid = payload.IsValid,
            OwnerName = payload.OwnerName,
            OwnerModule = Guid.Parse(payload.OwnerModule),
            OwnerPhone = payload.OwnerPhone,
            OwnerEmail = payload.OwnerEmail,
            RunOnMondayInd = payload.RunOnMonday,
            RunOnTuesdayInd = payload.RunOnTuesday,
            RunOnWednesdayInd = payload.RunOnWed,
            RunOnThursdayInd = payload.RunOnThrus,
            RunOnFridayInd = payload.RunOnFri,
            RunOnSaturdayInd = payload.RunOnSat,
            RunOnSundayInd = payload.RunOnSun,
            RunOnFirstDayOfMonthInd = payload.RunOnFrstDayOfMonthInd,
            RunOnLastDayOfMonthInd = payload.RunOnLastDayOfMonthInd,
            RunOnThisDayOfMonth = payload.RunOnThisDayOfMonth,
            StartTime24hhmm = payload.StartTime,
            EndTime24hhmm = payload.EndTime,
            TradingPartner = payload.TradingPartner,
            LastModifiedTs = now,
            OperatorId = payload.OperatorId,
        };

        _db.Entry(existing).CurrentValues.SetValues(updated);

        // F01-074: attempt destination upsert whenever SourceLocationId is non-null.
        if (payload.SourceLocationId.ToString() != null)
        {
            await AddDestinationAddressAsync(payload.Destinations, payload.OperatorId, ct);

            // F01-075: when IsValid == false, invalidate all destinations linked to the source location.
            if (!payload.IsValid)
            {
                var existingDestinations = await _db.Destinations
                    .Where(f => f.FtsSourceLocationId == _ftsCommonSourceLocationId)
                    .ToListAsync(ct);

                foreach (var destination in existingDestinations)
                {
                    var invalidated = destination with { IsValid = false, OperatorId = payload.OperatorId };
                    _db.Entry(destination).CurrentValues.SetValues(invalidated);
                }

                await _db.SaveChangesAsync(ct);
            }
        }
    }

    public async Task HandleSourceLocationInvalidatedAsync(
        FxferEventEnvelope envelope,
        SourceLocationInvalidatedProjectionPayload payload,
        CancellationToken ct)
    {
        // F01-076: invalidate the source location identified by SourceLocationId.
        _ftsCommonSourceLocationId = new Guid(payload.SourceLocationId);

        var sourceLocationId = Guid.Parse(payload.SourceLocationId);

        var existing = await _db.SourceLocations
            .Where(x => x.FtsSourceLocationId == sourceLocationId)
            .FirstOrDefaultAsync(ct);

        if (existing is null)
        {
            throw new NullReferenceException();
        }

        var updated = existing with { OperatorId = payload.OperatorId, IsValid = false };
        _db.Entry(existing).CurrentValues.SetValues(updated);

        if (payload.Destinations is not null)
        {
            await AddDestinationAddressAsync(payload.Destinations, payload.OperatorId, ct);
        }

        // F01-077: invalidate all destinations linked to the source location.
        var destinationsForSource = await _db.Destinations
            .Where(f => f.FtsSourceLocationId == sourceLocationId)
            .ToListAsync(ct);

        foreach (var destination in destinationsForSource)
        {
            var invalidated = destination with { IsValid = false, OperatorId = payload.OperatorId };
            _db.Entry(destination).CurrentValues.SetValues(invalidated);
        }

        await _db.SaveChangesAsync(ct);
    }

    private async Task AddDestinationAddressAsync(
        List<DestinationProjectionPayload> destinations,
        string userId,
        CancellationToken ct)
    {
        var existingDestination = _db.Destinations.Select(result => new { result.DestinationId });

        foreach (var destAddress in destinations)
        {
            var destinationAddress = await existingDestination
                .Where(x => x.DestinationId == destAddress.DestinationId)
                .FirstOrDefaultAsync(ct);

            if (destinationAddress != null)
            {
                await UpdateDestinationAsync(destAddress, userId, ct);
            }
            else
            {
                AddDestination(destAddress, userId);
            }
        }

        await _db.SaveChangesAsync(ct);
    }

    private void AddDestination(DestinationProjectionPayload dest, string userId)
    {
        var now = DateTimeOffset.UtcNow;

        _db.Destinations.Add(new FtsDestination
        {
            DestinationId = Guid.NewGuid(),
            FtsSourceLocationId = _ftsCommonSourceLocationId,
            DestinationAddress = dest.DestinationAddress,
            DestProtocolCd = dest.DestinationProtocol,
            OverwriteAtDestInd = dest.OverwriteAtDestination,
            CompressAtDestInd = dest.CompressAtDestination,
            RenameAtDestInd = dest.RenameAtDestination,
            DestIsArchiveInd = dest.DestinationArchive,
            CreatedTs = now,
            LastModifiedTs = now,
            IsValid = dest.IsValid,
            FtpServerId = dest.FtpServerId,
            OperatorId = dest.OperatorId,
            RenamePattern = dest.RenamePattern,
            ContactName = dest.ContactName,
            ContactPhone = dest.ContactPhone,
            ContactEmail = dest.ContactEmail,
            ExternalFileDefnId = dest.FteMetadataId,
            DestinationType = dest.DestinationType,
            ContractName = dest.ContractName,
        });
    }

    private async Task UpdateDestinationAsync(
        DestinationProjectionPayload dest,
        string userId,
        CancellationToken ct)
    {
        var destAddress = await _db.Destinations
            .Where(x => x.DestinationId == dest.DestinationId)
            .FirstOrDefaultAsync(ct);

        if (destAddress is null)
        {
            throw new NullReferenceException();
        }

        var now = DateTimeOffset.UtcNow;

        var updated = destAddress with
        {
            DestinationAddress = dest.DestinationAddress,
            DestProtocolCd = dest.DestinationProtocol,
            OverwriteAtDestInd = dest.OverwriteAtDestination,
            CompressAtDestInd = dest.CompressAtDestination,
            RenameAtDestInd = dest.RenameAtDestination,
            DestIsArchiveInd = dest.DestinationArchive,
            LastModifiedTs = now,
            IsValid = dest.IsValid,
            FtpServerId = dest.FtpServerId,
            OperatorId = dest.OperatorId,
            RenamePattern = dest.RenamePattern,
            ContactName = dest.ContactName,
            ContactPhone = dest.ContactPhone,
            ContactEmail = dest.ContactEmail,
            ExternalFileDefnId = dest.FteMetadataId,
            DestinationType = dest.DestinationType,
            ContractName = dest.ContractName,
        };

        _db.Entry(destAddress).CurrentValues.SetValues(updated);
    }
}


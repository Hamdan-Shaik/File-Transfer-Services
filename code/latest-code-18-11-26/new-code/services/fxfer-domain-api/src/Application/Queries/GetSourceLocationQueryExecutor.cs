using Microsoft.EntityFrameworkCore;

using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Contracts.Queries;
using FtpServerEntity = Fxfer.DomainApi.Domain.Entities.FtpServer;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

namespace Fxfer.DomainApi.Application.Queries;

public sealed class GetSourceLocationQueryExecutor
{
    public async Task<IReadOnlyList<SourceLocationModel>> ExecuteAsync(
        FxferDbContext dbContext,
        GetSourceLocationQuery query,
        CancellationToken ct)
    {
        // F01-100: Query-level validation is performed up front.
        query.Validate();

        // F01-102: Load destination + inbound-target rows into memory before mapping.
        var destinations = await dbContext.Set<FtsDestinationLocations>().ToListAsync(ct);
        var inboundTargets = await dbContext.Set<FtsInboundTarget>().ToListAsync(ct);

        // F01-101, F01-014: Parse-and-match by GUID; return a source location only when exactly one row matches.
        var sourceLocation = await GetSourceLocationAsync(dbContext, query, ct);
        var returnResult = new List<SourceLocationModel>();

        if (sourceLocation is not null)
        {
            var ftpServerDetails = await GetFtpServerAsync(dbContext, sourceLocation, ct);

            returnResult.Add(new SourceLocationModel
            {
                FtsSourceLocationId = sourceLocation.FtsSourceLocationId,
                ConfigId = sourceLocation.ConfigId,
                ConfigName = sourceLocation.ConfigName,
                Description = sourceLocation.Description,
                FtpServerName = ftpServerDetails != null ? ftpServerDetails.FtpServerName : null,
                FtpServerId = sourceLocation.FtpServerId,
                Address = sourceLocation.SourceAddress,
                OwnerName = sourceLocation.OwnerName,
                OwnerSubsystem = sourceLocation.OwnerModule,
                OwnerPhoneNumber = sourceLocation.OwnerPhone,
                OwnerEmail = sourceLocation.OwnerEmail,
                Protocol = sourceLocation.SourceProtocolCD,
                FileDirection = sourceLocation.FileDirection,
                Mask = sourceLocation.SourceFileMask,
                TradingPartner = sourceLocation.TradingPartner,
                RunOnMonday = sourceLocation.RunOnMondayInd,
                RunOnTuesday = sourceLocation.RunOnTuesdayInd,
                RunOnWednesday = sourceLocation.RunOnWednesdayInd,
                RunOnThursday = sourceLocation.RunOnThursdayInd,
                RunOnFriday = sourceLocation.RunOnFridayInd,
                RunOnSaturday = sourceLocation.RunOnSaturdayInd,
                RunOnSunday = sourceLocation.RunOnSundayInd,
                RunOnFirstDay = sourceLocation.RunOnFirstDayOfMonthInd,
                RunOnLastDay = sourceLocation.RunOnLastDayOfMonthInd,
                RunOnThisDay = sourceLocation.RunOnThisDayOfMonth,
                Suspended = sourceLocation.IsSuspended,
                StartWindow = sourceLocation.StartTime24HHMM,
                EndWindow = sourceLocation.EndTime24HHMM,
                SendNotification = sourceLocation.SendNotificationsInd,
                IsActive = sourceLocation.IsValid
            });
        }

        var baseQuery = returnResult.AsQueryable();

        // F01-105: Assign destination list onto the first result only.
        var destinationRecords = GetDestinationAddressObject(
            baseQuery.First().FtsSourceLocationId,
            destinations,
            inboundTargets);

        var results = baseQuery.ToList();
        results.First().DestinationAddress = destinationRecords;

        return results;
    }

    private static List<FtsDestinationLocationAdded> GetDestinationAddressObject(
        Guid sourceLocationId,
        List<FtsDestinationLocations> destinations,
        List<FtsInboundTarget> inboundTargets)
    {
        // F01-016, F01-106: Order destinations by CreationDate descending (newest first).
        var destinationRows = destinations
            .Where(d => d.FtsSourceLocationId == sourceLocationId)
            .OrderByDescending(d => d.CreationDate);

        var result = new List<FtsDestinationLocationAdded>();

        foreach (var destination in destinationRows)
        {
            result.Add(new FtsDestinationLocationAdded
            {
                DestinationId = destination.DestinationId,
                SourceLocationId = destination.FtsSourceLocationId,
                DestinationAddress = destination.DestinationAddress,
                DestinationProtocol = destination.DestProtocolCD,
                FtpServerId = destination.FtpServerId ?? Guid.Empty,
                RenamePattern = destination.RenamePattern,
                OverWriteAtDestination = destination.OverwriteAtDest,
                CompressAtDestination = destination.CompressAtDest,
                RenameAtDestination = destination.RenameAtDest,
                DestinationArchive = destination.DestIsArchive,
                ContactName = destination.ContactName,
                ContactPhone = destination.ContactPhone,
                ContactEmail = destination.ContactEmail,
                IsValid = destination.IsValid,
                FTEMetadataId = destination.FTEMetadataId,
                DestinationType = destination.DestinationType,
                ContractName = destination.ContractName,
                ImportJobName = GetImportJobName(destination.FTEMetadataId, inboundTargets)
            });
        }

        return result;
    }

    private static async Task<FtsSourceLocation?> GetSourceLocationAsync(
        FxferDbContext dbContext,
        GetSourceLocationQuery query,
        CancellationToken ct)
    {
        var locations = await dbContext
            .Set<FtsSourceLocation>()
            .Where(s => s.FtsSourceLocationId == query.Where!.SoureceLocationGuid)
            .ToListAsync(ct);

        return locations is not null && locations.Count == 1 ? locations.First() : null;
    }

    private static Task<FtpServerEntity?> GetFtpServerAsync(
        FxferDbContext dbContext,
        FtsSourceLocation sourceLocation,
        CancellationToken ct)
    {
        return sourceLocation.FtpServerId != null
            ? dbContext.Set<FtpServerEntity>().Where(f => f.FtpServerId == sourceLocation.FtpServerId).FirstOrDefaultAsync(ct)
            : Task.FromResult<FtpServerEntity?>(null);
    }

    private static string GetImportJobName(Guid? fteMetadataId, List<FtsInboundTarget> inboundTargets)
    {
        // F01-020: When FTEMetadataId is non-null, read ImportJobName from the first matching inbound target.
        string importName = string.Empty;
        if (fteMetadataId != null)
        {
            importName = inboundTargets.FirstOrDefault(f => f.FTEMetadataId == fteMetadataId).ImportJobName;
        }

        return importName;
    }
}


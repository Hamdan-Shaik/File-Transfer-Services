using FxferService.Application.Contracts.Queries;
using FxferService.Domain.Contracts.Domain;
using FxferService.Domain.Contracts.Events;
using FxferService.Infrastructure.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Entities;
using FxferService.Infrastructure.Queries;

namespace FxferService.Application.QueryHandlers;

public class GetSourceLocationQueryExecutor : QueryExecutorBase<SourceLocationModel, GetSourceLocationQuery>
{
    private List<FtsDestinationLocations> _dest = new();
    private List<FtsInboundTargetEntity> _inboundTarget = new();

    protected override IQueryable<SourceLocationModel> ApplySorting(IQueryable<SourceLocationModel> baseQuery, GetSourceLocationQuery query)
    {
        return baseQuery;
    }

    protected override IQueryable<SourceLocationModel> ApplyWhereClause(IQueryable<SourceLocationModel> baseQuery, GetSourceLocationQuery query)
    {
        return baseQuery;
    }

    protected override void AssignResults(IQueryable<SourceLocationModel> baseQuery, GetSourceLocationQuery query)
    {
        var destinationRecords = GetDestinationAddressObject(baseQuery.First().FtsSourceLocationId);
        query.Results = baseQuery.ToList();
        query.Results.First().DestinationAddress = destinationRecords;
    }

    protected override IQueryable<SourceLocationModel> GetBaseQuery(IDbContextBase databaseContext, GetSourceLocationQuery query)
    {
        // F02-014: loads destination + inbound-target tables in full into memory.
        query.Validate();

        _dest = databaseContext.Set<FtsDestinationLocations>().ToList();
        _inboundTarget = databaseContext.Set<FtsInboundTargetEntity>().ToList();
        // F02-015: returns a source location only when exactly one row matches.
        FtsSourceLocationEntity? sourceLocation = GetSourceLocation(databaseContext, query);
        List<SourceLocationModel> returnResult = new();

        if (sourceLocation != null)
        {
            // F02-016: when FtpServerId is present, looks up and copies the FTP server name.
            FtpServer? ftpServerDetails = GetFTPServer(databaseContext, sourceLocation);

            returnResult.Add(new SourceLocationModel()
            {
                FtsSourceLocationId = sourceLocation.FtsSourceLocationId,
                ConfigId = sourceLocation.ConfigId,
                ConfigName = sourceLocation.ConfigName,
                Description = sourceLocation.ConfigDescription!,
                FtpServerName = ftpServerDetails != null ? ftpServerDetails.FtpServerName : null,
                FtpServerId = sourceLocation.FtpServerId,
                Address = sourceLocation.SourceAddress,
                OwnerName = sourceLocation.OwnerName,
                OwnerSubsystem = sourceLocation.ModuleId,
                OwnerPhoneNumber = sourceLocation.OwnerPhone,
                OwnerEmail = sourceLocation.OwnerEmail,
                Protocol = sourceLocation.SourceProtocolCode,
                FileDirection = sourceLocation.FileDirection!,
                Mask = sourceLocation.SourceFileMask,
                TradingPartner = sourceLocation.TradingPartner!,
                RunOnMonday = sourceLocation.RunOnMonday,
                RunOnTuesday = sourceLocation.RunOnTuesday,
                RunOnWednesday = sourceLocation.RunOnWednesday,
                RunOnThursday = sourceLocation.RunOnThursday,
                RunOnFriday = sourceLocation.RunOnFriday,
                RunOnSaturday = sourceLocation.RunOnSaturday,
                RunOnSunday = sourceLocation.RunOnSunday,
                RunOnFirstDay = sourceLocation.RunOnFirstDayOfMonth,
                RunOnLastDay = sourceLocation.RunOnLastDayOfMonth,
                RunOnThisDay = (byte)sourceLocation.RunOnThisDayOfMonth,
                Suspended = sourceLocation.IsSuspended,
                StartWindow = sourceLocation.StartTime24hhmm,
                EndWindow = sourceLocation.EndTime24hhmm,
                SendNotification = sourceLocation.SendNotifications,
                IsActive = sourceLocation.IsValid,
            });
        }

        return returnResult.AsQueryable<SourceLocationModel>();
    }

    private List<FtsDestinationLocationAdded> GetDestinationAddressObject(Guid soureceLocationId)
    {
        // F02-017: returns destinations ordered by CreationDate desc and resolves ImportJobName from inbound-target.
        var destinations = _dest.Where(d => d.FtsSourceLocationId == soureceLocationId).OrderByDescending(d => d.CreationDate);

        List<FtsDestinationLocationAdded> result = new();

        foreach (FtsDestinationLocations destination in destinations)
        {
            result.Add(new FtsDestinationLocationAdded()
            {
                DestinationId = destination.DestinationId,
                SourceLocationId = destination.FtsSourceLocationId,
                DestinationAddress = destination.DestinationAddress,
                DestinationProtocol = destination.DestProtocolCD,
                FtpServerId = destination.FtpServerId,
                RenamePattern = destination.RenamePattern,
                OverWriteAtDestination = destination.OverwriteAtDest,
                CompressAtDestination = destination.CompressAtDest,
                RenameAtDestination = destination.ReNameAtDest,
                DestinationArchive = destination.DestIsArchive,
                ContactName = destination.ContactName,
                ContactPhone = destination.ContactPhone,
                ContactEmail = destination.ContactEmail,
                IsValid = destination.IsValid,
                FTEMetadataId = destination.FTEMetadataId,
                DestinationType = destination.DestinationType,
                ContractName = destination.ContractName,
                ImportJobName = GetImportJobName(destination.FTEMetadataId)
            });
        }

        return result;
    }

    private FtpServer? GetFTPServer(IDbContextBase context, FtsSourceLocationEntity sourceLocation)
    {
        return sourceLocation.FtpServerId != null
               ? context.Set<FtpServer>().Where(f => f.FtpServerId == sourceLocation.FtpServerId).FirstOrDefault()
               : null;
    }

    private string GetImportJobName(Guid? fteMetadataId)
    {
        // F02-017: if no inbound target exists, dereferencing ImportJobName throws.
        string importName = string.Empty;
        if (fteMetadataId != null)
        {
            importName = _inboundTarget.FirstOrDefault(f => f.ExternalFileDefnId == fteMetadataId)!.ImportJobName;
        }

        return importName;
    }

    private FtsSourceLocationEntity? GetSourceLocation(IDbContextBase context, GetSourceLocationQuery query)
    {
        List<FtsSourceLocationEntity> locations = context.Set<FtsSourceLocationEntity>().Where(s => s.FtsSourceLocationId == query.Where.SoureceLocationGuid).ToList();

        return locations != null && locations.Count == 1 ? locations.First() : null;
    }
}

using Fxfer.DomainApi.Application.Queries;
using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Contracts.Queries.Parameters;
using Fxfer.DomainApi.Contracts.Utilities;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fxfer.DomainApi.Api.Endpoints.Queries;

public static class TransferConfigurationsQueries
{
    public static RouteGroupBuilder MapTransferConfigurationsQueries(this RouteGroupBuilder group)
    {
        group.MapGet("/transfer-configurations", async (
            FxferDbContext dbContext,
            int? configId,
            string? configName,
            string? ownerName,
            Guid? ownerSubsystem,
            string? protocol,
            bool? isActive,
            int page = 1,
            int pageSize = 50,
            CancellationToken ct = default) =>
        {
            // F01-079: route the request through the query handler and return results.
            var query = new SourceLocationQuery
            {
                Where = new SourceLocationQueryParms
                {
                    ConfigId = configId,
                    ConfigName = configName,
                    OwnerName = ownerName,
                    OwnerSubsystem = ownerSubsystem?.ToString(),
                    Protocol = protocol,
                    IsActive = isActive.GetValueOrDefault()
                }
            };

            var executor = new SearchSourceLocationQueryExecutor();
            var results = await executor.ExecuteAsync(dbContext, query, ct);

            var paged = ApplyPaging(results, page, pageSize);

            return Results.Ok(new PagedResult<TransferConfigurationSummary>
            {
                Items = paged.Items.Select(MapSummary).ToArray(),
                Total = paged.Total,
                Page = page,
                PageSize = pageSize
            });
        });

        group.MapGet("/transfer-configurations/{sourceLocationId:guid}", async (
            FxferDbContext dbContext,
            Guid sourceLocationId,
            CancellationToken ct = default) =>
        {
            // F01-081: route the request through the query handler and return results.
            var query = new GetSourceLocationQuery
            {
                Where = new GetSourceLocationQueryParms
                {
                    SourceLocationId = sourceLocationId.ToString(),
                }
            };

            var executor = new GetSourceLocationQueryExecutor();
            _ = await executor.ExecuteAsync(dbContext, query, ct);

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

            var destinationRows = await dbContext.Set<FtsDestinationLocations>()
                .AsNoTracking()
                .Where(x => x.FtsSourceLocationId == sourceLocationId)
                .OrderByDescending(x => x.CreationDate)
                .ToListAsync(ct);

            var inboundTargets = await dbContext.Set<FtsInboundTarget>()
                .AsNoTracking()
                .ToListAsync(ct);

            var destinations = destinationRows.Select(d => MapDestination(d, inboundTargets)).ToList();

            return Results.Ok(MapTransferConfiguration(sourceLocation, ftpServerName, destinations));
        });

        return group;
    }

    private static (IReadOnlyList<SourceLocationModel> Items, int Total) ApplyPaging(
        IReadOnlyList<SourceLocationModel> items,
        int page,
        int pageSize)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return (items, items.Count);
        }

        var skip = (page - 1) * pageSize;
        return (items.Skip(skip).Take(pageSize).ToArray(), items.Count);
    }

    private static TransferConfigurationSummary MapSummary(SourceLocationModel model)
    {
        return new TransferConfigurationSummary
        {
            FtsSourceLocationId = model.FtsSourceLocationId,
            ConfigId = model.ConfigId,
            ConfigName = model.ConfigName ?? string.Empty,
            Protocol = model.Protocol ?? string.Empty,
            Address = model.Address ?? string.Empty,
            OwnerName = model.OwnerName ?? string.Empty,
            TradingPartner = model.TradingPartner,
            IsActive = model.IsActive,
            Suspended = model.Suspended
        };
    }

    private static Destination MapDestination(FtsDestinationLocations destination, IReadOnlyList<FtsInboundTarget> inboundTargets)
    {
        return new Destination
        {
            DestinationId = destination.DestinationId,
            DestinationAddress = destination.DestinationAddress,
            DestinationProtocol = destination.DestProtocolCD,
            FtpServerId = destination.FtpServerId,
            RenamePattern = destination.RenamePattern,
            OverWriteAtDestination = destination.OverwriteAtDest,
            CompressAtDestination = destination.CompressAtDest,
            RenameAtDestination = destination.RenameAtDest,
            DestinationArchive = destination.DestIsArchive,
            ContactName = destination.ContactName,
            ContactPhone = destination.ContactPhone,
            ContactEmail = destination.ContactEmail,
            IsValid = destination.IsValid,
            FteMetadataId = destination.FTEMetadataId,
            DestinationType = destination.DestinationType,
            ContractName = destination.ContractName,
            ImportJobName = destination.FTEMetadataId is null
                ? null
                : inboundTargets.FirstOrDefault(f => f.FTEMetadataId == destination.FTEMetadataId)!.ImportJobName,
            OperatorId = destination.OperatorId,
            CreationDate = destination.CreationDate,
            LastModifiedDate = destination.LastModifiedDate
        };
    }

    private static TransferConfiguration MapTransferConfiguration(
        FtsSourceLocation sourceLocation,
        string? ftpServerName,
        IReadOnlyList<Destination> destinations)
    {
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
            Destinations = destinations
        };
    }
}


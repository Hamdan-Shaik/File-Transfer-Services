using FxferService.Api.Commands.Models;
using FxferService.Api.Queries.Models;
using FxferService.Application.Contracts.Queries;
using FxferService.Application.QueryHandlers;
using FxferService.Domain.Contracts.Domain;
using FxferService.Domain.Contracts.Events;
using FxferService.Domain.Contracts.Queries.Parameters;
using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Persistence.Batch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FxferService.Api.Queries.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/transfer-configurations")]
public sealed class TransferConfigurationsQueriesController : ControllerBase
{
    private readonly FxferDbContext _dbContext;

    public TransferConfigurationsQueriesController(FxferDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<TransferConfigurationSummary>), StatusCodes.Status200OK)]
    public ActionResult<PagedResult<TransferConfigurationSummary>> SearchTransferConfigurations(
        [FromQuery] bool? isActive,
        [FromQuery] int? configNumber,
        [FromQuery] string? nameContains,
        [FromQuery] string? ownerNameContains,
        [FromQuery] string? protocolCode,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] string? sortBy,
        [FromQuery] string? sortDir)
    {
        var query = new SourceLocationQuery
        {
            CurrentPage = page ?? 1,
            PageSize = pageSize ?? 0,
            SortCriteria = string.IsNullOrWhiteSpace(sortBy)
                ? null
                : new List<SortCriteria>
                {
                    new SortCriteria
                    {
                        FieldName = sortBy,
                        Order = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase) ? SortOrder.Desc : SortOrder.Asc,
                    },
                },
            Where = new SourceLocationQueryParms
            {
                SoureceLocationId = string.Empty,
                ConfigId = configNumber,
                ConfigName = nameContains ?? string.Empty,
                OwnerName = ownerNameContains ?? string.Empty,
                OwnerSubsystem = string.Empty,
                Protocol = protocolCode ?? string.Empty,
                IsActive = isActive ?? false,
            },
        };

        using var fileTransferContext = new FileTransferEntityDbContext(_dbContext);

        // F02-001: forward the query through routing and return the routed response.
        var routed = new SearchSourceLocationQueryExecutor().Execute(fileTransferContext, query);
        var models = routed.Results ?? new List<SourceLocationModel>();

        var items = models
            .Select(m => new TransferConfigurationSummary(
                TransferConfigurationId: m.FtsSourceLocationId,
                ConfigNumber: m.ConfigId,
                Name: m.ConfigName,
                ProtocolCode: m.Protocol,
                Address: m.Address,
                FileMask: m.Mask,
                IsActive: m.IsActive,
                IsSuspended: m.Suspended))
            .ToArray();

        return Ok(new PagedResult<TransferConfigurationSummary>(
            Items: items,
            RowCount: routed.RowCount,
            CurrentPage: routed.CurrentPage,
            PageSize: routed.PageSize));
    }

    [HttpGet("{transferConfigurationId:guid}")]
    [ProducesResponseType(typeof(TransferConfiguration), StatusCodes.Status200OK)]
    public ActionResult<TransferConfiguration> GetTransferConfiguration([FromRoute] Guid transferConfigurationId)
    {
        var query = new GetSourceLocationQuery
        {
            Where = new GetSourceLocationQueryParms
            {
                SourceLocationId = transferConfigurationId.ToString(),
                IsActive = false,
            },
        };

        using var fileTransferContext = new FileTransferEntityDbContext(_dbContext);

        // F02-002: forward the query through routing and return the routed response.
        var routed = new GetSourceLocationQueryExecutor().Execute(fileTransferContext, query);
        var model = routed.Results!.First();

        var destinations = (model.DestinationAddress ?? new List<FtsDestinationLocationAdded>())
            .Select(d => new Destination(
                DestinationId: d.DestinationId,
                Address: d.DestinationAddress ?? string.Empty,
                ProtocolCode: d.DestinationProtocol ?? string.Empty,
                OverwriteAtDestination: d.OverWriteAtDestination,
                CompressAtDestination: d.CompressAtDestination,
                RenameAtDestination: d.RenameAtDestination,
                DestinationIsArchive: d.DestinationArchive,
                IsActive: d.IsValid,
                FtpServerId: d.FtpServerId,
                RenamePattern: d.RenamePattern,
                Contact: (d.ContactName == null && d.ContactPhone == null && d.ContactEmail == null)
                    ? null
                    : new DestinationContact(d.ContactName, d.ContactPhone, d.ContactEmail),
                Encryption: null,
                ExternalFileDefinitionId: d.FTEMetadataId,
                DestinationType: d.DestinationType,
                ContractName: d.ContractName))
            .ToArray();

        var response = new TransferConfiguration(
            TransferConfigurationId: model.FtsSourceLocationId,
            ConfigNumber: model.ConfigId,
            Name: model.ConfigName,
            Description: model.Description,
            ProtocolCode: model.Protocol,
            Address: model.Address,
            FileMask: model.Mask,
            FtpServerId: model.FtpServerId,
            SendNotifications: model.SendNotification,
            Owner: new OwnerInfo(model.OwnerName, model.OwnerSubsystem, model.OwnerPhoneNumber, model.OwnerEmail),
            Schedule: new ScheduleInfo(
                RunOnMonday: model.RunOnMonday,
                RunOnTuesday: model.RunOnTuesday,
                RunOnWednesday: model.RunOnWednesday,
                RunOnThursday: model.RunOnThursday,
                RunOnFriday: model.RunOnFriday,
                RunOnSaturday: model.RunOnSaturday,
                RunOnSunday: model.RunOnSunday,
                RunOnFirstDayOfMonth: model.RunOnFirstDay,
                RunOnLastDayOfMonth: model.RunOnLastDay,
                RunOnThisDayOfMonth: model.RunOnThisDay,
                StartTime24hhmm: model.StartWindow,
                EndTime24hhmm: model.EndWindow),
            FileDirection: model.FileDirection,
            TradingPartner: model.TradingPartner,
            IsActive: model.IsActive,
            IsSuspended: model.Suspended,
            SuspendedChangedAt: null,
            Destinations: destinations);

        return Ok(response);
    }
}


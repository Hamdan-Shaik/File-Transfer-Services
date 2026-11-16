using FxferService.Api.Commands.Models;
using FxferService.Api.Queries.Models;
using FxferService.Application.Contracts.Queries;
using FxferService.Application.QueryHandlers;
using FxferService.Domain.Contracts.Domain;
using FxferService.Domain.Contracts.Queries.Parameters;
using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Persistence.Batch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FxferService.Api.Queries.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/integration-files")]
public sealed class IntegrationFilesQueriesController : ControllerBase
{
    private readonly FxferDbContext _dbContext;

    public IntegrationFilesQueriesController(FxferDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<IntegrationFile>), StatusCodes.Status200OK)]
    public ActionResult<PagedResult<IntegrationFile>> SearchIntegrationFiles(
        [FromQuery] Guid? integrationFileId,
        [FromQuery] string? fileNameContains,
        [FromQuery] int? page,
        [FromQuery] int? pageSize)
    {
        var query = new GetIntegrationFileQuery
        {
            CurrentPage = page ?? 1,
            PageSize = pageSize ?? 0,
            Where = new IntegrationFileQueryParms
            {
                FTSIntegrationFileId = integrationFileId ?? Guid.Empty,
                FileName = fileNameContains ?? string.Empty,
            },
        };

        using var fileTransferContext = new FileTransferEntityDbContext(_dbContext);

        // F02-043: forward the query through routing and return the routed response.
        var routed = new IntegrationFileQueryExecutor().Execute(fileTransferContext, query);
        var models = routed.Results ?? new List<FtsIntegrationFileModel>();

        var items = models
            .Select(m => new IntegrationFile(
                IntegrationFileId: m.FtsIntegrationFileId,
                GroupId: m.GroupId,
                ConfigNumber: m.ConfigId,
                FileName: m.FileName,
                OutboundMetadataDefinitionId: m.ETFOutboundMetadataDefnId,
                IsProcessed: m.IsProcessed,
                IsActive: m.IsValid,
                CreatedAt: new DateTimeOffset(m.CreationDate, TimeSpan.Zero),
                LastModifiedAt: new DateTimeOffset(m.LastModifiedDate, TimeSpan.Zero)))
            .ToArray();

        return Ok(new PagedResult<IntegrationFile>(
            Items: items,
            RowCount: routed.RowCount,
            CurrentPage: routed.CurrentPage,
            PageSize: routed.PageSize));
    }
}


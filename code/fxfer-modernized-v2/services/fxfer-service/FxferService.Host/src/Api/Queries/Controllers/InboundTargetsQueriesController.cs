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
[Route("api/v1/inbound-targets")]
public sealed class InboundTargetsQueriesController : ControllerBase
{
    private readonly FxferDbContext _dbContext;

    public InboundTargetsQueriesController(FxferDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(InboundTarget[]), StatusCodes.Status200OK)]
    public ActionResult<InboundTarget[]> SearchInboundTargets(
        [FromQuery] string? contractName,
        [FromQuery] Guid? externalFileDefinitionId)
    {
        var query = new InboundTargetQuery
        {
            Where = new InboundTargetQueryParms
            {
                ContractName = contractName ?? string.Empty,
                FTEMetadataId = externalFileDefinitionId ?? Guid.Empty,
            },
        };

        using var fileTransferContext = new FileTransferEntityDbContext(_dbContext);
        var routed = new InboundTargetQueryExecutor().Execute(fileTransferContext, query);

        var items = (routed.Results ?? new List<FtsInboundTargetModel>())
            .Select(t => new InboundTarget(
                InboundTargetId: t.FtsInboundTargetId,
                ExternalFileDefinitionId: t.FTEMetadataId,
                ImportJobName: t.ImportJobName,
                ContractName: t.ContractName,
                IsActive: t.IsValid,
                CreatedAt: new DateTimeOffset(t.CreationDate, TimeSpan.Zero),
                LastModifiedAt: new DateTimeOffset(t.LastModifiedDate, TimeSpan.Zero)))
            .ToArray();

        return Ok(items);
    }
}


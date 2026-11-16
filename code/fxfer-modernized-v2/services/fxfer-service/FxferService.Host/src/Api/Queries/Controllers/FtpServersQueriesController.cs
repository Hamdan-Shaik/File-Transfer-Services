using FxferService.Api.Commands.Models;
using FxferService.Api.Queries.Models;
using FxferService.Application.Contracts.Queries;
using FxferService.Application.QueryHandlers;
using FxferService.Domain.Contracts.Domain;
using FxferService.Domain.Contracts.Queries.Parameters;
using FxferService.Domain.Utilities;
using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Persistence.Batch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FxferService.Api.Queries.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/ftp-servers")]
public sealed class FtpServersQueriesController : ControllerBase
{
    private readonly FxferDbContext _dbContext;

    public FtpServersQueriesController(FxferDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<FtpServer>), StatusCodes.Status200OK)]
    public ActionResult<PagedResult<FtpServer>> SearchFtpServers(
        [FromQuery] bool? isActive,
        [FromQuery] string? name,
        [FromQuery] string? description,
        [FromQuery] string? user,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] string? sortBy,
        [FromQuery] string? sortDir,
        [FromQuery] bool includePassword = false)
    {
        var query = new FtpServerQuery
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
            Where = new FtpServerQueryParms
            {
                IsValid = isActive,
                FtpServerId = Guid.Empty,
                FtpServerName = name ?? string.Empty,
                FtpDescription = description ?? string.Empty,
                FtpUser = user ?? string.Empty,
                SearchOptions = new List<SearchModeOption>(),
            },
        };

        using var fileTransferContext = new FileTransferEntityDbContext(_dbContext);

        // F01-001: forward the query through routing and return the routed response.
        var routed = new GetFtpServerQueryExecutor().Execute(fileTransferContext, query);
        var models = routed.Results ?? new List<FtpServerModel>();

        var items = models
            .Select(m => new FtpServer(
                FtpServerId: m.FtpServerId,
                Name: m.FtpServerName,
                IpAddress: m.FtpServerIp,
                TransferMode: m.FtpTransferMode,
                User: m.FtpUser,
                Password: includePassword ? m.FtpPassword : null,
                Description: m.FtpDescription,
                Port: m.FtpPort,
                ProxyAddress: string.IsNullOrWhiteSpace(m.FtpProxyAddress) ? null : m.FtpProxyAddress,
                ProxyPort: m.FtpProxyPort,
                SshAuthentication: m.UseSSHAuth,
                IsActive: m.IsValid,
                CreatedAt: new DateTimeOffset(m.CreationDate, TimeSpan.Zero),
                LastModifiedAt: new DateTimeOffset(m.LastModifiedDate, TimeSpan.Zero)))
            .ToArray();

        return Ok(new PagedResult<FtpServer>(
            Items: items,
            RowCount: routed.RowCount,
            CurrentPage: routed.CurrentPage,
            PageSize: routed.PageSize));
    }
}


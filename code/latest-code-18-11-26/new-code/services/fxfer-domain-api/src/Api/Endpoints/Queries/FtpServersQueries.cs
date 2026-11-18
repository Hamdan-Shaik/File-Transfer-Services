using Fxfer.DomainApi.Application.Queries;
using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Contracts.Queries.Parameters;
using Fxfer.DomainApi.Contracts.Utilities;
using Fxfer.DomainApi.Domain.Constants;
using Fxfer.DomainApi.Infrastructure.DataAccess;

namespace Fxfer.DomainApi.Api.Endpoints.Queries;

public static class FtpServersQueries
{
    public static RouteGroupBuilder MapFtpServersQueries(this RouteGroupBuilder group)
    {
        group.MapGet("/ftp-servers", async (
            FxferDbContext dbContext,
            bool? isValid,
            Guid? ftpServerId,
            string? ftpServerName,
            string? ftpDescription,
            string? nameMatch,
            string? descriptionMatch,
            int page = 1,
            int pageSize = 50,
            CancellationToken ct = default) =>
        {
            // F02-002, F02-003: route the request through the query handler and return results.
            var query = new FtpServerQuery
            {
                Where = new FtpServerQueryParms
                {
                    IsValid = isValid,
                    FtpServerId = ftpServerId ?? Guid.Empty,
                    FtpServerName = ftpServerName,
                    FtpDescription = ftpDescription,
                    SearchOptions = BuildSearchOptions(nameMatch, descriptionMatch)
                }
            };

            var executor = new GetFtpServerQueryExecutor();
            var results = await executor.ExecuteAsync(dbContext, query, ct);

            var paged = ApplyPaging(results, page, pageSize);

            return Results.Ok(new PagedResult<FtpServer>
            {
                Items = paged.Items.Select(MapFtpServer).ToArray(),
                Total = paged.Total,
                Page = page,
                PageSize = pageSize
            });
        });

        return group;
    }

    private static List<SearchModeOption>? BuildSearchOptions(string? nameMatch, string? descriptionMatch)
    {
        var nameMode = MapMatchMode(nameMatch);
        var descriptionMode = MapMatchMode(descriptionMatch);

        if (nameMode is null && descriptionMode is null)
        {
            return null;
        }

        var options = new List<SearchModeOption>();
        if (nameMode is not null)
        {
            options.Add(new SearchModeOption
            {
                FieldName = CommonConstants.SortFields.FtpServer.FtpServerName,
                FilterType = nameMode.Value
            });
        }

        if (descriptionMode is not null)
        {
            options.Add(new SearchModeOption
            {
                FieldName = CommonConstants.SortFields.FtpServer.FtpDescription,
                FilterType = descriptionMode.Value
            });
        }

        return options;
    }

    private static SearchModeOption.SearchMode? MapMatchMode(string? match)
    {
        return match?.Trim() switch
        {
            null or "" or "exact" => SearchModeOption.SearchMode.EqualTo,
            "contains" => SearchModeOption.SearchMode.Contains,
            "startsWith" => SearchModeOption.SearchMode.StartsWith,
            "endsWith" => SearchModeOption.SearchMode.EndsWith,
            _ => SearchModeOption.SearchMode.EqualTo
        };
    }

    private static (IReadOnlyList<FtpServerModel> Items, int Total) ApplyPaging(
        IReadOnlyList<FtpServerModel> items,
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

    private static FtpServer MapFtpServer(FtpServerModel model)
    {
        var creationUtc = DateTime.SpecifyKind(model.CreationDate, DateTimeKind.Utc);
        var lastModifiedUtc = DateTime.SpecifyKind(model.LastModifiedDate, DateTimeKind.Utc);

        return new FtpServer
        {
            FtpServerId = model.FtpServerId,
            FtpServerName = model.FtpServerName ?? string.Empty,
            FtpDescription = model.FtpDescription ?? string.Empty,
            FtpServerIp = model.FtpServerIp ?? string.Empty,
            FtpTransferMode = model.FtpTransferMode ?? string.Empty,
            FtpUser = model.FtpUser ?? string.Empty,
            CredentialSecretRef = model.FtpPassword,
            FtpPort = model.FtpPort,
            FtpProxyAddress = model.FtpProxyAddress,
            FtpProxyPort = model.FtpProxyPort,
            UseSshAuth = model.UseSSHAuth,
            IsValid = model.IsValid,
            OperatorId = model.OperatorId ?? string.Empty,
            CreationDate = new DateTimeOffset(creationUtc),
            LastModifiedDate = new DateTimeOffset(lastModifiedUtc)
        };
    }
}


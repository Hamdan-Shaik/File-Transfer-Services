using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using Fxfer.Bff.Contracts.Common;
using Fxfer.Bff.Contracts.FtpServers;
using Fxfer.Bff.Contracts.Tracking;
using Fxfer.Bff.Contracts.TransferConfigurations;

namespace Fxfer.Bff.Application.Providers;

public sealed class FileTransferQueryProvider : IFileTransferQueryProvider
{
    private const string DownstreamBasePath = "/api/fxfer-domain/v1";

    private readonly FxferDomainApiClient _domainApiClient;
    private readonly DataListsClient _dataListsClient;

    public FileTransferQueryProvider(FxferDomainApiClient domainApiClient, DataListsClient dataListsClient)
    {
        _domainApiClient = domainApiClient;
        _dataListsClient = dataListsClient;
    }

    public async Task<PagedResult<TransferConfigurationSummary>> SearchFileTransferAsync(TransferConfigurationSearchParams search, CancellationToken ct)
    {
        var queryParams = new List<(string Key, string? Value)>
        {
            ("configId", search.ConfigId?.ToString(CultureInfo.InvariantCulture)),
            ("configName", search.ConfigName),
            ("ownerName", search.OwnerName),
            ("ownerSubsystem", search.OwnerSubsystem?.ToString()),
            ("protocol", search.Protocol),
            ("isActive", search.IsActive?.ToString()),
            ("page", search.Page.ToString(CultureInfo.InvariantCulture)),
            ("pageSize", search.PageSize.ToString(CultureInfo.InvariantCulture))
        };

        foreach (var sort in BuildSourceLocationSortParms(search.SortCriteria))
        {
            queryParams.Add(("sortField", sort.FieldName));
            queryParams.Add(("sortOrder", sort.Order.ToString()));
        }

        var requestUri = BuildUri($"{DownstreamBasePath}/transfer-configurations", queryParams);

        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        using var response = await _domainApiClient.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PagedResult<TransferConfigurationSummary>>(cancellationToken: ct);
        return result ?? new PagedResult<TransferConfigurationSummary> { Items = Array.Empty<TransferConfigurationSummary>(), Total = 0, Page = search.Page, PageSize = search.PageSize };
    }

    public async Task<TransferConfiguration?> GetFileTransferConfigurationInformationAsync(Guid sourceLocationId, CancellationToken ct)
    {
        var requestUri = $"{DownstreamBasePath}/transfer-configurations/{sourceLocationId}";

        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        using var response = await _domainApiClient.SendAsync(request, ct);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<DownstreamTransferConfiguration>(cancellationToken: ct);
        return result is null ? null : MapToTransferConfiguration(result);
    }

    public async Task<IReadOnlyList<FtpServerOption>> GetFtpServersOptionsAsync(bool isValid, CancellationToken ct)
    {
        // Legacy GetFtpServers: no paging inputs; return all results where IsValid==true.

        const int pageSize = 500;
        var page = 1;
        var options = new List<FtpServerOption>();

        while (true)
        {
            var queryParams = new List<(string Key, string? Value)>
            {
                ("isValid", isValid.ToString()),
                ("page", page.ToString(CultureInfo.InvariantCulture)),
                ("pageSize", pageSize.ToString(CultureInfo.InvariantCulture))
            };

            var requestUri = BuildUri($"{DownstreamBasePath}/ftp-servers", queryParams);

            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            using var response = await _domainApiClient.SendAsync(request, ct);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PagedResult<FtpServer>>(cancellationToken: ct);
            if (result is null || result.Items.Count == 0)
            {
                break;
            }

            foreach (var item in result.Items)
            {
                options.Add(new FtpServerOption { FtpServerId = item.FtpServerId, FtpServerName = item.FtpServerName });
            }

            if (result.Items.Count < pageSize || (result.Total > 0 && options.Count >= result.Total))
            {
                break;
            }

            page++;
        }

        return options.OrderBy(x => x.FtpServerName, StringComparer.OrdinalIgnoreCase).ToList();
    }

    public async Task<PagedResult<FtpServer>> GetFtpServersAsync(FtpServerSearchParams search, bool removePassword, CancellationToken ct)
    {
        // drop: legacy BuildResponseModel/BuildQuery/GetFTPServersResponse/PopulateResponseResults; target uses explicit query parameters + PagedResult<T>.
        // drop: legacy MapFTPServerModel removePassword branch; BFF contract does not expose passwords.

        var queryParams = new List<(string Key, string? Value)>
        {
            ("isValid", search.IsValid?.ToString()),
            ("ftpServerId", search.FtpServerId?.ToString()),
            ("ftpServerName", search.FtpServerName),
            ("ftpDescription", search.FtpDescription),
            ("nameMatch", search.NameMatch),
            ("descriptionMatch", search.DescriptionMatch),
            ("page", search.Page.ToString(CultureInfo.InvariantCulture)),
            ("pageSize", search.PageSize.ToString(CultureInfo.InvariantCulture))
        };

        foreach (var sort in BuildFtpServerSortParms(search.SortCriteria))
        {
            queryParams.Add(("sortField", sort.FieldName));
            queryParams.Add(("sortOrder", sort.Order.ToString()));
        }

        var requestUri = BuildUri($"{DownstreamBasePath}/ftp-servers", queryParams);

        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        using var response = await _domainApiClient.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PagedResult<FtpServer>>(cancellationToken: ct);
        return result ?? new PagedResult<FtpServer> { Items = Array.Empty<FtpServer>(), Total = 0, Page = search.Page, PageSize = search.PageSize };
    }

    public async Task<IReadOnlyList<InboundTargetMetadata>> GetMetadataIdsAsync(string? contractName, CancellationToken ct)
    {
        // drop: legacy GetServiceQuery/GetSearchType/build ResponseModel wrappers; target uses explicit query parameters + contract DTOs.

        var queryParams = new List<(string Key, string? Value)>
        {
            ("fteMetadataId", Guid.Empty.ToString()),
            ("contractName", contractName)
        };

        var requestUri = BuildUri($"{DownstreamBasePath}/inbound-targets", queryParams);

        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        using var response = await _domainApiClient.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<IReadOnlyList<DownstreamInboundTarget>>(cancellationToken: ct);
        if (result is null || result.Count == 0)
        {
            return Array.Empty<InboundTargetMetadata>();
        }

        return result
            .Select(item => new InboundTargetMetadata { FteMetadataId = item.FteMetadataId, ImportJobName = item.ImportJobName })
            .ToArray();
    }

    public async Task<IReadOnlyList<SourceLocationOption>> GetSourceLocationsAsync(bool isActive, CancellationToken ct)
    {
        var queryParams = new List<(string Key, string? Value)> { ("isActive", isActive.ToString()) };
        var requestUri = BuildUri($"{DownstreamBasePath}/source-locations", queryParams);

        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        using var response = await _domainApiClient.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<IReadOnlyList<SourceLocationOption>>(cancellationToken: ct);
        if (result is null || result.Count == 0)
        {
            return Array.Empty<SourceLocationOption>();
        }

        var mapped = result.ToList();

        // Legacy GetSourceLocations sorts by ConfigName ascending.
        mapped.Sort((a1, a2) => a1.ConfigName.CompareTo(a2.ConfigName));
        return mapped;
    }

    public async Task<PagedResult<FileTransferTracking>> SearchFileTransferTrackingAsync(FileTrackingSearchParams search, CancellationToken ct)
    {
        var currentPage = BuildServicePagingCriteria(search.Page);
        var pageSize = BuildServicePageSizeCriteria(search.PageSize);

        var searchFields = new List<TrackingSearchField>();

        if (search.FileId is not null)
        {
            searchFields.Add(new TrackingSearchField("FileId", search.FileId));
        }

        if (search.FileName is not null)
        {
            searchFields.Add(new TrackingSearchField("FileName", search.FileName));
        }

        if (search.FileStatus is not null)
        {
            searchFields.Add(new TrackingSearchField("FileStatus", search.FileStatus));
        }

        if (search.FileDirection is not null)
        {
            searchFields.Add(new TrackingSearchField("Direction", search.FileDirection));
        }

        if (search.FileTransportType is not null)
        {
            searchFields.Add(new TrackingSearchField("TransportType", search.FileTransportType));
        }

        if (search.StartDate is not null)
        {
            searchFields.Add(new TrackingSearchField("FromDate", search.StartDate));
        }

        if (search.EndDate is not null)
        {
            searchFields.Add(new TrackingSearchField("ToDate", search.EndDate));
        }

        if (search.FileSizeFrom is not null)
        {
            searchFields.Add(new TrackingSearchField("FromSize", search.FileSizeFrom));
        }

        if (search.FileSizeThrough is not null)
        {
            searchFields.Add(new TrackingSearchField("ToSize", search.FileSizeThrough));
        }

        if (search.TradingPartner is not null)
        {
            searchFields.Add(new TrackingSearchField("TradingPartnerId", search.TradingPartner));
        }

        if (search.FileSourceLocationId is not null)
        {
            searchFields.Add(new TrackingSearchField("SourceLocationId", search.FileSourceLocationId));
        }

        var where = BuildWhereQueryParms(searchFields);

        var queryParams = new List<(string Key, string? Value)>
        {
            ("fileId", where.FileId),
            ("fileName", where.FileName),
            ("fileStatus", where.FileStatus),
            ("fileDirection", where.FileDirection),
            ("fileTransportType", where.FileTransportType),
            ("fileSourceLocationId", where.FileSourceLocationId),
            ("startDate", where.StartDate?.ToString("O", CultureInfo.InvariantCulture)),
            ("endDate", where.EndDate?.ToString("O", CultureInfo.InvariantCulture)),
            ("fileSizeFrom", where.FileSizeFrom?.ToString(CultureInfo.InvariantCulture)),
            ("fileSizeThrough", where.FileSizeThrough?.ToString(CultureInfo.InvariantCulture)),
            ("tradingPartner", where.TradingPartner),
            ("page", currentPage.ToString(CultureInfo.InvariantCulture)),
            ("pageSize", pageSize.ToString(CultureInfo.InvariantCulture))
        };

        foreach (var sort in BuildServiceSortCriteria(search.SortCriteria))
        {
            queryParams.Add(("sortField", sort.FieldName));
            queryParams.Add(("sortOrder", sort.Order.ToString()));
        }

        var requestUri = BuildUri($"{DownstreamBasePath}/file-tracking", queryParams);

        var result = await SearchFileTransferMovementAsync(requestUri, ct);
        if (result is null)
        {
            return new PagedResult<FileTransferTracking> { Items = Array.Empty<FileTransferTracking>(), Total = 0, Page = currentPage, PageSize = pageSize };
        }

        var items = result.Items.Select(MapToInternalFileTransferTrackingResultModel).ToArray();
        return new PagedResult<FileTransferTracking> { Items = items, Total = result.Total, Page = result.Page, PageSize = result.PageSize };
    }

    private async Task<PagedResult<DownstreamFileTransferTracking>?> SearchFileTransferMovementAsync(Uri requestUri, CancellationToken ct)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        using var response = await _domainApiClient.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<PagedResult<DownstreamFileTransferTracking>>(cancellationToken: ct);
    }

    public async Task<FileTransferTracking?> GetFileTransferTrackingDetailAsync(Guid fileId, CancellationToken ct)
    {
        // drop: legacy MapToServiceRequestor; request context is propagated via headers.
        var requestUri = $"{DownstreamBasePath}/file-tracking/{fileId}";

        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        using var response = await _domainApiClient.SendAsync(request, ct);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<DownstreamFileTransferTracking>(cancellationToken: ct);
        return result is null ? null : MapToInternalFileTransferTrackingDetailModel(result);
    }

    private static TransferConfiguration MapToTransferConfiguration(DownstreamTransferConfiguration serviceModel)
    {
        var destinations = serviceModel.Destinations.Select(MapToDestination).ToArray();

        return new TransferConfiguration
        {
            FtsSourceLocationId = serviceModel.FtsSourceLocationId,
            ConfigId = serviceModel.ConfigId,
            ConfigName = serviceModel.ConfigName,
            Description = serviceModel.Description ?? string.Empty,
            FtpServerId = serviceModel.FtpServerId ?? Guid.Empty,
            FtpServerName = serviceModel.FtpServerName ?? string.Empty,
            Address = serviceModel.Address,
            Mask = serviceModel.Mask ?? string.Empty,
            Protocol = serviceModel.Protocol,
            FileDirection = serviceModel.FileDirection ?? string.Empty,
            OwnerName = serviceModel.OwnerName,
            OwnerEmail = serviceModel.OwnerEmail,
            OwnerSubsystem = serviceModel.OwnerSubsystem,
            OwnerPhoneNumber = serviceModel.OwnerPhoneNumber,
            TradingPartner = serviceModel.TradingPartner,
            OperatorId = serviceModel.OperatorId,
            RunOnMonday = serviceModel.RunOnMonday,
            RunOnTuesday = serviceModel.RunOnTuesday,
            RunOnWednesday = serviceModel.RunOnWednesday,
            RunOnThursday = serviceModel.RunOnThursday,
            RunOnFriday = serviceModel.RunOnFriday,
            RunOnSaturday = serviceModel.RunOnSaturday,
            RunOnSunday = serviceModel.RunOnSunday,
            RunOnFirstDay = serviceModel.RunOnFirstDay,
            RunOnLastDay = serviceModel.RunOnLastDay,
            RunOnThisDay = serviceModel.RunOnThisDay,
            StartWindow = serviceModel.StartWindow,
            EndWindow = serviceModel.EndWindow,
            SendNotification = serviceModel.SendNotification,
            IsActive = serviceModel.IsActive,
            ValidChangeDate = serviceModel.ValidChangeDate,
            Suspended = serviceModel.Suspended,
            SuspendedChangeDate = serviceModel.SuspendedChangeDate,
            LastModifiedDate = serviceModel.LastModifiedDate,
            CreationDate = serviceModel.CreationDate,
            Destinations = destinations
        };
    }

    private static Destination MapToDestination(DownstreamDestination destination)
    {
        // NOTE: Downstream contract does not carry the full legacy destination detail shape.
        return new Destination
        {
            DestinationId = destination.FtsDestinationId,
            DestinationAddress = destination.Address,
            DestinationProtocol = destination.Protocol,
            FtpServerId = null,
            RenamePattern = null,
            OverWriteAtDestination = null,
            CompressAtDestination = null,
            RenameAtDestination = null,
            DestinationArchive = null,
            ContactName = null,
            ContactPhone = null,
            ContactEmail = null,
            IsValid = destination.IsValid,
            FteMetadataId = null,
            DestinationType = destination.DestinationType,
            ContractName = null,
            ImportJobName = null,
            OperatorId = destination.OperatorId,
            CreationDate = destination.CreationDate,
            LastModifiedDate = destination.LastModifiedDate
        };
    }

    // MaintainabilityDetails: ApprovedIndex = 52, ReviewedOnSprint = 94, ReviewerName = "Josue Chavez"
    private static FileTransferTracking MapToInternalFileTransferTrackingResultModel(DownstreamFileTransferTracking serviceModel)
    {
        // drop: legacy LocalizeObjectProperties<T>; target has no equivalent localization layer.
        return new FileTransferTracking
        {
            FileId = serviceModel.FileId,
            FileSourceLocationId = serviceModel.FileSourceLocationId,
            FileName = serviceModel.FileName,
            FileStatus = serviceModel.FileStatus,
            FileDirection = serviceModel.FileDirection,
            FileTransportType = serviceModel.FileTransportType,
            CreatedDate = serviceModel.CreatedDate,
            TradingPartner = serviceModel.TradingPartner,
            CheckSumCode = serviceModel.CheckSumCode,
            FileSize = serviceModel.FileSize,
            SourceLocAddress = serviceModel.SourceLocAddress,
            SourceLocConfigName = serviceModel.SourceLocConfigName,
            OriginatingServerName = serviceModel.OriginatingServerName,
            Events = Array.Empty<FileEvent>(),
            Exceptions = Array.Empty<FileException>()
        };
    }

    private static FileTransferTracking MapToInternalFileTransferTrackingDetailModel(DownstreamFileTransferTracking serviceModel)
    {
        var information = MapToInternalFileTransferTrackingInformationModel(serviceModel);
        var events = MapToInternalFileTransferTrackingEvents(serviceModel.Events);
        var exceptions = MapToInternalFileTransferTrackingExceptions(serviceModel.Exceptions);

        return information with { Events = events, Exceptions = exceptions };
    }

    // MaintainabilityDetails: ApprovedIndex = 50, ReviewedOnSprint = 94, ReviewerName = "Josue Chavez"
    private static FileTransferTracking MapToInternalFileTransferTrackingInformationModel(DownstreamFileTransferTracking serviceModel)
    {
        // NOTE: Legacy maps SourceLocation = "{ConfigName} - {Address}"; target contract keeps them separate.
        // drop: legacy LocalizeObjectProperties<T>; target has no equivalent localization layer.
        return new FileTransferTracking
        {
            FileId = serviceModel.FileId,
            FileSourceLocationId = serviceModel.FileSourceLocationId,
            FileName = serviceModel.FileName,
            FileStatus = serviceModel.FileStatus,
            FileDirection = serviceModel.FileDirection,
            FileTransportType = serviceModel.FileTransportType,
            CreatedDate = serviceModel.CreatedDate,
            TradingPartner = serviceModel.TradingPartner,
            CheckSumCode = serviceModel.CheckSumCode,
            FileSize = serviceModel.FileSize,
            SourceLocAddress = serviceModel.SourceLocAddress,
            SourceLocConfigName = serviceModel.SourceLocConfigName,
            OriginatingServerName = serviceModel.OriginatingServerName,
            Events = Array.Empty<FileEvent>(),
            Exceptions = Array.Empty<FileException>()
        };
    }

    private static IReadOnlyList<FileEvent> MapToInternalFileTransferTrackingEvents(IReadOnlyList<DownstreamFileEvent> serviceEvents)
    {
        // drop: legacy LocalizeObjectProperties<T>; target has no equivalent localization layer.
        var trackingEvents = new List<FileEvent>();

        foreach (var serviceEvent in serviceEvents)
        {
            trackingEvents.Add(MapToFileEvent(serviceEvent));
        }

        return trackingEvents;
    }

    private static IReadOnlyList<FileException> MapToInternalFileTransferTrackingExceptions(IReadOnlyList<DownstreamFileException> serviceExceptions)
    {
        // drop: legacy LocalizeObjectProperties<T>; target has no equivalent localization layer.
        var trackingExceptions = new List<FileException>();

        foreach (var serviceException in serviceExceptions)
        {
            trackingExceptions.Add(MapToFileException(serviceException));
        }

        return trackingExceptions;
    }

    private static FileEvent MapToFileEvent(DownstreamFileEvent serviceEvent)
    {
        return new FileEvent
        {
            EventId = serviceEvent.EventId,
            FileId = serviceEvent.FileId,
            DestinationId = serviceEvent.DestinationId,
            EventCode = serviceEvent.EventCode,
            EventTrackDescription = serviceEvent.EventTrackDescription,
            CreatedDate = serviceEvent.CreatedDate
        };
    }

    private static FileException MapToFileException(DownstreamFileException serviceException)
    {
        return new FileException
        {
            ExceptionId = serviceException.ExceptionId,
            FileId = serviceException.FileId,
            DestinationId = serviceException.DestinationId,
            EventId = serviceException.EventId,
            ExceptionType = serviceException.ExceptionType,
            MethodName = serviceException.MethodName,
            Message = serviceException.Message ?? string.Empty,
            StackTrace = serviceException.StackTrace ?? string.Empty,
            CreatedDate = serviceException.CreatedDate
        };
    }

    private async Task<string> GetModuleNameAsync(string moduleId, CancellationToken ct)
    {
        var items = await _dataListsClient.GetItemsAsync("Module", startsWith: null, localeId: null, cancellationToken: ct);
        foreach (var item in items)
        {
            if (string.Equals(moduleId, item.Id, StringComparison.OrdinalIgnoreCase))
            {
                return item.Name;
            }
        }

        return string.Empty;
    }

    // F05-035: map only supported tracking filters; apply date/size only when parsable.
    internal static FileTransferTrackingQueryParms BuildWhereQueryParms(IReadOnlyList<TrackingSearchField>? searchFields)
    {
        var queryParms = new FileTransferTrackingQueryParms();

        if (searchFields != null && searchFields.Count > 0)
        {
            foreach (var searchField in searchFields)
            {
                switch (searchField.FieldName)
                {
                    case "FileId":
                        queryParms.FileId = searchField.FieldValue.ToString();
                        break;
                    case "FileName":
                        queryParms.FileName = searchField.FieldValue.ToString();
                        break;
                    case "FileStatus":
                        queryParms.FileStatus = searchField.FieldValue.ToString();
                        break;
                    case "Direction":
                        queryParms.FileDirection = searchField.FieldValue.ToString();
                        break;
                    case "TransportType":
                        queryParms.FileTransportType = searchField.FieldValue.ToString();
                        break;
                    case "FromDate":
                        if (DateTime.TryParse(searchField.FieldValue.ToString(), out var fromDate))
                        {
                            queryParms.StartDate = fromDate;
                        }

                        break;
                    case "ToDate":
                        if (DateTime.TryParse(searchField.FieldValue.ToString(), out var toDate))
                        {
                            queryParms.EndDate = toDate;
                        }

                        break;
                    case "FromSize":
                        if (decimal.TryParse(searchField.FieldValue.ToString(), out var fromSize))
                        {
                            queryParms.FileSizeFrom = fromSize;
                        }

                        break;
                    case "ToSize":
                        if (decimal.TryParse(searchField.FieldValue.ToString(), out var toSize))
                        {
                            queryParms.FileSizeThrough = toSize;
                        }

                        break;
                    case "TradingPartnerId":
                        queryParms.TradingPartner = searchField.FieldValue.ToString();
                        break;
                    case "SourceLocationId":
                        queryParms.FileSourceLocationId = searchField.FieldValue.ToString();
                        break;
                }
            }
        }

        return queryParms;
    }

    private static Uri BuildUri(string basePath, IReadOnlyList<(string Key, string? Value)> queryParams)
    {
        var filtered = queryParams
            .Where(p => !string.IsNullOrWhiteSpace(p.Value))
            .Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value!)}");

        var queryString = string.Join("&", filtered);
        var uriText = string.IsNullOrEmpty(queryString) ? basePath : $"{basePath}?{queryString}";
        return new Uri(uriText, UriKind.Relative);
    }

    private static IReadOnlyList<ServiceSortCriteria> BuildServiceSortCriteria(IReadOnlyList<SortCriteriaParam>? sortCriteria)
    {
        var serviceSortCriterias = new List<ServiceSortCriteria>();

        if (sortCriteria != null && sortCriteria.Count > 0)
        {
            var i = 0;
            foreach (var sort in sortCriteria)
            {
                switch (sort.SortField)
                {
                    case "FileId":
                        serviceSortCriterias.Add(new ServiceSortCriteria { FieldName = "FileId" });
                        serviceSortCriterias[i].Order = BuildServiceSortOrder(sort);
                        break;
                    case "FileName":
                        serviceSortCriterias.Add(new ServiceSortCriteria { FieldName = "FileName" });
                        serviceSortCriterias[i].Order = BuildServiceSortOrder(sort);
                        break;
                    case "FileStatus.FormattedText":
                        serviceSortCriterias.Add(new ServiceSortCriteria { FieldName = "FileStatus" });
                        serviceSortCriterias[i].Order = BuildServiceSortOrder(sort);
                        break;
                    case "Direction.FormattedText":
                        serviceSortCriterias.Add(new ServiceSortCriteria { FieldName = "FileDirection" });
                        serviceSortCriterias[i].Order = BuildServiceSortOrder(sort);
                        break;
                    case "Size":
                        serviceSortCriterias.Add(new ServiceSortCriteria { FieldName = "FileSize" });
                        serviceSortCriterias[i].Order = BuildServiceSortOrder(sort);
                        break;
                    case "CreateDate":
                        serviceSortCriterias.Add(new ServiceSortCriteria { FieldName = "CreatedDate" });
                        serviceSortCriterias[i].Order = BuildServiceSortOrder(sort);
                        break;
                    case "TradingPartnerId":
                        serviceSortCriterias.Add(new ServiceSortCriteria { FieldName = "TradingPartnerId" });
                        serviceSortCriterias[i].Order = BuildServiceSortOrder(sort);
                        break;
                }

                i++;
            }
        }

        return serviceSortCriterias;
    }

    private static int BuildServicePagingCriteria(int? page)
    {
        var currentPage = 1;

        if (page is not null)
        {
            currentPage = page.Value;
        }

        return currentPage;
    }

    private static int BuildServicePageSizeCriteria(int? pageSize)
    {
        var rowsPerPage = 5;

        if (pageSize is not null)
        {
            rowsPerPage = pageSize.Value;
        }

        return rowsPerPage;
    }

    private static IReadOnlyList<ServiceSortCriteria> BuildSourceLocationSortParms(IReadOnlyList<SortCriteriaParam>? sortCriteria)
    {
        var serviceSortCriterias = new List<ServiceSortCriteria>();

        if (sortCriteria != null && sortCriteria.Count > 0)
        {
            foreach (var sort in sortCriteria)
            {
                switch (sort.SortField)
                {
                    case "ConfigId":
                        serviceSortCriterias.Add(new ServiceSortCriteria { FieldName = "ConfigId", Order = BuildServiceSortOrder(sort) });
                        break;
                    case "ConfigName":
                        serviceSortCriterias.Add(new ServiceSortCriteria { FieldName = "ConfigName", Order = BuildServiceSortOrder(sort) });
                        break;
                    case "SourceAddress":
                        serviceSortCriterias.Add(new ServiceSortCriteria { FieldName = "SourceAddress", Order = BuildServiceSortOrder(sort) });
                        break;
                    case "SourceFileMask":
                        serviceSortCriterias.Add(new ServiceSortCriteria { FieldName = "SourceFileMask", Order = BuildServiceSortOrder(sort) });
                        break;
                    case "OwnerName":
                        serviceSortCriterias.Add(new ServiceSortCriteria { FieldName = "OwnerName", Order = BuildServiceSortOrder(sort) });
                        break;
                    case "OwnerSubsystem.Name":
                        serviceSortCriterias.Add(new ServiceSortCriteria { FieldName = "ModuleName", Order = BuildServiceSortOrder(sort) });
                        break;
                    case "Status":
                        serviceSortCriterias.Add(new ServiceSortCriteria { FieldName = "Status", Order = BuildServiceSortOrder(sort) });
                        break;
                    default:
                        serviceSortCriterias.Add(new ServiceSortCriteria { FieldName = "ConfigId", Order = SortOrder.Asc });
                        break;
                }
            }
        }

        if (serviceSortCriterias.Count == 0)
        {
            serviceSortCriterias.Add(new ServiceSortCriteria { FieldName = "ConfigId", Order = SortOrder.Asc });
        }

        return serviceSortCriterias;
    }

    private static IReadOnlyList<ServiceSortCriteria> BuildFtpServerSortParms(IReadOnlyList<SortCriteriaParam>? sortCriteria)
    {
        // No explicit legacy mapping method; accept pass-through sort fields for downstream when provided.
        var results = new List<ServiceSortCriteria>();

        if (sortCriteria != null && sortCriteria.Count > 0)
        {
            foreach (var sort in sortCriteria)
            {
                results.Add(new ServiceSortCriteria { FieldName = sort.SortField, Order = BuildServiceSortOrder(sort) });
            }
        }

        return results;
    }

    private static SortOrder BuildServiceSortOrder(SortCriteriaParam sortModel)
    {
        return sortModel.SortDirection == SortDirectionType.Ascending ? SortOrder.Asc : SortOrder.Desc;
    }

    private sealed record DownstreamTransferConfiguration
    {
        public Guid FtsSourceLocationId { get; init; }
        public int ConfigId { get; init; }
        public required string ConfigName { get; init; }
        public string? Description { get; init; }
        public Guid? FtpServerId { get; init; }
        public string? FtpServerName { get; init; }
        public required string Address { get; init; }
        public string? Mask { get; init; }
        public required string Protocol { get; init; }
        public string? FileDirection { get; init; }
        public required string OwnerName { get; init; }
        public required string OwnerEmail { get; init; }
        public Guid OwnerSubsystem { get; init; }
        public required string OwnerPhoneNumber { get; init; }
        public string? TradingPartner { get; init; }
        public required string OperatorId { get; init; }
        public bool RunOnMonday { get; init; }
        public bool RunOnTuesday { get; init; }
        public bool RunOnWednesday { get; init; }
        public bool RunOnThursday { get; init; }
        public bool RunOnFriday { get; init; }
        public bool RunOnSaturday { get; init; }
        public bool RunOnSunday { get; init; }
        public bool RunOnFirstDay { get; init; }
        public bool RunOnLastDay { get; init; }
        public int RunOnThisDay { get; init; }
        public required string StartWindow { get; init; }
        public required string EndWindow { get; init; }
        public bool SendNotification { get; init; }
        public bool IsActive { get; init; }
        public DateTimeOffset ValidChangeDate { get; init; }
        public bool Suspended { get; init; }
        public DateTimeOffset SuspendedChangeDate { get; init; }
        public DateTimeOffset LastModifiedDate { get; init; }
        public DateTimeOffset CreationDate { get; init; }
        public IReadOnlyList<DownstreamDestination> Destinations { get; init; } = Array.Empty<DownstreamDestination>();
    }

    private sealed record DownstreamDestination
    {
        public Guid FtsDestinationId { get; init; }
        public Guid FtsSourceLocationId { get; init; }
        public required string Address { get; init; }
        public string? Mask { get; init; }
        public required string Protocol { get; init; }
        public string? TradingPartner { get; init; }
        public string? DestinationType { get; init; }
        public bool IsValid { get; init; }
        public required string OperatorId { get; init; }
        public DateTimeOffset CreationDate { get; init; }
        public DateTimeOffset LastModifiedDate { get; init; }
    }

    private sealed record DownstreamInboundTarget
    {
        public Guid FteMetadataId { get; init; }
        public required string ImportJobName { get; init; }
    }

    private sealed record DownstreamFileTransferTracking
    {
        public Guid FileId { get; init; }
        public Guid FileSourceLocationId { get; init; }
        public required string FileName { get; init; }
        public required string FileStatus { get; init; }
        public required string FileDirection { get; init; }
        public required string FileTransportType { get; init; }
        public DateTimeOffset CreatedDate { get; init; }
        public string? TradingPartner { get; init; }
        public string? CheckSumCode { get; init; }
        public decimal FileSize { get; init; }
        public required string SourceLocAddress { get; init; }
        public required string SourceLocConfigName { get; init; }
        public required string OriginatingServerName { get; init; }
        public IReadOnlyList<DownstreamFileEvent> Events { get; init; } = Array.Empty<DownstreamFileEvent>();
        public IReadOnlyList<DownstreamFileException> Exceptions { get; init; } = Array.Empty<DownstreamFileException>();
    }

    private sealed record DownstreamFileEvent
    {
        public Guid EventId { get; init; }
        public Guid FileId { get; init; }
        public Guid DestinationId { get; init; }
        public required string EventCode { get; init; }
        public required string EventTrackDescription { get; init; }
        public DateTimeOffset CreatedDate { get; init; }
    }

    private sealed record DownstreamFileException
    {
        public Guid ExceptionId { get; init; }
        public Guid FileId { get; init; }
        public Guid DestinationId { get; init; }
        public Guid EventId { get; init; }
        public required string ExceptionType { get; init; }
        public required string MethodName { get; init; }
        public string? Message { get; init; }
        public string? StackTrace { get; init; }
        public DateTimeOffset CreatedDate { get; init; }
    }

    internal sealed record TrackingSearchField(string FieldName, object FieldValue);

    internal sealed record FileTransferTrackingQueryParms
    {
        public string? FileId { get; set; }
        public string? FileName { get; set; }
        public string? FileStatus { get; set; }
        public string? FileDirection { get; set; }
        public string? FileTransportType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? FileSizeFrom { get; set; }
        public decimal? FileSizeThrough { get; set; }
        public string? TradingPartner { get; set; }
        public string? FileSourceLocationId { get; set; }
    }

    private enum SortOrder
    {
        Asc = 0,
        Desc = 1
    }

    private sealed record ServiceSortCriteria
    {
        public required string FieldName { get; init; }
        public SortOrder Order { get; set; }
    }
}


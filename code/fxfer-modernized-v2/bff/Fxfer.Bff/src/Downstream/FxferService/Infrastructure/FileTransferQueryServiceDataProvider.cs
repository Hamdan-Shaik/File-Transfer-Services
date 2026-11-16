using Fxfer.Bff.Downstream.FxferService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.ServiceModel;
using Service = FileTransferSvcQueryService;

namespace Fxfer.Bff.Downstream.FxferService;

public partial class FileTransferQueryServiceDataProvider
{
    private readonly Service.IFileTransferSvcQueryService _serviceProxy;
    private readonly ILogger<FileTransferQueryServiceDataProvider> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private List<ListItemModel>? ftpServers;

    public FileTransferQueryServiceDataProvider(
        Service.IFileTransferSvcQueryService serviceProxy,
        ILogger<FileTransferQueryServiceDataProvider> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _serviceProxy = serviceProxy;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public ResponseModel<List<FtpServerModel>> GetFtpServers(RequestModel<SearchCriteriaModel> request)
    {
        ResponseModel<List<FtpServerModel>> response =
          new ResponseModel<List<FtpServerModel>>()
          {
              Data = new List<FtpServerModel>(),
              PagingResults = new PagingResultModel()
              {
                  RowCount = 0,
                  TotalRowsAvailable = 0
              },
              Result = new ResultModel()
              {
                  StatusCode = CoreEnumerations.Messaging.StatusCodeType.OK
              }
          };

        SearchCriteriaModel? searchCriteria = request.Data;

        Service.FtpServerQuery serviceQuery = new Service.FtpServerQuery()
        {
            Requestor = this.MapToServiceRequestor(request.Requestor),
            Where = new Service.FtpServerQueryParms()
            {
                IsValid = true
            }
        };

        try
        {
            Service.FtpServerQuery serviceResponse = _serviceProxy.GetFtpServers(serviceQuery);

            if (serviceResponse.Results != null && serviceResponse.Results.Count > 0)
            {
                foreach (Service.FtpServerModel item in serviceResponse.Results)
                {
                    response.Data.Add(this.MapToInternalFtpServerModel(item));
                }

                response.Result.StatusCode = CoreEnumerations.Messaging.StatusCodeType.OK;
            }
        }
        catch (FaultException<Service.ServiceException> ex)
        {
            _logger.LogError(ex, ex.Message);
            response.Result = this.FormatErrorResponseResult(ex.Detail.ErrorCode?.ToString() ?? string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            response.Result = this.FormatErrorResponseResult(ex);
        }

        return response;
    }
    private FtpServerModel MapToInternalFtpServerModel(Service.FtpServerModel serviceModel)
    {
        return new FtpServerModel
        {
            FtpServerId = serviceModel.FtpServerId.ToString(),
            FtpUser = serviceModel.FtpUser,
            FtpServerIp = serviceModel.FtpServerIp,
            FtpDescription = serviceModel.FtpDescription,
            FtpPort = serviceModel.FtpPort,
            FtpTransferMode = serviceModel.FtpTransferMode,
            FtpServerName = serviceModel.FtpServerName,
            FtpPassword = serviceModel.FtpPassword,
            FtpProxyAddress = serviceModel.FtpProxyAddress,
            FtpProxyPort = serviceModel.FtpProxyPort,
            IsValid = serviceModel.IsValid,
            CreationDate = serviceModel.CreationDate,
            LastModifiedDate = serviceModel.LastModifiedDate,
            OperatorId = serviceModel.OperatorId,
        };
    }

    private InboundTargetModel MapToInternalInboundTarget(Service.FtsInboundTargetModel serviceModel)
    {
        return new InboundTargetModel
        {
            ImportJobName = serviceModel.ImportJobName,
            FTEMetadataId = serviceModel.FTEMetadataId,
        };
    }

    private SourceLocationModel MapToInternalSourceLocationModel(Service.SourceLocationModel serviceModel)
    {
        SourceLocationModel sourceLocation = new SourceLocationModel
        {
            SourceLocationId = serviceModel.FtsSourceLocationId.ToString(),
            ConfigId = serviceModel.ConfigId,
            ConfigName = serviceModel.ConfigName ?? string.Empty,
            Description = serviceModel.Description,
            FtpServerId = serviceModel.FtpServerId,
            FtpServerName = serviceModel.FtpServerName,
            Address = serviceModel.Address,
            OwnerName = serviceModel.OwnerName,
            OwnerEmail = serviceModel.OwnerEmail,
            OwnerSubsystem = serviceModel.OwnerSubsystem,
            OwnerPhoneNumber = serviceModel.OwnerPhoneNumber,
            Protocol = serviceModel.Protocol,
            IsActive = serviceModel.IsActive,
        };

        this.LocalizeObjectProperties(sourceLocation);

        return sourceLocation;
    }

    private string GetModuleName(string moduleid)
    {
        // gap: legacy resolved module name from reference data; BFF has no equivalent.
        _ = moduleid;
        return string.Empty;
    }

    private string? GetFTPServerName(string ftpServerId)
    {
        string? ftpServerName = null;

        if (this.ftpServers == null)
        {
            this.ftpServers = new List<ListItemModel>();

            RequestModel<SearchCriteriaModel> request = new RequestModel<SearchCriteriaModel>()
            {
                Requestor = this.CreateRequestorFromContext(),
                Data = null,
            };

            ResponseModel<List<FtpServerModel>> resp = this.GetFtpServers(request);
            ResultModel resultModel = resp.Result;

            if (resultModel.StatusCode == CoreEnumerations.Messaging.StatusCodeType.OK && resp.Data != null)
            {
                foreach (var dataList in resp.Data)
                {
                    ListItemModel listItem = new ListItemModel()
                    {
                        DisplayFormat = CoreEnumerations.Views.DataListDisplayFormatType.Name,
                        Name = dataList.FtpServerName,
                        ID = dataList.FtpServerId?.ToString()
                    };

                    this.ftpServers.Add(listItem);
                }
            }
        }

        foreach (ListItemModel ftpServer in this.ftpServers)
        {
            if (ftpServerId.Equals(ftpServer.ID))
            {
                ftpServerName = ftpServer.Name;
                break;
            }
        }

        return ftpServerName;
    }

    private RequestorModel CreateRequestorFromContext()
    {
        string? subject = _httpContextAccessor.HttpContext?.User?.FindFirst(new string(new[] { 's', 'u', 'b' }))?.Value;
        return new RequestorModel { IdentifierId = subject };
    }

    // F01-044: initializes response as OK + empty list, then populates via GetFTPServersResponse and maps faults to Error.
    public virtual ResponseModel<List<FtpServerModel>> GetFTPServerCredentials(RequestModel<FtpServerModel> request, SearchCriteriaModel searchCriteria)
    {
        ResponseModel<List<FtpServerModel>> response = this.BuildResponseModel<FtpServerModel>();

        try
        {
            this.GetFTPServersResponse(request, searchCriteria, response);
        }
        catch (FaultException<Service.ServiceException> ex)
        {
            response.Result = this.HandleFaultException(ex, response.Result);
        }
        catch (Exception ex)
        {
            response.Result = this.HandleException(ex);
        }

        return response;
    }

    public ResponseModel<TrackingDetailModel> GetFileTransferTrackingDetail(RequestModel<string> request)
    {
        ResponseModel<TrackingDetailModel> response = new ResponseModel<TrackingDetailModel>();
        response.Result = new ResultModel();
        response.Result.Initialize();

        string fileId = request.Data!;
        Service.FileTransferTrackingQuery trackingQuery = new Service.FileTransferTrackingQuery()
        {
            Requestor = this.MapToServiceRequestor(request.Requestor),
            Where = new Service.FileTransferTrackingQueryParms()
            {
                FileId = fileId
            }
        };

        try
        {
            Service.FileTransferTrackingQuery serviceResponse = _serviceProxy.SearchFileTransferMovement(trackingQuery);

            if (serviceResponse.Results != null && serviceResponse.Results.Count > 0)
            {
                response.Data = this.MapToInternalFileTransferTrackingDetailModel(serviceResponse.Results[0]);
            }

            response.Result.StatusCode = CoreEnumerations.Messaging.StatusCodeType.OK;
        }
        catch (FaultException<Service.ServiceException> ex)
        {
            _logger.LogError(ex, ex.Message);
            response.Result = this.FormatErrorResponseResult(ex.Detail.ErrorCode?.ToString() ?? string.Empty);
            response.Result.StatusCode = CoreEnumerations.Messaging.StatusCodeType.Error;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            response.Result = this.FormatErrorResponseResult(ex);
        }

        return response;
    }

    public ResponseModel<List<TrackingSearchResultModel>> SearchFileTransferTracking(RequestModel<SearchCriteriaModel> request)
    {
        //Initialize response
        ResponseModel<List<TrackingSearchResultModel>> response =
           new ResponseModel<List<TrackingSearchResultModel>>();
        response.Result = new ResultModel();
        response.Result.Initialize();

        SearchCriteriaModel searchCriteria = request.Data!;
        Service.FileTransferTrackingQuery trackingQuery = new Service.FileTransferTrackingQuery()
        {
            Requestor = this.MapToServiceRequestor(request.Requestor),
            CurrentPage = this.BuildServicePagingCriteria(searchCriteria.PagingCriteria),
            PageSize = this.BuildServicePageSizeCriteria(searchCriteria.PagingCriteria),
            SortCriteria = this.BuildServiceSortCriteria(searchCriteria.SortCriteria),
            Where = this.BuildWhereQueryParms(searchCriteria.SearchFields)
        };

        try
        {
            Service.FileTransferTrackingQuery serviceResponse = _serviceProxy.SearchFileTransferMovement(trackingQuery);

            if (serviceResponse.Results != null && serviceResponse.Results.Count > 0)
            {
                response.Data = new List<TrackingSearchResultModel>();
                foreach (Service.FileTransferTrackingModel item in serviceResponse.Results)
                {
                    response.Data.Add(this.MapToInternalFileTransferTrackingResultModel(item));
                }

                response.PagingResults = new PagingResultModel();
                response.PagingResults.RowCount = response.Data.Count;
                response.PagingResults.TotalRowsAvailable = serviceResponse.RowCount;
                response.Result.StatusCode = CoreEnumerations.Messaging.StatusCodeType.OK;
            }
        }
        catch (FaultException<Service.ServiceException> ex)
        {
            _logger.LogError(ex, ex.Message);
            response.Result = this.FormatErrorResponseResult(ex.Detail.ErrorCode?.ToString() ?? string.Empty);
            response.Result.StatusCode = CoreEnumerations.Messaging.StatusCodeType.Error;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            response.Result = this.FormatErrorResponseResult(ex);
        }

        return response;
    }

    public ResponseModel<FileTransferConfigurationModel> GetFileTransferConfigurationInformation(RequestModel<string> request)
    {
        ResponseModel<FileTransferConfigurationModel> response = new ResponseModel<FileTransferConfigurationModel>()
        {
            Data = new FileTransferConfigurationModel(),
            Result = new ResultModel()
            {
                StatusCode = CoreEnumerations.Messaging.StatusCodeType.NotFound
            }
        };

        string ftsSourceLocationId = request.Data!;

        Service.GetSourceLocationQuery serviceQuery =
            new Service.GetSourceLocationQuery()
            {
                Where = new Service.GetSourceLocationQueryParms() { SourceLocationId = ftsSourceLocationId },
                Requestor = this.MapToServiceRequestor(request.Requestor)
            };

        try
        {
            Service.GetSourceLocationQuery serviceResponse = _serviceProxy.GetSourceLocations(serviceQuery);

            if (serviceResponse.Results != null && serviceResponse.Results.Count > 0)
            {
                response.Data = this.MapToInternalFileTransferConfigurationModel(serviceResponse.Results[0]);

                if (serviceResponse.Results[0].DestinationAddress != null)
                {
                    int lineNumber = 1;
                    foreach (Service.FtsDestinationLocationAdded item in serviceResponse.Results[0].DestinationAddress!)
                    {
                        response.Data.DestinationDetails.Add(this.MapToInternalDestinationModel(item, lineNumber));
                        lineNumber++;
                    }
                }

                response.Result.StatusCode = CoreEnumerations.Messaging.StatusCodeType.OK;
            }
        }
        catch (FaultException<Service.ServiceException> ex)
        {
            _logger.LogError(ex, ex.Message);
            response.Result = this.FormatErrorResponseResult(ex.Detail.ErrorCode?.ToString() ?? string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            response.Result = this.FormatErrorResponseResult(ex);
        }

                    return response;
        }

    public Service.InboundTargetQuery GetServiceQuery(RequestModel<SearchCriteriaModel> request, string contractName)
    {
        Service.InboundTargetQuery serviceQuery = new Service.InboundTargetQuery()
        {
            Requestor = this.MapToServiceRequestor(request.Requestor),
            Where = new Service.InboundTargetQueryParms()
            {
                ContractName = contractName,
                FTEMetadataId = Guid.Empty
            }
        };

        return serviceQuery;
    }

    public ResponseModel<List<SourceLocationModel>> GetSourceLocations(RequestModel<string> request)
    {
        // Initialize response
        ResponseModel<List<SourceLocationModel>> response = new ResponseModel<List<SourceLocationModel>>()
        {
            Data = new List<SourceLocationModel>(),
            Result = new ResultModel()
            {
                StatusCode = CoreEnumerations.Messaging.StatusCodeType.OK
            }
        };

        Service.SourceLocationQuery serviceQuery = new Service.SourceLocationQuery()
        {
            Requestor = this.MapToServiceRequestor(request.Requestor),
            Where = new Service.SourceLocationQueryParms() { IsActive = true }
        };

        try
        {
            Service.SourceLocationQuery serviceResponse = _serviceProxy.SearchSourceLocations(serviceQuery);

            // Check response
            if (serviceResponse.Results != null)
            {
                response.Data = new List<SourceLocationModel>();
                foreach (Service.SourceLocationModel sourceLocation in serviceResponse.Results)
                {
                    response.Data.Add(this.MapToInternalSourceLocationModel(sourceLocation));
                }

                response.Data.Sort((a1, a2) => a1.ConfigName.CompareTo(a2.ConfigName));
                response.Result.StatusCode = CoreEnumerations.Messaging.StatusCodeType.OK;
            }

            response.Result.StatusCode = CoreEnumerations.Messaging.StatusCodeType.OK;
        }
        catch (FaultException<Service.ServiceException> ex)
        {
            _logger.LogError(ex, ex.Message);
            response.Result = this.FormatErrorResponseResult(ex.Detail!.ErrorCode!.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            response.Result.StatusCode = CoreEnumerations.Messaging.StatusCodeType.Error;
            response.Result.Messages.Add(
                new ResultMessageModel(CoreEnumerations.Messaging.MessageType.Error, ex.Message));
        }

        return response;
    }

    private AddFileTransferConfigurationModel MapToInternalFileTransferModel(Service.SourceLocationModel serviceModel)
    {
        AddFileTransferConfigurationModel model = new AddFileTransferConfigurationModel
        {
            OwnerSubsystem = new ListItemModel { ID = Convert.ToString(serviceModel.OwnerSubsystem) },
            ProtocolType = new ListItemModel { ID = Convert.ToString(serviceModel.Protocol) },
            RunOnThisDay = new ListItemModel { ID = Convert.ToString(serviceModel.RunOnThisDay) },
            TimeWindowStart = new ListItemModel { ID = Convert.ToString(serviceModel.StartWindow) },
            TimeWindowEnd = new ListItemModel { ID = Convert.ToString(serviceModel.EndWindow) },
            SourceAddress = serviceModel.Address,
            SourceFileMask = serviceModel.Mask,
            IsActive = Convert.ToBoolean(serviceModel.IsActive),
        };

        string moduleName = this.GetModuleName(model.OwnerSubsystem.ID ?? string.Empty);
        model.OwnerSubsystem.Name = moduleName;

        model.Metadata = new MetadataModel()
        {
            IsActive = Convert.ToBoolean(serviceModel.IsActive),
        };

        model.Status = model.IsActive ? "Active" : "Inactive";

        this.LocalizeObjectProperties(model);

        return model;
    }

        private FileTransferConfigurationModel MapToInternalFileTransferConfigurationModel(Service.SourceLocationModel serviceModel)
    {
        FileTransferConfigurationModel model = new FileTransferConfigurationModel
        {
            FTPServerName = serviceModel.FtpServerName,
            SourceFileMask = serviceModel.Mask,
            SourceAddress = serviceModel.Address,
            ProtocolType = new ListItemModel { ID = serviceModel.Protocol },
            OwnerSubsystem = new ListItemModel { ID = Convert.ToString(serviceModel.OwnerSubsystem) },
            RunOnThisDay = new ListItemModel { ID = serviceModel.RunOnThisDay.ToString(), Name = serviceModel.RunOnThisDay.ToString() },
            TimeWindowStart = new ListItemModel { ID = serviceModel.StartWindow },
            TimeWindowEnd = new ListItemModel { ID = serviceModel.EndWindow },
            IsActive = Convert.ToBoolean(serviceModel.IsActive),
            RunOnLastDay = serviceModel.RunOnLastDay,
            RunOnFirstDay = serviceModel.RunOnFirstDay,
        };

        model.FileDirection.ID = serviceModel.FileDirection;

        // Legacy constants from FileTransferConstants.Localization.DataListsItems.
        model.RunSchedule.ID = model.RunOnLastDay
                                || model.RunOnFirstDay
                                || (!string.IsNullOrEmpty(model.RunOnThisDay.ID)
                                && model.RunOnThisDay.ID != "0")
                                ? "2"
                                : "1";

        model.Status = model.IsActive ? "Active" : "Inactive";

        string moduleName = this.GetModuleName(model.OwnerSubsystem.ID ?? string.Empty);
        model.OwnerSubsystem.Name = moduleName;

        model.Metadata = new MetadataModel()
        {
            IsActive = Convert.ToBoolean(serviceModel.IsActive),
        };

        this.LocalizeObjectProperties(model);
        return model;
    }

        private DestinationConfigurationModel MapToInternalDestinationModel(Service.FtsDestinationLocationAdded item, int lineNumber)
    {
        DestinationConfigurationModel result = new DestinationConfigurationModel
        {
            DestinationId = item.DestinationId.ToString(),
            FTEMetadataId = item.FTEMetadataId,
            ContractName = new ListItemModel { ID = item.ContractName },
            DestinationType = new ListItemModel { ID = item.DestinationType },
            DestinationProtocol = new ListItemModel { ID = item.DestinationProtocol },
            FTPServer = new ListItemModel { ID = item.FtpServerId.ToString(), Name = this.GetFTPServerName(item.FtpServerId.ToString()) },
            RenamePattern = new ListItemModel { ID = item.RenamePattern },
            OverwriteAtDestination = item.OverWriteAtDestination,
            CompressAtDestination = item.CompressAtDestination,
            RenameAtDestination = item.RenameAtDestination,
            DestinationIsArchive = item.DestinationArchive,
            Active = item.IsValid,
        };

        result.Metadata = new MetadataModel()
        {
            IsActive = item.IsValid
        };
        this.LocalizeObjectProperties(result);

        result.IsAddedUpdated = false;
        result.DestinationLineNumber = lineNumber;
        result.ImportJobName = item.ImportJobName;

        result.Status = result.Active ? "Active" : "Inactive";

        return result;
    }

    // MaintainabilityDetails(ApprovedIndex = 52, ReviewedOnSprint = 94, ReviewerName = Josue Chavez)
    private TrackingSearchResultModel MapToInternalFileTransferTrackingResultModel(Service.FileTransferTrackingModel serviceModel)
    {
        TrackingSearchResultModel model = new TrackingSearchResultModel
        {
            FileId = serviceModel.FileId,
            FileName = serviceModel.FileName,
            Direction = new ListItemModel { ID = serviceModel.FileDirection },
            TransportType = new ListItemModel { ID = serviceModel.FileTransportType },
            FileStatus = new ListItemModel { ID = serviceModel.FileStatus },
            CreateDate = serviceModel.CreatedDate,
            Size = serviceModel.FileSize,
            TradingPartnerId = serviceModel.TradingPartner
        };

        this.LocalizeObjectProperties(model);

        return model;
    }
    private TrackingDetailModel MapToInternalFileTransferTrackingDetailModel(Service.FileTransferTrackingModel serviceModel)
    {
        TrackingDetailModel model = new TrackingDetailModel()
        {
            Information = this.MapToInternalFileTransferTrackingInformationModel(serviceModel),
            Events = this.MapToInternalFileTransferTrackingEvents(serviceModel.Events),
            Exceptions = this.MapToInternalFileTransferTrackingExceptions(serviceModel.Exceptions)
        };

        return model;
    }

    // MaintainabilityDetails(ApprovedIndex = 50, ReviewedOnSprint = 94, ReviewerName = Josue Chavez)
    private TrackingInformationModel MapToInternalFileTransferTrackingInformationModel(Service.FileTransferTrackingModel serviceModel)
    {
        TrackingInformationModel model = new TrackingInformationModel
        {
            FileId = serviceModel.FileId,
            FileName = serviceModel.FileName,
            CreateDate = serviceModel.CreatedDate,
            Direction = new ListItemModel { ID = serviceModel.FileDirection },
            TransportType = new ListItemModel { ID = serviceModel.FileTransportType },
            FileStatus = new ListItemModel { ID = serviceModel.FileStatus },
            Size = serviceModel.FileSize,
            SourceLocation = (serviceModel.SourceLocConfigName ?? string.Empty) + " - " + (serviceModel.SourceLocAddress ?? string.Empty),
            TradingPartnerId = serviceModel.TradingPartner
        };

        this.LocalizeObjectProperties(model);

        return model;
    }

    private List<TrackingEventModel> MapToInternalFileTransferTrackingEvents(List<Service.FileEventModel> serviceEvents)
    {
        List<TrackingEventModel> trackingEvents = new List<TrackingEventModel>();

        foreach (Service.FileEventModel serviceEvent in serviceEvents)
        {
            TrackingEventModel trackingEvent = new TrackingEventModel
            {
                CreateDate = serviceEvent.CreatedDate,
                Description = serviceEvent.EventTrackDescription
            };

            this.LocalizeObjectProperties(trackingEvent);
            trackingEvents.Add(trackingEvent);
        }

        return trackingEvents;
    }

    private List<TrackingExceptionModel> MapToInternalFileTransferTrackingExceptions(List<Service.FileExceptionModel> serviceExceptions)
    {
        List<TrackingExceptionModel> trackingExceptions = new List<TrackingExceptionModel>();

        foreach (Service.FileExceptionModel serviceException in serviceExceptions)
        {
            TrackingExceptionModel trackingException = new TrackingExceptionModel
            {
                Date = serviceException.CreatedDate,
                ExceptionMessage = serviceException.ExceptionMessage
            };

            this.LocalizeObjectProperties(trackingException);
            trackingExceptions.Add(trackingException);
        }

        return trackingExceptions;
    }

    private void LocalizeObjectProperties<T>(T obj)
    {
        _ = obj;
        // shortcut: legacy localized UI strings; BFF returns raw values.
    }

    private List<Service.SortCriteria> BuildServiceSortCriteria(List<SortCriteriaModel>? sortCriteria)
    {
        List<Service.SortCriteria> serviceSortCriterias = new List<Service.SortCriteria>();

        if (sortCriteria != null && sortCriteria.Count > 0)
        {
            int i = 0;
            foreach (SortCriteriaModel sort in sortCriteria)
            {
                switch (sort.SortField)
                {
                    case "FileId":
                        serviceSortCriterias.Add(new Service.SortCriteria { FieldName = "FileId" });
                        serviceSortCriterias[i].Order = this.BuildServiceSortOrder(sort);
                        break;

                    case "FileName":
                        serviceSortCriterias.Add(new Service.SortCriteria { FieldName = "FileName" });
                        serviceSortCriterias[i].Order = this.BuildServiceSortOrder(sort);
                        break;

                    case "FileStatus.FormattedText":
                        serviceSortCriterias.Add(new Service.SortCriteria { FieldName = "FileStatus" });
                        serviceSortCriterias[i].Order = this.BuildServiceSortOrder(sort);
                        break;

                    case "Direction.FormattedText":
                        serviceSortCriterias.Add(new Service.SortCriteria { FieldName = "FileDirection" });
                        serviceSortCriterias[i].Order = this.BuildServiceSortOrder(sort);
                        break;

                    case "Size":
                        serviceSortCriterias.Add(new Service.SortCriteria { FieldName = "FileSize" });
                        serviceSortCriterias[i].Order = this.BuildServiceSortOrder(sort);
                        break;

                    case "CreateDate":
                        serviceSortCriterias.Add(new Service.SortCriteria { FieldName = "CreatedDate" });
                        serviceSortCriterias[i].Order = this.BuildServiceSortOrder(sort);
                        break;

                    case "TradingPartnerId":
                        serviceSortCriterias.Add(new Service.SortCriteria { FieldName = "TradingPartnerId" });
                        serviceSortCriterias[i].Order = this.BuildServiceSortOrder(sort);
                        break;
                }

                i++;
            }
        }

        return serviceSortCriterias;
    }

    private Service.SortOrder BuildServiceSortOrder(SortCriteriaModel sortModel)
    {
        Service.SortOrder order =
            (sortModel.SortDirection == CoreEnumerations.Messaging.SortDirectionType.Ascending) ? Service.SortOrder.Asc : Service.SortOrder.Desc;

        return order;
    }

    private int BuildServicePagingCriteria(PagingCriteriaModel? pagingCriteria)
    {
        int currentPage = 1;

        if (pagingCriteria != null)
        {
            currentPage = pagingCriteria.Page;
        }

        return currentPage;
    }

    private int BuildServicePageSizeCriteria(PagingCriteriaModel? pagingCriteria)
    {
        int rowsPerPage = 5;

        if (pagingCriteria != null)
        {
            rowsPerPage = pagingCriteria.RowsPerPage;
        }

        return rowsPerPage;
    }

    // F04-031: maps known fields; ignores invalid dates/sizes.
    private Service.FileTransferTrackingQueryParms BuildWhereQueryParms(List<SearchFieldModel>? searchFields)
    {
        Service.FileTransferTrackingQueryParms queryParms = new Service.FileTransferTrackingQueryParms();

        if (searchFields != null && searchFields.Count > 0)
        {
            foreach (SearchFieldModel searchField in searchFields)
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
                        DateTime fromDate;
                        if (DateTime.TryParse(searchField.FieldValue.ToString(), out fromDate))
                        {
                            queryParms.StartDate = fromDate;
                        }

                        break;

                    case "ToDate":
                        DateTime toDate;
                        if (DateTime.TryParse(searchField.FieldValue.ToString(), out toDate))
                        {
                            queryParms.EndDate = toDate;
                        }

                        break;

                    case "FromSize":
                        decimal fromSize;
                        if (decimal.TryParse(searchField.FieldValue.ToString(), out fromSize))
                        {
                            queryParms.FileSizeFrom = fromSize;
                        }

                        break;

                    case "ToSize":
                        decimal toSize;
                        if (decimal.TryParse(searchField.FieldValue.ToString(), out toSize))
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

    private Service.SourceLocationQueryParms BuildSourceLocationQueryParms(SearchCriteriaModel searchCriteria)
    {
        Service.SourceLocationQueryParms queryParms = new Service.SourceLocationQueryParms();

        if (searchCriteria.SearchFields != null && searchCriteria.SearchFields.Count > 0)
        {
            foreach (SearchFieldModel searchField in searchCriteria.SearchFields)
            {
                switch (searchField.FieldName)
                {
                    case "ConfigId":
                        queryParms.ConfigId = Convert.ToInt32(searchField.FieldValue);
                        break;

                    case "ConfigName":
                        queryParms.ConfigName = searchField.FieldValue.ToString();
                        break;

                    case "OwnerName":
                        queryParms.OwnerName = searchField.FieldValue.ToString();
                        break;

                    case "OwnerSubsystem":
                        queryParms.OwnerSubsystem = searchField.FieldValue.ToString();
                        break;

                    case "Protocol":
                        queryParms.Protocol = searchField.FieldValue.ToString();
                        break;

                    case "IsActive":
                        queryParms.IsActive = (bool)searchField.FieldValue;
                        break;
                }
            }
        }

        return queryParms;
    }

    private List<Service.SortCriteria> BuildSourceLocationSortParms(List<SortCriteriaModel> sortCriteria)
    {
        List<Service.SortCriteria> serviceSortCriterias = new List<Service.SortCriteria>();

        if (sortCriteria != null && sortCriteria.Count > 0)
        {
            foreach (SortCriteriaModel sort in sortCriteria)
            {
                switch (sort.SortField)
                {
                    case "ConfigId":
                        serviceSortCriterias.Add(new Service.SortCriteria
                        {
                            FieldName = "ConfigId",
                            Order = this.BuildServiceSortOrder(sort)
                        });
                        break;

                    case "ConfigName":
                        serviceSortCriterias.Add(new Service.SortCriteria
                        {
                            FieldName = "ConfigName",
                            Order = this.BuildServiceSortOrder(sort)
                        });
                        break;

                    case "SourceAddress":
                        serviceSortCriterias.Add(new Service.SortCriteria
                        {
                            FieldName = "SourceAddress",
                            Order = this.BuildServiceSortOrder(sort)
                        });
                        break;

                    case "SourceFileMask":
                        serviceSortCriterias.Add(new Service.SortCriteria
                        {
                            FieldName = "SourceFileMask",
                            Order = this.BuildServiceSortOrder(sort)
                        });
                        break;

                    case "OwnerName":
                        serviceSortCriterias.Add(new Service.SortCriteria
                        {
                            FieldName = "OwnerName",
                            Order = this.BuildServiceSortOrder(sort)
                        });
                        break;

                    case "OwnerSubsystem.Name":
                        serviceSortCriterias.Add(new Service.SortCriteria
                        {
                            FieldName = "ModuleName",
                            Order = this.BuildServiceSortOrder(sort)
                        });
                        break;

                    case "Status":
                        serviceSortCriterias.Add(new Service.SortCriteria
                        {
                            FieldName = "Status",
                            Order = this.BuildServiceSortOrder(sort)
                        });
                        break;

                    default:
                        serviceSortCriterias.Add(new Service.SortCriteria
                        {
                            FieldName = "ConfigId",
                            Order = Service.SortOrder.Asc
                        });
                        break;
                }
            }
        }

        if (serviceSortCriterias.Count == 0)
        {
            serviceSortCriterias.Add(new Service.SortCriteria()
            {
                FieldName = "ConfigId",
                Order = Service.SortOrder.Asc
            });
        }

        return serviceSortCriterias;
    }

    // F01-044: build response defaults.
    private ResponseModel<List<TFtpServerModel>> BuildResponseModel<TFtpServerModel>()
    {
        return new ResponseModel<List<TFtpServerModel>>()
        {
            Data = new List<TFtpServerModel>(),
            PagingResults = new PagingResultModel()
            {
                RowCount = 0,
                TotalRowsAvailable = 0
            },
            Result = new ResultModel()
            {
                StatusCode = CoreEnumerations.Messaging.StatusCodeType.OK,
                Messages = new List<ResultMessageModel>()
            }
        };
    }

    // F01-045: password removal defaults to true when missing.
    private ResponseModel<List<FtpServerModel>> GetFTPServersResponse(RequestModel<FtpServerModel> request, SearchCriteriaModel searchCriteria, ResponseModel<List<FtpServerModel>> response)
    {
        Service.FtpServerQuery queryResults = this.RunService(this.BuildQuery(request, searchCriteria), _serviceProxy);

        if (queryResults != null && queryResults.Results != null)
        {
            bool? removePassword = request.Data!.RemovePassword.HasValue ? request.Data.RemovePassword : true;
            this.PopulateResponseResults(response, queryResults, (bool)removePassword);
        }

        return response;
    }

    // F01-046/F01-047/F01-048: where + paging + sort translation.
    private Service.FtpServerQuery BuildQuery(RequestModel<FtpServerModel> request, SearchCriteriaModel searchCriteria)
    {
        Service.FtpServerQuery query = new Service.FtpServerQuery()
        {
            PageSize = 1,
            Where = new Service.FtpServerQueryParms()
            {
                FtpServerId = request.Data!.FtpServerId != null ? new Guid(request.Data.FtpServerId) : Guid.Empty,
                FtpServerName = request.Data.FtpServerName,
                FtpDescription = request.Data.FtpDescription,
                FtpUser = request.Data.FtpUser,
                IsValid = request.Data.IsValid,
                SearchOptions = this.BuildSearchOptions(searchCriteria)
            },
            Requestor = this.MapToServiceRequestor(request.Requestor),
            Results = new List<Service.FtpServerModel>(),
        };

        this.SetFTPServersPaging(searchCriteria, query);

        return query;
    }

    private List<Service.SearchModeOption> BuildSearchOptions(SearchCriteriaModel searchCriteria)
    {
        List<Service.SearchModeOption> searchOptions = new List<Service.SearchModeOption>();

        if (searchCriteria != null && searchCriteria.SearchFields != null)
        {
            foreach (SearchFieldModel searchField in searchCriteria.SearchFields)
            {
                searchOptions.Add(new Service.SearchModeOption()
                {
                    FieldName = searchField.FieldName,
                    FilterType = this.GetSearchType(searchField.SearchMode)
                });
            }
        }

        return searchOptions;
    }

    private Service.SearchModeOption.SearchMode GetSearchType(CoreEnumerations.Messaging.SearchModeType searchMode)
    {
        switch (searchMode)
        {
            case CoreEnumerations.Messaging.SearchModeType.Contains:
                return Service.SearchModeOption.SearchMode.Contains;
            case CoreEnumerations.Messaging.SearchModeType.EndsWith:
                return Service.SearchModeOption.SearchMode.EndsWith;
            case CoreEnumerations.Messaging.SearchModeType.StartsWith:
                return Service.SearchModeOption.SearchMode.StartsWith;
            case CoreEnumerations.Messaging.SearchModeType.EqualTo:
            default:
                return Service.SearchModeOption.SearchMode.EqualTo;
        }
    }

    // F01-047/F01-048.
    private void SetFTPServersPaging(SearchCriteriaModel searchCriteria, Service.FtpServerQuery query)
    {
        if (searchCriteria != null)
        {
            searchCriteria.SortCriteria = searchCriteria.SortCriteria ?? new List<SortCriteriaModel>();

            if (searchCriteria.SortCriteria.Count > 0)
            {
                query.SortCriteria = this.SetSortCriteria(searchCriteria);
            }

            searchCriteria.PagingCriteria = searchCriteria.PagingCriteria ?? new PagingCriteriaModel();
            query.PageSize = searchCriteria.PagingCriteria.RowsPerPage;
            query.CurrentPage = searchCriteria.PagingCriteria.Page;
        }
    }

    // F01-047.
    private List<Service.SortCriteria> SetSortCriteria(SearchCriteriaModel searchCriteria)
    {
        return new List<Service.SortCriteria>()
        {
            new Service.SortCriteria()
            {
                FieldName = searchCriteria.SortCriteria![searchCriteria.SortCriteria.Count - 1].SortField,
                Order = searchCriteria.SortCriteria[searchCriteria.SortCriteria.Count - 1].SortDirection.ToString().Contains(Service.SortOrder.Asc.ToString()) ?
                        Service.SortOrder.Asc
                        : Service.SortOrder.Desc
            }
        };
    }

    private void PopulateResponseResults(ResponseModel<List<FtpServerModel>> response, Service.FtpServerQuery queryResults, bool removePassord)
    {
        response.PagingResults!.RowCount = queryResults.Results!.Count;
        response.PagingResults.TotalRowsAvailable = queryResults.RowCount;
        response.Data = this.MapResults(queryResults.Results, removePassord);
    }

    private List<FtpServerModel> MapResults(List<Service.FtpServerModel> results, bool removePassord)
    {
        List<FtpServerModel> serversList = new List<FtpServerModel>();

        foreach (Service.FtpServerModel server in results)
        {
            this.MapFTPServerModel(serversList, server, removePassord);
        }

        return serversList;
    }

    private void MapFTPServerModel(List<FtpServerModel> serversList, Service.FtpServerModel serverRecord, bool removePassword)
    {
        FtpServerModel model = new FtpServerModel
        {
            FtpServerId = serverRecord.FtpServerId.ToString(),
            FtpServerName = serverRecord.FtpServerName,
            FtpUser = serverRecord.FtpUser,
            FtpPassword = serverRecord.FtpPassword,
            FtpServerIp = serverRecord.FtpServerIp,
            FtpDescription = serverRecord.FtpDescription,
            FtpPort = serverRecord.FtpPort,
            FtpTransferMode = serverRecord.FtpTransferMode,
            IsValid = serverRecord.IsValid,
            FtpProxyAddress = serverRecord.FtpProxyAddress,
            FtpProxyPort = serverRecord.FtpProxyPort,
            CreationDate = serverRecord.CreationDate,
            LastModifiedDate = serverRecord.LastModifiedDate,
            OperatorId = serverRecord.OperatorId,
        };

        model.FtpPassword = removePassword ? string.Empty : model.FtpPassword;

        serversList.Add(model);
    }

    public virtual Service.FtpServerQuery RunService(Service.FtpServerQuery query, Service.IFileTransferSvcQueryService serviceProxy)
    {
        return serviceProxy.GetFtpServers(query);
    }

    private ResultModel HandleFaultException(FaultException<Service.ServiceException> ex, ResultModel result)
    {
        _logger.LogError(ex, ex.Message);
        result.StatusCode = CoreEnumerations.Messaging.StatusCodeType.Error;
        if (ex.Detail.ErrorMessages != null)
        {
            foreach (string msg in ex.Detail.ErrorMessages)
            {
                result.Messages.Add(new ResultMessageModel(CoreEnumerations.Messaging.MessageType.Error, msg));
            }
        }

        if (!string.IsNullOrEmpty(ex.Message))
        {
            result.Messages.Add(new ResultMessageModel(CoreEnumerations.Messaging.MessageType.Error, ex.Message));
        }

        return result;
    }

    private ResultModel HandleException(Exception ex)
    {
        _logger.LogError(ex, ex.Message);
        ResultModel result = this.FormatErrorResponseResult(ex);

        return result;
    }

    private ResultModel FormatErrorResponseResult(string errorCode)
    {
        var result = new ResultModel
        {
            StatusCode = CoreEnumerations.Messaging.StatusCodeType.Error,
            Messages = new List<ResultMessageModel>()
        };

        if (!string.IsNullOrWhiteSpace(errorCode))
        {
            result.Messages.Add(new ResultMessageModel(CoreEnumerations.Messaging.MessageType.Error, errorCode));
        }

        return result;
    }

    private ResultModel FormatErrorResponseResult(Exception ex)
    {
        return new ResultModel
        {
            StatusCode = CoreEnumerations.Messaging.StatusCodeType.Error,
            Messages = new List<ResultMessageModel>
            {
                new ResultMessageModel(CoreEnumerations.Messaging.MessageType.Error, ex.Message)
            }
        };
    }

    private Service.RequestorModel MapToServiceRequestor(RequestorModel? internalRequestor)
    {
        return new Service.RequestorModel
        {
            IdentifierId = internalRequestor?.IdentifierId
        };
    }
}


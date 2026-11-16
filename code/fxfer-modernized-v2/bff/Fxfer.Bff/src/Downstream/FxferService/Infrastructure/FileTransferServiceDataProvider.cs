using Fxfer.Bff.Downstream.FxferService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Fxfer.Bff.Downstream.FxferService;

public partial class FileTransferServiceDataProvider
{
    private readonly FileTransferConfig.IFileTransferSvcService _serviceProxy;
    private readonly FileTransferQueryServiceDataProvider _queryDataProvider;
    private readonly ILogger<FileTransferServiceDataProvider> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    private FileTransferConfig.AddFtpServerCommand? addFtpServerCommand;
    private FileTransferConfig.UpdateFtpServerCommand? updateFtpServerCommand;
    private FileTransferConfig.InvalidateFtpServerCommand? invalidateFtpServerCommand;

    public FileTransferServiceDataProvider(
        FileTransferConfig.IFileTransferSvcService serviceProxy,
        FileTransferQueryServiceDataProvider queryDataProvider,
        ILogger<FileTransferServiceDataProvider> logger,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
    {
        _serviceProxy = serviceProxy;
        _queryDataProvider = queryDataProvider;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public ResponseModel<FileTransferConfigurationModel> AddFileTransferBaseConfiguration(RequestModel<FileTransferConfigurationModel> request)
    {
        ResponseModel<FileTransferConfigurationModel> response = new ResponseModel<FileTransferConfigurationModel>()
        {
            Data = null,
            Result = new ResultModel()
            {
                StatusCode = CoreEnumerations.Messaging.StatusCodeType.Error
            }
        };

        string externalFTECommonFolder = _configuration[string.Concat(
            FileTransferConstants.ModuleName,
            new string(new[] { ':' }),
            FileTransferConstants.ApplicationSettings.ExternalFTECommonFolder)] ?? string.Empty;

        int destCount = request.Data!.DestinationDetails.Count;

        FileTransferConfig.FtsDestinationLocationAdded[] destArray = new FileTransferConfig.FtsDestinationLocationAdded[destCount];
        int i = 0;
        foreach (DestinationConfigurationModel destination in request.Data.DestinationDetails)
        {
            FileTransferConfig.FtsDestinationLocationAdded destinationTempData = new FileTransferConfig.FtsDestinationLocationAdded();

            destinationTempData.CompressAtDestination = destination.CompressAtDestination;
            destinationTempData.ContactName = destination.ContactName;
            destinationTempData.ContactEmail = destination.ContactEmail;
            if (destination.ContactPhone != null)
            {
                // F02-023: strip '-' from contact phone.
                destinationTempData.ContactPhone = destination.ContactPhone.Replace(new string(new[] { '-' }), string.Empty);
            }

            // F02-023: FileToEvent destination overrides destination address.
            if (destination.DestinationType.ID == FileTransferConstants.Miscellaneous.FileToEvent)
            {
                destination.DestinationAddress = externalFTECommonFolder;
            }

            destinationTempData.DestinationAddress = destination.DestinationAddress;
            destinationTempData.DestinationArchive = destination.DestinationIsArchive;
            destinationTempData.DestinationType = destination.DestinationType.ID;
            destinationTempData.DestinationProtocol = destination.DestinationProtocol.ID;
            destinationTempData.FTEMetadataId = destination.FTEMetadataId;
            destinationTempData.ContractName = destination.ContractName.ID;

            // F02-024: empty FTPServer.ID maps to Guid.Empty; IsValid comes from Active.
            destinationTempData.FtpServerId = destination.FTPServer.ID == string.Empty ? Guid.Empty : Guid.Parse(destination.FTPServer.ID!);
            destinationTempData.IsValid = destination.Active;
            destinationTempData.OverWriteAtDestination = destination.OverwriteAtDestination;
            destinationTempData.RenameAtDestination = destination.RenameAtDestination;
            destinationTempData.RenamePattern = destination.RenamePattern.ID;
            destinationTempData.OperatorId = this.GetOperatorId() ?? string.Empty;

            destArray[i] = destinationTempData;
            i++;
        }

        FileTransferConfig.FtsSourceLocationAddCommand cmd = new FileTransferConfig.FtsSourceLocationAddCommand
        {
            Requestor = this.MapToServiceRequestor(request.Requestor),
            ConfigName = request.Data.ConfigName,
            Description = request.Data.Description,
            FtpServerId = request.Data.FTPServerId,
            Protocol = request.Data.ProtocolType.ID,
            Address = request.Data.SourceAddress,
            Mask = request.Data.SourceFileMask,
            FileDirection = request.Data.FileDirection.ID,
            SendNotifications = request.Data.SendNotification,
            IsValid = request.Data.IsActive,
            OwnerName = request.Data.OwnerName,
            OwnerModule = request.Data.OwnerSubsystemId,
            OwnerPhone = request.Data.OwnerPhoneNumber,
            OwnerEmail = request.Data.OwnerEmail,
            RunOnMonday = request.Data.RunOnMonday,
            RunOnTuesday = request.Data.RunOnTuesday,
            RunOnWed = request.Data.RunOnWednesday,
            RunOnThrus = request.Data.RunOnThursday,
            RunOnFri = request.Data.RunOnFriday,
            RunOnSat = request.Data.RunOnSaturday,
            RunOnSun = request.Data.RunOnSunday,
            RunOnFrstDayOfMonthInd = request.Data.RunOnFirstDay,
            RunOnLastDayOfMonthInd = request.Data.RunOnLastDay,

            // F02-025: day-of-month defaults to 0 when the ID is empty.
            RunOnThisDayOfMonth = request.Data.RunOnThisDay.ID != string.Empty ? Convert.ToByte(request.Data.RunOnThisDay.ID) : Convert.ToByte(0),
            StartTime = request.Data.TimeWindowStart.ID!.ToString(),
            EndTime = request.Data.TimeWindowEnd.ID!.ToString(),
            TradingPartner = request.Data.TradingPartner,
            OperatorId = this.GetOperatorId() ?? string.Empty,
            DestinationAddress = destArray,
            IsSuspended = request.Data.Suspended,
            // F02-025: IsSuspendedChangedDate is set only when Suspended is true.
            IsSuspendedChangedDate = request.Data.Suspended ? (DateTime?)DateTime.UtcNow : null,
        };

        try
        {
            // F02-026: call backend FileTransferAdd and map faults/exceptions into Error responses.
            FileTransferConfig.FtsSourceLocationAdded serviceResponse = _serviceProxy.FileTransferAdd(cmd);

            if (serviceResponse != null)
            {
                response = this.GenerateReponse<FileTransferConfigurationModel>(
                    this.MapToInternalProvider(serviceResponse),
                    CoreEnumerations.Messaging.StatusCodeType.OK);
            }
        }
        catch (FaultException<FileTransferConfig.ServiceException> ex)
        {
            response = this.HandleServiceException<FileTransferConfigurationModel>(ex);
        }
        catch (FaultException<FileTransferConfig.BusinessValidationException> ex)
        {
            response = this.HandleBusinessException<FileTransferConfigurationModel>(ex);
        }
        catch (Exception ex)
        {
            response = this.HandleException<FileTransferConfigurationModel>(ex);
        }

        return response;
    }

    public ResponseModel<FileTransferConfigurationModel> UpdateFileTransferBaseConfiguration(RequestModel<FileTransferConfigurationModel> request)
    {
        ResponseModel<FileTransferConfigurationModel> response = new ResponseModel<FileTransferConfigurationModel>()
        {
            Data = null,
            Result = new ResultModel()
            {
                StatusCode = CoreEnumerations.Messaging.StatusCodeType.Error
            }
        };

        string externalFTECommonFolder = _configuration[string.Concat(
            FileTransferConstants.ModuleName,
            new string(new[] { ':' }),
            FileTransferConstants.ApplicationSettings.ExternalFTECommonFolder)] ?? string.Empty;

        int destCount = request.Data!.DestinationDetails.Where(x => x.IsAddedUpdated == true).Count();

        FileTransferConfig.FtsDestinationLocationAdded[] destArray = new FileTransferConfig.FtsDestinationLocationAdded[destCount];
        int i = 0;
        foreach (DestinationConfigurationModel destination in request.Data.DestinationDetails)
        {
            if (destination.IsAddedUpdated == true)
            {
                FileTransferConfig.FtsDestinationLocationAdded destinationTempData = new FileTransferConfig.FtsDestinationLocationAdded();
                if (destination.DestinationId != null)
                {
                    destinationTempData.DestinationId = Guid.Parse(destination.DestinationId);
                }

                destinationTempData.CompressAtDestination = destination.CompressAtDestination;
                destinationTempData.ContactName = destination.ContactName;
                destinationTempData.ContactEmail = destination.ContactEmail;
                if (destination.ContactPhone != null)
                {
                    destinationTempData.ContactPhone = destination.ContactPhone.Replace(new string(new[] { '-' }), string.Empty);
                }

                destinationTempData.ContractName = destination.ContractName.ID;

                if (destination.DestinationType.ID == FileTransferConstants.Miscellaneous.FileToEvent)
                {
                    destination.DestinationAddress = externalFTECommonFolder;
                }

                destinationTempData.DestinationAddress = destination.DestinationAddress;
                destinationTempData.DestinationArchive = destination.DestinationIsArchive;
                destinationTempData.DestinationProtocol = destination.DestinationProtocol.ID;
                destinationTempData.DestinationType = destination.DestinationType.ID;
                destinationTempData.FTEMetadataId = destination.FTEMetadataId;
                destinationTempData.FtpServerId = destination.FTPServer.ID == string.Empty ? Guid.Empty : Guid.Parse(destination.FTPServer.ID!);
                destinationTempData.IsValid = destination.Active;
                destinationTempData.OverWriteAtDestination = destination.OverwriteAtDestination;
                destinationTempData.RenameAtDestination = destination.RenameAtDestination;
                destinationTempData.RenamePattern = destination.RenamePattern.ID;
                destinationTempData.OperatorId = this.GetOperatorId() ?? string.Empty;

                destArray[i] = destinationTempData;
                i++;
            }
        }

        FileTransferConfig.FtsSourceLocationUpdateCommand cmd = new FileTransferConfig.FtsSourceLocationUpdateCommand
        {
            ConfigId = request.Data.ConfigId,
            Requestor = this.MapToServiceRequestor(request.Requestor),
            ConfigName = request.Data.ConfigName,
            Description = request.Data.Description,
            FtpServerId = request.Data.FTPServerId,
            Protocol = request.Data.ProtocolType.ID,
            Address = request.Data.SourceAddress,
            Mask = request.Data.SourceFileMask,
            FileDirection = request.Data.FileDirection.ID,
            SendNotifications = request.Data.SendNotification,
            IsValid = request.Data.IsActive,
            OwnerName = request.Data.OwnerName,
            OwnerModule = request.Data.OwnerSubsystemId,
            OwnerPhone = request.Data.OwnerPhoneNumber,
            OwnerEmail = request.Data.OwnerEmail,
            RunOnMonday = request.Data.RunOnMonday,
            RunOnTuesday = request.Data.RunOnTuesday,
            RunOnWed = request.Data.RunOnWednesday,
            RunOnThrus = request.Data.RunOnThursday,
            RunOnFri = request.Data.RunOnFriday,
            RunOnSat = request.Data.RunOnSaturday,
            RunOnSun = request.Data.RunOnSunday,
            RunOnFrstDayOfMonthInd = request.Data.RunOnFirstDay,
            RunOnLastDayOfMonthInd = request.Data.RunOnLastDay,
            RunOnThisDayOfMonth = request.Data.RunOnThisDay.ID != string.Empty ? Convert.ToByte(request.Data.RunOnThisDay.ID) : Convert.ToByte(0),
            StartTime = request.Data.TimeWindowStart.ID!.ToString(),
            EndTime = request.Data.TimeWindowEnd.ID!.ToString(),
            TradingPartner = request.Data.TradingPartner,
            OperatorId = this.GetOperatorId() ?? string.Empty,
            DestinationAddress = destArray,
            FtsSourceLocationId = request.Data.FtsSourceLocationId,
            IsSuspended = request.Data.Suspended,
            IsSuspendedChangedDate = request.Data.Suspended ? (DateTime?)DateTime.UtcNow : null
        };

        try
        {
            FileTransferConfig.FtsSourceLocationUpdated serviceResponse = _serviceProxy.UpdateFileTransferSourceLocation(cmd);

            if (serviceResponse != null)
            {
                response = this.GenerateReponse<FileTransferConfigurationModel>(
                    request.Data,
                    CoreEnumerations.Messaging.StatusCodeType.OK);
            }
        }
        catch (FaultException<FileTransferConfig.ServiceException> ex)
        {
            response = this.HandleServiceException<FileTransferConfigurationModel>(ex);
        }
        catch (FaultException<FileTransferConfig.BusinessValidationException> ex)
        {
            response = this.HandleBusinessException<FileTransferConfigurationModel>(ex);
        }
        catch (Exception ex)
        {
            response = this.HandleException<FileTransferConfigurationModel>(ex);
        }

        return response;
    }

    public ResponseModel<FileTransferConfigurationModel> InvalidateFileTransferBaseConfiguration(RequestModel<FileTransferConfigurationModel> request)
    {
        ResponseModel<FileTransferConfigurationModel> response = new ResponseModel<FileTransferConfigurationModel>()
        {
            Data = null,
            Result = new ResultModel()
            {
                StatusCode = CoreEnumerations.Messaging.StatusCodeType.Error
            }
        };

        int destCount = request.Data!.DestinationDetails.Where(x => x.IsAddedUpdated == true).Count();

        FileTransferConfig.FtsDestinationLocationAdded[] destArray = new FileTransferConfig.FtsDestinationLocationAdded[destCount];
        int i = 0;
        foreach (DestinationConfigurationModel destination in request.Data.DestinationDetails)
        {
            if (destination.IsAddedUpdated == true)
            {
                FileTransferConfig.FtsDestinationLocationAdded destinationTempData = new FileTransferConfig.FtsDestinationLocationAdded();
                if (destination.DestinationId != null)
                {
                    destinationTempData.DestinationId = Guid.Parse(destination.DestinationId);
                }

                destinationTempData.CompressAtDestination = destination.CompressAtDestination;
                destinationTempData.ContactName = destination.ContactName;
                destinationTempData.ContactEmail = destination.ContactEmail;
                if (destination.ContactPhone != null)
                {
                    destinationTempData.ContactPhone = destination.ContactPhone.Replace(new string(new[] { '-' }), string.Empty);
                }

                destinationTempData.ContractName = destination.ContractName.ID;
                destinationTempData.DestinationAddress = destination.DestinationAddress;
                destinationTempData.DestinationArchive = destination.DestinationIsArchive;
                destinationTempData.DestinationProtocol = destination.DestinationProtocol.ID;
                destinationTempData.DestinationType = destination.DestinationType.ID;
                destinationTempData.FTEMetadataId = destination.FTEMetadataId;
                destinationTempData.FtpServerId = destination.FTPServer.ID == string.Empty ? Guid.Empty : Guid.Parse(destination.FTPServer.ID!);
                destinationTempData.IsValid = destination.Active;
                destinationTempData.OverWriteAtDestination = destination.OverwriteAtDestination;
                destinationTempData.RenameAtDestination = destination.RenameAtDestination;
                destinationTempData.RenamePattern = destination.RenamePattern.ID;
                destinationTempData.OperatorId = this.GetOperatorId() ?? string.Empty;

                destArray[i] = destinationTempData;
                i++;
            }
        }

        FileTransferConfig.FtsSourceLocationInvalidateCommand cmd = new FileTransferConfig.FtsSourceLocationInvalidateCommand
        {
            ConfigId = request.Data.ConfigId,
            Requestor = this.MapToServiceRequestor(request.Requestor),
            OperatorId = this.GetOperatorId() ?? string.Empty,
            FtsSourceLocationId = request.Data.FtsSourceLocationId,
            DestinationAddress = destArray
        };

        try
        {
            _ = _serviceProxy.InvalidateFileTransferSourceLocation(cmd);

            response = this.GenerateReponse<FileTransferConfigurationModel>(
                request.Data,
                CoreEnumerations.Messaging.StatusCodeType.OK);
        }
        catch (FaultException<FileTransferConfig.ServiceException> ex)
        {
            response = this.HandleServiceException<FileTransferConfigurationModel>(ex);
        }
        catch (Exception ex)
        {
            response = this.HandleException<FileTransferConfigurationModel>(ex);
        }

        return response;
    }

    // F01-052: build add command, run add, and map faults to Error with either error codes or localized business errors.
    public ResponseModel<FtpServerModel> AddFtpServer(RequestModel<FtpServerModel> request)
    {
        ResponseModel<FtpServerModel> response = this.BuildFtpServerModel();

        this.BuildAddFtpServerCommand(request);

        try
        {
            this.AddFtpServer(request, response);
        }
        catch (FaultException<FileTransferConfig.ServiceException> ex)
        {
            _logger.LogError(ex, ex.Message);
            response.Result = this.FormatErrorResponseResult(ex.Detail.ErrorCode ?? string.Empty);
        }
        catch (FaultException<FileTransferConfig.BusinessValidationException> ex)
        {
            _logger.LogError(ex, ex.Message);
            response.Result = this.HandleFtpServerBusinessExceptions(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            response.Result = this.FormatErrorResponseResult(ex);
        }

        return response;
    }

    // F01-053: on success, return model with id and original Editor + OK.
    private void AddFtpServer(RequestModel<FtpServerModel> request, ResponseModel<FtpServerModel> response)
    {
        FileTransferConfig.FtpServerAdded serviceResponse = this.RunAddFtpServer(this.addFtpServerCommand!, _serviceProxy);

        if (serviceResponse != null)
        {
            response.Data = new FtpServerModel()
            {
                FtpServerId = serviceResponse.FtpServerId.ToString()
            };

            response.Data.Editor = request.Data!.Editor;
            response.Result.StatusCode = CoreEnumerations.Messaging.StatusCodeType.OK;
        }
    }

    private ResponseModel<FtpServerModel> BuildFtpServerModel()
    {
        return new ResponseModel<FtpServerModel>()
        {
            Result = new ResultModel()
            {
                StatusCode = CoreEnumerations.Messaging.StatusCodeType.Error
            }
        };
    }

    // F01-052: build the add command forwarded to AddFtpServer.
    private void BuildAddFtpServerCommand(RequestModel<FtpServerModel> request)
    {
        this.addFtpServerCommand = new FileTransferConfig.AddFtpServerCommand
        {
            Requestor = this.MapToServiceRequestor(request.Requestor),
            FtpServerId = Guid.NewGuid(),
            FtpServerName = request.Data!.FtpServerName,
            FtpDescription = request.Data.FtpDescription,
            FtpPassword = request.Data.FtpPassword,
            FtpPort = request.Data.FtpPort,
            FtpProxyAddress = request.Data.FtpProxyAddress,
            FtpProxyPort = request.Data.FtpProxyPort,
            FtpServerIp = request.Data.FtpServerIp,
            FtpTransferMode = request.Data.FtpTransferMode,
            FtpUser = request.Data.FtpUser,
            OperatorId = request.Requestor?.IdentifierId
        };
    }

    // F01-054: password comes from request when non-empty, otherwise read existing credentials.
    private void BuildUpdateFtpServerCommand(RequestModel<FtpServerModel> request)
    {
        Guid ftpServerId = Guid.Parse(request.Data!.FtpServerId!);
        string? ftpServerPass = !string.IsNullOrEmpty(request.Data.FtpPassword) ? request.Data.FtpPassword : this.GetFTPServerPass(ftpServerId);

        this.updateFtpServerCommand = new FileTransferConfig.UpdateFtpServerCommand
        {
            Requestor = this.MapToServiceRequestor(request.Requestor),
            FtpServerId = ftpServerId,
            FtpServerName = request.Data.FtpServerName,
            FtpDescription = request.Data.FtpDescription,
            FtpPort = request.Data.FtpPort,
            FtpProxyAddress = request.Data.FtpProxyAddress,
            FtpProxyPort = request.Data.FtpProxyPort,
            FtpServerIp = request.Data.FtpServerIp,
            FtpTransferMode = request.Data.FtpTransferMode,
            FtpUser = request.Data.FtpUser,
            FtpPassword = ftpServerPass
        };
    }

    // F01-054: reads credentials with RemovePassword=false and returns the first result's password when OK.
    private string? GetFTPServerPass(Guid ftpServerId)
    {
        string? ftpPass = null;

        ResponseModel<List<FtpServerModel>> resp = _queryDataProvider.GetFTPServerCredentials(this.BuildRequest(ftpServerId), this.BuildCriteria(ftpServerId));
        ResultModel resultModel = resp.Result;

        if (resultModel.StatusCode == CoreEnumerations.Messaging.StatusCodeType.OK && resp.Data != null)
        {
            FtpServerModel server = resp.Data.First();
            ftpPass = server.FtpPassword;
        }

        return ftpPass;
    }

    // F01-054: request uses IsValid=true and RemovePassword=false.
    private RequestModel<FtpServerModel> BuildRequest(Guid ftpServer)
    {
        string ftpServerId = ftpServer.ToString();

        return new RequestModel<FtpServerModel>()
        {
            Data = new FtpServerModel()
            {
                FtpServerId = ftpServerId,
                IsValid = true,
                RemovePassword = false
            },
            Requestor = this.CreateRequestorFromContext()
        };
    }

    // F01-054: criteria searches for FtpServerId equal to the provided Guid.
    private SearchCriteriaModel BuildCriteria(Guid ftpServerId)
    {
        return new SearchCriteriaModel()
        {
            SearchFields = new List<SearchFieldModel>()
            {
                new SearchFieldModel()
                {
                    FieldName = FileTransferConstants.SearchFields.FtpServerId,
                    SearchMode = CoreEnumerations.Messaging.SearchModeType.EqualTo,
                    FieldValue = ftpServerId
                }
            }
        };
    }

    public virtual FileTransferConfig.FtpServerUpdated RunUpdateFtpServer(FileTransferConfig.UpdateFtpServerCommand command, FileTransferConfig.IFileTransferSvcService serviceProxy)
    {
        return serviceProxy.UpdateFtpServer(command);
    }

    // F01-056: map business validation failures into localized Error results.
    public ResponseModel<FtpServerModel> UpdateFtpServer(RequestModel<FtpServerModel> request)
    {
        ResponseModel<FtpServerModel> response = this.BuildFtpServerModel();

        this.BuildUpdateFtpServerCommand(request);

        try
        {
            this.UpdateFtpServer(request, response);
        }
        catch (FaultException<FileTransferConfig.ServiceException> ex)
        {
            _logger.LogError(ex, ex.Message);
            response.Result = this.FormatErrorResponseResult(ex.Detail.ErrorCode ?? string.Empty);
        }
        catch (FaultException<FileTransferConfig.BusinessValidationException> ex)
        {
            _logger.LogError(ex, ex.Message);
            response.Result = this.HandleFtpServerBusinessExceptions(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            response.Result = this.FormatErrorResponseResult(ex);
        }

        return response;
    }

    // F01-055: update success returns id + IsValid + original Editor.
    private void UpdateFtpServer(RequestModel<FtpServerModel> request, ResponseModel<FtpServerModel> response)
    {
        FileTransferConfig.FtpServerUpdated serviceResponse = this.RunUpdateFtpServer(this.updateFtpServerCommand!, _serviceProxy);

        if (serviceResponse != null)
        {
            response.Data = new FtpServerModel()
            {
                FtpServerId = serviceResponse.FtpServerId.ToString(),
                IsValid = serviceResponse.IsValid
            };

            response.Data.Editor = request.Data!.Editor;
            response.Result.StatusCode = CoreEnumerations.Messaging.StatusCodeType.OK;
        }
    }

    public virtual FileTransferConfig.FtpServerAdded RunAddFtpServer(FileTransferConfig.AddFtpServerCommand command, FileTransferConfig.IFileTransferSvcService serviceProxy)
    {
        return serviceProxy.AddFtpServer(command);
    }

    public ResponseModel<FtpServerModel> InvalidateFtpServer(RequestModel<FtpServerModel> request)
    {
        ResponseModel<FtpServerModel> response = this.BuildFtpServerModel();

        this.BuildInvalidateFtpServerCommand(request);

        try
        {
            this.InvalidateFtpServer(request, response);
        }
        catch (FaultException<FileTransferConfig.ServiceException> ex)
        {
            _logger.LogError(ex, ex.Message);
            response.Result = this.FormatErrorResponseResult(ex.Detail.ErrorCode ?? string.Empty);
        }
        catch (FaultException<FileTransferConfig.BusinessValidationException> ex)
        {
            _logger.LogError(ex, ex.Message);
            response.Result = this.HandleFtpServerBusinessExceptions(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            response.Result = this.FormatErrorResponseResult(ex);
        }

        return response;
    }

    // F01-055: invalidate success returns id + IsValid + original Editor.
    private void InvalidateFtpServer(RequestModel<FtpServerModel> request, ResponseModel<FtpServerModel> response)
    {
        FileTransferConfig.FtpServerInvalidated serviceResponse = this.RunInvalidateFtpServer(this.invalidateFtpServerCommand!, _serviceProxy);

        if (serviceResponse != null)
        {
            response.Data = new FtpServerModel()
            {
                FtpServerId = serviceResponse.FtpServerId.ToString(),
                IsValid = serviceResponse.IsValid
            };

            response.Data.Editor = request.Data!.Editor;
            response.Result.StatusCode = CoreEnumerations.Messaging.StatusCodeType.OK;
        }
    }

    private void BuildInvalidateFtpServerCommand(RequestModel<FtpServerModel> request)
    {
        this.invalidateFtpServerCommand = new FileTransferConfig.InvalidateFtpServerCommand
        {
            FtpServerId = Guid.Parse(request.Data!.FtpServerId!),
            Requestor = this.MapToServiceRequestor(request.Requestor)
        };
    }

    public virtual FileTransferConfig.FtpServerInvalidated RunInvalidateFtpServer(FileTransferConfig.InvalidateFtpServerCommand command, FileTransferConfig.IFileTransferSvcService serviceProxy)
    {
        return serviceProxy.InvalidateFtpServer(command);
    }

    // F01-056: maps business exception keys to localized message texts/ids.
    private ResultModel HandleFtpServerBusinessExceptions(FaultException<FileTransferConfig.BusinessValidationException> ex)
    {
        ResultModel resultModel = new ResultModel();
        resultModel.StatusCode = CoreEnumerations.Messaging.StatusCodeType.Error;

        ex.Detail.BusinessMessages.ToList().ForEach(err => this.GetErrorMessage(resultModel, err));

        return resultModel;
    }

    // F01-056: maps well-known MessageKey values to validation messages.
    private void GetErrorMessage(ResultModel resultModel, FileTransferConfig.BusinessExceptionMessage message)
    {
        FileTransferConfig.LocaleConfigurationMessageModel localeConfigurationMessage;
        switch (message.MessageKey)
        {
            case FileTransferConstants.BusinessValidationExceptions.DuplicatedFTPServerName:
                localeConfigurationMessage = this.GetValidationMessage(FileTransferConstants.Localization.Messages.DuplicatedFTPServerName);
                resultModel.Messages.Add(new ResultMessageModel(CoreEnumerations.Messaging.MessageType.Error, localeConfigurationMessage.Text));
                break;

            case FileTransferConstants.BusinessValidationExceptions.InvalidIpAddressFormat:
                localeConfigurationMessage = this.GetValidationMessage(FileTransferConstants.Localization.Messages.InvalidIpServerFormat);
                resultModel.Messages.Add(new ResultMessageModel(CoreEnumerations.Messaging.MessageType.Error, localeConfigurationMessage.Text));
                break;

            case FileTransferConstants.BusinessValidationExceptions.InvalidFTPProxyPort:
                localeConfigurationMessage = this.GetValidationMessage(FileTransferConstants.Localization.Messages.InvalidFTPProxyPort);
                resultModel.Messages.Add(new ResultMessageModel(CoreEnumerations.Messaging.MessageType.Error, localeConfigurationMessage.Text));
                break;

            default:
                resultModel.Messages.Add(new ResultMessageModel(CoreEnumerations.Messaging.MessageType.Error, message.MessageKey ?? string.Empty));
                break;
        }
    }

    private FileTransferConfig.LocaleConfigurationMessageModel GetValidationMessage(string validationMessage)
    {
        return new FileTransferConfig.LocaleConfigurationMessageModel { Text = validationMessage };
    }

    private ResponseModel<T> HandleServiceException<T>(FaultException<FileTransferConfig.ServiceException> ex) where T : class
    {
        _logger.LogError(ex, ex.Message);
        var response = this.GenerateReponse<T>(null, CoreEnumerations.Messaging.StatusCodeType.Error);
        response.Result = this.FormatErrorResponseResult(ex.Detail.ErrorCode ?? string.Empty);

        return response;
    }

    private ResponseModel<T> HandleBusinessException<T>(FaultException<FileTransferConfig.BusinessValidationException> ex) where T : class
    {
        _logger.LogError(ex, ex.Message);
        var response = this.GenerateReponse<T>(null, CoreEnumerations.Messaging.StatusCodeType.Error);
        ResultModel resultModel = this.BuildMessage(ex);
        response.Result = resultModel;
        response.Result.StatusCode = CoreEnumerations.Messaging.StatusCodeType.Error;

        return response;
    }

    private ResultModel BuildMessage(FaultException<FileTransferConfig.BusinessValidationException> ex)
    {
        List<FileTransferConfig.BusinessExceptionMessage> messagesList = new List<FileTransferConfig.BusinessExceptionMessage>(ex.Detail.BusinessMessages);
        string message = this.BuildErrorMessage(messagesList);
        message = message.TrimEnd(',');
        ResultModel resultModel = new ResultModel();
        resultModel.Messages.Add(new ResultMessageModel(CoreEnumerations.Messaging.MessageType.Error, message));

        return resultModel;
    }

    private string BuildErrorMessage(List<FileTransferConfig.BusinessExceptionMessage> messagesList)
    {
        string message = string.Empty;
        foreach (FileTransferConfig.BusinessExceptionMessage msg in messagesList)
        {
            message = string.Concat(msg.MessageDefault, (char)32, msg.FieldName);
        }

        return message;
    }

    private ResponseModel<T> HandleException<T>(Exception ex) where T : class
    {
        _logger.LogError(ex, ex.Message);
        ResponseModel<T> response = this.GenerateReponse<T>(null, CoreEnumerations.Messaging.StatusCodeType.Error);
        response.Result = this.FormatErrorResponseResult(ex);

        return response;
    }

    private ResponseModel<T> GenerateReponse<T>(T? data, CoreEnumerations.Messaging.StatusCodeType statusCode) where T : class
    {
        ResponseModel<T> response = new ResponseModel<T>()
        {
            Data = data,
            Result = new ResultModel()
            {
                StatusCode = statusCode
            }
        };

        return response;
    }

    // MaintainabilityDetails(ApprovedIndex = 38, ReviewedOnSprint = 94, ReviewerName = "Josue Chavez")
    private FileTransferConfigurationModel? MapToInternalProvider(FileTransferConfig.FtsSourceLocationAdded serviceProvider)
    {
        if (serviceProvider == null)
        {
            return null;
        }

        FileTransferConfigurationModel internalProvider = new FileTransferConfigurationModel();

        if (serviceProvider.FtpServerId != null)
        {
            internalProvider.FTPServer = new ListItemModel { Name = serviceProvider.FtpServerId };
        }

        if (serviceProvider.Mask != null)
        {
            internalProvider.SourceFileMask = serviceProvider.Mask;
        }

        if (serviceProvider.Protocol != null)
        {
            internalProvider.ProtocolType = new ListItemModel { Name = serviceProvider.Protocol };
        }

        if (serviceProvider.OwnerPhone != null)
        {
            internalProvider.OwnerPhoneNumber = serviceProvider.OwnerPhone;
        }

        if (serviceProvider.OwnerModule != null)
        {
            internalProvider.OwnerSubsystem = new ListItemModel { Name = serviceProvider.OwnerModule };
        }

        internalProvider.RunOnMonday = serviceProvider.RunOnMonday;
        internalProvider.RunOnTuesday = serviceProvider.RunOnTuesday;
        internalProvider.RunOnWednesday = serviceProvider.RunOnWed;
        internalProvider.RunOnThursday = serviceProvider.RunOnThrus;
        internalProvider.RunOnFriday = serviceProvider.RunOnFri;
        internalProvider.RunOnSaturday = serviceProvider.RunOnSat;
        internalProvider.RunOnSunday = serviceProvider.RunOnSun;
        internalProvider.RunOnFirstDay = serviceProvider.RunOnFrstDayOfMonthInd;
        internalProvider.RunOnLastDay = serviceProvider.RunOnLastDayOfMonthInd;

        internalProvider.RunOnThisDay = new ListItemModel { Name = serviceProvider.RunOnThisDayOfMonth.ToString() };
        internalProvider.TimeWindowStart = new ListItemModel { Name = serviceProvider.StartTime?.ToString() };
        internalProvider.TimeWindowEnd = new ListItemModel { Name = serviceProvider.EndTime?.ToString() };
        internalProvider.SendNotification = serviceProvider.SendNotifications;
        internalProvider.FileDirection = new ListItemModel { Name = serviceProvider.FileDirection };

        return internalProvider;
    }

    private RequestorModel CreateRequestorFromContext()
    {
        string? subject = _httpContextAccessor.HttpContext?.User?.FindFirst(new string(new[] { 's', 'u', 'b' }))?.Value;
        return new RequestorModel { IdentifierId = subject };
    }

    private string? GetOperatorId()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(new string(new[] { 's', 'u', 'b' }))?.Value;
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

    private FileTransferConfig.RequestorModel MapToServiceRequestor(RequestorModel? internalRequestor)
    {
        return new FileTransferConfig.RequestorModel
        {
            IdentifierId = internalRequestor?.IdentifierId
        };
    }

}

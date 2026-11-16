using Fxfer.Bff.Downstream.FxferService.Models;
using System.Collections.Generic;

namespace Fxfer.Bff.Downstream.FxferService;

public interface IFileTransferQueryServiceDataProvider
{
    ResponseModel<List<AddFileTransferConfigurationModel>> SearchFileTransfer(RequestModel<SearchCriteriaModel> request);
    ResponseModel<List<FtpServerModel>> GetFtpServers(RequestModel<SearchCriteriaModel> request);
    ResponseModel<List<InboundTargetModel>> GetMetadataIds(RequestModel<SearchCriteriaModel> request, string contractName);
    ResponseModel<FileTransferConfigurationModel> GetFileTransferConfigurationInformation(RequestModel<string> request);
    ResponseModel<List<TrackingSearchResultModel>> SearchFileTransferTracking(RequestModel<SearchCriteriaModel> request);
    ResponseModel<List<SourceLocationModel>> GetSourceLocations(RequestModel<string> request);
    ResponseModel<TrackingDetailModel> GetFileTransferTrackingDetail(RequestModel<string> request);
    ResponseModel<List<FtpServerModel>> GetFTPServerCredentials(RequestModel<FtpServerModel> request, SearchCriteriaModel searchCriteria);
}


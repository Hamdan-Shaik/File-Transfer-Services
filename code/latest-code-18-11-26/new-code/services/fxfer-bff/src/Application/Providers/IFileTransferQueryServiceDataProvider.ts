export interface IFileTransferQueryServiceDataProvider {
  SearchFileTransfer(
    request: RequestModel<SearchCriteriaModel>,
  ): ResponseModel<AddFileTransferConfigurationModel[]>;

  GetFtpServers(request: RequestModel<SearchCriteriaModel>): ResponseModel<FtpServerModel[]>;

  GetMetadataIds(
    request: RequestModel<SearchCriteriaModel>,
    contractName: string,
  ): ResponseModel<InboundTargetModel[]>;

  GetFileTransferConfigurationInformation(
    request: RequestModel<string>,
  ): ResponseModel<FileTransferConfigurationModel>;

  SearchFileTransferTracking(
    request: RequestModel<SearchCriteriaModel>,
  ): ResponseModel<TrackingSearchResultModel[]>;

  GetSourceLocations(request: RequestModel<string>): ResponseModel<SourceLocationModel[]>;

  GetFileTransferTrackingDetail(request: RequestModel<string>): ResponseModel<TrackingDetailModel>;

  GetFTPServerCredentials(
    request: RequestModel<FtpServerModel>,
    searchCriteria: SearchCriteriaModel,
  ): ResponseModel<FtpServerModel[]>;
}

type RequestModel<T> = {
  Data: T;
};

type ResponseModel<T> = {
  Data: T;
  Result?: unknown;
};

type SearchCriteriaModel = unknown;
type AddFileTransferConfigurationModel = unknown;
type FtpServerModel = unknown;
type InboundTargetModel = unknown;
type FileTransferConfigurationModel = unknown;
type TrackingSearchResultModel = unknown;
type SourceLocationModel = unknown;
type TrackingDetailModel = unknown;

import { createFxferBffClient } from '../../../lib/api/fxferBffClient';
import type { FxferRemoteContextValue } from '../../../lib/auth/FxferRemoteContext';

export type PagedResult<T> = {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
};

export type FtpServer = {
  ftpServerId: string;
  name: string;
  ipAddress: string;
  transferMode: string;
  user: string;
  password?: string;
  description: string;
  port: number;
  proxyAddress?: string;
  proxyPort?: number;
  sshAuthentication: boolean;
  isActive: boolean;
  createdAt: string;
  lastModifiedAt: string;
};

export type TransferConfigurationSummary = {
  transferConfigurationId: string;
  configNumber: number;
  name: string;
  protocolCode: string;
  address: string;
  fileMask: string;
  isActive: boolean;
  isSuspended: boolean;
};

export type TransferConfiguration = {
  transferConfigurationId: string;
  configNumber: number;
  name: string;
  description?: string;
  protocolCode: string;
  address: string;
  fileMask: string;
  ftpServerId?: string;
  ftpServerName?: string;
  sendNotifications: boolean;
  owner: { name: string; moduleId: string; phone: string; email: string };
  schedule: {
    runOnMonday: boolean;
    runOnTuesday: boolean;
    runOnWednesday: boolean;
    runOnThursday: boolean;
    runOnFriday: boolean;
    runOnSaturday: boolean;
    runOnSunday: boolean;
    runOnFirstDayOfMonth: boolean;
    runOnLastDayOfMonth: boolean;
    runOnThisDayOfMonth: number;
    startTime24hhmm: string;
    endTime24hhmm: string;
  };
  fileDirection?: string;
  tradingPartner?: string;
  isActive: boolean;
  isSuspended: boolean;
  suspendedChangedAt?: string;
  destinations: Destination[];
};

export type Destination = {
  destinationId: string;
  address: string;
  protocolCode: string;
  overwriteAtDestination: boolean;
  compressAtDestination: boolean;
  renameAtDestination: boolean;
  destinationIsArchive: boolean;
  isActive: boolean;
  ftpServerId: string;
  renamePattern?: string;
  contact?: { name?: string; phone?: string; email?: string };
  encryption?: {
    cdeEncryption?: string;
    publicKey?: string;
    privateKey?: string;
    privateKeyPassword?: string;
  };
  externalFileDefinitionId?: string;
  destinationType?: string;
  contractName?: string;
};

export type TrackedFile = {
  fileId: string;
  fileName: string;
  fileStatus: string;
  fileDirection: string;
  fileTransportType: string;
  createdAt: string;
  fileSize: number;
  tradingPartner?: string;
  sourceLocationId?: string;
  sourceLocationConfigName?: string;
  sourceLocationAddress?: string;
  originatingServerName?: string;
  events: { eventId: string; eventCode: string; createdAt: string; description: string }[];
  exceptions: {
    exceptionId: string;
    exceptionType: string;
    message: string;
    stackTrace: string;
    methodName: string;
    createdAt: string;
  }[];
};

export type InboundTarget = {
  inboundTargetId: string;
  externalFileDefinitionId: string;
  importJobName: string;
  contractName: string;
  isActive: boolean;
  createdAt: string;
  lastModifiedAt: string;
};

export type TransferConfigurationSearchParams = {
  isActive?: boolean;
  configNumber?: number;
  nameContains?: string;
  ownerNameContains?: string;
  protocolCode?: string;
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortDir?: 'asc' | 'desc';
};

export type FtpServerSearchParams = {
  isActive?: boolean;
  name?: string;
  description?: string;
  user?: string;
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortDir?: 'asc' | 'desc';
  includePassword?: boolean;
};

export type InboundTargetSearchParams = {
  contractName: string;
  externalFileDefinitionId: string;
};

export type FileTransferSearch = {
  page?: number;
  pageSize?: number;
  orderBy?: string;
  orderDir?: 'asc' | 'desc';
  filters?: {
    fileId?: string;
    fileNameContains?: string;
    fileStatus?: string;
    fileDirection?: string;
    transferMethod?: string;
    createdFrom?: string;
    createdTo?: string;
    fileSizeMin?: number;
    fileSizeMax?: number;
    tradingPartner?: string;
    sourceLocationId?: string;
  };
};

function withQuery(path: string, params: Record<string, unknown>) {
  const search = new URLSearchParams();
  for (const [key, value] of Object.entries(params)) {
    if (value === null || value === undefined) continue;
    search.set(key, String(value));
  }
  const qs = search.toString();
  return qs === '' ? path : `${path}?${qs}`;
}

export function createFileTransferQueries(ctx: FxferRemoteContextValue) {
  const client = createFxferBffClient(ctx);

  return {
    // F02-003: SearchSourceLocations returns 200 OK with the query-service result.
    //          Target: return the BFF response body unchanged on success.
    searchSourceLocations: (params: TransferConfigurationSearchParams) =>
      client.get<PagedResult<TransferConfigurationSummary>>(
        withQuery('/transfer-configurations', params),
      ),

    // F02-004: GetSourceLocations returns 200 OK with the query-service result.
    //          Target: return the BFF response body unchanged on success.
    getSourceLocations: (transferConfigurationId: string) =>
      client.get<TransferConfiguration>(
        `/transfer-configurations/${encodeURIComponent(transferConfigurationId)}`,
      ),

    // F01-002: GetFtpServers returns 200 OK with the query-service result.
    //          Target: return the BFF response body unchanged on success.
    getFtpServers: (params: FtpServerSearchParams) =>
      client.get<PagedResult<FtpServer>>(withQuery('/ftp-servers', params)),

    // F04-025: SearchFileTransferMovement returns 200 OK with the query-service result.
    //          Target: return the BFF response body unchanged on success.
    searchFileTransferMovement: (search: FileTransferSearch) =>
      client.post<PagedResult<TrackedFile>>('/file-transfers/search', search),

    // F04-036: SearchInboundTarget returns 200 OK with the query-service result.
    //          Target: return the BFF response body unchanged on success.
    searchInboundTarget: (params: InboundTargetSearchParams) =>
      client.get<InboundTarget[]>(withQuery('/inbound-targets/search', params)),
  };
}

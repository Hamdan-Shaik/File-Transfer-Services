import { createIdempotencyKey, getCorrelationId, getTraceContextHeaders } from '@/observability/headers';
import { getAccessToken } from '@/auth/shellAuth';

export type PagedResult<T> = {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
};

export type TransferConfigurationSummary = {
  ftsSourceLocationId: string;
  configId: number;
  configName: string;
  protocol: string;
  address: string;
  ownerName: string;
  tradingPartner?: string | null;
  isActive: boolean;
  suspended: boolean;
};

export type Destination = {
  destinationId?: string | null;
  destinationAddress: string;
  destinationProtocol: string;
  ftpServerId?: string | null;
  renamePattern?: string | null;
  overWriteAtDestination?: boolean | null;
  compressAtDestination?: boolean | null;
  renameAtDestination?: boolean | null;
  destinationArchive?: boolean | null;
  contactName?: string | null;
  contactPhone?: string | null;
  contactEmail?: string | null;
  isValid?: boolean | null;
  fteMetadataId?: string | null;
  destinationType?: string | null;
  contractName?: string | null;
  importJobName?: string | null;
  operatorId?: string | null;
  creationDate: string;
  lastModifiedDate: string;
};

export type TransferConfiguration = {
  ftsSourceLocationId: string;
  configId: number;
  configName: string;
  description: string;
  ftpServerId: string;
  ftpServerName: string;
  address: string;
  mask: string;
  protocol: string;
  fileDirection: string;
  ownerName: string;
  ownerEmail: string;
  ownerSubsystem: string;
  ownerPhoneNumber: string;
  tradingPartner?: string | null;
  operatorId: string;
  runOnMonday: boolean;
  runOnTuesday: boolean;
  runOnWednesday: boolean;
  runOnThursday: boolean;
  runOnFriday: boolean;
  runOnSaturday: boolean;
  runOnSunday: boolean;
  runOnFirstDay: boolean;
  runOnLastDay: boolean;
  runOnThisDay: number;
  startWindow: string;
  endWindow: string;
  sendNotification: boolean;
  isActive: boolean;
  validChangeDate: string;
  suspended: boolean;
  suspendedChangeDate: string;
  lastModifiedDate: string;
  creationDate: string;
  destinations: Destination[];
};

export type TransferConfigurationUpsertRequest = {
  configName: string;
  description?: string;
  ftpServerId?: string | null;
  address: string;
  mask?: string;
  protocol: string;
  fileDirection?: string;
  ownerName: string;
  ownerEmail: string;
  ownerPhoneNumber: string;
  ownerSubsystem: string;
  tradingPartner?: string;
  sendNotification?: boolean;
  runOnMonday?: boolean;
  runOnTuesday?: boolean;
  runOnWednesday?: boolean;
  runOnThursday?: boolean;
  runOnFriday?: boolean;
  runOnSaturday?: boolean;
  runOnSunday?: boolean;
  runOnFirstDay?: boolean;
  runOnLastDay?: boolean;
  runOnThisDay?: number;
  startWindow: string;
  endWindow: string;
  isActive?: boolean;
  suspended?: boolean;
  destinations: Array<{
    destinationId?: string | null;
    destinationAddress: string;
    destinationProtocol: string;
    ftpServerId?: string | null;
    renamePattern?: string | null;
    overWriteAtDestination?: boolean | null;
    compressAtDestination?: boolean | null;
    renameAtDestination?: boolean | null;
    destinationArchive?: boolean | null;
    contactName?: string | null;
    contactPhone?: string | null;
    contactEmail?: string | null;
    isValid?: boolean | null;
    fteMetadataId?: string | null;
    destinationType?: string | null;
    contractName?: string | null;
    importJobName?: string | null;
  }>;
  operatorId: string;
};

export type TransferConfigurationPatchRequest = {
  isActive?: boolean;
  suspended?: boolean;
  operatorId: string;
};

export type FtpServer = {
  ftpServerId: string;
  ftpServerName: string;
  ftpDescription: string;
  ftpServerIp: string;
  ftpTransferMode: string;
  ftpUser: string;
  ftpPort: number;
  ftpProxyAddress?: string | null;
  ftpProxyPort?: number | null;
  useSshAuth: boolean;
  isValid: boolean;
  operatorId: string;
  creationDate: string;
  lastModifiedDate: string;
};

export type FtpServerUpsertRequest = {
  ftpServerName: string;
  ftpDescription: string;
  ftpServerIp: string;
  ftpTransferMode: string;
  ftpUser: string;
  ftpPort: number;
  ftpPassword?: string;
  ftpPasswordConfirmation?: string;
  ftpProxyAddress?: string;
  ftpProxyPort?: number;
  useSshAuth: boolean;
  isValid: boolean;
  operatorId: string;
};

export type FtpServerPatchRequest = {
  isValid?: boolean;
  operatorId: string;
};

export type InboundTargetMetadata = {
  fteMetadataId: string;
  importJobName: string;
};

export type FileEvent = {
  eventId: string;
  fileId: string;
  eventName: string;
  eventDescription: string;
  createdDate: string;
  createdBy: string;
};

export type FileException = {
  exceptionId: string;
  fileId: string;
  exceptionName: string;
  exceptionDescription: string;
  createdDate: string;
  createdBy: string;
};

export type FileTransferTracking = {
  fileId: string;
  fileSourceLocationId: string;
  fileName: string;
  fileStatus: string;
  fileDirection: string;
  fileTransportType: string;
  createdDate: string;
  tradingPartner?: string | null;
  checkSumCode?: string | null;
  fileSize: number;
  sourceLocAddress: string;
  sourceLocConfigName: string;
  originatingServerName: string;
  events: FileEvent[];
  exceptions: FileException[];
};

export type ProblemDetails = {
  type?: string;
  title?: string;
  status?: number;
  detail?: string;
  instance?: string;
  correlation_id?: string;
};

export class FxferBffError extends Error {
  public readonly problem: ProblemDetails;

  public constructor(message: string, problem: ProblemDetails) {
    super(message);
    this.name = 'FxferBffError';
    this.problem = problem;
  }
}

const BFF_BASE_PATH = '/api/file-transfer/v1';

type HttpMethod = 'GET' | 'POST' | 'PUT' | 'PATCH' | 'DELETE';

export async function fxferFetchJson<TResponse>(
  path: string,
  init?: {
    method?: HttpMethod;
    body?: unknown;
    idempotentWrite?: boolean;
    signal?: AbortSignal;
  }
): Promise<TResponse> {
  const token = await getAccessToken();
  const method = init?.method ?? 'GET';

  const headers: Record<string, string> = {
    Accept: 'application/json',
    'Content-Type': 'application/json',
    Authorization: `Bearer ${token}`,
    'X-Correlation-ID': getCorrelationId(),
    ...getTraceContextHeaders()
  };

  if (init?.idempotentWrite && method !== 'GET') {
    headers['Idempotency-Key'] = createIdempotencyKey();
  }

  const res = await fetch(`${BFF_BASE_PATH}${path}`, {
    method,
    headers,
    body: init?.body === undefined ? undefined : JSON.stringify(init.body),
    signal: init?.signal
  });

  if (res.ok) {
    return (await res.json()) as TResponse;
  }

  let problem: ProblemDetails = { status: res.status, title: res.statusText };
  try {
    if (res.headers.get('content-type')?.includes('application/problem+json')) {
      problem = (await res.json()) as ProblemDetails;
    }
  } catch {
    // best-effort
  }

  throw new FxferBffError(problem.title ?? 'BFF request failed', problem);
}

function buildQueryString(params: Record<string, string | number | boolean | undefined | null>): string {
  const parts: string[] = [];
  for (const [key, value] of Object.entries(params)) {
    if (value === undefined || value === null) {
      continue;
    }
    if (typeof value === 'string' && value.trim() === '') {
      continue;
    }
    parts.push(`${encodeURIComponent(key)}=${encodeURIComponent(String(value))}`);
  }
  return parts.length === 0 ? '' : `?${parts.join('&')}`;
}

export class FxferBffClient {
  // F01-080: BFF search returns 200 with query results.
  public static async searchSourceLocations(args: {
    configId?: number | null;
    configName?: string | null;
    ownerName?: string | null;
    ownerSubsystem?: string | null;
    protocol?: string | null;
    isActive: boolean;
    page?: number;
    pageSize?: number;
  }): Promise<PagedResult<TransferConfigurationSummary>> {
    const qs = buildQueryString({
      configId: args.configId ?? undefined,
      configName: args.configName ?? undefined,
      ownerName: args.ownerName ?? undefined,
      ownerSubsystem: args.ownerSubsystem ?? undefined,
      protocol: args.protocol ?? undefined,
      isActive: args.isActive,
      page: args.page ?? 1,
      pageSize: args.pageSize ?? 50
    });

    return fxferFetchJson<PagedResult<TransferConfigurationSummary>>(`/transfer-configurations${qs}`);
  }

  // F01-082: BFF door returns 200 with the query result (or 404).
  public static async getSourceLocationConfiguration(ftsSourceLocationId: string): Promise<TransferConfiguration> {
    return fxferFetchJson<TransferConfiguration>(`/transfer-configurations/${encodeURIComponent(ftsSourceLocationId)}`);
  }

  // F01-037: Add source location responds 200 OK with the service result.
  public static async addSourceLocation(request: TransferConfigurationUpsertRequest): Promise<TransferConfiguration> {
    return fxferFetchJson<TransferConfiguration>('/transfer-configurations', {
      method: 'POST',
      body: request,
      idempotentWrite: true
    });
  }

  // F01-056: Update source location responds 200 OK with the service result.
  public static async updateSourceLocation(
    ftsSourceLocationId: string,
    request: TransferConfigurationUpsertRequest
  ): Promise<unknown> {
    return fxferFetchJson(`/transfer-configurations/${encodeURIComponent(ftsSourceLocationId)}`, {
      method: 'PUT',
      body: request,
      idempotentWrite: true
    });
  }

  // F01-058: Invalidate source location responds 200 OK with the service result.
  public static async invalidateSourceLocation(
    ftsSourceLocationId: string,
    request: TransferConfigurationPatchRequest
  ): Promise<unknown> {
    return fxferFetchJson(`/transfer-configurations/${encodeURIComponent(ftsSourceLocationId)}`, {
      method: 'PATCH',
      body: request,
      idempotentWrite: true
    });
  }

  // F02-004: BFF FTP server search returns 200 with the query result.
  public static async getFtpServers(args: {
    isValid?: boolean | null;
    ftpServerName?: string | null;
    nameMatch?: 'contains' | 'eq' | string | null;
    page?: number;
    pageSize?: number;
  }): Promise<PagedResult<FtpServer>> {
    const qs = buildQueryString({
      isValid: args.isValid ?? undefined,
      ftpServerName: args.ftpServerName ?? undefined,
      nameMatch: args.nameMatch ?? undefined,
      page: args.page ?? 1,
      pageSize: args.pageSize ?? 50
    });

    return fxferFetchJson<PagedResult<FtpServer>>(`/ftp-servers${qs}`);
  }

  // F02-018: Add FTP server responds 200 OK with the service result.
  public static async addFtpServer(request: FtpServerUpsertRequest): Promise<FtpServer> {
    return fxferFetchJson<FtpServer>('/ftp-servers', {
      method: 'POST',
      body: request,
      idempotentWrite: true
    });
  }

  // F02-020: Update FTP server responds 200 OK with the service result.
  public static async updateFtpServer(ftpServerId: string, request: FtpServerUpsertRequest): Promise<unknown> {
    return fxferFetchJson(`/ftp-servers/${encodeURIComponent(ftpServerId)}`, {
      method: 'PUT',
      body: request,
      idempotentWrite: true
    });
  }

  // F02-022: Invalidate FTP server responds 200 OK with the service result.
  public static async invalidateFtpServer(ftpServerId: string, request: FtpServerPatchRequest): Promise<unknown> {
    return fxferFetchJson(`/ftp-servers/${encodeURIComponent(ftpServerId)}`, {
      method: 'PATCH',
      body: request,
      idempotentWrite: true
    });
  }

  // F05-020: BFF tracking search returns 200 with query result.
  public static async searchFileTransferMovement(args: {
    fileId?: string | null;
    fileName?: string | null;
    fileStatus?: string | null;
    direction?: string | null;
    transportType?: string | null;
    fromDate?: string | null;
    toDate?: string | null;
    fromSize?: string | null;
    toSize?: string | null;
    tradingPartnerId?: string | null;
    sourceLocationId?: string | null;
    page?: number;
    pageSize?: number;
  }): Promise<PagedResult<FileTransferTracking>> {
    const qs = buildQueryString({
      fileId: args.fileId ?? undefined,
      fileName: args.fileName ?? undefined,
      fileStatus: args.fileStatus ?? undefined,
      fileDirection: args.direction ?? undefined,
      fileTransportType: args.transportType ?? undefined,
      startDate: args.fromDate ?? undefined,
      endDate: args.toDate ?? undefined,
      fileSizeFrom: args.fromSize ?? undefined,
      fileSizeThrough: args.toSize ?? undefined,
      tradingPartner: args.tradingPartnerId ?? undefined,
      fileSourceLocationId: args.sourceLocationId ?? undefined,
      page: args.page ?? 1,
      pageSize: args.pageSize ?? 50
    });

    return fxferFetchJson<PagedResult<FileTransferTracking>>(`/file-tracking${qs}`);
  }

  public static async getFileTransferTrackingDetail(fileId: string): Promise<FileTransferTracking> {
    return fxferFetchJson<FileTransferTracking>(`/file-tracking/${encodeURIComponent(fileId)}`);
  }

  // F05-022: BFF inbound target lookup returns 200 with query result.
  public static async searchInboundTargets(contractName?: string | null): Promise<InboundTargetMetadata[]> {
    const qs = buildQueryString({ contractName: contractName ?? undefined });
    return fxferFetchJson<InboundTargetMetadata[]>(`/inbound-target-metadata${qs}`);
  }
}


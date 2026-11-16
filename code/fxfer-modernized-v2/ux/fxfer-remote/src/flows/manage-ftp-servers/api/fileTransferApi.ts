import { createFxferBffClient } from '../../../lib/api/fxferBffClient';
import type { FxferRemoteContextValue } from '../../../lib/auth/FxferRemoteContext';
import { FileTransferConstants } from '../../../shared/file-transfer/constants/FileTransferConstants';
import type { SearchCriteriaModel, SearchFieldModel } from '../../../shared/file-transfer/models';
import {
  buildFtpServerCredentialsSearchModel,
  getSearchFieldValue,
} from '../../../shared/file-transfer/api';

export type SelectionItem = {
  id: string;
  displayName: string;
};

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
  password?: string | null;
  description: string;
  port: number;
  proxyAddress?: string | null;
  proxyPort?: number | null;
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

export type TransferConfigurationUpsert = {
  transferConfigurationId?: string | null;
  configNumber: number;
  name: string;
  description?: string | null;
  protocolCode: string;
  address: string;
  fileMask: string;
  ftpServerId?: string | null;
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
  fileDirection?: string | null;
  tradingPartner?: string | null;
  isActive: boolean;
  isSuspended: boolean;
};

export type TrackedFile = {
  fileId: string;
  fileName: string;
  fileStatus: string;
  fileDirection: string;
  fileTransportType: string;
  createdAt: string;
  fileSize: number;
  tradingPartner?: string | null;
  sourceLocationId?: string | null;
  sourceLocationConfigName?: string | null;
  sourceLocationAddress?: string | null;
  originatingServerName?: string | null;
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

export type PagingCriteriaModel = {
  page: number;
  rowsPerPage: number;
};

export type SortDirectionType = 'Ascending' | 'Descending' | string;

export type SortCriteriaModel = {
  sortField: string;
  sortDirection: SortDirectionType;
};

export type LegacySearchCriteriaModel = SearchCriteriaModel & {
  pagingCriteria?: PagingCriteriaModel | null;
  sortCriteria?: SortCriteriaModel[] | null;
  searchFields?: SearchFieldModel[] | null;
};

export type FtpServerUpsert = {
  name: string;
  ipAddress: string;
  transferMode: string;
  user: string;
  password?: string | null;
  description: string;
  port: number;
  proxyAddress?: string | null;
  proxyPort?: number | null;
  sshAuthentication: boolean;
  isActive: boolean;
};

export type SaveFtpServerCredentialsModel = FtpServerUpsert & {
  ftpServerId?: string | null;
};

export type FileTransferSearch = {
  page: number;
  pageSize: number;
  orderBy?: string | null;
  orderDir?: 'asc' | 'desc' | null;
  filters: {
    fileId?: string | null;
    fileNameContains?: string | null;
    fileStatus?: string | null;
    fileDirection?: string | null;
    transferMethod?: string | null;
    createdFrom?: string | null;
    createdTo?: string | null;
    fileSizeMin?: number | null;
    fileSizeMax?: number | null;
    tradingPartner?: string | null;
    sourceLocationId?: string | null;
  };
};

let defaultCtx: FxferRemoteContextValue | null = null;

export function setFileTransferApiContext(ctx: FxferRemoteContextValue) {
  defaultCtx = ctx;
}

function requireContext(): FxferRemoteContextValue {
  if (!defaultCtx) {
    throw new Error('fileTransferApi context missing; call setFileTransferApiContext(ctx)');
  }
  return defaultCtx;
}

function withQuery(path: string, params: Record<string, unknown>) {
  const search = new URLSearchParams();
  for (const [key, value] of Object.entries(params)) {
    if (value === null || value === undefined) continue;
    if (value === '') continue;
    search.set(key, String(value));
  }
  const qs = search.toString();
  return qs === '' ? path : `${path}?${qs}`;
}

function getIsActiveFromLegacyIsValid(searchCriteria: SearchCriteriaModel): boolean | undefined {
  // F01-043: Any non-empty `IsValid` search-field value forces `IsValid=true`; empty/missing => null.
  const isValidRaw = getSearchFieldValue(searchCriteria, FileTransferConstants.SearchFields.IsValid);
  return isValidRaw !== '' ? true : undefined;
}

function getLegacyPaging(searchCriteria: LegacySearchCriteriaModel) {
  const page = searchCriteria.pagingCriteria?.page ?? 1;
  const pageSize = searchCriteria.pagingCriteria?.rowsPerPage ?? 5;
  return { page, pageSize };
}

function mapLegacySortDirection(sortDirection: SortDirectionType): 'asc' | 'desc' {
  return String(sortDirection).includes('Asc') ? 'asc' : 'desc';
}

function getLegacySort(
  searchCriteria: LegacySearchCriteriaModel,
): { sortBy: string; sortDir: 'asc' | 'desc' } | undefined {
  const sorts = searchCriteria.sortCriteria ?? undefined;
  if (!sorts || sorts.length === 0) return undefined;

  // Legacy SetSortCriteria: uses the last sort item.
  const sort = sorts[sorts.length - 1];
  return { sortBy: sort.sortField, sortDir: mapLegacySortDirection(sort.sortDirection) };
}

// F01-013: Default behaviour requests active servers only.
export async function getFtpServerSelection(activeOnly: boolean = true): Promise<SelectionItem[]> {
  // F01-003: Selection list is shown when the system can retrieve the server list (otherwise surfaced as an error).
  // F01-014: Preserve the server ordering returned by the underlying system (no client-side re-sorting).
  const client = createFxferBffClient(requireContext());
  return client.get<SelectionItem[]>(withQuery('/ftp-servers/selection', { activeOnly }));
}

// Legacy door: GetFTPServerCredentials + BuildRequest + provider paging/sort.
export async function searchFtpServers(
  searchCriteria: LegacySearchCriteriaModel,
): Promise<PagedResult<FtpServer>> {
  // F01-038: Signed-in support staff can view a paged credential list (and failures are surfaced to the UI).
  const client = createFxferBffClient(requireContext());

  const model = buildFtpServerCredentialsSearchModel(searchCriteria);

  const { page, pageSize } = getLegacyPaging(searchCriteria);
  const sort = getLegacySort(searchCriteria);

  // F01-045: Credential-list results omit the stored FTP password by default.
  // Target: only include password when explicitly requested.
  const includePassword = false;

  return client.get<PagedResult<FtpServer>>(
    withQuery('/ftp-servers', {
      isActive: getIsActiveFromLegacyIsValid(searchCriteria),
      name: model.ftpServerName,
      description: model.ftpDescription,
      user: model.ftpUser,
      page,
      pageSize,
      ...(sort ?? {}),
      ...(includePassword ? { includePassword: true } : {}),
    }),
  );
}

function resolveSuccessMessageKey(action: 'add' | 'edit', isActive: boolean): string {
  switch (action) {
    case 'add':
      return FileTransferConstants.Localization.Messages.AddFtpServerCredentialsSuccess;
    case 'edit':
      return isActive
        ? FileTransferConstants.Localization.Messages.EditFtpServerCredentialsSuccess
        : FileTransferConstants.Localization.Messages.InvalidateFtpServerCredentialsSuccess;
  }
}

function resolveSuccessMessageText(messageKey: string): string {
  // shortcut: runtime localization provider is not wired yet; return the message key
  // (tests assert message-id parity; UI can translate later).
  return messageKey;
}

export async function saveFtpServerCredentials(model: SaveFtpServerCredentialsModel): Promise<{
  ftpServerId: string;
  isActive?: boolean;
  successMessage: string;
}> {
  const client = createFxferBffClient(requireContext());

  const ftpServerId = model.ftpServerId ?? '';
  const isActive = model.isActive;

  // Legacy uses `.Value` on IsValid (nullable bool); null -> throws.
  if (isActive === null || isActive === undefined) {
    throw new Error('isActive is required');
  }

  if (ftpServerId === '') {
    // F01-040: Valid create saves a new server (invalid requests are surfaced via the shared HTTP client).
    // F01-016: AddFtpServer returns HTTP 200 OK containing the service result.
    const created = await client.post<{ ftpServerId: string; isActive?: boolean }>(
      '/ftp-servers',
      {
        name: model.name,
        ipAddress: model.ipAddress,
        transferMode: model.transferMode,
        user: model.user,
        password: model.password ?? null,
        description: model.description,
        port: model.port,
        proxyAddress: model.proxyAddress ?? null,
        proxyPort: model.proxyPort ?? null,
        sshAuthentication: model.sshAuthentication,
        isActive,
      } satisfies FtpServerUpsert,
    );

    // F01-049: On success, the UI uses the returned saved values (id/isActive) rather than the submitted model.
    // F01-050: Successful save returns a confirmation message suitable for the action.
    return {
      ...created,
      successMessage: resolveSuccessMessageText(resolveSuccessMessageKey('add', true)),
    };
  }

  if (isActive) {
    // F01-041/F01-051: Update when active; invalidate when inactive.
    // F01-018: UpdateFtpServer returns HTTP 200 OK containing the service result.
    const updated = await client.put<{ ftpServerId: string; isActive: boolean }>(
      `/ftp-servers/${encodeURIComponent(ftpServerId)}`,
      {
        name: model.name,
        ipAddress: model.ipAddress,
        transferMode: model.transferMode,
        user: model.user,
        // If omitted on update, stored credential is retained.
        ...(model.password !== undefined ? { password: model.password } : {}),
        description: model.description,
        port: model.port,
        proxyAddress: model.proxyAddress ?? null,
        proxyPort: model.proxyPort ?? null,
        sshAuthentication: model.sshAuthentication,
        isActive,
      } satisfies FtpServerUpsert,
    );

    // F01-049: On success, the UI uses the returned saved values (id/isActive) rather than the submitted model.
    // F01-050: Successful save returns a confirmation message suitable for the action.
    return {
      ...updated,
      successMessage: resolveSuccessMessageText(resolveSuccessMessageKey('edit', true)),
    };
  }

  // F01-041/F01-051: Update when active; invalidate when inactive.
  // F01-020: InvalidateFtpServer returns HTTP 200 OK containing the service result.
  const invalidated = await client.delete<{ ftpServerId: string; isActive: boolean }>(
    `/ftp-servers/${encodeURIComponent(ftpServerId)}`,
  );

  // F01-049: On success, the UI uses the returned saved values (id/isActive) rather than the submitted model.
  // F01-050: Successful save returns a confirmation message suitable for the action.
  return {
    ...invalidated,
    successMessage: resolveSuccessMessageText(resolveSuccessMessageKey('edit', false)),
  };
}

// F02-020: remove '-' from owner.phone before sending.
export async function upsertTransferConfiguration(
  model: TransferConfigurationUpsert,
): Promise<{ transferConfigurationId: string }> {
  const client = createFxferBffClient(requireContext());

  // F02-021: empty id => create/add semantics (legacy treated empty `FtsSourceLocationId` as add).
  const { transferConfigurationId: rawId, ...rest } = model;
  const transferConfigurationId =
    typeof rawId === 'string' && rawId === '' ? undefined : (rawId ?? undefined);

  const normalized = {
    ...rest,
    ...(transferConfigurationId ? { transferConfigurationId } : {}),
    owner: {
      ...rest.owner,
      // Legacy only normalizes when non-null.
      phone:
        typeof rest.owner.phone === 'string'
          ? rest.owner.phone.replace(/-/g, '')
          : (rest.owner.phone as unknown as string),
    },
  } satisfies TransferConfigurationUpsert;

  // F02-019/F02-022: 400 errors are surfaced from BFF (shared `fetchJson` throws on non-2xx).
  // F02-028/F02-040: FileTransferAdd/UpdateFileTransferSourceLocation returns HTTP 200 OK containing the service result.
  return client.post<{ transferConfigurationId: string }>('/transfer-configurations', normalized);
}

export async function invalidateTransferConfiguration(
  transferConfigurationId: string,
): Promise<{ transferConfigurationId: string; isActive: boolean }> {
  const client = createFxferBffClient(requireContext());
  // F02-042: InvalidateFileTransferSourceLocation returns HTTP 200 OK containing the service result.
  return client.delete<{ transferConfigurationId: string; isActive: boolean }>(
    `/transfer-configurations/${encodeURIComponent(transferConfigurationId)}`,
  );
}

function getSearchField(searchCriteria: LegacySearchCriteriaModel, fieldName: string) {
  const fields = searchCriteria.searchFields ?? undefined;
  if (!fields || fields.length === 0) return undefined;
  return fields.find((f) => f.fieldName === fieldName);
}

export async function searchTransferConfigurations(
  searchCriteria: LegacySearchCriteriaModel,
): Promise<PagedResult<TransferConfigurationSummary>> {
  const client = createFxferBffClient(requireContext());

  const { page, pageSize } = getLegacyPaging(searchCriteria);

  const configIdRaw = getSearchFieldValue(searchCriteria, 'ConfigId');
  // F04-008: ConfigId filter applies only when > 0.
  const configNumber = (() => {
    if (configIdRaw === '') return undefined;
    const parsed = Number(configIdRaw);
    if (Number.isNaN(parsed)) return undefined;
    return parsed > 0 ? parsed : undefined;
  })();

  const ownerSubsystem = getSearchFieldValue(searchCriteria, 'OwnerSubsystem');
  void ownerSubsystem; // drop: no corresponding query param in BFF contract

  const sort = getLegacySort(searchCriteria);

  function mapLegacySourceLocationSortField(sortField: string): string | undefined {
    // F04-006/F04-007: UX sort fields are mapped to service fields; unknown fields fall back.
    switch (sortField) {
      case 'ConfigId':
        return 'configNumber';
      case 'ConfigName':
        return 'name';
      case 'SourceAddress':
        return 'address';
      case 'SourceFileMask':
        return 'fileMask';
      case 'Protocol':
        return 'protocolCode';
      default:
        return undefined;
    }
  }

  // F04-006: Defaults to sorting by ConfigId ascending when no usable sort is provided.
  // shortcut: legacy default also adds a secondary `IsActive desc`; BFF only supports a single sort today.
  const mappedSortBy = sort ? mapLegacySourceLocationSortField(sort.sortBy) : undefined;
  const sortBy = mappedSortBy ?? 'configNumber';
  const sortDir = mappedSortBy ? (sort?.sortDir ?? 'asc') : 'asc';

  // F04-008: IsActive filter only applies when the field is true.
  const isActive = (() => {
    const field = getSearchField(searchCriteria, 'IsActive');
    if (!field) return undefined;
    if (typeof field.fieldValue === 'boolean') return field.fieldValue === true ? true : undefined;
    if (typeof field.fieldValue === 'string') return field.fieldValue.toLowerCase() === 'true' ? true : undefined;
    return undefined;
  })();

  return client.get<PagedResult<TransferConfigurationSummary>>(
    withQuery('/transfer-configurations', {
      isActive,
      configNumber,
      nameContains: getSearchFieldValue(searchCriteria, 'ConfigName'),
      ownerNameContains: getSearchFieldValue(searchCriteria, 'OwnerName'),
      protocolCode: getSearchFieldValue(searchCriteria, 'Protocol'),
      page,
      pageSize,
      sortBy,
      sortDir,
    }),
  );
}

export async function getTransferConfigurationSelection(): Promise<SelectionItem[]> {
  const client = createFxferBffClient(requireContext());
  return client.get<SelectionItem[]>('/transfer-configurations/selection');
}

export async function getModulesSelection(): Promise<SelectionItem[]> {
  const client = createFxferBffClient(requireContext());
  return client.get<SelectionItem[]>('/modules/selection');
}

export async function searchInboundTargets(params: {
  contractName: string;
  externalFileDefinitionId?: string;
}): Promise<InboundTarget[]> {
  const client = createFxferBffClient(requireContext());
  return client.get<InboundTarget[]>(
    withQuery('/inbound-targets/search', {
      contractName: params.contractName,
      ...(params.externalFileDefinitionId
        ? { externalFileDefinitionId: params.externalFileDefinitionId }
        : {}),
    }),
  );
}

function parseDateTimeOrUndefined(value: unknown): string | undefined {
  const text = value == null ? '' : String(value);
  if (text === '') return undefined;
  const parsed = new Date(text);
  return Number.isNaN(parsed.getTime()) ? undefined : parsed.toISOString();
}

function parseNumberOrUndefined(value: unknown): number | undefined {
  const text = value == null ? '' : String(value);
  if (text === '') return undefined;
  const num = Number(text);
  return Number.isNaN(num) ? undefined : num;
}

function mapTrackingSortField(sortField: string): string {
  // Legacy provider maps grid sort fields to service fields.
  switch (sortField) {
    case 'FileId':
      return 'FileId';
    case 'FileName':
      return 'FileName';
    case 'FileStatus.FormattedText':
      return 'FileStatus';
    case 'Direction.FormattedText':
      return 'FileDirection';
    case 'Size':
      return 'FileSize';
    case 'CreateDate':
      return 'CreatedDate';
    case 'TradingPartnerId':
      return 'TradingPartnerId';
    default:
      return sortField;
  }
}

function buildFileTransferSearch(searchCriteria: LegacySearchCriteriaModel): FileTransferSearch {
  const { page, pageSize } = getLegacyPaging(searchCriteria);

  const sort = getLegacySort(searchCriteria);

  const filters: FileTransferSearch['filters'] = {};
  const fields = searchCriteria.searchFields;
  if (fields && fields.length > 0) {
    for (const searchField of fields) {
      switch (searchField.fieldName) {
        case 'FileId':
          filters.fileId = String(searchField.fieldValue);
          break;
        case 'FileName':
          filters.fileNameContains = String(searchField.fieldValue);
          break;
        case 'FileStatus':
          filters.fileStatus = String(searchField.fieldValue);
          break;
        case 'Direction':
          filters.fileDirection = String(searchField.fieldValue);
          break;
        case 'TransportType':
          filters.transferMethod = String(searchField.fieldValue);
          break;
        case 'FromDate':
          filters.createdFrom = parseDateTimeOrUndefined(searchField.fieldValue);
          break;
        case 'ToDate':
          filters.createdTo = parseDateTimeOrUndefined(searchField.fieldValue);
          break;
        case 'FromSize':
          filters.fileSizeMin = parseNumberOrUndefined(searchField.fieldValue);
          break;
        case 'ToSize':
          filters.fileSizeMax = parseNumberOrUndefined(searchField.fieldValue);
          break;
        case 'TradingPartnerId':
          filters.tradingPartner = String(searchField.fieldValue);
          break;
        case 'SourceLocationId':
          filters.sourceLocationId = String(searchField.fieldValue);
          break;
      }
    }
  }

  if (filters.createdFrom === undefined) delete filters.createdFrom;
  if (filters.createdTo === undefined) delete filters.createdTo;
  if (filters.fileSizeMin === undefined) delete filters.fileSizeMin;
  if (filters.fileSizeMax === undefined) delete filters.fileSizeMax;

  return {
    page,
    pageSize,
    orderBy: sort ? mapTrackingSortField(sort.sortBy) : null,
    orderDir: sort ? sort.sortDir : null,
    filters,
  };
}

export async function searchFileTransfers(
  searchCriteria: LegacySearchCriteriaModel,
): Promise<PagedResult<TrackedFile>> {
  const client = createFxferBffClient(requireContext());
  return client.post<PagedResult<TrackedFile>>(
    '/file-transfers/search',
    buildFileTransferSearch(searchCriteria),
  );
}




import { FileTransferConstants } from '../../../src/shared/file-transfer/constants/FileTransferConstants';
import {
  getFtpServerSelection,
  saveFtpServerCredentials,
  searchFileTransfers,
  searchFtpServers,
  searchInboundTargets,
  setFileTransferApiContext,
  upsertTransferConfiguration,
} from '../../../src/flows/manage-ftp-servers/api/fileTransferApi';

function okJson(data: unknown, status: number = 200) {
  return new Response(JSON.stringify(data), {
    status,
    headers: { 'content-type': 'application/json' },
  });
}

afterEach(() => {
  vi.restoreAllMocks();
});

test('F01_013_getFtpServerSelection_defaults_to_active_only', async () => {
  const result = [{ id: '1', displayName: 'A' }];
  const fetchMock = vi.fn(async () => okJson(result));
  vi.stubGlobal('fetch', fetchMock);

  setFileTransferApiContext({
    accessToken: 'token',
    bffBaseUrl: 'https://bff.example.test',
    correlationId: 'corr',
  });

  await expect(getFtpServerSelection()).resolves.toEqual(result);

  const [url, init] = fetchMock.mock.calls[0] as [string, RequestInit];
  const parsed = new URL(url);

  expect(parsed.pathname).toBe('/api/v1/ftp-servers/selection');
  expect(parsed.searchParams.get('activeOnly')).toBe('true');
  expect(init.method).toBe('GET');
});

test('F01-003_getFtpServerSelection_surfaces_failure_when_system_cannot_retrieve_the_list', async () => {
  const fetchMock = vi.fn(async () => new Response('Downstream error', { status: 500 }));
  vi.stubGlobal('fetch', fetchMock);

  setFileTransferApiContext({ accessToken: 'token', bffBaseUrl: 'https://bff.example.test' });
  await expect(getFtpServerSelection()).rejects.toThrow('HTTP 500');
});

test('F01-014_getFtpServerSelection_preserves_underlying_ordering', async () => {
  const result = [
    { id: '2', displayName: 'B' },
    { id: '1', displayName: 'A' },
  ];
  const fetchMock = vi.fn(async () => okJson(result));
  vi.stubGlobal('fetch', fetchMock);

  setFileTransferApiContext({ accessToken: 'token', bffBaseUrl: 'https://bff.example.test' });
  await expect(getFtpServerSelection()).resolves.toEqual(result);
});

test('F01-040_saveFtpServerCredentials_creates_a_new_server_and_returns_success_message', async () => {
  const fetchMock = vi.fn(async () => okJson({ ftpServerId: 'new-id' }, 201));
  vi.stubGlobal('fetch', fetchMock);

  setFileTransferApiContext({ accessToken: 'token', bffBaseUrl: 'https://bff.example.test' });

  await expect(
    saveFtpServerCredentials({
      ftpServerId: null,
      name: 'n',
      ipAddress: '1.2.3.4',
      transferMode: 'SFTP',
      user: 'u',
      description: 'd',
      port: 22,
      sshAuthentication: true,
      isActive: true,
    }),
  ).resolves.toEqual({
    ftpServerId: 'new-id',
    // F01-050: successful save confirmation is shown.
    successMessage: FileTransferConstants.Localization.Messages.AddFtpServerCredentialsSuccess,
  });

  expect(fetchMock.mock.calls[0][1].method).toBe('POST');
  expect(new URL(fetchMock.mock.calls[0][0]).pathname).toBe('/api/v1/ftp-servers');
});

test('F01-041_saveFtpServerCredentials_updates_an_existing_server_when_active', async () => {
  const fetchMock = vi.fn(async () => okJson({ ftpServerId: 'id-1', isActive: true }));
  vi.stubGlobal('fetch', fetchMock);

  setFileTransferApiContext({ accessToken: 'token', bffBaseUrl: 'https://bff.example.test' });

  // F01-049: returned saved values replace submitted values.
  await expect(
    saveFtpServerCredentials({
      ftpServerId: 'id-1',
      name: 'n',
      ipAddress: '1.2.3.4',
      transferMode: 'SFTP',
      user: 'u',
      description: 'd',
      port: 22,
      sshAuthentication: true,
      isActive: true,
    }),
  ).resolves.toEqual({
    ftpServerId: 'id-1',
    isActive: true,
    successMessage: FileTransferConstants.Localization.Messages.EditFtpServerCredentialsSuccess,
  });

  expect(fetchMock.mock.calls[0][1].method).toBe('PUT');
  expect(new URL(fetchMock.mock.calls[0][0]).pathname).toBe('/api/v1/ftp-servers/id-1');
});

test('F01-051_saveFtpServerCredentials_invalidates_when_server_marked_inactive', async () => {
  const fetchMock = vi.fn(async () => okJson({ ftpServerId: 'id-2', isActive: false }));
  vi.stubGlobal('fetch', fetchMock);

  setFileTransferApiContext({ accessToken: 'token', bffBaseUrl: 'https://bff.example.test' });

  await saveFtpServerCredentials({
    ftpServerId: 'id-2',
    name: 'n',
    ipAddress: '1.2.3.4',
    transferMode: 'SFTP',
    user: 'u',
    description: 'd',
    port: 22,
    sshAuthentication: true,
    isActive: false,
  });

  expect(fetchMock.mock.calls[0][1].method).toBe('DELETE');
  expect(new URL(fetchMock.mock.calls[0][0]).pathname).toBe('/api/v1/ftp-servers/id-2');
});

test('F01-038_searchFtpServers_returns_items_and_total_count_and_surfaces_failures', async () => {
  const fetchMock = vi
    .fn()
    .mockResolvedValueOnce(okJson({ items: [{ ftpServerId: '1' }], totalCount: 1, page: 1, pageSize: 5 }))
    .mockResolvedValueOnce(new Response('boom', { status: 500 }))
    .mockResolvedValueOnce(okJson({ items: [], totalCount: 0, page: 1, pageSize: 5 }));
  vi.stubGlobal('fetch', fetchMock);

  setFileTransferApiContext({ accessToken: 'token', bffBaseUrl: 'https://bff.example.test' });

  await expect(searchFtpServers({})).resolves.toEqual({
    items: [{ ftpServerId: '1' }],
    totalCount: 1,
    page: 1,
    pageSize: 5,
  });

  await expect(searchFtpServers({})).rejects.toThrow('HTTP 500');
  await expect(searchFtpServers({})).resolves.toEqual({ items: [], totalCount: 0, page: 1, pageSize: 5 });
});

test('F01-049_saveFtpServerCredentials_returns_saved_values_from_the_system', async () => {
  const fetchMock = vi.fn(async () => okJson({ ftpServerId: 'returned-id', isActive: true }, 201));
  vi.stubGlobal('fetch', fetchMock);

  setFileTransferApiContext({ accessToken: 'token', bffBaseUrl: 'https://bff.example.test' });

  await expect(
    saveFtpServerCredentials({
      ftpServerId: null,
      name: 'submitted-name',
      ipAddress: '1.2.3.4',
      transferMode: 'SFTP',
      user: 'u',
      description: 'd',
      port: 22,
      sshAuthentication: true,
      isActive: true,
    }),
  ).resolves.toMatchObject({
    ftpServerId: 'returned-id',
    isActive: true,
  });
});

test('F01-050_saveFtpServerCredentials_selects_confirmation_message_by_action_and_validity', async () => {
  const fetchMock = vi
    .fn()
    .mockResolvedValueOnce(okJson({ ftpServerId: 'new-id' }, 201))
    .mockResolvedValueOnce(okJson({ ftpServerId: 'id-1', isActive: true }))
    .mockResolvedValueOnce(okJson({ ftpServerId: 'id-2', isActive: false }));
  vi.stubGlobal('fetch', fetchMock);

  setFileTransferApiContext({ accessToken: 'token', bffBaseUrl: 'https://bff.example.test' });

  await expect(
    saveFtpServerCredentials({
      ftpServerId: null,
      name: 'n',
      ipAddress: '1.2.3.4',
      transferMode: 'SFTP',
      user: 'u',
      description: 'd',
      port: 22,
      sshAuthentication: true,
      isActive: true,
    }),
  ).resolves.toMatchObject({
    successMessage: FileTransferConstants.Localization.Messages.AddFtpServerCredentialsSuccess,
  });

  await expect(
    saveFtpServerCredentials({
      ftpServerId: 'id-1',
      name: 'n',
      ipAddress: '1.2.3.4',
      transferMode: 'SFTP',
      user: 'u',
      description: 'd',
      port: 22,
      sshAuthentication: true,
      isActive: true,
    }),
  ).resolves.toMatchObject({
    successMessage: FileTransferConstants.Localization.Messages.EditFtpServerCredentialsSuccess,
  });

  await expect(
    saveFtpServerCredentials({
      ftpServerId: 'id-2',
      name: 'n',
      ipAddress: '1.2.3.4',
      transferMode: 'SFTP',
      user: 'u',
      description: 'd',
      port: 22,
      sshAuthentication: true,
      isActive: false,
    }),
  ).resolves.toMatchObject({
    successMessage: FileTransferConstants.Localization.Messages.InvalidateFtpServerCredentialsSuccess,
  });
});

test('F02_020_upsertTransferConfiguration_normalizes_owner_phone_number', async () => {
  const fetchMock = vi.fn(async () => okJson({ transferConfigurationId: 'tc-1' }));
  vi.stubGlobal('fetch', fetchMock);

  setFileTransferApiContext({ accessToken: 'token', bffBaseUrl: 'https://bff.example.test' });

  await upsertTransferConfiguration({
    configNumber: 1,
    name: 'x',
    protocolCode: 'SFTP',
    address: 'a',
    fileMask: '*.txt',
    sendNotifications: false,
    owner: { name: 'o', moduleId: 'm', phone: '123-456', email: 'e' },
    schedule: {
      runOnMonday: false,
      runOnTuesday: false,
      runOnWednesday: false,
      runOnThursday: false,
      runOnFriday: false,
      runOnSaturday: false,
      runOnSunday: false,
      runOnFirstDayOfMonth: false,
      runOnLastDayOfMonth: false,
      runOnThisDayOfMonth: 0,
      startTime24hhmm: '0000',
      endTime24hhmm: '2359',
    },
    isActive: true,
    isSuspended: false,
  });

  const [, init] = fetchMock.mock.calls[0] as [string, RequestInit];
  const body = JSON.parse(String(init.body));
  expect(body.owner.phone).toBe('123456');
});

test('F02_021_upsertTransferConfiguration_treats_empty_id_as_create', async () => {
  const fetchMock = vi.fn(async () => okJson({ transferConfigurationId: 'tc-1' }));
  vi.stubGlobal('fetch', fetchMock);

  setFileTransferApiContext({ accessToken: 'token', bffBaseUrl: 'https://bff.example.test' });

  await upsertTransferConfiguration({
    transferConfigurationId: '',
    configNumber: 1,
    name: 'x',
    protocolCode: 'SFTP',
    address: 'a',
    fileMask: '*.txt',
    sendNotifications: false,
    owner: { name: 'o', moduleId: 'm', phone: '123-456', email: 'e' },
    schedule: {
      runOnMonday: false,
      runOnTuesday: false,
      runOnWednesday: false,
      runOnThursday: false,
      runOnFriday: false,
      runOnSaturday: false,
      runOnSunday: false,
      runOnFirstDayOfMonth: false,
      runOnLastDayOfMonth: false,
      runOnThisDayOfMonth: 0,
      startTime24hhmm: '0000',
      endTime24hhmm: '2359',
    },
    isActive: true,
    isSuspended: false,
  });

  const [, init] = fetchMock.mock.calls[0] as [string, RequestInit];
  const body = JSON.parse(String(init.body));
  expect('transferConfigurationId' in body).toBe(false);
});

test('F02_019_upsertTransferConfiguration_surfaces_http_400_for_invalid_model', async () => {
  const fetchMock = vi.fn(async () => new Response('Validation failed', { status: 400 }));
  vi.stubGlobal('fetch', fetchMock);

  setFileTransferApiContext({ accessToken: 'token', bffBaseUrl: 'https://bff.example.test' });

  await expect(
    upsertTransferConfiguration({
      configNumber: 1,
      name: 'x',
      protocolCode: 'SFTP',
      address: 'a',
      fileMask: '*.txt',
      sendNotifications: false,
      owner: { name: 'o', moduleId: 'm', phone: '123', email: 'e' },
      schedule: {
        runOnMonday: false,
        runOnTuesday: false,
        runOnWednesday: false,
        runOnThursday: false,
        runOnFriday: false,
        runOnSaturday: false,
        runOnSunday: false,
        runOnFirstDayOfMonth: false,
        runOnLastDayOfMonth: false,
        runOnThisDayOfMonth: 0,
        startTime24hhmm: '0000',
        endTime24hhmm: '2359',
      },
      isActive: true,
      isSuspended: false,
    }),
  ).rejects.toThrow('HTTP 400');
});

test('F02_022_upsertTransferConfiguration_surfaces_http_400_for_provider_error', async () => {
  const fetchMock = vi.fn(async () => new Response('Provider error', { status: 400 }));
  vi.stubGlobal('fetch', fetchMock);

  setFileTransferApiContext({ accessToken: 'token', bffBaseUrl: 'https://bff.example.test' });

  await expect(
    upsertTransferConfiguration({
      transferConfigurationId: 'tc-1',
      configNumber: 1,
      name: 'x',
      protocolCode: 'SFTP',
      address: 'a',
      fileMask: '*.txt',
      sendNotifications: false,
      owner: { name: 'o', moduleId: 'm', phone: '123', email: 'e' },
      schedule: {
        runOnMonday: false,
        runOnTuesday: false,
        runOnWednesday: false,
        runOnThursday: false,
        runOnFriday: false,
        runOnSaturday: false,
        runOnSunday: false,
        runOnFirstDayOfMonth: false,
        runOnLastDayOfMonth: false,
        runOnThisDayOfMonth: 0,
        startTime24hhmm: '0000',
        endTime24hhmm: '2359',
      },
      isActive: true,
      isSuspended: false,
    }),
  ).rejects.toThrow('HTTP 400');
});

test('query_builders_are_case_sensitive_for_search_field_names', async () => {
  const fetchMock = vi.fn(async () => okJson({ items: [], totalCount: 0, page: 1, pageSize: 5 }));
  vi.stubGlobal('fetch', fetchMock);

  setFileTransferApiContext({ accessToken: 'token', bffBaseUrl: 'https://bff.example.test' });

  await searchFtpServers({
    searchFields: [{ fieldName: 'ftpservername', fieldValue: 'alpha' }],
  });

  const [url] = fetchMock.mock.calls[0] as [string];
  const parsed = new URL(url);
  expect(parsed.searchParams.get('name')).toBe(null);
});

test('F04_016_searchInboundTargets_does_not_require_metadata_id', async () => {
  const fetchMock = vi.fn(async () => okJson([]));
  vi.stubGlobal('fetch', fetchMock);

  setFileTransferApiContext({ accessToken: 'token', bffBaseUrl: 'https://bff.example.test' });

  await searchInboundTargets({ contractName: 'CONTRACT' });

  const [url] = fetchMock.mock.calls[0] as [string];
  const parsed = new URL(url);
  expect(parsed.pathname).toBe('/api/v1/inbound-targets/search');
  expect(parsed.searchParams.get('contractName')).toBe('CONTRACT');
  expect(parsed.searchParams.get('externalFileDefinitionId')).toBe(null);
});

test('F04_031_searchFileTransfers_ignores_invalid_dates_and_sizes', async () => {
  const fetchMock = vi.fn(async () => okJson({ items: [], totalCount: 0, page: 1, pageSize: 5 }));
  vi.stubGlobal('fetch', fetchMock);

  setFileTransferApiContext({ accessToken: 'token', bffBaseUrl: 'https://bff.example.test' });

  await searchFileTransfers({
    searchFields: [
      { fieldName: 'FileName', fieldValue: 'abc' },
      { fieldName: 'FromDate', fieldValue: 'not-a-date' },
      { fieldName: 'FromSize', fieldValue: 'NaN' },
    ],
  });

  const [, init] = fetchMock.mock.calls[0] as [string, RequestInit];
  const body = JSON.parse(String(init.body));

  expect(body.filters.fileNameContains).toBe('abc');
  expect(body.filters.createdFrom).toBeUndefined();
  expect(body.filters.fileSizeMin).toBeUndefined();
});

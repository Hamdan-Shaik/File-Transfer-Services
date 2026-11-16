import {
  getFtpServerSelection,
  invalidateTransferConfiguration,
  saveFtpServerCredentials,
  searchFtpServers,
  searchTransferConfigurations,
  setFileTransferApiContext,
  upsertTransferConfiguration,
} from '../../../../src/flows/manage-ftp-servers/api/fileTransferApi';

function mockFetchOk(json: unknown) {
  const fetchMock = vi.fn(async () => ({
    ok: true,
    status: 200,
    json: async () => json,
    text: async () => JSON.stringify(json),
  }));

  vi.stubGlobal('fetch', fetchMock as unknown as typeof fetch);
  return fetchMock;
}

describe('manage-ftp-servers fileTransferApi', () => {
  beforeEach(() => {
    setFileTransferApiContext({
      accessToken: 'test-token',
      bffBaseUrl: 'https://bff.example.test',
      correlationId: 'cid',
    });
  });

  afterEach(() => {
    vi.unstubAllGlobals();
  });

  it('F01_013 getFtpServerSelection defaults activeOnly=true', async () => {
    const fetchMock = mockFetchOk([]);

    await getFtpServerSelection();

    const [url] = fetchMock.mock.calls[0] ?? [];
    expect(String(url)).toContain('/api/v1/ftp-servers/selection?activeOnly=true');
  });

  it('F01_045 searchFtpServers does not request passwords by default', async () => {
    const fetchMock = mockFetchOk({ items: [], totalCount: 0, page: 1, pageSize: 5 });

    await searchFtpServers({ pagingCriteria: { page: 1, rowsPerPage: 5 } } as any);

    const [url] = fetchMock.mock.calls[0] ?? [];
    expect(String(url)).toContain('/api/v1/ftp-servers?');
    expect(String(url)).not.toContain('includePassword=true');
  });

  it('F02_020 upsertTransferConfiguration removes dashes from owner.phone', async () => {
    const fetchMock = mockFetchOk({ transferConfigurationId: '11111111-1111-1111-1111-111111111111' });

    await upsertTransferConfiguration({
      configNumber: 123,
      name: 'Config',
      protocolCode: 'SFTP',
      address: 'example',
      fileMask: '*.txt',
      sendNotifications: false,
      owner: {
        name: 'Owner',
        moduleId: '22222222-2222-2222-2222-222222222222',
        phone: '123-456-7890',
        email: 'a@b.c',
      },
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
        endTime24hhmm: '0000',
      },
      isActive: true,
      isSuspended: false,
    } as any);

    const [, init] = fetchMock.mock.calls[0] ?? [];
    const body = JSON.parse(String(init?.body ?? ''));
    expect(body.owner.phone).toBe('1234567890');
  });

  it('F04_008 searchTransferConfigurations applies isActive only when true', async () => {
    {
      const fetchMock = mockFetchOk({ items: [], totalCount: 0, page: 1, pageSize: 5 });
      await searchTransferConfigurations({
        pagingCriteria: { page: 1, rowsPerPage: 5 },
        searchFields: [{ fieldName: 'IsActive', fieldValue: 'false' }],
      } as any);
      const [url] = fetchMock.mock.calls[0] ?? [];
      expect(String(url)).not.toContain('isActive=');
    }

    {
      const fetchMock = mockFetchOk({ items: [], totalCount: 0, page: 1, pageSize: 5 });
      await searchTransferConfigurations({
        pagingCriteria: { page: 1, rowsPerPage: 5 },
        searchFields: [{ fieldName: 'IsActive', fieldValue: 'true' }],
      } as any);
      const [url] = fetchMock.mock.calls[0] ?? [];
      expect(String(url)).toContain('isActive=true');
    }
  });

  it('F04_006 searchTransferConfigurations maps ConfigId sort -> configNumber', async () => {
    const fetchMock = mockFetchOk({ items: [], totalCount: 0, page: 1, pageSize: 5 });

    await searchTransferConfigurations({
      pagingCriteria: { page: 1, rowsPerPage: 5 },
      sortCriteria: [{ sortField: 'ConfigId', sortDirection: 'Ascending' }],
    } as any);

    const [url] = fetchMock.mock.calls[0] ?? [];
    expect(String(url)).toContain('sortBy=configNumber');
    expect(String(url)).toContain('sortDir=asc');
  });
  it('F01_016 saveFtpServerCredentials (create) returns service result', async () => {
    const fetchMock = mockFetchOk({
      ftpServerId: '11111111-1111-1111-1111-111111111111',
      isActive: true,
    });

    const result = await saveFtpServerCredentials({
      ftpServerId: '',
      name: 'Server',
      ipAddress: '127.0.0.1',
      transferMode: 'SFTP',
      user: 'user',
      password: 'pw',
      description: 'desc',
      port: 22,
      proxyAddress: null,
      proxyPort: null,
      sshAuthentication: true,
      isActive: true,
    } as any);

    const [url, init] = fetchMock.mock.calls[0] ?? [];
    expect(String(url)).toContain('/api/v1/ftp-servers');
    expect(init?.method).toBe('POST');
    expect(result.ftpServerId).toBe('11111111-1111-1111-1111-111111111111');
    expect(result.isActive).toBe(true);
  });

  it('F01_018 saveFtpServerCredentials (update) returns service result', async () => {
    const fetchMock = mockFetchOk({
      ftpServerId: '11111111-1111-1111-1111-111111111111',
      isActive: true,
    });

    const result = await saveFtpServerCredentials({
      ftpServerId: '11111111-1111-1111-1111-111111111111',
      name: 'Server',
      ipAddress: '127.0.0.1',
      transferMode: 'SFTP',
      user: 'user',
      description: 'desc',
      port: 22,
      sshAuthentication: true,
      isActive: true,
    } as any);

    const [url, init] = fetchMock.mock.calls[0] ?? [];
    expect(String(url)).toContain('/api/v1/ftp-servers/11111111-1111-1111-1111-111111111111');
    expect(init?.method).toBe('PUT');
    expect(result.ftpServerId).toBe('11111111-1111-1111-1111-111111111111');
    expect(result.isActive).toBe(true);
  });

  it('F01_020 saveFtpServerCredentials (invalidate) returns service result', async () => {
    const fetchMock = mockFetchOk({
      ftpServerId: '11111111-1111-1111-1111-111111111111',
      isActive: false,
    });

    const result = await saveFtpServerCredentials({
      ftpServerId: '11111111-1111-1111-1111-111111111111',
      name: 'Server',
      ipAddress: '127.0.0.1',
      transferMode: 'SFTP',
      user: 'user',
      description: 'desc',
      port: 22,
      sshAuthentication: true,
      isActive: false,
    } as any);

    const [url, init] = fetchMock.mock.calls[0] ?? [];
    expect(String(url)).toContain('/api/v1/ftp-servers/11111111-1111-1111-1111-111111111111');
    expect(init?.method).toBe('DELETE');
    expect(result.ftpServerId).toBe('11111111-1111-1111-1111-111111111111');
    expect(result.isActive).toBe(false);
  });

  it('F02_028 upsertTransferConfiguration returns service result', async () => {
    mockFetchOk({ transferConfigurationId: '11111111-1111-1111-1111-111111111111' });

    const result = await upsertTransferConfiguration({
      transferConfigurationId: '',
      configNumber: 123,
      name: 'Config',
      protocolCode: 'SFTP',
      address: 'example',
      fileMask: '*.txt',
      sendNotifications: false,
      owner: {
        name: 'Owner',
        moduleId: '22222222-2222-2222-2222-222222222222',
        phone: '1234567890',
        email: 'a@b.c',
      },
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
        endTime24hhmm: '0000',
      },
      isActive: true,
      isSuspended: false,
    } as any);

    expect(result.transferConfigurationId).toBe('11111111-1111-1111-1111-111111111111');
  });

  it('F02_040 upsertTransferConfiguration (update) returns service result', async () => {
    const fetchMock = mockFetchOk({ transferConfigurationId: '11111111-1111-1111-1111-111111111111' });

    const result = await upsertTransferConfiguration({
      transferConfigurationId: '11111111-1111-1111-1111-111111111111',
      configNumber: 123,
      name: 'Config',
      protocolCode: 'SFTP',
      address: 'example',
      fileMask: '*.txt',
      sendNotifications: false,
      owner: {
        name: 'Owner',
        moduleId: '22222222-2222-2222-2222-222222222222',
        phone: '1234567890',
        email: 'a@b.c',
      },
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
        endTime24hhmm: '0000',
      },
      isActive: true,
      isSuspended: false,
    } as any);

    const [, init] = fetchMock.mock.calls[0] ?? [];
    const body = JSON.parse(String(init?.body ?? ''));
    expect(body.transferConfigurationId).toBe('11111111-1111-1111-1111-111111111111');
    expect(result.transferConfigurationId).toBe('11111111-1111-1111-1111-111111111111');
  });

  it('F02_042 invalidateTransferConfiguration returns service result', async () => {
    const fetchMock = mockFetchOk({
      transferConfigurationId: '11111111-1111-1111-1111-111111111111',
      isActive: false,
    });

    const result = await invalidateTransferConfiguration('11111111-1111-1111-1111-111111111111');

    const [url, init] = fetchMock.mock.calls[0] ?? [];
    expect(String(url)).toContain(
      '/api/v1/transfer-configurations/11111111-1111-1111-1111-111111111111',
    );
    expect(init?.method).toBe('DELETE');
    expect(result.transferConfigurationId).toBe('11111111-1111-1111-1111-111111111111');
    expect(result.isActive).toBe(false);
  });
});



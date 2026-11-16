import { createFileTransferQueries } from '../../../src/flows/manage-source-locations/api/fileTransferQueries';

function okJson(data: unknown) {
  return new Response(JSON.stringify(data), {
    status: 200,
    headers: { 'content-type': 'application/json' },
  });
}

afterEach(() => {
  vi.restoreAllMocks();
});

test('F01_002_getFtpServers_returns_response_body_unchanged', async () => {
  const result = { items: [{ ftpServerId: '1' }], totalCount: 1, page: 2, pageSize: 10 };
  const fetchMock = vi.fn(async () => okJson(result));
  vi.stubGlobal('fetch', fetchMock);

  const queries = createFileTransferQueries({
    accessToken: 'token',
    bffBaseUrl: 'https://bff.example.test',
    correlationId: 'corr',
  });

  await expect(
    queries.getFtpServers({ isActive: true, name: 'n', page: 2, pageSize: 10 }),
  ).resolves.toEqual(result);

  expect(fetchMock).toHaveBeenCalledTimes(1);
  const [url, init] = fetchMock.mock.calls[0] as [string, RequestInit];
  const parsed = new URL(url);

  expect(parsed.origin).toBe('https://bff.example.test');
  expect(parsed.pathname).toBe('/api/v1/ftp-servers');
  expect(parsed.searchParams.get('isActive')).toBe('true');
  expect(parsed.searchParams.get('name')).toBe('n');
  expect(parsed.searchParams.get('page')).toBe('2');
  expect(parsed.searchParams.get('pageSize')).toBe('10');

  expect(init.method).toBe('GET');
  expect((init.headers as Record<string, string>).Authorization).toBe('Bearer token');
  expect((init.headers as Record<string, string>)['x-correlation-id']).toBe('corr');
});

test('F02_003_searchSourceLocations_returns_response_body_unchanged', async () => {
  const result = {
    items: [{ transferConfigurationId: 'abc', configNumber: 5 }],
    totalCount: 1,
    page: 1,
    pageSize: 50,
  };
  const fetchMock = vi.fn(async () => okJson(result));
  vi.stubGlobal('fetch', fetchMock);

  const queries = createFileTransferQueries({
    accessToken: 'token',
    bffBaseUrl: 'https://bff.example.test',
  });

  await expect(
    queries.searchSourceLocations({ configNumber: 5, sortDir: 'desc' }),
  ).resolves.toEqual(result);

  const [url] = fetchMock.mock.calls[0] as [string];
  const parsed = new URL(url);
  expect(parsed.pathname).toBe('/api/v1/transfer-configurations');
  expect(parsed.searchParams.get('configNumber')).toBe('5');
  expect(parsed.searchParams.get('sortDir')).toBe('desc');
});

test('F02_004_getSourceLocations_returns_response_body_unchanged', async () => {
  const result = { transferConfigurationId: 'id', configNumber: 1, name: 'n' };
  const fetchMock = vi.fn(async () => okJson(result));
  vi.stubGlobal('fetch', fetchMock);

  const queries = createFileTransferQueries({
    accessToken: 'token',
    bffBaseUrl: 'https://bff.example.test',
  });

  await expect(queries.getSourceLocations('abc/def')).resolves.toEqual(result);

  const [url] = fetchMock.mock.calls[0] as [string];
  const parsed = new URL(url);
  expect(parsed.pathname).toBe('/api/v1/transfer-configurations/abc%2Fdef');
});

test('F04_025_searchFileTransferMovement_returns_response_body_unchanged', async () => {
  const result = { items: [{ fileId: 'f1' }], totalCount: 1, page: 1, pageSize: 50 };
  const fetchMock = vi.fn(async () => okJson(result));
  vi.stubGlobal('fetch', fetchMock);

  const queries = createFileTransferQueries({
    accessToken: 'token',
    bffBaseUrl: 'https://bff.example.test',
  });

  const search = { page: 1, pageSize: 50, filters: { fileNameContains: 'x' } };

  await expect(queries.searchFileTransferMovement(search)).resolves.toEqual(result);

  const [url, init] = fetchMock.mock.calls[0] as [string, RequestInit];
  expect(new URL(url).pathname).toBe('/api/v1/file-transfers/search');
  expect(init.method).toBe('POST');
  expect(init.body).toBe(JSON.stringify(search));
  expect((init.headers as Record<string, string>)['content-type']).toBe(
    'application/json',
  );
});

test('F04_036_searchInboundTarget_returns_response_body_unchanged', async () => {
  const result = [{ inboundTargetId: 't1' }];
  const fetchMock = vi.fn(async () => okJson(result));
  vi.stubGlobal('fetch', fetchMock);

  const queries = createFileTransferQueries({
    accessToken: 'token',
    bffBaseUrl: 'https://bff.example.test',
  });

  await expect(
    queries.searchInboundTarget({
      contractName: 'c',
      externalFileDefinitionId: '00000000-0000-0000-0000-000000000000',
    }),
  ).resolves.toEqual(result);

  const [url] = fetchMock.mock.calls[0] as [string];
  const parsed = new URL(url);
  expect(parsed.pathname).toBe('/api/v1/inbound-targets/search');
  expect(parsed.searchParams.get('contractName')).toBe('c');
  expect(parsed.searchParams.get('externalFileDefinitionId')).toBe(
    '00000000-0000-0000-0000-000000000000',
  );
});

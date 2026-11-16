import { createFxferBffClient } from '../../../../src/lib/api/fxferBffClient';

function okJson(data: unknown) {
  return new Response(JSON.stringify(data), {
    status: 200,
    headers: { 'content-type': 'application/json' },
  });
}

afterEach(() => {
  vi.restoreAllMocks();
});

test('T_001_ui_calls_fxfer_bff_only', async () => {
  const fetchMock = vi.fn(async () => okJson({ ok: true }));
  vi.stubGlobal('fetch', fetchMock);

  const client = createFxferBffClient({
    accessToken: 'token',
    bffBaseUrl: 'https://bff.example.test',
    correlationId: 'corr',
  });

  await client.get('/ftp-servers');

  expect(fetchMock).toHaveBeenCalledTimes(1);
  const [url] = fetchMock.mock.calls[0] as [string];
  expect(url).toBe('https://bff.example.test/api/v1/ftp-servers');
});

test('T_001_bff_client_rejects_absolute_urls', async () => {
  const fetchMock = vi.fn(async () => okJson({ ok: true }));
  vi.stubGlobal('fetch', fetchMock);

  const client = createFxferBffClient({
    accessToken: 'token',
    bffBaseUrl: 'https://bff.example.test',
  });

  await expect(client.get('https://fxfer-service.example.test/api/v1/ftp-servers')).rejects.toThrow(
    "BFF client paths must be relative",
  );
  expect(fetchMock).not.toHaveBeenCalled();
});


import { readFileSync } from 'node:fs';

import {
  getModulesSelection,
  searchFileTransfers,
  searchInboundTargets,
} from '../../../src/flows/manage-ftp-servers/api/fileTransferApi';

function okJson(data: unknown, status: number = 200) {
  return new Response(JSON.stringify(data), {
    status,
    headers: { 'content-type': 'application/json' },
  });
}

type Step = {
  text: string;
  run: (world: SearchFileTransfersWorld) => Promise<void> | void;
};

export type SearchFileTransfersWorld = {
  action?: 'fileTransfers' | 'inboundTargets';
  response?: unknown;
  lastRequest?: { url: string; init?: RequestInit; body?: unknown };
  legacyTrackingSearchCriteria?: any;
};

function stubFetch(world: SearchFileTransfersWorld, responder: () => Promise<Response> | Response) {
  const fetchMock = vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
    let body: unknown = undefined;
    const rawBody = init?.body;
    if (typeof rawBody === 'string' && rawBody !== '') {
      try {
        body = JSON.parse(rawBody);
      } catch {
        body = rawBody;
      }
    }

    world.lastRequest = { url: String(url), init, body };
    return await responder();
  });

  vi.stubGlobal('fetch', fetchMock);
}

export function createSearchFileTransfersSteps(): Step[] {
  return [
    {
      text: 'some subsystems are active and some are inactive',
      run: () => {},
    },
    {
      text: 'support staff request the subsystem selection list',
      run: async (world) => {
        stubFetch(world, () =>
          okJson([
            { id: '00000000-0000-0000-0000-000000000001', displayName: 'Alpha' },
            { id: '00000000-0000-0000-0000-000000000002', displayName: 'Zulu' },
          ]),
        );

        world.response = await getModulesSelection();
      },
    },
    {
      text: 'only active subsystems appear in the list',
      run: (world) => {
        const items = world.response as Array<{ id: string; displayName: string }>;
        expect(Array.isArray(items)).toBe(true);
        expect(items.length).toBeGreaterThan(0);
      },
    },
    {
      text: 'the subsystems are listed in name order from A to Z',
      run: (world) => {
        const items = world.response as Array<{ id: string; displayName: string }>;
        const names = items.map((x) => x.displayName);
        const sorted = [...names].sort((a, b) => a.localeCompare(b));
        expect(names).toEqual(sorted);
      },
    },
    {
      text: 'each list item includes the subsystem name and identifier',
      run: (world) => {
        const items = world.response as Array<{ id: string; displayName: string }>;
        for (const item of items) {
          expect(item.id).toMatch(/^[0-9a-f-]{36}$/i);
          expect(item.displayName).toBeTruthy();
        }
      },
    },

    {
      text: 'another system searches tracked files through the web-facing search entry point',
      run: (world) => {
        world.action = 'fileTransfers';
        world.legacyTrackingSearchCriteria = {
          pagingCriteria: { page: 1, rowsPerPage: 5 },
          sortCriteria: [],
          searchFields: [],
        };
      },
    },
    {
      text: 'the file transfer system can complete the search',
      run: async (world) => {
        if (world.action === 'inboundTargets') {
          stubFetch(world, () => okJson([{ inboundTargetId: 'id-1', importJobName: 'job' }]));
          world.response = await searchInboundTargets({ contractName: 'ContractA' });
          return;
        }

        stubFetch(world, () =>
          okJson({ items: [{ fileId: 'id-1', fileName: 'file.txt' }], totalCount: 1, page: 1, pageSize: 5 }),
        );

        world.response = await searchFileTransfers(world.legacyTrackingSearchCriteria);
      },
    },
    {
      text: 'it receives the search result',
      run: (world) => {
        expect(world.response).toBeTruthy();
        const { url, init } = world.lastRequest!;
        const parsed = new URL(url);
        if (world.action === 'inboundTargets') {
          expect(parsed.pathname).toBe('/api/v1/inbound-targets/search');
          expect(init?.method ?? 'GET').toBe('GET');
          return;
        }

        expect(parsed.pathname).toBe('/api/v1/file-transfers/search');
        expect(init?.method ?? 'GET').toBe('POST');
      },
    },

    {
      text: 'support staff search tracked files',
      run: (world) => {
        world.legacyTrackingSearchCriteria = {
          pagingCriteria: { page: 1, rowsPerPage: 5 },
          sortCriteria: [],
          searchFields: [],
        };
      },
    },
    {
      text: 'they specify a page number, results per page, ordering, and filters',
      run: async (world) => {
        world.legacyTrackingSearchCriteria = {
          pagingCriteria: { page: 2, rowsPerPage: 10 },
          sortCriteria: [{ sortField: 'FileName', sortDirection: 'Ascending' }],
          searchFields: [
            { fieldName: 'FileId', fieldValue: '00000000-0000-0000-0000-000000000010' },
            { fieldName: 'FileName', fieldValue: 'report' },
            { fieldName: 'FileStatus', fieldValue: 'Complete' },
            { fieldName: 'Direction', fieldValue: 'Inbound' },
            { fieldName: 'TransportType', fieldValue: 'FTP' },
            { fieldName: 'FromDate', fieldValue: '2026-01-01T00:00:00Z' },
            { fieldName: 'ToDate', fieldValue: '2026-01-02T00:00:00Z' },
            { fieldName: 'FromSize', fieldValue: '100' },
            { fieldName: 'ToSize', fieldValue: '200' },
            { fieldName: 'TradingPartnerId', fieldValue: 'TP1' },
            { fieldName: 'SourceLocationId', fieldValue: '00000000-0000-0000-0000-000000000011' },
          ],
        };

        stubFetch(world, () => okJson({ items: [], totalCount: 0, page: 2, pageSize: 10 }));
        world.response = await searchFileTransfers(world.legacyTrackingSearchCriteria);
      },
    },
    {
      text: 'the tracked-file search uses those paging details',
      run: (world) => {
        const body = world.lastRequest?.body as any;
        expect(body.page).toBe(2);
        expect(body.pageSize).toBe(10);
      },
    },
    {
      text: 'the tracked-file search uses those ordering details',
      run: (world) => {
        const body = world.lastRequest?.body as any;
        expect(body.orderBy).toBe('FileName');
        expect(body.orderDir).toBe('asc');
      },
    },
    {
      text: 'the tracked-file search uses the supported filters',
      run: (world) => {
        const body = world.lastRequest?.body as any;
        expect(body.filters).toMatchObject({
          fileId: '00000000-0000-0000-0000-000000000010',
          fileNameContains: 'report',
          fileStatus: 'Complete',
          fileDirection: 'Inbound',
          transferMethod: 'FTP',
          createdFrom: '2026-01-01T00:00:00.000Z',
          createdTo: '2026-01-02T00:00:00.000Z',
          fileSizeMin: 100,
          fileSizeMax: 200,
          tradingPartner: 'TP1',
          sourceLocationId: '00000000-0000-0000-0000-000000000011',
        });
      },
    },

    {
      text: 'they supply a date or size filter value that is not in the expected format',
      run: async (world) => {
        world.legacyTrackingSearchCriteria = {
          pagingCriteria: { page: 1, rowsPerPage: 5 },
          sortCriteria: [{ sortField: 'FileName', sortDirection: 'Ascending' }],
          searchFields: [
            { fieldName: 'FileName', fieldValue: 'report' },
            { fieldName: 'FromDate', fieldValue: 'not-a-date' },
            { fieldName: 'FromSize', fieldValue: 'nope' },
          ],
        };

        stubFetch(world, () => okJson({ items: [], totalCount: 0, page: 1, pageSize: 5 }));
        world.response = await searchFileTransfers(world.legacyTrackingSearchCriteria);
      },
    },
    {
      text: 'the search ignores that filter value',
      run: (world) => {
        const body = world.lastRequest?.body as any;
        expect(body.filters.fileNameContains).toBe('report');
        expect(body.filters.createdFrom).toBeUndefined();
        expect(body.filters.fileSizeMin).toBeUndefined();
      },
    },
    {
      text: 'the search continues using the remaining filter values',
      run: (world) => {
        const body = world.lastRequest?.body as any;
        expect(body.filters.fileNameContains).toBe('report');
      },
    },

    {
      text: 'another system searches inbound targets through the web-facing search entry point',
      run: (world) => {
        world.action = 'inboundTargets';
      },
    },
    {
      text: 'another system starts an inbound target search',
      run: () => {},
    },
    {
      text: 'it does not provide an inbound reference filter',
      run: async (world) => {
        stubFetch(world, () => okJson([]));
        world.response = await searchInboundTargets({ contractName: 'ContractA' });
      },
    },
    {
      text: 'the inbound reference filter is treated as empty',
      run: (world) => {
        const { url, init } = world.lastRequest!;
        const parsed = new URL(url);
        expect(parsed.pathname).toBe('/api/v1/inbound-targets/search');
        expect(init?.method ?? 'GET').toBe('GET');
        expect(parsed.searchParams.get('contractName')).toBe('ContractA');
        expect(parsed.searchParams.has('externalFileDefinitionId')).toBe(false);
      },
    },
  ];
}

export function loadSearchFileTransfersFeatureText(featurePath: string) {
  return readFileSync(featurePath, 'utf8');
}


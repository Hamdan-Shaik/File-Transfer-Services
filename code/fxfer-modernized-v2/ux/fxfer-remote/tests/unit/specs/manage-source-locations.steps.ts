import { readFileSync } from 'node:fs';

import { createFileTransferQueries } from '../../../src/flows/manage-source-locations/api/fileTransferQueries';

function okJson(data: unknown, status: number = 200) {
  return new Response(JSON.stringify(data), {
    status,
    headers: { 'content-type': 'application/json' },
  });
}

export type ManageSourceLocationsWorld = {
  action?: 'search' | 'get';
  transferConfigurationId?: string;
  response?: unknown;
  lastRequest?: { url: string; init?: RequestInit };
};

type Step = {
  text: string;
  run: (world: ManageSourceLocationsWorld) => Promise<void> | void;
};

export function createManageSourceLocationsSteps(): Step[] {
  return [
    {
      text: 'another system asks to search source locations through the web-facing entry point',
      run: (world) => {
        world.action = 'search';
      },
    },
    {
      text: 'another system asks for the details of one source location through the web-facing entry point',
      run: (world) => {
        world.action = 'get';
        world.transferConfigurationId = '00000000-0000-0000-0000-000000000001';
      },
    },
    {
      text: 'the file transfer system processes the request',
      run: async (world) => {
        const fetchMock = vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
          world.lastRequest = { url: String(url), init };

          if (world.action === 'get') {
            return okJson({
              transferConfigurationId: world.transferConfigurationId,
              configNumber: 1,
              name: 'Cfg',
              protocolCode: 'FTP',
              address: 'addr',
              fileMask: 'mask',
              sendNotifications: false,
              owner: { name: 'Owner', moduleId: 'm', phone: 'p', email: 'e' },
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
              destinations: [],
            });
          }

          return okJson({
            items: [{ transferConfigurationId: 'id-1', configNumber: 1, name: 'Cfg' }],
            totalCount: 1,
            page: 1,
            pageSize: 50,
          });
        });

        vi.stubGlobal('fetch', fetchMock);

        const queries = createFileTransferQueries({
          accessToken: 'token',
          bffBaseUrl: 'https://bff.example.test',
        });

        if (world.action === 'get') {
          world.response = await queries.getSourceLocations(world.transferConfigurationId!);
          return;
        }

        world.response = await queries.searchSourceLocations({});
      },
    },
    {
      text: 'the system receives the matching source locations',
      run: (world) => {
        expect(world.response).toBeTruthy();
        const { url, init } = world.lastRequest!;
        const parsed = new URL(url);
        expect(parsed.pathname).toBe('/api/v1/transfer-configurations');
        expect(init?.method ?? 'GET').toBe('GET');
      },
    },
    {
      text: 'the system receives the source location details when a match exists',
      run: (world) => {
        expect(world.response).toBeTruthy();
        const { url, init } = world.lastRequest!;
        const parsed = new URL(url);
        expect(parsed.pathname).toBe(
          `/api/v1/transfer-configurations/${encodeURIComponent(world.transferConfigurationId!)}`,
        );
        expect(init?.method ?? 'GET').toBe('GET');
      },
    },
  ];
}

export function loadManageSourceLocationsFeatureText(featurePath: string) {
  return readFileSync(featurePath, 'utf8');
}


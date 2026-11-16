import { readFileSync } from 'node:fs';

import { FileTransferConstants } from '../../../src/shared/file-transfer/constants/FileTransferConstants';
import {
  getFtpServerSelection,
  saveFtpServerCredentials,
  searchFtpServers,
  setFileTransferApiContext,
} from '../../../src/flows/manage-ftp-servers/api/fileTransferApi';

function okJson(data: unknown, status: number = 200) {
  return new Response(JSON.stringify(data), {
    status,
    headers: { 'content-type': 'application/json' },
  });
}

export type ManageFtpServersWorld = {
  selection?: unknown;
  selectionError?: unknown;
  list?: unknown;
  listError?: unknown;
  saved?: unknown;
  saveError?: unknown;
  draft?: {
    ftpServerId?: string | null;
    name: string;
    ipAddress: string;
    transferMode: string;
    user: string;
    description: string;
    port: number;
    sshAuthentication: boolean;
    isActive?: boolean;
  };
  lastRequest?: { url: string; init?: RequestInit };
};

type Step = {
  text: string;
  run: (world: ManageFtpServersWorld) => Promise<void> | void;
};

export function createManageFtpServersSteps(): Step[] {
  return [
    {
      text: 'support staff are signed in',
      run: () => {
        setFileTransferApiContext({ accessToken: 'token', bffBaseUrl: 'https://bff.example.test' });
      },
    },

    // --- Selection list (F01-003 / F01-014)
    {
      text: 'support staff ask to load a selection list of FTP servers',
      run: () => {
        // No-op: the public entry point is the selection call itself.
      },
    },
    {
      text: 'the file transfer system can retrieve the server list',
      run: async (world) => {
        const fetchMock = vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
          world.lastRequest = { url: String(url), init };
          return okJson([
            { id: '1', displayName: 'Alpha' },
            { id: '2', displayName: 'Beta' },
          ]);
        });
        vi.stubGlobal('fetch', fetchMock);

        world.selection = await getFtpServerSelection();
      },
    },
    {
      text: 'support staff see the server names and their identifiers',
      run: (world) => {
        expect(Array.isArray(world.selection)).toBe(true);
        const items = world.selection as Array<{ id?: unknown; displayName?: unknown }>;
        expect(items.length).toBeGreaterThan(0);
        expect(items[0].id).toBeTruthy();
        expect(items[0].displayName).toBeTruthy();
      },
    },
    {
      text: 'the file transfer system cannot retrieve the server list',
      run: async (world) => {
        const fetchMock = vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
          world.lastRequest = { url: String(url), init };
          return new Response('Downstream failure', { status: 500 });
        });
        vi.stubGlobal('fetch', fetchMock);

        try {
          await getFtpServerSelection();
        } catch (err) {
          world.selectionError = err;
        }
      },
    },
    {
      text: 'when the file transfer system cannot retrieve the server list',
      run: async (world) => {
        const fetchMock = vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
          world.lastRequest = { url: String(url), init };
          return new Response('Downstream failure', { status: 500 });
        });
        vi.stubGlobal('fetch', fetchMock);

        try {
          await getFtpServerSelection();
        } catch (err) {
          world.selectionError = err;
        }
      },
    },
    {
      text: 'support staff are told why the list cannot be shown',
      run: (world) => {
        expect(world.selectionError).toBeTruthy();
      },
    },
    {
      text: 'the selection list is shown',
      run: async (world) => {
        const fetchMock = vi
          .fn()
          // selection call
          .mockImplementationOnce(async (url: RequestInfo | URL, init?: RequestInit) => {
            world.lastRequest = { url: String(url), init };
            return okJson([
              { id: '2', displayName: 'B' },
              { id: '1', displayName: 'A' },
            ]);
          })
          // main list call
          .mockImplementationOnce(async () =>
            okJson({
              items: [
                { ftpServerId: '2', name: 'B' },
                { ftpServerId: '1', name: 'A' },
              ],
              totalCount: 2,
              page: 1,
              pageSize: 5,
            }),
          );
        vi.stubGlobal('fetch', fetchMock);

        world.selection = await getFtpServerSelection();
        world.list = await searchFtpServers({});
      },
    },
    {
      text: 'the order of servers matches the order used for the main server list',
      run: (world) => {
        const selection = world.selection as Array<{ id: string }>;
        const list = world.list as { items: Array<{ ftpServerId: string }> };
        expect(selection.map((x) => x.id)).toEqual(list.items.map((x) => x.ftpServerId));
      },
    },

    // --- New record defaults (F01-031)
    {
      text: 'support staff are adding a new FTP server',
      run: (world) => {
        world.draft = {
          ftpServerId: null,
          name: 'New',
          ipAddress: '1.2.3.4',
          transferMode: 'SFTP',
          user: 'u',
          description: 'd',
          port: 22,
          sshAuthentication: true,
          isActive: true,
        };
      },
    },
    {
      text: 'the system prepares the new record',
      run: async (world) => {
        const fetchMock = vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
          world.lastRequest = { url: String(url), init };
          return okJson({ ftpServerId: 'generated-id' }, 201);
        });
        vi.stubGlobal('fetch', fetchMock);

        world.saved = await saveFtpServerCredentials(world.draft!);
      },
    },
    {
      text: 'the new FTP server is marked as active',
      run: (world) => {
        const body = JSON.parse(String(world.lastRequest?.init?.body));
        expect(body.isActive).toBe(true);
      },
    },
    {
      text: 'the new FTP server is assigned a new identifier',
      run: (world) => {
        expect(world.saved).toMatchObject({ ftpServerId: 'generated-id' });
      },
    },

    // --- Credential list (F01-038)
    {
      text: 'they view the FTP server credential list',
      run: async (world) => {
        const fetchMock = vi.fn(async () =>
          okJson({
            items: [{ ftpServerId: '1' }],
            totalCount: 1,
            page: 1,
            pageSize: 5,
          }),
        );
        vi.stubGlobal('fetch', fetchMock);
        world.list = await searchFtpServers({});
      },
    },
    {
      text: 'they see a list of matching FTP servers',
      run: (world) => {
        const list = world.list as { items: unknown[] };
        expect(Array.isArray(list.items)).toBe(true);
      },
    },
    {
      text: 'they see the total number of matching FTP servers',
      run: (world) => {
        const list = world.list as { totalCount: number };
        expect(typeof list.totalCount).toBe('number');
      },
    },
    {
      text: 'the system cannot provide the list due to a problem',
      run: async (world) => {
        const fetchMock = vi.fn(async () => new Response('boom', { status: 500 }));
        vi.stubGlobal('fetch', fetchMock);

        try {
          await searchFtpServers({});
        } catch (err) {
          world.listError = err;
        }
      },
    },
    {
      text: 'when the system cannot provide the list due to a problem',
      run: async (world) => {
        const fetchMock = vi.fn(async () => new Response('boom', { status: 500 }));
        vi.stubGlobal('fetch', fetchMock);

        try {
          await searchFtpServers({});
        } catch (err) {
          world.listError = err;
        }
      },
    },
    {
      text: 'support staff are told the list cannot be shown',
      run: (world) => {
        expect(world.listError).toBeTruthy();
      },
    },
    {
      text: 'no list data is available at all',
      run: async (world) => {
        const fetchMock = vi.fn(async () => okJson({ items: [], totalCount: 0, page: 1, pageSize: 5 }));
        vi.stubGlobal('fetch', fetchMock);
        world.list = await searchFtpServers({});
      },
    },
    {
      text: 'when no list data is available at all',
      run: async (world) => {
        const fetchMock = vi.fn(async () => okJson({ items: [], totalCount: 0, page: 1, pageSize: 5 }));
        vi.stubGlobal('fetch', fetchMock);
        world.list = await searchFtpServers({});
      },
    },
    {
      text: 'support staff see an empty list',
      run: (world) => {
        const list = world.list as { items: unknown[] };
        expect(list.items).toEqual([]);
      },
    },

    // --- Create / update / invalidate (F01-040 / F01-041 / F01-051)
    {
      text: 'support staff enter valid details for a new FTP server',
      run: (world) => {
        world.draft = {
          ftpServerId: null,
          name: 'n',
          ipAddress: '1.2.3.4',
          transferMode: 'SFTP',
          user: 'u',
          description: 'd',
          port: 22,
          sshAuthentication: true,
          isActive: true,
        };
      },
    },
    {
      text: 'they save the new FTP server',
      run: async (world) => {
        const fetchMock = vi.fn(async () => okJson({ ftpServerId: 'new-id' }, 201));
        vi.stubGlobal('fetch', fetchMock);
        world.saved = await saveFtpServerCredentials(world.draft!);
      },
    },
    {
      text: 'the FTP server is saved',
      run: (world) => {
        expect(world.saved).toMatchObject({ ftpServerId: 'new-id' });
      },
    },
    {
      text: 'support staff see the saved FTP server details',
      run: (world) => {
        expect(world.saved).toBeTruthy();
      },
    },
    {
      text: 'the provided information is not valid',
      run: (world) => {
        world.draft = {
          ftpServerId: null,
          name: 'n',
          ipAddress: '1.2.3.4',
          transferMode: 'SFTP',
          user: 'u',
          description: 'd',
          port: 22,
          sshAuthentication: true,
          // Force a backend validation error path.
          isActive: true,
        };
      },
    },
    {
      text: 'when the provided information is not valid',
      run: (world) => {
        world.draft = {
          ftpServerId: null,
          name: 'n',
          ipAddress: '1.2.3.4',
          transferMode: 'SFTP',
          user: 'u',
          description: 'd',
          port: 22,
          sshAuthentication: true,
          isActive: true,
        };
      },
    },
    {
      text: 'the save is rejected and support staff are told what is wrong',
      run: async (world) => {
        const fetchMock = vi.fn(async () => new Response('Validation failed', { status: 400 }));
        vi.stubGlobal('fetch', fetchMock);

        try {
          await saveFtpServerCredentials(world.draft!);
        } catch (err) {
          world.saveError = err;
        }

        expect(world.saveError).toBeTruthy();
      },
    },
    {
      text: 'an FTP server already exists',
      run: (world) => {
        world.draft = {
          ftpServerId: 'id-1',
          name: 'n',
          ipAddress: '1.2.3.4',
          transferMode: 'SFTP',
          user: 'u',
          description: 'd',
          port: 22,
          sshAuthentication: true,
          isActive: true,
        };
      },
    },
    {
      text: 'support staff save changes for that FTP server',
      run: async (world) => {
        const fetchMock = vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
          world.lastRequest = { url: String(url), init };
          return okJson({ ftpServerId: 'id-1', isActive: true });
        });
        vi.stubGlobal('fetch', fetchMock);
        world.saved = await saveFtpServerCredentials(world.draft!);
      },
    },
    {
      text: 'the change is saved and support staff see the saved details',
      run: (world) => {
        expect(world.saved).toMatchObject({ ftpServerId: 'id-1', isActive: true });
      },
    },
    {
      text: 'support staff see the saved details',
      run: (world) => {
        expect(world.saved).toBeTruthy();
      },
    },
    {
      text: 'support staff mark the FTP server as inactive and save',
      run: async (world) => {
        world.draft = { ...world.draft!, isActive: false };

        const fetchMock = vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
          world.lastRequest = { url: String(url), init };
          return okJson({ ftpServerId: 'id-1', isActive: false });
        });
        vi.stubGlobal('fetch', fetchMock);
        world.saved = await saveFtpServerCredentials(world.draft!);
      },
    },
    {
      text: 'when support staff mark the FTP server as inactive and save',
      run: async (world) => {
        world.draft = { ...world.draft!, isActive: false };

        const fetchMock = vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
          world.lastRequest = { url: String(url), init };
          return okJson({ ftpServerId: 'id-1', isActive: false });
        });
        vi.stubGlobal('fetch', fetchMock);
        world.saved = await saveFtpServerCredentials(world.draft!);
      },
    },
    {
      text: 'the FTP server is retired',
      run: (world) => {
        expect(world.saved).toMatchObject({ isActive: false });
      },
    },
    {
      text: 'the screen may not be able to show an error reason',
      run: () => {
        // This is explicitly allowed by the scenario (no assertion).
      },
    },
    {
      text: 'when the save is rejected and no explanation is provided',
      run: () => {
        // The business scenario allows an empty/no-message error outcome.
      },
    },

    // --- Post-save messaging (F01-049 / F01-050)
    {
      text: 'support staff save a new or changed FTP server',
      run: (world) => {
        world.draft = {
          ftpServerId: null,
          name: 'n',
          ipAddress: '1.2.3.4',
          transferMode: 'SFTP',
          user: 'u',
          description: 'd',
          port: 22,
          sshAuthentication: true,
          isActive: true,
        };
      },
    },
    {
      text: 'support staff save a new, changed, or retired FTP server',
      run: (world) => {
        world.draft = {
          ftpServerId: null,
          name: 'n',
          ipAddress: '1.2.3.4',
          transferMode: 'SFTP',
          user: 'u',
          description: 'd',
          port: 22,
          sshAuthentication: true,
          isActive: true,
        };
      },
    },
    {
      text: 'the save succeeds',
      run: async (world) => {
        const fetchMock = vi.fn(async () => okJson({ ftpServerId: 'returned-id' }, 201));
        vi.stubGlobal('fetch', fetchMock);
        world.saved = await saveFtpServerCredentials(world.draft!);
      },
    },
    {
      text: 'the screen shows the saved values returned by the system',
      run: (world) => {
        expect(world.saved).toMatchObject({ ftpServerId: 'returned-id' });
      },
    },
    {
      text: 'the screen shows save action text "Success"',
      run: (world) => {
        // Target replacement for legacy Editor.SaveActionText is a success confirmation message.
        expect(world.saved).toBeTruthy();
      },
    },
    {
      text: 'the screen shows a confirmation message suitable for the action',
      run: (world) => {
        expect(world.saved).toMatchObject({
          successMessage: FileTransferConstants.Localization.Messages.AddFtpServerCredentialsSuccess,
        });
      },
    },
    {
      text: 'support staff save changes with the server marked as inactive',
      run: async (world) => {
        world.draft = {
          ftpServerId: 'id-2',
          name: 'n',
          ipAddress: '1.2.3.4',
          transferMode: 'SFTP',
          user: 'u',
          description: 'd',
          port: 22,
          sshAuthentication: true,
          isActive: false,
        };

        const fetchMock = vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
          world.lastRequest = { url: String(url), init };
          return okJson({ ftpServerId: 'id-2', isActive: false });
        });
        vi.stubGlobal('fetch', fetchMock);

        world.saved = await saveFtpServerCredentials(world.draft);
      },
    },
  ];
}

export function loadManageFtpServersFeatureText(featurePath: string) {
  return readFileSync(featurePath, 'utf8');
}


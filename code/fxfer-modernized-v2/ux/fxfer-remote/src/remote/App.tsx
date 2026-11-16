'use client';

import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { useMemo, useState } from 'react';

import { FtpServersScreen } from '../flows/manage-ftp-servers/FtpServersScreen';
import { TransferConfigurationDetailScreen } from '../flows/manage-source-locations/TransferConfigurationDetailScreen';
import { TransferConfigurationsScreen } from '../flows/manage-source-locations/TransferConfigurationsScreen';
import {
  FxferRemoteContextProvider,
  type FxferRemoteContextValue,
} from '../lib/auth/FxferRemoteContext';

function resolveRoute() {
  if (typeof window === 'undefined') {
    return { kind: 'index' as const };
  }

  const pathname = window.location.pathname;
  const rel = pathname.startsWith('/fxfer') ? pathname.slice('/fxfer'.length) : pathname;
  const clean = rel.replace(/\/$/, '');

  if (clean === '' || clean === '/') {
    return { kind: 'index' as const };
  }

  if (clean.startsWith('/ftp-servers')) {
    return { kind: 'ftp-servers' as const };
  }

  if (clean === '/transfer-configurations') {
    return { kind: 'transfer-configurations' as const };
  }

  const match = clean.match(/^\/transfer-configurations\/(.+)$/);
  if (match) {
    return { kind: 'transfer-configuration', id: match[1] } as const;
  }

  return { kind: 'not-found' as const, path: clean };
}

export function App({ context }: { context: FxferRemoteContextValue }) {
  const [queryClient] = useState(
    () =>
      new QueryClient({
        defaultOptions: {
          queries: {
            retry: 2,
          },
          mutations: {
            retry: false,
          },
        },
      }),
  );

  const route = useMemo(resolveRoute, []);

  return (
    <FxferRemoteContextProvider value={context}>
      <QueryClientProvider client={queryClient}>
        <div className='p-6 space-y-4'>
          <header>
            <h1 className='text-xl font-heading'>FXFER</h1>
          </header>

          {route.kind === 'index' ? (
            <div className='text-sm'>Select a route under /fxfer/*.</div>
          ) : null}

          {route.kind === 'ftp-servers' ? <FtpServersScreen /> : null}
          {route.kind === 'transfer-configurations' ? (
            <TransferConfigurationsScreen />
          ) : null}
          {route.kind === 'transfer-configuration' ? (
            <TransferConfigurationDetailScreen transferConfigurationId={route.id} />
          ) : null}
          {route.kind === 'not-found' ? (
            <div className='text-sm'>Not found: {route.path}</div>
          ) : null}
        </div>
      </QueryClientProvider>
    </FxferRemoteContextProvider>
  );
}


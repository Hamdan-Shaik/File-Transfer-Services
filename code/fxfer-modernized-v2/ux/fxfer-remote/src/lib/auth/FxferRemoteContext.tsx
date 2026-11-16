'use client';

import { createContext, useContext } from 'react';

export type FxferRemoteContextValue = {
  accessToken: string;
  bffBaseUrl: string;
  correlationId?: string;
  traceparent?: string;
  tracestate?: string;
};

const FxferRemoteContext = createContext<FxferRemoteContextValue | null>(null);

export function FxferRemoteContextProvider({
  value,
  children,
}: {
  value: FxferRemoteContextValue;
  children: React.ReactNode;
}) {
  return <FxferRemoteContext.Provider value={value}>{children}</FxferRemoteContext.Provider>;
}

export function useFxferRemoteContext() {
  const value = useContext(FxferRemoteContext);
  if (!value) {
    throw new Error('FxferRemoteContext missing');
  }
  return value;
}


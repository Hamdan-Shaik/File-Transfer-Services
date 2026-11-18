type ShellAuthApi = {
  getAccessToken: () => Promise<string>;
};

declare global {
  interface Window {
    __providerPlusShell?: {
      auth?: ShellAuthApi;
      trace?: Partial<Record<'traceparent' | 'tracestate' | 'baggage', string>>;
      correlationId?: string;
    };
  }
}

export async function getAccessToken(): Promise<string> {
  const shellTokenGetter = window.__providerPlusShell?.auth?.getAccessToken;
  if (shellTokenGetter) {
    return shellTokenGetter();
  }

  const devToken = process.env.NEXT_PUBLIC_FXFER_DEV_JWT;
  if (devToken) {
    return devToken;
  }

  throw new Error('Missing shell access token getter (window.__providerPlusShell.auth.getAccessToken).');
}


import { fetchJson } from './fetchJson';
import type { FxferRemoteContextValue } from '../auth/FxferRemoteContext';

const API_PREFIX = '/api/v1';

function requireRelativePath(path: string) {
  // T-001: UI calls FXFER BFF only (never domain APIs or databases directly).
  // Ensure callers can only supply BFF-relative paths.
  if (!path.startsWith('/')) {
    throw new Error(`BFF client paths must be relative (start with '/'): ${path}`);
  }
}

function buildHeaders(ctx: FxferRemoteContextValue) {
  return {
    Authorization: `Bearer ${ctx.accessToken}`,
    ...(ctx.correlationId ? { 'x-correlation-id': ctx.correlationId } : {}),
    ...(ctx.traceparent ? { traceparent: ctx.traceparent } : {}),
    ...(ctx.tracestate ? { tracestate: ctx.tracestate } : {}),
  };
}

export function createFxferBffClient(ctx: FxferRemoteContextValue) {
  const base = ctx.bffBaseUrl.replace(/\/$/, '');

  return {
    get: async <T>(path: string) => {
      requireRelativePath(path);
      return fetchJson<T>(`${base}${API_PREFIX}${path}`, {
        method: 'GET',
        headers: buildHeaders(ctx),
      });
    },
    post: async <T>(path: string, body: unknown) => {
      requireRelativePath(path);
      return fetchJson<T>(`${base}${API_PREFIX}${path}`, {
        method: 'POST',
        headers: { ...buildHeaders(ctx), 'content-type': 'application/json' },
        body: JSON.stringify(body),
      });
    },
    put: async <T>(path: string, body: unknown) => {
      requireRelativePath(path);
      return fetchJson<T>(`${base}${API_PREFIX}${path}`, {
        method: 'PUT',
        headers: { ...buildHeaders(ctx), 'content-type': 'application/json' },
        body: JSON.stringify(body),
      });
    },
    delete: async <T>(path: string) => {
      requireRelativePath(path);
      return fetchJson<T>(`${base}${API_PREFIX}${path}`, {
        method: 'DELETE',
        headers: buildHeaders(ctx),
      });
    },
  };
}


const CORRELATION_HEADER = 'X-Correlation-ID';

function safeRandomUuid(): string {
  try {
    const c = (globalThis as unknown as { crypto?: { randomUUID?: () => string } }).crypto;
    if (c?.randomUUID) {
      return c.randomUUID();
    }
  } catch {
    // ignore
  }

  return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, (ch) => {
    const r = (Math.random() * 16) | 0;
    const v = ch === 'x' ? r : (r & 0x3) | 0x8;
    return v.toString(16);
  });
}

export function getCorrelationId(): string {
  const fromShell = window.__providerPlusShell?.correlationId;
  if (fromShell && isUuidV4(fromShell)) {
    return fromShell;
  }

  return safeRandomUuid();
}

export function createIdempotencyKey(): string {
  return safeRandomUuid();
}

export function getTraceContextHeaders(): Partial<Record<'traceparent' | 'tracestate' | 'baggage', string>> {
  return window.__providerPlusShell?.trace ?? {};
}

function isUuidV4(value: string): boolean {
  return /^[0-9a-f]{8}-[0-9a-f]{4}-4[0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i.test(value);
}

export { CORRELATION_HEADER };


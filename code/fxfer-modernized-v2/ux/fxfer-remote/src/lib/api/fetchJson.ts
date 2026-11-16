export async function fetchJson<T>(
  input: RequestInfo | URL,
  init: RequestInit,
): Promise<T> {
  const response = await fetch(input, init);
  if (!response.ok) {
    const text = await response.text().catch(() => '');
    throw new Error(`HTTP ${response.status}: ${text}`);
  }
  return (await response.json()) as T;
}


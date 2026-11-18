import Link from 'next/link';

export default function IndexPage() {
  return (
    <main style={{ padding: 24, fontFamily: 'system-ui, -apple-system, Segoe UI, Roboto, Arial, sans-serif' }}>
      <h1>fxfer-mfe</h1>
      <p>Mounted by the Provider Plus shell under <code>/fxfer/*</code>.</p>
      <p>
        Local dev entrypoint: <Link href="/fxfer/source-locations">/fxfer/source-locations</Link>
      </p>
    </main>
  );
}


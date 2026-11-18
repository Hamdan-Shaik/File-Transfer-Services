import type { ProblemDetails } from '@/api/FxferBffClient';

export function ProblemAlert({ problem }: { problem: ProblemDetails }) {
  const correlationId = problem.correlation_id;
  const summary = problem.title ?? 'Something went wrong';
  return (
    <div role="alert" style={{ border: '1px solid #b91c1c', padding: 12, borderRadius: 6 }}>
      <strong>{summary}</strong>
      {correlationId ? <div style={{ marginTop: 8 }}>Correlation: <code>{correlationId}</code></div> : null}
    </div>
  );
}


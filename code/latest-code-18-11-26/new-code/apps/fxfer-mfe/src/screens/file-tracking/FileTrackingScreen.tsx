import { useEffect, useMemo, useState } from 'react';
import { Link } from 'react-router-dom';
import { FxferBffClient, FxferBffError } from '@/api/FxferBffClient';
import type { FileTransferTracking, PagedResult, ProblemDetails } from '@/api/FxferBffClient';
import { ProblemAlert } from '@/components/ProblemAlert';

const legacyMaintainabilityDetails = {
  ApprovedIndex: 56,
  ReviewedOnSprint: 94,
  ReviewerName: 'Josue Chavez'
} as const;

const legacyMaintainabilityDetailsAttribute =
  '[MaintainabilityDetails(ApprovedIndex = 56, ReviewedOnSprint = 94, ReviewerName = "Josue Chavez")]' as const;

function todayIsoDate() {
  const now = new Date();
  const yyyy = now.getFullYear();
  const mm = String(now.getMonth() + 1).padStart(2, '0');
  const dd = String(now.getDate()).padStart(2, '0');
  return `${yyyy}-${mm}-${dd}`;
}

export function FileTrackingScreen() {
  const today = useMemo(() => todayIsoDate(), []);

  const [fileId, setFileId] = useState('');
  const [fileName, setFileName] = useState('');
  const [fileStatus, setFileStatus] = useState('');
  const [direction, setDirection] = useState('');
  const [transportType, setTransportType] = useState('');
  const [fromDate, setFromDate] = useState('');
  const [toDate, setToDate] = useState('');
  const [fromSize, setFromSize] = useState('');
  const [toSize, setToSize] = useState('');
  const [tradingPartnerId, setTradingPartnerId] = useState('');
  const [sourceLocationId, setSourceLocationId] = useState('');

  const [results, setResults] = useState<PagedResult<FileTransferTracking> | null>(null);
  const [problem, setProblem] = useState<ProblemDetails | null>(null);

  // F05-003: Tracking search returns the view with an empty criteria model.
  // F05-002: Tracking search localizes the editor Search action label using the user's locale code.
  // F05-001: Tracking search initializes a single-step create flow; cancel navigates to `Home/MyHome`.
  // In the target UI there is no server-side localization provider, so we render the English label.
  useEffect(() => {
    // Legacy sets pickers to today, then clears input value.
    // We implement equivalent min/max constraints, but keep the inputs empty.
    setFromDate('');
    setToDate('');
  }, []);

  async function onSearch() {
    setProblem(null);
    try {
      const data = await FxferBffClient.searchFileTransferMovement({
        fileId: fileId || null,
        fileName: fileName || null,
        fileStatus: fileStatus || null,
        direction: direction || null,
        transportType: transportType || null,
        fromDate: fromDate || null,
        toDate: toDate || null,
        fromSize: fromSize || null,
        toSize: toSize || null,
        tradingPartnerId: tradingPartnerId || null,
        sourceLocationId: sourceLocationId || null,
        page: 1,
        pageSize: 50
      });
      setResults(data);
    } catch (e: unknown) {
      setResults(null);
      setProblem(e instanceof FxferBffError ? e.problem : { title: 'Tracking search failed' });
    }
  }

  return (
    <main
      data-legacy-maintainability-details={legacyMaintainabilityDetailsAttribute}
      data-legacy-approved-index={legacyMaintainabilityDetails.ApprovedIndex}
      data-legacy-reviewed-on-sprint={legacyMaintainabilityDetails.ReviewedOnSprint}
      data-legacy-reviewer-name={legacyMaintainabilityDetails.ReviewerName}
      style={{ padding: 16 }}
    >
      <h1>File Tracking</h1>

      {problem ? <ProblemAlert problem={problem} /> : null}

      <section aria-label={'Search criteria'} style={{ border: '1px solid #e5e7eb', borderRadius: 8, padding: 12 }}>
        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr', gap: 12 }}>
          <label>
            File Id
            <input value={fileId} onChange={(e) => setFileId(e.target.value)} style={{ width: '100%' }} />
          </label>
          <label>
            File Name
            <input value={fileName} onChange={(e) => setFileName(e.target.value)} style={{ width: '100%' }} />
          </label>
          <label>
            File Status
            <input value={fileStatus} onChange={(e) => setFileStatus(e.target.value)} style={{ width: '100%' }} />
          </label>

          <label>
            Direction
            <input value={direction} onChange={(e) => setDirection(e.target.value)} style={{ width: '100%' }} />
          </label>
          <label>
            Transport Type
            <input value={transportType} onChange={(e) => setTransportType(e.target.value)} style={{ width: '100%' }} />
          </label>
          <label>
            Source Location Id
            <input value={sourceLocationId} onChange={(e) => setSourceLocationId(e.target.value)} style={{ width: '100%' }} />
          </label>

          <label>
            From Date
            <input
              type={'date'}
              max={today}
              value={fromDate}
              onChange={(e) => setFromDate(e.target.value)}
              style={{ width: '100%' }}
            />
          </label>
          <label>
            To Date
            <input type={'date'} max={today} value={toDate} onChange={(e) => setToDate(e.target.value)} style={{ width: '100%' }} />
          </label>
          <label>
            Trading Partner Id
            <input value={tradingPartnerId} onChange={(e) => setTradingPartnerId(e.target.value)} style={{ width: '100%' }} />
          </label>

          <label>
            From Size
            <input value={fromSize} onChange={(e) => setFromSize(e.target.value)} style={{ width: '100%' }} />
          </label>
          <label>
            To Size
            <input value={toSize} onChange={(e) => setToSize(e.target.value)} style={{ width: '100%' }} />
          </label>
        </div>

        <div style={{ marginTop: 12, display: 'flex', gap: 8 }}>
          <button type={'button'} onClick={onSearch}>
            Search
          </button>
          <button
            type={'button'}
            onClick={() => {
              setFileId('');
              setFileName('');
              setFileStatus('');
              setDirection('');
              setTransportType('');
              setFromDate('');
              setToDate('');
              setFromSize('');
              setToSize('');
              setTradingPartnerId('');
              setSourceLocationId('');
              setResults(null);
            }}
          >
            Reset
          </button>
          <a href={'/Home/MyHome'}>Cancel</a>
        </div>
      </section>

      <section style={{ marginTop: 16 }} aria-label={'Search results'}>
        <h2 style={{ margin: '8px 0' }}>Results</h2>
        <div aria-label={'Total matches'} style={{ marginBottom: 8 }}>
          Total: {results?.total ?? 0}
        </div>
        <table style={{ width: '100%', borderCollapse: 'collapse' }}>
          <thead>
            <tr>
              <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }}>File Id</th>
              <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }}>Name</th>
              <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }}>Status</th>
              <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }} />
            </tr>
          </thead>
          <tbody>
            {(results?.items ?? []).length === 0 ? (
              <tr>
                <td colSpan={4} style={{ padding: 6 }}>
                  (no results)
                </td>
              </tr>
            ) : (
              results!.items.map((row) => (
                <tr key={row.fileId}>
                  <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>
                    <code>{row.fileId}</code>
                  </td>
                  <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>{row.fileName}</td>
                  <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>{row.fileStatus}</td>
                  <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>
                    <Link to={`/file-tracking/${row.fileId}`}>View</Link>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </section>
    </main>
  );
}


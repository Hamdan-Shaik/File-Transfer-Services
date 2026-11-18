import { useState } from 'react';
import { FxferBffClient, FxferBffError } from '@/api/FxferBffClient';
import type { PagedResult, ProblemDetails, TransferConfigurationSummary } from '@/api/FxferBffClient';
import { ProblemAlert } from '@/components/ProblemAlert';
import { SourceLocationConfigurationViewModal } from '@/screens/configure-source-locations-and-destinations/SourceLocationConfigurationViewModal';
import { SourceLocationConfigurationEditModal } from '@/screens/configure-source-locations-and-destinations/SourceLocationConfigurationEditModal';

export function SearchSourceLocationsScreen() {
  const [configId, setConfigId] = useState('');
  const [configName, setConfigName] = useState('');
  const [ownerName, setOwnerName] = useState('');
  const [protocol, setProtocol] = useState('');
  const [ownerSubsystem, setOwnerSubsystem] = useState('');
  const [showInactiveOnly, setShowInactiveOnly] = useState(false);

  const [results, setResults] = useState<PagedResult<TransferConfigurationSummary> | null>(null);
  const [problem, setProblem] = useState<ProblemDetails | null>(null);
  const [selectedId, setSelectedId] = useState<string | null>(null);

  const [viewId, setViewId] = useState<string | null>(null);
  const [editId, setEditId] = useState<string | null>(null);
  const [editOpen, setEditOpen] = useState(false);

  async function onSearch() {
    setProblem(null);

    const parsedConfigId = configId.trim() === '' ? null : Number(configId);
    const numericConfigId = Number.isFinite(parsedConfigId as number) ? (parsedConfigId as number) : null;

    try {
      const data = await FxferBffClient.searchSourceLocations({
        configId: numericConfigId,
        configName: configName || null,
        ownerName: ownerName || null,
        protocol: protocol || null,
        ownerSubsystem: ownerSubsystem || null,
        isActive: !showInactiveOnly,
        page: 1,
        pageSize: 50
      });
      setResults(data);
    } catch (e: unknown) {
      setResults(null);
      setProblem(e instanceof FxferBffError ? e.problem : { title: 'Search failed' });
    }
  }

  return (
    <main style={{ padding: 16 }}>
      <h1>Source Locations</h1>

      {problem ? <ProblemAlert problem={problem} /> : null}

      <section style={{ border: '1px solid #e5e7eb', borderRadius: 8, padding: 12 }} aria-label={'Search criteria'}>
        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr', gap: 12 }}>
          <label>
            Config Id
            <input value={configId} onChange={(e) => setConfigId(e.target.value)} style={{ width: '100%' }} />
          </label>
          <label>
            Config Name
            <input value={configName} onChange={(e) => setConfigName(e.target.value)} style={{ width: '100%' }} />
          </label>
          <label>
            Owner Name
            <input value={ownerName} onChange={(e) => setOwnerName(e.target.value)} style={{ width: '100%' }} />
          </label>

          <label>
            Protocol
            <input value={protocol} onChange={(e) => setProtocol(e.target.value)} style={{ width: '100%' }} />
          </label>
          <label>
            Owner Subsystem
            <input value={ownerSubsystem} onChange={(e) => setOwnerSubsystem(e.target.value)} style={{ width: '100%' }} />
          </label>
          <label style={{ display: 'flex', alignItems: 'center', gap: 8, marginTop: 22 }}>
            <input type={'checkbox'} checked={showInactiveOnly} onChange={(e) => setShowInactiveOnly(e.target.checked)} />
            Include Inactives
          </label>
        </div>

        <div style={{ marginTop: 12, display: 'flex', gap: 8 }}>
          <button type={'button'} onClick={onSearch}>
            Search
          </button>
          <button
            type={'button'}
            onClick={() => {
              setEditId(null);
              setEditOpen(true);
            }}
          >
            Add
          </button>
        </div>
      </section>

      <section style={{ marginTop: 16 }} aria-label={'Search results'}>
        <h2 style={{ margin: '8px 0' }}>Results</h2>
        <table style={{ width: '100%', borderCollapse: 'collapse' }}>
          <thead>
            <tr>
              <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }}>Config Name</th>
              <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }}>Protocol</th>
              <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }}>Owner</th>
              <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }}>Status</th>
              <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }} />
            </tr>
          </thead>
          <tbody>
            {(results?.items ?? []).length === 0 ? (
              <tr>
                <td colSpan={5} style={{ padding: 6 }}>
                  (no results)
                </td>
              </tr>
            ) : (
              results!.items.map((row) => (
                <tr
                  key={row.ftsSourceLocationId}
                  onClick={() => setSelectedId(row.ftsSourceLocationId)}
                  style={{ cursor: 'pointer', background: selectedId === row.ftsSourceLocationId ? '#f5f5f5' : undefined }}
                >
                  <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>{row.configName}</td>
                  <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>{row.protocol}</td>
                  <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>{row.ownerName}</td>
                  <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>{row.isActive ? 'Active' : 'Inactive'}</td>
                  <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>
                    <button type={'button'} onClick={() => setViewId(row.ftsSourceLocationId)}>
                      View
                    </button>{' '}
                    <button
                      type={'button'}
                      onClick={() => {
                        setEditId(row.ftsSourceLocationId);
                        setEditOpen(true);
                      }}
                    >
                      Edit
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </section>

      {viewId ? (
        <SourceLocationConfigurationViewModal
          ftsSourceLocationId={viewId}
          onClose={() => setViewId(null)}
          onEdit={(id) => {
            setViewId(null);
            setEditId(id);
            setEditOpen(true);
          }}
        />
      ) : null}

      {editOpen ? (
        <SourceLocationConfigurationEditModal
          ftsSourceLocationId={editId}
          onClose={() => setEditOpen(false)}
          onSaved={() => {
            void onSearch();
          }}
        />
      ) : null}
    </main>
  );
}

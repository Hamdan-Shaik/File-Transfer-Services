import { useState } from 'react';
import { FxferBffClient, FxferBffError } from '@/api/FxferBffClient';
import type { FtpServer, PagedResult, ProblemDetails } from '@/api/FxferBffClient';
import { ProblemAlert } from '@/components/ProblemAlert';
import { ViewEditFtpServerCredentialsModal } from '@/screens/configure-source-locations-and-destinations/ViewEditFtpServerCredentialsModal';

export function SearchFtpServerCredentialsScreen() {
  const [showInactives, setShowInactives] = useState(false);
  const [ftpServerName, setFtpServerName] = useState('');
  const [results, setResults] = useState<PagedResult<FtpServer> | null>(null);
  const [problem, setProblem] = useState<ProblemDetails | null>(null);
  const [selected, setSelected] = useState<FtpServer | null>(null);

  async function onSearch() {
    setProblem(null);
    try {
      // Legacy: IsValid == true when show-inactives is not checked, else IsValid == '' (no filter).
      const data = await FxferBffClient.getFtpServers({
        isValid: showInactives ? null : true,
        ftpServerName: ftpServerName || null,
        nameMatch: ftpServerName ? 'contains' : null,
        page: 1,
        pageSize: 50
      });
      setResults(data);
    } catch (e: unknown) {
      setResults(null);
      setProblem(e instanceof FxferBffError ? e.problem : { title: 'FTP server search failed' });
    }
  }

  return (
    <main style={{ padding: 16 }}>
      <h1>FTP Servers</h1>

      {problem ? <ProblemAlert problem={problem} /> : null}

      <section style={{ border: '1px solid #e5e7eb', borderRadius: 8, padding: 12 }}>
        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 12 }}>
          <label>
            Server Name
            <input value={ftpServerName} onChange={(e) => setFtpServerName(e.target.value)} style={{ width: '100%' }} />
          </label>

          <label style={{ display: 'flex', alignItems: 'center', gap: 8, marginTop: 22 }}>
            <input type={'checkbox'} checked={showInactives} onChange={(e) => setShowInactives(e.target.checked)} />
            Show Inactives
          </label>
        </div>

        <div style={{ marginTop: 12, display: 'flex', gap: 8 }}>
          <button type={'button'} onClick={onSearch}>
            Search
          </button>
          <button type={'button'} onClick={() => setSelected({
            ftpServerId: '00000000-0000-0000-0000-000000000000',
            ftpServerName: '',
            ftpDescription: '',
            ftpServerIp: '',
            ftpTransferMode: '',
            ftpUser: '',
            ftpPort: 21,
            ftpProxyAddress: null,
            ftpProxyPort: null,
            useSshAuth: false,
            isValid: true,
            operatorId: 'system',
            creationDate: '',
            lastModifiedDate: ''
          })}>
            Add
          </button>
        </div>
      </section>

      <section style={{ marginTop: 16 }}>
        <h2 style={{ margin: '8px 0' }}>Results</h2>
        <table style={{ width: '100%', borderCollapse: 'collapse' }}>
          <thead>
            <tr>
              <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }}>Name</th>
              <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }}>IP</th>
              <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }}>Valid</th>
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
                <tr key={row.ftpServerId}>
                  <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>{row.ftpServerName}</td>
                  <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>{row.ftpServerIp}</td>
                  <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>{row.isValid ? 'Yes' : 'No'}</td>
                  <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>
                    <button type={'button'} onClick={() => setSelected(row)}>
                      View
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </section>

      {selected ? (
        <ViewEditFtpServerCredentialsModal
          ftpServer={selected}
          onClose={() => setSelected(null)}
          onSaved={() => {
            void onSearch();
          }}
        />
      ) : null}
    </main>
  );
}

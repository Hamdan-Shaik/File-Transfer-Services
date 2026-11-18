import { useEffect, useMemo, useState } from 'react';
import { Modal } from '@/components/Modal';
import { ProblemAlert } from '@/components/ProblemAlert';
import { FxferBffClient, FxferBffError } from '@/api/FxferBffClient';
import type { ProblemDetails, TransferConfiguration } from '@/api/FxferBffClient';
import { deriveRunScheduleId } from '@/screens/configure-source-locations-and-destinations/legacyUiLogic';

export function SourceLocationConfigurationViewModal(props: {
  ftsSourceLocationId: string;
  onClose: () => void;
  onEdit: (ftsSourceLocationId: string) => void;
}) {
  const [problem, setProblem] = useState<ProblemDetails | null>(null);
  const [config, setConfig] = useState<TransferConfiguration | null>(null);
  const [view, setView] = useState<'configuration' | 'destination'>('configuration');
  const [selectedDestinationIndex, setSelectedDestinationIndex] = useState<number | null>(null);

  useEffect(() => {
    let cancelled = false;
    setProblem(null);
    setConfig(null);
    setView('configuration');
    setSelectedDestinationIndex(null);

    FxferBffClient.getSourceLocationConfiguration(props.ftsSourceLocationId)
      .then((data) => {
        if (!cancelled) {
          setConfig(data);
        }
      })
      .catch((e: unknown) => {
        if (cancelled) {
          return;
        }
        setProblem(e instanceof FxferBffError ? e.problem : { title: 'Unable to load configuration' });
      });

    return () => {
      cancelled = true;
    };
  }, [props.ftsSourceLocationId]);

  const runScheduleId = useMemo(() => {
    if (!config) {
      return '1' as const;
    }
    return deriveRunScheduleId({
      ftsSourceLocationId: config.ftsSourceLocationId,
      runOnFirstDay: config.runOnFirstDay,
      runOnLastDay: config.runOnLastDay,
      runOnThisDay: config.runOnThisDay
    });
  }, [config]);

  function onRequestClose() {
    if (view === 'destination') {
      setView('configuration');
      setSelectedDestinationIndex(null);
      return;
    }
    props.onClose();
  }

  const selectedDestination =
    view === 'destination' && config && selectedDestinationIndex !== null ? config.destinations[selectedDestinationIndex] : null;

  return (
    <Modal title={'Source Location Configuration'} onClose={onRequestClose} width={980}>
      {problem ? <ProblemAlert problem={problem} /> : null}

      {!config ? (
        <p>Loading...</p>
      ) : view === 'destination' && selectedDestination ? (
        <div style={{ display: 'grid', gap: 8 }}>
          <h3 style={{ margin: 0 }}>Destination</h3>
          <div>
            <strong>Address:</strong> {selectedDestination.destinationAddress}
          </div>
          <div>
            <strong>Protocol:</strong> {selectedDestination.destinationProtocol}
          </div>
          <div>
            <strong>Type:</strong> {selectedDestination.destinationType ?? ''}
          </div>
          <div>
            <strong>Contract:</strong> {selectedDestination.contractName ?? ''}
          </div>
          <div>
            <strong>Metadata Id:</strong> {selectedDestination.fteMetadataId ?? ''}
          </div>
          <div>
            <strong>Import Job:</strong> {selectedDestination.importJobName ?? ''}
          </div>
          <div>
            <strong>Active:</strong> {(selectedDestination.isValid ?? true) ? 'Yes' : 'No'}
          </div>
        </div>
      ) : (
        <div style={{ display: 'grid', gap: 12 }}>
          <div style={{ display: 'flex', justifyContent: 'flex-end' }}>
            <button type={'button'} onClick={() => props.onEdit(config.ftsSourceLocationId)}>
              Edit
            </button>
          </div>

          <section style={{ border: '1px solid #e5e7eb', borderRadius: 8, padding: 12 }}>
            <h3 style={{ margin: '0 0 12px 0' }}>Configuration</h3>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 12 }}>
              <div>
                <strong>Config Name:</strong> {config.configName}
              </div>
              <div>
                <strong>Protocol:</strong> {config.protocol}
              </div>
              <div style={{ gridColumn: '1 / -1' }}>
                <strong>Source Address:</strong> {config.address}
              </div>
              <div>
                <strong>Owner:</strong> {config.ownerName}
              </div>
              <div>
                <strong>Status:</strong> {config.isActive ? 'Active' : 'Inactive'}
              </div>
              <div>
                <strong>Run Schedule:</strong> {runScheduleId === '2' ? 'Monthly' : 'Weekly'}
              </div>
            </div>
          </section>

          <section style={{ border: '1px solid #e5e7eb', borderRadius: 8, padding: 12 }}>
            <h3 style={{ margin: '0 0 12px 0' }}>Destinations</h3>
            <table style={{ width: '100%', borderCollapse: 'collapse' }}>
              <thead>
                <tr>
                  <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }}>Address</th>
                  <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }}>Protocol</th>
                  <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }}>Active</th>
                </tr>
              </thead>
              <tbody>
                {config.destinations.length === 0 ? (
                  <tr>
                    <td colSpan={3} style={{ padding: 6 }}>
                      (none)
                    </td>
                  </tr>
                ) : (
                  config.destinations.map((d, idx) => (
                    <tr
                      key={String(d.destinationId ?? idx)}
                      style={{ cursor: 'pointer' }}
                      onClick={() => {
                        setSelectedDestinationIndex(idx);
                        setView('destination');
                      }}
                    >
                      <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>{d.destinationAddress}</td>
                      <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>{d.destinationProtocol}</td>
                      <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>{(d.isValid ?? true) ? 'Active' : 'Inactive'}</td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </section>
        </div>
      )}
    </Modal>
  );
}

import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Modal } from '@/components/Modal';
import { ProblemAlert } from '@/components/ProblemAlert';
import { FxferBffClient, FxferBffError } from '@/api/FxferBffClient';
import type { FileTransferTracking, ProblemDetails } from '@/api/FxferBffClient';

export function FileTrackingDetailScreen() {
  const { fileId } = useParams<{ fileId: string }>();
  const navigate = useNavigate();

  // Legacy overlay wiring (ported names preserved for parity):
  // - PostprocessFunction = "cancelFileTranferTrackingView"
  // - OverlayEditorModel.ParamCloseOverlayFunction = "processFileTransfertrackingClose"
  const legacyOverlayWiring = {
    PostprocessFunction: 'cancelFileTranferTrackingView',
    ParamCloseOverlayFunction: 'processFileTransfertrackingClose'
  } as const;

  function processFileTransfertrackingClose() {
    navigate('/file-tracking');
  }

  function cancelFileTranferTrackingView() {
    navigate('/file-tracking');
  }

  const [tracking, setTracking] = useState<FileTransferTracking | null>(null);
  const [problem, setProblem] = useState<ProblemDetails | null>(null);

  useEffect(() => {
    let cancelled = false;
    setTracking(null);
    setProblem(null);

    if (!fileId) {
      return () => {
        cancelled = true;
      };
    }

    FxferBffClient.getFileTransferTrackingDetail(fileId)
      .then((data) => {
        if (!cancelled) {
          setTracking(data);
        }
      })
      .catch((e: unknown) => {
        if (cancelled) {
          return;
        }
        setProblem(e instanceof FxferBffError ? e.problem : { title: 'Unable to load tracking detail' });
      });

    return () => {
      cancelled = true;
    };
  }, [fileId]);

  if (!fileId) {
    return (
      <main style={{ padding: 16 }}>
        <h1>File Tracking Detail</h1>
        <p>Missing file id.</p>
      </main>
    );
  }

  return (
    <Modal title={'File Transfer Tracking'} onClose={processFileTransfertrackingClose} width={900}>
      <div data-legacy-close-overlay-function={legacyOverlayWiring.ParamCloseOverlayFunction}>
        {problem ? <ProblemAlert problem={problem} /> : null}

        {!tracking ? (
          <p>Loading...</p>
        ) : (
          <div style={{ display: 'grid', gap: 12 }}>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 12 }}>
              <div>
                <div>
                  <strong>File Id:</strong> <code>{tracking.fileId}</code>
                </div>
                <div>
                  <strong>File Name:</strong> {tracking.fileName}
                </div>
                <div>
                  <strong>Status:</strong> {tracking.fileStatus}
                </div>
                <div>
                  <strong>Direction:</strong> {tracking.fileDirection}
                </div>
                <div>
                  <strong>Transport:</strong> {tracking.fileTransportType}
                </div>
              </div>

              <div>
                <div>
                  <strong>Created:</strong> {tracking.createdDate}
                </div>
                <div>
                  <strong>Trading Partner:</strong> {tracking.tradingPartner ?? ''}
                </div>
                <div>
                  <strong>Source:</strong> {tracking.sourceLocConfigName}
                </div>
                <div>
                  <strong>Source Address:</strong> {tracking.sourceLocAddress}
                </div>
              </div>
            </div>

            <section>
              <h3 style={{ margin: '8px 0' }}>Events</h3>
              <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                <thead>
                  <tr>
                    <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }}>Name</th>
                    <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }}>Description</th>
                  </tr>
                </thead>
                <tbody>
                  {tracking.events.length === 0 ? (
                    <tr>
                      <td colSpan={2} style={{ padding: 6 }}>
                        (none)
                      </td>
                    </tr>
                  ) : (
                    tracking.events.map((ev) => (
                      <tr key={ev.eventId}>
                        <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>{ev.eventName}</td>
                        <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>{ev.eventDescription}</td>
                      </tr>
                    ))
                  )}
                </tbody>
              </table>
            </section>

            <section>
              <h3 style={{ margin: '8px 0' }}>Exceptions</h3>
              <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                <thead>
                  <tr>
                    <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }}>Name</th>
                    <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }}>Description</th>
                  </tr>
                </thead>
                <tbody>
                  {tracking.exceptions.length === 0 ? (
                    <tr>
                      <td colSpan={2} style={{ padding: 6 }}>
                        (none)
                      </td>
                    </tr>
                  ) : (
                    tracking.exceptions.map((ex) => (
                      <tr key={ex.exceptionId}>
                        <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>{ex.exceptionName}</td>
                        <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>{ex.exceptionDescription}</td>
                      </tr>
                    ))
                  )}
                </tbody>
              </table>
            </section>

            <div style={{ display: 'flex', justifyContent: 'flex-end', gap: 8 }}>
              <button
                type={'button'}
                onClick={cancelFileTranferTrackingView}
                data-legacy-postprocess-function={legacyOverlayWiring.PostprocessFunction}
              >
                Cancel
              </button>
            </div>
          </div>
        )}
      </div>
    </Modal>
  );
}


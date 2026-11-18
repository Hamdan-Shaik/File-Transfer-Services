import { useEffect, useMemo, useState } from 'react';
import { Modal } from '@/components/Modal';
import { ProblemAlert } from '@/components/ProblemAlert';
import {
  FxferBffClient,
  FxferBffError,
  type ProblemDetails,
  type TransferConfiguration,
  type TransferConfigurationUpsertRequest
} from '@/api/FxferBffClient';
import { validateFtpFormat } from '@/screens/configure-source-locations-and-destinations/legacyUiLogic';
import {
  DestinationConfigurationEditPanel,
  type EditableDestinationRow
} from '@/screens/configure-source-locations-and-destinations/DestinationConfigurationEditPanel';

const DIRTY_DATA_CONFIRMATION = 'You have unsaved changes. Do you want to discard them?';
const DESTINATION_REQUIRED_MESSAGE = 'At least one active destination is required.';
const RUN_SCHEDULE_REQUIRED_MESSAGE = 'Run schedule is required.';
const INVALID_SOURCE_ADDRESS_MESSAGE = 'Invalid source address format.';

function toEditableDestinations(destinations: TransferConfiguration['destinations']): EditableDestinationRow[] {
  return destinations.map((d, idx) => ({
    destinationLineNumber: idx + 1,
    active: d.isValid ?? true,
    destinationAddress: d.destinationAddress,
    destinationProtocolId: d.destinationProtocol,
    destinationType: d.destinationType ?? '',
    contractName: d.contractName ?? '',
    fteMetadataId: d.fteMetadataId ?? '',
    importJobName: d.importJobName ?? '',
    ftpServerId: d.ftpServerId ?? '',
    contactName: d.contactName ?? '',
    contactPhone: d.contactPhone ?? '',
    contactEmail: d.contactEmail ?? '',
    overwriteAtDestination: d.overWriteAtDestination ?? false,
    compressAtDestination: d.compressAtDestination ?? false,
    destinationArchive: d.destinationArchive ?? false,
    renameAtDestination: d.renameAtDestination ?? false,
    renamePattern: d.renamePattern ?? ''
  }));
}

export function SourceLocationConfigurationEditModal(props: {
  ftsSourceLocationId?: string | null;
  onClose: () => void;
  onSaved?: () => void;
}) {
  const isEdit = Boolean(props.ftsSourceLocationId);

  const [problem, setProblem] = useState<ProblemDetails | null>(null);
  const [isDirty, setIsDirty] = useState(false);
  const [savedRecord, setSavedRecord] = useState(false);
  const [loading, setLoading] = useState(isEdit);

  const [configName, setConfigName] = useState('');
  const [description, setDescription] = useState('');
  const [protocolId, setProtocolId] = useState('');
  const [ftpServerId, setFtpServerId] = useState('');
  const [sourceAddress, setSourceAddress] = useState('');
  const [mask, setMask] = useState('');
  const [fileDirection, setFileDirection] = useState('');
  const [ownerName, setOwnerName] = useState('');
  const [ownerEmail, setOwnerEmail] = useState('');
  const [ownerPhoneNumber, setOwnerPhoneNumber] = useState('');
  const [ownerSubsystem, setOwnerSubsystem] = useState('');
  const [tradingPartner, setTradingPartner] = useState('');
  const [operatorId, setOperatorId] = useState('system');

  const [startWindow, setStartWindow] = useState('');
  const [endWindow, setEndWindow] = useState('');

  const [isActive, setIsActive] = useState(true);
  const [suspended, setSuspended] = useState(false);
  const [sendNotification, setSendNotification] = useState(false);

  const [runScheduleId, setRunScheduleId] = useState<'1' | '2' | ''>('');
  const [runOnFirstDay, setRunOnFirstDay] = useState(false);
  const [runOnLastDay, setRunOnLastDay] = useState(false);
  const [runOnThisDay, setRunOnThisDay] = useState(0);

  const [destinations, setDestinations] = useState<EditableDestinationRow[]>([]);
  const [destinationEditor, setDestinationEditor] = useState<{
    open: boolean;
    row: EditableDestinationRow | null;
    isEdit: boolean;
  }>({ open: false, row: null, isEdit: false });

  const ftpServerRequired = useMemo(() => protocolId === '1' || protocolId === '2' || protocolId === '4', [protocolId]);

  useEffect(() => {
    if (!isEdit || !props.ftsSourceLocationId) {
      // F01-021 / F01-002: default IsActive=true when id missing.
      setIsActive(true);
      // Legacy add-mode clears RunSchedule radio selection.
      setRunScheduleId('');
      setLoading(false);
      return;
    }

    setLoading(true);
    setProblem(null);

    FxferBffClient.getSourceLocationConfiguration(props.ftsSourceLocationId)
      .then((c) => {
        setConfigName(c.configName);
        setDescription(c.description);
        setProtocolId(c.protocol);
        setFtpServerId(c.ftpServerId);
        setSourceAddress(c.address);
        setMask(c.mask);
        setFileDirection(c.fileDirection);
        setOwnerName(c.ownerName);
        setOwnerEmail(c.ownerEmail);
        setOwnerPhoneNumber(c.ownerPhoneNumber);
        setOwnerSubsystem(c.ownerSubsystem);
        setTradingPartner(c.tradingPartner ?? '');
        setOperatorId(c.operatorId);
        setStartWindow(c.startWindow);
        setEndWindow(c.endWindow);
        setIsActive(c.isActive);
        setSuspended(c.suspended);
        setSendNotification(c.sendNotification);
        setRunOnFirstDay(c.runOnFirstDay);
        setRunOnLastDay(c.runOnLastDay);
        setRunOnThisDay(c.runOnThisDay);

        const monthly = c.runOnFirstDay || c.runOnLastDay || c.runOnThisDay !== 0;
        setRunScheduleId(monthly ? '2' : '1');

        setDestinations(toEditableDestinations(c.destinations));
      })
      .catch((e: unknown) => {
        setProblem(e instanceof FxferBffError ? e.problem : { title: 'Unable to load configuration' });
      })
      .finally(() => {
        setLoading(false);
      });
  }, [isEdit, props.ftsSourceLocationId]);

  function onRequestClose() {
    if (isDirty || savedRecord) {
      if (!window.confirm(DIRTY_DATA_CONFIRMATION)) {
        return;
      }
    }
    props.onClose();
  }

  function markDirty<T>(setter: (v: T) => void, value: T) {
    setIsDirty(true);
    setter(value);
  }

  function destinationHasActiveRow() {
    return destinations.some((d) => d.active === true);
  }

  async function onSave() {
    setProblem(null);

    if (!validateFtpFormat({ sourceAddress, protocolId })) {
      setProblem({ title: INVALID_SOURCE_ADDRESS_MESSAGE });
      return;
    }

    if (!destinationHasActiveRow()) {
      setProblem({ title: DESTINATION_REQUIRED_MESSAGE });
      return;
    }

    if (ftpServerRequired && ftpServerId.trim() === '') {
      setProblem({ title: 'FTP server selection is required.' });
      return;
    }

    if (runScheduleId === '') {
      setProblem({ title: RUN_SCHEDULE_REQUIRED_MESSAGE });
      return;
    }

    const request: TransferConfigurationUpsertRequest = {
      configName,
      description: description || undefined,
      ftpServerId: ftpServerRequired ? ftpServerId : null,
      address: sourceAddress,
      mask: mask || undefined,
      protocol: protocolId,
      fileDirection: fileDirection || undefined,
      ownerName,
      ownerEmail,
      ownerPhoneNumber,
      ownerSubsystem,
      tradingPartner: tradingPartner || undefined,
      sendNotification,
      runOnFirstDay: runScheduleId === '2' ? runOnFirstDay : false,
      runOnLastDay: runScheduleId === '2' ? runOnLastDay : false,
      runOnThisDay: runScheduleId === '2' ? runOnThisDay : 0,
      startWindow,
      endWindow,
      isActive,
      suspended,
      destinations: destinations.map((d) => ({
        destinationAddress: d.destinationAddress,
        destinationProtocol: d.destinationProtocolId,
        ftpServerId: d.destinationProtocolId === '1' ? d.ftpServerId : null,
        renamePattern: d.renameAtDestination ? d.renamePattern : null,
        overWriteAtDestination: d.overwriteAtDestination,
        compressAtDestination: d.compressAtDestination,
        renameAtDestination: d.renameAtDestination,
        destinationArchive: d.destinationArchive,
        contactName: d.contactName || null,
        contactPhone: d.contactPhone || null,
        contactEmail: d.contactEmail || null,
        isValid: d.active,
        fteMetadataId: d.fteMetadataId || null,
        destinationType: d.destinationType || null,
        contractName: d.contractName || null,
        importJobName: d.importJobName || null
      })),
      operatorId
    };

    try {
      if (isEdit && props.ftsSourceLocationId) {
        await FxferBffClient.updateSourceLocation(props.ftsSourceLocationId, request);
      } else {
        await FxferBffClient.addSourceLocation(request);
      }

      setSavedRecord(true);
      setIsDirty(false);
      props.onSaved?.();
      props.onClose();
    } catch (e: unknown) {
      setProblem(e instanceof FxferBffError ? e.problem : { title: 'Save failed' });
    }
  }

  return (
    <Modal title={isEdit ? 'Edit Source Location Configuration' : 'Add Source Location Configuration'} onClose={onRequestClose} width={980}>
      {problem ? <ProblemAlert problem={problem} /> : null}
      {loading ? (
        <p>Loading...</p>
      ) : (
        <div style={{ display: 'grid', gap: 12 }}>
          <section style={{ border: '1px solid #e5e7eb', borderRadius: 8, padding: 12 }}>
            <h3 style={{ margin: '0 0 12px 0' }}>Configuration</h3>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr', gap: 12 }}>
              <label>
                Config Name
                <input value={configName} onChange={(e) => markDirty(setConfigName, e.target.value)} style={{ width: '100%' }} />
              </label>
              <label>
                Protocol Id
                <input value={protocolId} onChange={(e) => markDirty(setProtocolId, e.target.value)} style={{ width: '100%' }} />
              </label>
              {ftpServerRequired ? (
                <label>
                  FTP Server Id
                  <input value={ftpServerId} onChange={(e) => markDirty(setFtpServerId, e.target.value)} style={{ width: '100%' }} />
                </label>
              ) : null}

              <label style={{ gridColumn: '1 / -1' }}>
                Source Address
                <input value={sourceAddress} onChange={(e) => markDirty(setSourceAddress, e.target.value)} style={{ width: '100%' }} />
              </label>

              <label>
                Mask
                <input value={mask} onChange={(e) => markDirty(setMask, e.target.value)} style={{ width: '100%' }} />
              </label>
              <label>
                File Direction
                <input value={fileDirection} onChange={(e) => markDirty(setFileDirection, e.target.value)} style={{ width: '100%' }} />
              </label>
              <label>
                Owner Subsystem
                <input value={ownerSubsystem} onChange={(e) => markDirty(setOwnerSubsystem, e.target.value)} style={{ width: '100%' }} />
              </label>

              <label>
                Owner Name
                <input value={ownerName} onChange={(e) => markDirty(setOwnerName, e.target.value)} style={{ width: '100%' }} />
              </label>
              <label>
                Owner Email
                <input value={ownerEmail} onChange={(e) => markDirty(setOwnerEmail, e.target.value)} style={{ width: '100%' }} />
              </label>
              <label>
                Owner Phone
                <input value={ownerPhoneNumber} onChange={(e) => markDirty(setOwnerPhoneNumber, e.target.value)} style={{ width: '100%' }} />
              </label>

              <label>
                Trading Partner
                <input value={tradingPartner} onChange={(e) => markDirty(setTradingPartner, e.target.value)} style={{ width: '100%' }} />
              </label>
              <label>
                Start Window
                <input value={startWindow} onChange={(e) => markDirty(setStartWindow, e.target.value)} style={{ width: '100%' }} />
              </label>
              <label>
                End Window
                <input value={endWindow} onChange={(e) => markDirty(setEndWindow, e.target.value)} style={{ width: '100%' }} />
              </label>

              <label>
                Operator Id
                <input value={operatorId} onChange={(e) => markDirty(setOperatorId, e.target.value)} style={{ width: '100%' }} />
              </label>

              <label>
                <input type={'checkbox'} checked={isActive} onChange={(e) => markDirty(setIsActive, e.target.checked)} />{' '}
                Active
              </label>

              <label>
                <input type={'checkbox'} checked={suspended} onChange={(e) => markDirty(setSuspended, e.target.checked)} />{' '}
                Suspended
              </label>

              <label>
                <input type={'checkbox'} checked={sendNotification} onChange={(e) => markDirty(setSendNotification, e.target.checked)} />{' '}
                Send Notification
              </label>

              <div style={{ gridColumn: '1 / -1' }}>
                <div style={{ fontWeight: 600, marginBottom: 6 }}>Run Schedule</div>
                <label style={{ marginRight: 12 }}>
                  <input type={'radio'} name={'runSchedule'} value={'1'} checked={runScheduleId === '1'} onChange={() => markDirty(setRunScheduleId, '1')} />{' '}
                  Weekly
                </label>
                <label>
                  <input type={'radio'} name={'runSchedule'} value={'2'} checked={runScheduleId === '2'} onChange={() => markDirty(setRunScheduleId, '2')} />{' '}
                  Monthly
                </label>
              </div>

              {runScheduleId === '2' ? (
                <div style={{ gridColumn: '1 / -1', display: 'flex', gap: 12, flexWrap: 'wrap' }}>
                  <label>
                    <input type={'checkbox'} checked={runOnFirstDay} onChange={(e) => markDirty(setRunOnFirstDay, e.target.checked)} />{' '}
                    Run on First Day
                  </label>
                  <label>
                    <input type={'checkbox'} checked={runOnLastDay} onChange={(e) => markDirty(setRunOnLastDay, e.target.checked)} />{' '}
                    Run on Last Day
                  </label>
                  <label>
                    Run on This Day
                    <input
                      type={'number'}
                      value={String(runOnThisDay)}
                      onChange={(e) => markDirty(setRunOnThisDay, Number(e.target.value || 0))}
                      style={{ width: 120, marginLeft: 8 }}
                    />
                  </label>
                </div>
              ) : null}
            </div>
          </section>

          <section style={{ border: '1px solid #e5e7eb', borderRadius: 8, padding: 12 }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', gap: 12 }}>
              <h3 style={{ margin: 0 }}>Destinations</h3>
              <button
                type={'button'}
                onClick={() => {
                  setDestinationEditor({
                    open: true,
                    isEdit: false,
                    row: {
                      destinationLineNumber: 0,
                      active: true,
                      destinationAddress: '',
                      destinationProtocolId: '',
                      destinationType: '',
                      contractName: '',
                      fteMetadataId: '',
                      importJobName: '',
                      ftpServerId: '',
                      contactName: '',
                      contactPhone: '',
                      contactEmail: '',
                      overwriteAtDestination: false,
                      compressAtDestination: false,
                      destinationArchive: false,
                      renameAtDestination: false,
                      renamePattern: ''
                    }
                  });
                }}
              >
                Add Destination
              </button>
            </div>

            <table style={{ width: '100%', borderCollapse: 'collapse', marginTop: 12 }}>
              <thead>
                <tr>
                  <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }}>#</th>
                  <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }}>Address</th>
                  <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }}>Active</th>
                  <th style={{ textAlign: 'left', borderBottom: '1px solid #ddd', padding: 6 }} />
                </tr>
              </thead>
              <tbody>
                {destinations.length === 0 ? (
                  <tr>
                    <td colSpan={4} style={{ padding: 6 }}>
                      (none)
                    </td>
                  </tr>
                ) : (
                  destinations.map((d) => (
                    <tr key={d.destinationLineNumber}>
                      <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>{d.destinationLineNumber}</td>
                      <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>{d.destinationAddress}</td>
                      <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>{d.active ? 'Yes' : 'No'}</td>
                      <td style={{ padding: 6, borderBottom: '1px solid #f1f1f1' }}>
                        <button
                          type={'button'}
                          onClick={() => {
                            setDestinationEditor({ open: true, isEdit: true, row: d });
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

          <div style={{ display: 'flex', justifyContent: 'flex-end', gap: 8 }}>
            <button type={'button'} onClick={onSave}>
              Save
            </button>
          </div>
        </div>
      )}

      {destinationEditor.open && destinationEditor.row ? (
        <DestinationConfigurationEditPanel
          title={destinationEditor.isEdit ? 'Edit Destination' : 'Add Destination'}
          initial={destinationEditor.row}
          isEdit={destinationEditor.isEdit}
          onCancel={() => setDestinationEditor({ open: false, row: null, isEdit: false })}
          onSave={(row, mode) => {
            setIsDirty(true);
            setDestinations((prev) => {
              if (destinationEditor.isEdit) {
                return prev.map((p) => (p.destinationLineNumber === row.destinationLineNumber ? row : p));
              }

              const nextLineNumber = prev.length + 1;
              const newRow = { ...row, destinationLineNumber: nextLineNumber };
              return [...prev, newRow];
            });

            if (mode === 'close') {
              setDestinationEditor({ open: false, row: null, isEdit: false });
            }
          }}
        />
      ) : null}
    </Modal>
  );
}

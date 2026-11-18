import { useEffect, useMemo, useState } from 'react';
import { Modal } from '@/components/Modal';
import { FxferBffClient, FxferBffError } from '@/api/FxferBffClient';
import type { InboundTargetMetadata, ProblemDetails } from '@/api/FxferBffClient';
import { ProblemAlert } from '@/components/ProblemAlert';

export type EditableDestinationRow = {
  destinationLineNumber: number;
  active: boolean;
  destinationAddress: string;
  destinationProtocolId: string;
  destinationType: string;
  contractName: string;
  fteMetadataId: string;
  importJobName: string;
  ftpServerId: string;
  contactName: string;
  contactPhone: string;
  contactEmail: string;
  overwriteAtDestination: boolean;
  compressAtDestination: boolean;
  destinationArchive: boolean;
  renameAtDestination: boolean;
  renamePattern: string;
};

export function DestinationConfigurationEditPanel(props: {
  title: string;
  initial: EditableDestinationRow;
  isEdit: boolean;
  onSave: (row: EditableDestinationRow, mode: 'addAnother' | 'close') => void;
  onCancel: () => void;
}) {
  const [row, setRow] = useState<EditableDestinationRow>(props.initial);
  const [problem, setProblem] = useState<ProblemDetails | null>(null);
  const [metadata, setMetadata] = useState<InboundTargetMetadata[]>([]);

  const isFileToEvent = useMemo(() => row.destinationType === 'FileToEvent', [row.destinationType]);

  useEffect(() => {
    setRow(props.initial);
  }, [props.initial]);

  useEffect(() => {
    if (!row.contractName) {
      setMetadata([]);
      return;
    }

    FxferBffClient.searchInboundTargets(row.contractName)
      .then((items) => setMetadata(items))
      .catch(() => setMetadata([]));
  }, [row.contractName]);

  function setField<K extends keyof EditableDestinationRow>(key: K, value: EditableDestinationRow[K]) {
    setRow((prev) => ({ ...prev, [key]: value }));
  }

  function validateRequired() {
    if (!row.destinationProtocolId) {
      return 'Destination protocol is required.';
    }
    if (!row.destinationType) {
      return 'Destination type is required.';
    }
    if (row.destinationAddress.trim() === '') {
      return 'Destination address is required.';
    }

    if (row.destinationType === 'FileToEvent') {
      if (!row.contractName) {
        return 'Contract name is required.';
      }
      if (!row.fteMetadataId) {
        return 'Metadata id is required.';
      }
    }

    return null;
  }

  function onSubmit(mode: 'addAnother' | 'close') {
    setProblem(null);
    const msg = validateRequired();
    if (msg) {
      setProblem({ title: msg });
      return;
    }

    props.onSave(row, mode);

    if (mode === 'addAnother') {
      setRow((prev) => ({
        ...prev,
        destinationLineNumber: 0,
        active: true,
        destinationAddress: '',
        contractName: '',
        fteMetadataId: '',
        importJobName: '',
        contactName: '',
        contactPhone: '',
        contactEmail: '',
        overwriteAtDestination: false,
        compressAtDestination: false,
        destinationArchive: false,
        renameAtDestination: false,
        renamePattern: ''
      }));
    }
  }

  return (
    <Modal title={props.title} onClose={props.onCancel} width={900}>
      {problem ? <ProblemAlert problem={problem} /> : null}

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 12 }}>
        <label>
          Destination Protocol Id
          <input
            value={row.destinationProtocolId}
            onChange={(e) => setField('destinationProtocolId', e.target.value)}
            style={{ width: '100%' }}
            disabled={isFileToEvent}
          />
        </label>

        <label>
          Destination Type
          <input
            value={row.destinationType}
            onChange={(e) => setField('destinationType', e.target.value)}
            style={{ width: '100%' }}
            disabled={isFileToEvent}
          />
        </label>

        <label style={{ gridColumn: '1 / -1' }}>
          Destination Address
          <input
            value={row.destinationAddress}
            onChange={(e) => setField('destinationAddress', e.target.value)}
            style={{ width: '100%' }}
            disabled={isFileToEvent}
          />
        </label>

        <label>
          Contract Name
          <input
            value={row.contractName}
            onChange={(e) => setField('contractName', e.target.value)}
            style={{ width: '100%' }}
            disabled={isFileToEvent}
          />
        </label>

        <label>
          Metadata Id
          <select
            value={row.fteMetadataId}
            onChange={(e) => {
              setField('fteMetadataId', e.target.value);
              const found = metadata.find((m) => m.fteMetadataId === e.target.value);
              setField('importJobName', found?.importJobName ?? '');
            }}
            style={{ width: '100%' }}
            disabled={isFileToEvent}
          >
            <option value={''} />
            {metadata.map((m) => (
              <option key={m.fteMetadataId} value={m.fteMetadataId}>
                {m.fteMetadataId}
              </option>
            ))}
          </select>
        </label>

        <label>
          Import Job Name
          <input value={row.importJobName} readOnly style={{ width: '100%' }} />
        </label>

        <label>
          FTP Server Id
          <input
            value={row.ftpServerId}
            onChange={(e) => setField('ftpServerId', e.target.value)}
            style={{ width: '100%' }}
            disabled={row.destinationProtocolId !== '1'}
          />
        </label>

        <label>
          Contact Name
          <input
            value={row.contactName}
            onChange={(e) => setField('contactName', e.target.value)}
            style={{ width: '100%' }}
            disabled={isFileToEvent}
          />
        </label>

        <label>
          Contact Phone
          <input
            value={row.contactPhone}
            onChange={(e) => setField('contactPhone', e.target.value)}
            style={{ width: '100%' }}
            disabled={isFileToEvent}
          />
        </label>

        <label>
          Contact Email
          <input
            value={row.contactEmail}
            onChange={(e) => setField('contactEmail', e.target.value)}
            style={{ width: '100%' }}
            disabled={isFileToEvent}
          />
        </label>

        <label style={{ gridColumn: '1 / -1' }}>
          <input type={'checkbox'} checked={row.renameAtDestination} onChange={(e) => setField('renameAtDestination', e.target.checked)} />{' '}
          Rename At Destination
        </label>

        {row.renameAtDestination ? (
          <label style={{ gridColumn: '1 / -1' }}>
            Rename Pattern
            <input value={row.renamePattern} onChange={(e) => setField('renamePattern', e.target.value)} style={{ width: '100%' }} />
          </label>
        ) : null}

        <label>
          <input type={'checkbox'} checked={row.overwriteAtDestination} onChange={(e) => setField('overwriteAtDestination', e.target.checked)} />{' '}
          Overwrite At Destination
        </label>

        <label>
          <input type={'checkbox'} checked={row.compressAtDestination} onChange={(e) => setField('compressAtDestination', e.target.checked)} />{' '}
          Compress At Destination
        </label>

        <label>
          <input type={'checkbox'} checked={row.destinationArchive} onChange={(e) => setField('destinationArchive', e.target.checked)} />{' '}
          Destination Archive
        </label>

        <label>
          <input type={'checkbox'} checked={row.active} onChange={(e) => setField('active', e.target.checked)} />{' '}
          Active
        </label>
      </div>

      <div style={{ display: 'flex', justifyContent: 'flex-end', gap: 8, marginTop: 16 }}>
        {!props.isEdit ? (
          <button type={'button'} onClick={() => onSubmit('addAnother')}>
            Add Another
          </button>
        ) : null}

        <button type={'button'} onClick={() => onSubmit('close')}>
          {props.isEdit ? 'Update and Close' : 'Save and Close'}
        </button>
      </div>
    </Modal>
  );
}

import { useMemo, useState } from 'react';
import { Modal } from '@/components/Modal';
import { ProblemAlert } from '@/components/ProblemAlert';
import { FxferBffClient, FxferBffError } from '@/api/FxferBffClient';
import type { FtpServer, ProblemDetails } from '@/api/FxferBffClient';
import { validateIpAddressFormat } from '@/screens/configure-source-locations-and-destinations/legacyUiLogic';

const DEFAULT_GUID = '00000000-0000-0000-0000-000000000000';

export function ViewEditFtpServerCredentialsModal(props: {
  ftpServer?: FtpServer | null;
  onClose: () => void;
  onSaved?: () => void;
}) {
  const isExisting = Boolean(props.ftpServer && props.ftpServer.ftpServerId && props.ftpServer.ftpServerId !== DEFAULT_GUID);
  const isAdd = !isExisting;

  const [problem, setProblem] = useState<ProblemDetails | null>(null);
  const [form, setForm] = useState(() => {
    const s = props.ftpServer;
    return {
      ftpServerId: s?.ftpServerId ?? DEFAULT_GUID,
      ftpServerName: s?.ftpServerName ?? '',
      ftpDescription: s?.ftpDescription ?? '',
      ftpServerIp: s?.ftpServerIp ?? '',
      ftpTransferMode: s?.ftpTransferMode ?? '',
      ftpUser: s?.ftpUser ?? '',
      ftpPort: s?.ftpPort ?? 21,
      ftpPassword: '',
      ftpPasswordConfirmation: '',
      ftpProxyAddress: s?.ftpProxyAddress ?? '',
      ftpProxyPort: s?.ftpProxyPort ?? null,
      useSshAuth: s?.useSshAuth ?? false,
      isValid: s?.isValid ?? true,
      operatorId: s?.operatorId ?? 'system'
    };
  });

  const readOnlyAll = useMemo(() => isExisting && form.isValid === false, [form.isValid, isExisting]);

  function setField<K extends keyof typeof form>(key: K, value: (typeof form)[K]) {
    setForm((prev) => ({ ...prev, [key]: value }));
  }

  function validateBeforeSave() {
    if (!validateIpAddressFormat(form.ftpServerIp)) {
      return { ok: false as const, message: 'Invalid server IP address format.' };
    }

    if (form.ftpProxyAddress.trim() !== '') {
      if (!validateIpAddressFormat(form.ftpProxyAddress)) {
        return { ok: false as const, message: 'Invalid proxy address format.' };
      }
      if (!form.ftpProxyPort) {
        return { ok: false as const, message: 'Proxy port is required when proxy address is provided.' };
      }
    }

    const pw = form.ftpPassword.trim();
    const pwc = form.ftpPasswordConfirmation.trim();
    if (!isAdd && pw === '' && pwc === '') {
      return { ok: true as const };
    }

    if (pw !== '' || pwc !== '') {
      if (pw === '' || pwc === '') {
        return { ok: false as const, message: 'Both password fields are required.' };
      }
      if (pw !== pwc) {
        return { ok: false as const, message: 'Password and confirmation must match.' };
      }
    }

    return { ok: true as const };
  }

  async function onSave() {
    setProblem(null);
    const validated = validateBeforeSave();
    if (!validated.ok) {
      setProblem({ title: validated.message });
      return;
    }

    try {
      if (isAdd) {
        await FxferBffClient.addFtpServer({
          ftpServerName: form.ftpServerName,
          ftpDescription: form.ftpDescription,
          ftpServerIp: form.ftpServerIp,
          ftpTransferMode: form.ftpTransferMode,
          ftpUser: form.ftpUser,
          ftpPort: Number(form.ftpPort),
          ftpPassword: form.ftpPassword,
          ftpPasswordConfirmation: form.ftpPasswordConfirmation,
          ftpProxyAddress: form.ftpProxyAddress || undefined,
          ftpProxyPort: form.ftpProxyPort ?? undefined,
          useSshAuth: form.useSshAuth,
          isValid: true,
          operatorId: form.operatorId
        });
      } else {
        await FxferBffClient.updateFtpServer(form.ftpServerId, {
          ftpServerName: form.ftpServerName,
          ftpDescription: form.ftpDescription,
          ftpServerIp: form.ftpServerIp,
          ftpTransferMode: form.ftpTransferMode,
          ftpUser: form.ftpUser,
          ftpPort: Number(form.ftpPort),
          ftpPassword: form.ftpPassword,
          ftpPasswordConfirmation: form.ftpPasswordConfirmation,
          ftpProxyAddress: form.ftpProxyAddress || undefined,
          ftpProxyPort: form.ftpProxyPort ?? undefined,
          useSshAuth: form.useSshAuth,
          isValid: true,
          operatorId: form.operatorId
        });
      }

      props.onSaved?.();
      props.onClose();
    } catch (e: unknown) {
      setProblem(e instanceof FxferBffError ? e.problem : { title: 'Save failed' });
    }
  }

  async function onInvalidate() {
    // Legacy prompts with invalidation warning.
    if (!window.confirm('Are you sure you want to invalidate this FTP server?')) {
      return;
    }

    setProblem(null);
    try {
      await FxferBffClient.invalidateFtpServer(form.ftpServerId, { isValid: false, operatorId: form.operatorId });
      props.onSaved?.();
      props.onClose();
    } catch (e: unknown) {
      setProblem(e instanceof FxferBffError ? e.problem : { title: 'Invalidate failed' });
    }
  }

  return (
    <Modal title={isAdd ? 'Add FTP Server Credentials' : 'View/Edit FTP Server Credentials'} onClose={props.onClose} width={780}>
      {problem ? <ProblemAlert problem={problem} /> : null}

      {isExisting ? (
        <div style={{ marginBottom: 12, fontSize: 12 }}>
          <strong>Audit</strong>
          <div>Operator: {props.ftpServer?.operatorId}</div>
          <div>Created: {props.ftpServer?.creationDate}</div>
          <div>Last Modified: {props.ftpServer?.lastModifiedDate}</div>
        </div>
      ) : null}

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 12 }}>
        <label>
          Server Name
          <input
            value={form.ftpServerName}
            onChange={(e) => setField('ftpServerName', e.target.value)}
            style={{ width: '100%' }}
            readOnly={readOnlyAll || isExisting}
          />
        </label>

        <label>
          Description
          <input
            value={form.ftpDescription}
            onChange={(e) => setField('ftpDescription', e.target.value)}
            style={{ width: '100%' }}
            readOnly={readOnlyAll}
          />
        </label>

        <label>
          Server IP
          <input
            value={form.ftpServerIp}
            onChange={(e) => setField('ftpServerIp', e.target.value)}
            style={{ width: '100%' }}
            readOnly={readOnlyAll}
          />
        </label>

        <label>
          Transfer Mode
          <input
            value={form.ftpTransferMode}
            onChange={(e) => setField('ftpTransferMode', e.target.value)}
            style={{ width: '100%' }}
            readOnly={readOnlyAll}
          />
        </label>

        <label>
          User
          <input value={form.ftpUser} onChange={(e) => setField('ftpUser', e.target.value)} style={{ width: '100%' }} readOnly={readOnlyAll} />
        </label>

        <label>
          Port
          <input
            type={'number'}
            value={String(form.ftpPort)}
            onChange={(e) => setField('ftpPort', Number(e.target.value || 0))}
            style={{ width: '100%' }}
            readOnly={readOnlyAll}
          />
        </label>

        <label>
          Password
          <input
            type={'password'}
            value={form.ftpPassword}
            onChange={(e) => setField('ftpPassword', e.target.value)}
            style={{ width: '100%' }}
            readOnly={readOnlyAll}
          />
        </label>

        <label>
          Password Confirmation
          <input
            type={'password'}
            value={form.ftpPasswordConfirmation}
            onChange={(e) => setField('ftpPasswordConfirmation', e.target.value)}
            style={{ width: '100%' }}
            readOnly={readOnlyAll}
          />
        </label>

        <label>
          Proxy Address
          <input
            value={form.ftpProxyAddress}
            onChange={(e) => setField('ftpProxyAddress', e.target.value)}
            style={{ width: '100%' }}
            readOnly={readOnlyAll}
          />
        </label>

        <label>
          Proxy Port
          <input
            type={'number'}
            value={form.ftpProxyPort === null ? '' : String(form.ftpProxyPort)}
            onChange={(e) => setField('ftpProxyPort', e.target.value === '' ? null : Number(e.target.value))}
            style={{ width: '100%' }}
            readOnly={readOnlyAll}
          />
        </label>

        <label style={{ gridColumn: '1 / -1' }}>
          <input
            type={'checkbox'}
            checked={form.useSshAuth}
            onChange={(e) => setField('useSshAuth', e.target.checked)}
            disabled={readOnlyAll}
          />{' '}
          Use SSH Auth
        </label>
      </div>

      {!readOnlyAll ? (
        <div style={{ display: 'flex', justifyContent: 'flex-end', gap: 8, marginTop: 16 }}>
          {!isAdd ? (
            <button type={'button'} onClick={onInvalidate}>
              Invalidate
            </button>
          ) : null}
          <button type={'button'} onClick={onSave}>
            Save
          </button>
        </div>
      ) : null}
    </Modal>
  );
}

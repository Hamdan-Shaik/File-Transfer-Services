import { render, screen } from '@testing-library/react';
import { SearchFileTransferTrackingScreen } from '@/screens/configure-source-locations-and-destinations/SearchFileTransferTrackingScreen';
import { FxferBffClient } from '@/api/FxferBffClient';

describe('ui-configure-source-locations-and-destinations rules tracing', () => {
  it('F05_001_initializes_single_step_create_and_cancel_navigates_to_home_myhome', () => {
    render(<SearchFileTransferTrackingScreen />);
    expect(screen.getByRole('link', { name: 'Cancel' })).toHaveAttribute('href', '/Home/MyHome');
  });

  it('F05_002_uses_user_locale_code_to_localize_search_action_label', () => {
    render(<SearchFileTransferTrackingScreen />);
    expect(screen.getByRole('button', { name: 'Search' })).toBeInTheDocument();
  });

  it('F05_003_renders_empty_tracking_search_criteria_model', () => {
    render(<SearchFileTransferTrackingScreen />);

    expect(screen.getByLabelText('File Id')).toHaveValue('');
    expect(screen.getByLabelText('File Name')).toHaveValue('');
    expect(screen.getByLabelText('File Status')).toHaveValue('');
    expect(screen.getByLabelText('Direction')).toHaveValue('');
    expect(screen.getByLabelText('Transport Type')).toHaveValue('');
    expect(screen.getByLabelText('Source Location Id')).toHaveValue('');
    expect(screen.getByLabelText('From Date')).toHaveValue('');
    expect(screen.getByLabelText('To Date')).toHaveValue('');
    expect(screen.getByLabelText('Trading Partner Id')).toHaveValue('');
    expect(screen.getByLabelText('From Size')).toHaveValue('');
    expect(screen.getByLabelText('To Size')).toHaveValue('');
  });
});

describe('FxferBffClient rules tracing', () => {
  const FIXED_UUID = '11111111-1111-4111-8111-111111111111';

  beforeEach(() => {
    window.__providerPlusShell = {
      auth: { getAccessToken: jest.fn().mockResolvedValue('jwt') },
      trace: {},
      correlationId: FIXED_UUID
    };

    const existingCrypto = (globalThis as unknown as { crypto?: unknown }).crypto;
    const cryptoWithUuid = {
      ...(typeof existingCrypto === 'object' && existingCrypto ? existingCrypto : {}),
      randomUUID: jest.fn().mockReturnValue(FIXED_UUID)
    };
    Object.defineProperty(globalThis, 'crypto', { value: cryptoWithUuid, configurable: true });
    global.fetch = jest.fn();
  });

  afterEach(() => {
    jest.restoreAllMocks();
    // @ts-expect-error test cleanup
    delete window.__providerPlusShell;
  });

  function mockFetchOk(json: unknown) {
    (global.fetch as jest.Mock).mockResolvedValue({
      ok: true,
      status: 200,
      statusText: 'OK',
      headers: { get: () => 'application/json' },
      json: async () => json
    });
  }

  it('F01_037_add_source_location_posts_and_returns_json', async () => {
    const expected = { ftsSourceLocationId: 'sl1' };
    mockFetchOk(expected);

    const result = await FxferBffClient.addSourceLocation({
      configName: 'cfg',
      address: 'addr',
      protocol: 'FTP',
      ownerName: 'owner',
      ownerEmail: 'owner@example.com',
      ownerPhoneNumber: '555',
      ownerSubsystem: 'sub',
      startWindow: '00:00',
      endWindow: '23:59',
      destinations: [],
      operatorId: 'op'
    });

    expect(global.fetch).toHaveBeenCalledWith(
      '/api/file-transfer/v1/transfer-configurations',
      expect.objectContaining({ method: 'POST' })
    );
    expect(result).toEqual(expected);
  });

  it('F01_056_update_source_location_puts_to_transfer_configurations_id', async () => {
    mockFetchOk({});

    await FxferBffClient.updateSourceLocation('sl-123', {
      configName: 'cfg',
      address: 'addr',
      protocol: 'FTP',
      ownerName: 'owner',
      ownerEmail: 'owner@example.com',
      ownerPhoneNumber: '555',
      ownerSubsystem: 'sub',
      startWindow: '00:00',
      endWindow: '23:59',
      destinations: [],
      operatorId: 'op'
    });

    expect(global.fetch).toHaveBeenCalledWith(
      '/api/file-transfer/v1/transfer-configurations/sl-123',
      expect.objectContaining({ method: 'PUT' })
    );
  });

  it('F01_058_invalidate_source_location_patches_to_transfer_configurations_id', async () => {
    mockFetchOk({});

    await FxferBffClient.invalidateSourceLocation('sl-123', { isActive: false, operatorId: 'op' });

    expect(global.fetch).toHaveBeenCalledWith(
      '/api/file-transfer/v1/transfer-configurations/sl-123',
      expect.objectContaining({ method: 'PATCH' })
    );
  });

  it('F02_018_add_ftp_server_posts_and_returns_json', async () => {
    const expected = { ftpServerId: 'ftp1' };
    mockFetchOk(expected);

    const result = await FxferBffClient.addFtpServer({
      ftpServerName: 'name',
      ftpDescription: 'desc',
      ftpServerIp: '127.0.0.1',
      ftpTransferMode: 'Passive',
      ftpUser: 'user',
      ftpPort: 21,
      useSshAuth: false,
      isValid: true,
      operatorId: 'op'
    });

    expect(global.fetch).toHaveBeenCalledWith('/api/file-transfer/v1/ftp-servers', expect.objectContaining({ method: 'POST' }));
    expect(result).toEqual(expected);
  });

  it('F02_020_update_ftp_server_puts_to_ftp_servers_id', async () => {
    mockFetchOk({});

    await FxferBffClient.updateFtpServer('ftp-123', {
      ftpServerName: 'name',
      ftpDescription: 'desc',
      ftpServerIp: '127.0.0.1',
      ftpTransferMode: 'Passive',
      ftpUser: 'user',
      ftpPort: 21,
      useSshAuth: false,
      isValid: true,
      operatorId: 'op'
    });

    expect(global.fetch).toHaveBeenCalledWith('/api/file-transfer/v1/ftp-servers/ftp-123', expect.objectContaining({ method: 'PUT' }));
  });

  it('F02_022_invalidate_ftp_server_patches_to_ftp_servers_id', async () => {
    mockFetchOk({});

    await FxferBffClient.invalidateFtpServer('ftp-123', { isValid: false, operatorId: 'op' });

    expect(global.fetch).toHaveBeenCalledWith(
      '/api/file-transfer/v1/ftp-servers/ftp-123',
      expect.objectContaining({ method: 'PATCH' })
    );
  });
});


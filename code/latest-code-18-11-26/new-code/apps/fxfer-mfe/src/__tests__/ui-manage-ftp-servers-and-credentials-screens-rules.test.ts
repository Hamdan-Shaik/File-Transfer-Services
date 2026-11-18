import { FxferBffClient, FxferBffError } from '@/api/FxferBffClient';
import { TransferConfigurationBaseApi } from '@/screens/manage-ftp-servers-and-credentials/Api/TransferConfigurationBaseApi';

describe('ui-manage-ftp-servers-and-credentials-screens rules tracing', () => {
  afterEach(() => {
    jest.restoreAllMocks();
  });

  it('F01_023_rejects_invalid_models_with_http_400', async () => {
    await expect(TransferConfigurationBaseApi.upsertBaseConfiguration(null as never)).rejects.toBeInstanceOf(FxferBffError);
    await expect(TransferConfigurationBaseApi.upsertBaseConfiguration(null as never)).rejects.toMatchObject({
      problem: { status: 400 }
    });
  });

  it('F01_024_removes_hyphens_from_owner_phone_number_before_processing', async () => {
    const addSpy = jest.spyOn(FxferBffClient, 'addSourceLocation').mockResolvedValue({} as never);

    const model = {
      ftsSourceLocationId: '',
      configName: 'cfg',
      address: 'addr',
      protocol: 'FTP',
      ownerName: 'owner',
      ownerEmail: 'owner@example.com',
      ownerPhoneNumber: '555-444-3333',
      ownerSubsystem: 'sub',
      startWindow: '00:00',
      endWindow: '23:59',
      destinations: [],
      operatorId: 'op'
    };

    const result = await TransferConfigurationBaseApi.upsertBaseConfiguration(model);

    expect(addSpy).toHaveBeenCalledWith(expect.objectContaining({ ownerPhoneNumber: '5554443333' }));
    expect(result.ownerPhoneNumber).toBe('5554443333');
  });

  it('F01_025_calls_add_when_fts_source_location_id_is_empty_otherwise_calls_update', async () => {
    const addSpy = jest.spyOn(FxferBffClient, 'addSourceLocation').mockResolvedValue({} as never);
    const updateSpy = jest.spyOn(FxferBffClient, 'updateSourceLocation').mockResolvedValue({} as never);

    await TransferConfigurationBaseApi.upsertBaseConfiguration({
      ftsSourceLocationId: '',
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

    await TransferConfigurationBaseApi.upsertBaseConfiguration({
      ftsSourceLocationId: 'sl-123',
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

    expect(addSpy).toHaveBeenCalledTimes(1);
    expect(updateSpy).toHaveBeenCalledTimes(1);
    expect(updateSpy).toHaveBeenCalledWith('sl-123', expect.anything());
  });

  it('F01_026_returns_the_submitted_model_on_success', async () => {
    jest.spyOn(FxferBffClient, 'addSourceLocation').mockResolvedValue({
      ftsSourceLocationId: 'server-sl-id'
    } as never);

    const model = {
      ftsSourceLocationId: '',
      configName: 'cfg',
      address: 'addr',
      protocol: 'FTP',
      ownerName: 'owner',
      ownerEmail: 'owner@example.com',
      ownerPhoneNumber: '555-000',
      ownerSubsystem: 'sub',
      startWindow: '00:00',
      endWindow: '23:59',
      destinations: [],
      operatorId: 'op'
    };

    const result = await TransferConfigurationBaseApi.upsertBaseConfiguration(model);

    expect(result).toEqual({ ...model, ownerPhoneNumber: '555000' });
    expect(result.ftsSourceLocationId).toBe('');
  });
});


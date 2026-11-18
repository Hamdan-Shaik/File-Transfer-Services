import { fxferFetchJson } from '@/api/FxferBffClient';
import type { BaseTransferConfigurationModel } from '@/screens/manage-ftp-servers-and-credentials/Api/TransferConfigurationBaseApi';
import { TransferConfigurationBaseApi } from '@/screens/manage-ftp-servers-and-credentials/Api/TransferConfigurationBaseApi';
import type { ListItemModel } from '@/screens/manage-ftp-servers-and-credentials/types';

type SourceLocationOption = {
  ftsSourceLocationId: string;
  configName: string;
  address: string;
};

export class SourceLocationOptionsApi {
  public static async getSourceLocationOptions(baseConfiguration?: BaseTransferConfigurationModel): Promise<ListItemModel[]> {
    // F01-083, F01-084, F01-085, F01-086, F01-087, F01-088, F01-089, F01-090: map source locations to list items.
    if (baseConfiguration !== undefined) {
      if (baseConfiguration === null) {
        TransferConfigurationBaseApi.GenerateInvalidModelStateRepsonse();
      }

      const processorResult = TransferConfigurationBaseApi.GenerateAddFileTransferBaseConfigurationResult(baseConfiguration);
      if (processorResult.operation === 'add') {
        await TransferConfigurationBaseApi.AddFileTransferBaseConfiguration(processorResult.request);
      } else {
        await TransferConfigurationBaseApi.UpdateFileTransferBaseConfiguration(processorResult.ftsSourceLocationId!, processorResult.request);
      }
    }

    const sourceLocations = await fxferFetchJson<SourceLocationOption[]>('/source-locations');
    const safe = Array.isArray(sourceLocations) ? sourceLocations : [];

    return safe.map((sl) => ({
      name: `${sl.configName} - ${sl.address}`,
      id: String(sl.ftsSourceLocationId)
    }));
  }
}


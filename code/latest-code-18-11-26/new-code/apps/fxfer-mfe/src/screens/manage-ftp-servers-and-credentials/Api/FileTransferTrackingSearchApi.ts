import { FxferBffClient } from '@/api/FxferBffClient';
import type { FileTransferTracking } from '@/api/FxferBffClient';
import type { BaseTransferConfigurationModel } from '@/screens/manage-ftp-servers-and-credentials/Api/TransferConfigurationBaseApi';
import { TransferConfigurationBaseApi } from '@/screens/manage-ftp-servers-and-credentials/Api/TransferConfigurationBaseApi';
import type { GridState } from '@/screens/manage-ftp-servers-and-credentials/types';

function valueOrNull(value: string | undefined): string | null {
  if (!value) {
    return null;
  }
  return value === '' ? null : value;
}

export class FileTransferTrackingSearchApi {
  public static async searchTracking(
    gridState: GridState & { criteria?: Record<string, string | undefined>; baseConfiguration?: BaseTransferConfigurationModel }
  ): Promise<{
    data: FileTransferTracking[];
    total: number;
  }> {
    // F05-004, F05-005, F05-006, F05-007, F05-008: tracking search returns Kendo-style data/total.
    if (gridState.baseConfiguration !== undefined) {
      const baseConfiguration = gridState.baseConfiguration;
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

    const c = gridState.criteria ?? {};

    const result = await FxferBffClient.searchFileTransferMovement({
      fileId: valueOrNull(c.fileId),
      fileName: valueOrNull(c.fileName),
      fileStatus: valueOrNull(c.fileStatus),
      direction: valueOrNull(c.direction),
      transportType: valueOrNull(c.transportType),
      fromDate: valueOrNull(c.fromDate),
      toDate: valueOrNull(c.toDate),
      fromSize: valueOrNull(c.fromSize),
      toSize: valueOrNull(c.toSize),
      tradingPartnerId: valueOrNull(c.tradingPartnerId),
      sourceLocationId: valueOrNull(c.sourceLocationId),
      page: gridState.page ?? 1,
      pageSize: gridState.pageSize ?? 50
    });

    return {
      data: Array.isArray(result.items) ? result.items : [],
      total: typeof result.total === 'number' ? result.total : 0
    };
  }
}


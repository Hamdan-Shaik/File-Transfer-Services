import { FxferBffClient, FxferBffError } from '@/api/FxferBffClient';
import type { PagedResult, TransferConfigurationSummary } from '@/api/FxferBffClient';
import { getCorrelationId } from '@/observability/headers';
import type { BaseTransferConfigurationModel } from '@/screens/manage-ftp-servers-and-credentials/Api/TransferConfigurationBaseApi';
import { TransferConfigurationBaseApi } from '@/screens/manage-ftp-servers-and-credentials/Api/TransferConfigurationBaseApi';
import type { GridState } from '@/screens/manage-ftp-servers-and-credentials/types';

export class FileTransferSearchApi {
  public static async searchFileTransfers(
    gridState: GridState & { criteria?: Record<string, string | undefined>; baseConfiguration?: BaseTransferConfigurationModel }
  ): Promise<PagedResult<TransferConfigurationSummary>> {
    // F05-009, F05-010, F05-011, F05-036: search returns OK only when data exists.
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

    const result = await FxferBffClient.searchSourceLocations({
      configId: c.configId ? Number(c.configId) : null,
      configName: c.configName ?? null,
      ownerName: c.ownerName ?? null,
      ownerSubsystem: c.ownerSubsystem ?? null,
      protocol: c.protocol ?? null,
      isActive: c.isActive !== undefined ? Boolean(c.isActive) : true,
      page: gridState.page ?? 1,
      pageSize: gridState.pageSize ?? 50
    });
    if (!result || !Array.isArray(result.items)) {
      const title = 'File transfer search returned no data.';
      throw new FxferBffError(title, { status: 400, title, correlation_id: getCorrelationId() });
    }
    return result;
  }
}


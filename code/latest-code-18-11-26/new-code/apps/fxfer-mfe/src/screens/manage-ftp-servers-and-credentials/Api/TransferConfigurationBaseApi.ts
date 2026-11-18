import { FxferBffClient, FxferBffError } from '@/api/FxferBffClient';
import type { TransferConfigurationUpsertRequest } from '@/api/FxferBffClient';
import { getCorrelationId } from '@/observability/headers';

export type BaseTransferConfigurationModel = TransferConfigurationUpsertRequest & {
  ftsSourceLocationId?: string | null;
};

function isNullOrEmpty(value: string | null | undefined): boolean {
  return value === null || value === undefined || value === '';
}

type AddFileTransferBaseConfigurationPlan = {
  normalizedModel: BaseTransferConfigurationModel;
  operation: 'add' | 'update';
  request: TransferConfigurationUpsertRequest;
  ftsSourceLocationId?: string | null;
};

export class TransferConfigurationBaseApi {
  public static GenerateInvalidModelStateRepsonse(message = 'Bad Request'): never {
    throw new FxferBffError(message, { status: 400, title: message, correlation_id: getCorrelationId() });
  }

  public static GenerateAddFileTransferBaseConfigurationResult(model: BaseTransferConfigurationModel): AddFileTransferBaseConfigurationPlan {
    if (!model) {
      return TransferConfigurationBaseApi.GenerateInvalidModelStateRepsonse();
    }

    const normalized: BaseTransferConfigurationModel = {
      ...model,
      ownerPhoneNumber: model.ownerPhoneNumber ? model.ownerPhoneNumber.replace(/-/g, '') : model.ownerPhoneNumber
    };

    const { ftsSourceLocationId, ...request } = normalized;

    return {
      normalizedModel: normalized,
      operation: isNullOrEmpty(ftsSourceLocationId) ? 'add' : 'update',
      request,
      ftsSourceLocationId
    };
  }

  public static async AddFileTransferBaseConfiguration(request: TransferConfigurationUpsertRequest): Promise<void> {
    await FxferBffClient.addSourceLocation(request);
  }

  public static async UpdateFileTransferBaseConfiguration(
    ftsSourceLocationId: string,
    request: TransferConfigurationUpsertRequest
  ): Promise<void> {
    await FxferBffClient.updateSourceLocation(ftsSourceLocationId, request);
  }

  // F01-023: Reject invalid payloads with 400.
  // F01-024: Remove hyphens from OwnerPhoneNumber before processing.
  // F01-025: Empty FtsSourceLocationId triggers add; otherwise update.
  // F01-026: On success, return the submitted model (echo).
  public static async upsertBaseConfiguration(model: BaseTransferConfigurationModel): Promise<BaseTransferConfigurationModel> {
    const plan = TransferConfigurationBaseApi.GenerateAddFileTransferBaseConfigurationResult(model);

    if (plan.operation === 'add') {
      await TransferConfigurationBaseApi.AddFileTransferBaseConfiguration(plan.request);
    } else {
      await TransferConfigurationBaseApi.UpdateFileTransferBaseConfiguration(plan.ftsSourceLocationId!, plan.request);
    }

    return plan.normalizedModel;
  }
}

import { FxferBffClient, FxferBffError } from '@/api/FxferBffClient';
import { getCorrelationId } from '@/observability/headers';
import type { FtpServer, FtpServerUpsertRequest } from '@/api/FxferBffClient';
import { FileTransferConstants } from '@/legacy-constants';
import type { BaseTransferConfigurationModel } from '@/screens/manage-ftp-servers-and-credentials/Api/TransferConfigurationBaseApi';
import { TransferConfigurationBaseApi } from '@/screens/manage-ftp-servers-and-credentials/Api/TransferConfigurationBaseApi';
import { getGridFilterValue } from '@/screens/manage-ftp-servers-and-credentials/types';
import type { EditorModel, GridState } from '@/screens/manage-ftp-servers-and-credentials/types';

export type FtpServerModel = {
  ftpServerId: string;
  ftpServerName: string;
  ftpDescription: string;
  ftpServerIp: string;
  ftpTransferMode: string;
  ftpUser: string;
  ftpPort: number;
  ftpPassword?: string;
  ftpPasswordConfirmation?: string;
  ftpProxyAddress?: string | null;
  ftpProxyPort?: number | null;
  useSshAuth: boolean;
  isValid?: boolean | null;
  operatorId: string;
  editor: EditorModel;
};

function isNullOrEmpty(value: string | null | undefined): boolean {
  return value === null || value === undefined || value === '';
}

function toUpsertRequest(model: FtpServerModel): FtpServerUpsertRequest {
  return {
    ftpServerName: model.ftpServerName,
    ftpDescription: model.ftpDescription,
    ftpServerIp: model.ftpServerIp,
    ftpTransferMode: model.ftpTransferMode,
    ftpUser: model.ftpUser,
    ftpPort: Number(model.ftpPort),
    ftpPassword: model.ftpPassword?.length ? model.ftpPassword : undefined,
    ftpPasswordConfirmation: model.ftpPasswordConfirmation?.length ? model.ftpPasswordConfirmation : undefined,
    ftpProxyAddress: model.ftpProxyAddress ?? undefined,
    ftpProxyPort: model.ftpProxyPort ?? undefined,
    useSshAuth: model.useSshAuth,
    isValid: model.isValid ?? true,
    operatorId: model.operatorId
  };
}

function applySavedResponse(model: FtpServerModel, saved: FtpServer | null | undefined): FtpServerModel {
  if (!saved) {
    return { ...model, ftpPassword: '', ftpPasswordConfirmation: '' };
  }

  return {
    ...model,
    ftpServerId: saved.ftpServerId,
    ftpServerName: saved.ftpServerName,
    ftpDescription: saved.ftpDescription,
    ftpServerIp: saved.ftpServerIp,
    ftpTransferMode: saved.ftpTransferMode,
    ftpUser: saved.ftpUser,
    ftpPort: saved.ftpPort,
    ftpProxyAddress: saved.ftpProxyAddress ?? null,
    ftpProxyPort: saved.ftpProxyPort ?? null,
    useSshAuth: saved.useSshAuth,
    isValid: saved.isValid,
    operatorId: saved.operatorId,
    ftpPassword: '',
    ftpPasswordConfirmation: ''
  };
}

function getSuccessMessage(contentId: string): string {
  // shortcut: shell-owned localization provider not ported; upgrade when a message-lookup API exists.
  switch (contentId) {
    case FileTransferConstants.Localization.Messages.AddFtpServerCredentialsSuccess:
      return 'FTP server credentials added successfully.';
    case FileTransferConstants.Localization.Messages.EditFtpServerCredentialsSuccess:
      return 'FTP server credentials saved successfully.';
    case FileTransferConstants.Localization.Messages.InvalidateFtpServerCredentialsSuccess:
      return 'FTP server credentials invalidated successfully.';
    default:
      return contentId;
  }
}

function ftpServerCredentialsSave(action: string, ftpServerModel: FtpServerModel): FtpServerModel {
  let contentId = '';
  switch (action) {
    case FileTransferConstants.WebApi.Actions.AddFTPServerCredentials:
      contentId = FileTransferConstants.Localization.Messages.AddFtpServerCredentialsSuccess;
      break;
    case FileTransferConstants.WebApi.Actions.EditFTPServerCredentials:
      contentId = ftpServerModel.isValid
        ? FileTransferConstants.Localization.Messages.EditFtpServerCredentialsSuccess
        : FileTransferConstants.Localization.Messages.InvalidateFtpServerCredentialsSuccess;
      break;
  }

  return {
    ...ftpServerModel,
    editor: {
      ...ftpServerModel.editor,
      savedMessage: getSuccessMessage(contentId),
      saveActionText: 'Success'
    }
  };
}

function formatInvalidResponse(message: string): never {
  throw new FxferBffError(message, { status: 400, title: message, correlation_id: getCorrelationId() });
}

export class FtpServerCredentialsApi {
  public static async searchCredentials(gridState: GridState): Promise<{ data: FtpServerModel[]; total: number }> {
    const ftpServerName = getGridFilterValue(gridState, FileTransferConstants.SearchFields.FtpServerName) || null;
    const ftpDescription = getGridFilterValue(gridState, FileTransferConstants.SearchFields.FtpDescription) || null;
    const ftpUser = getGridFilterValue(gridState, FileTransferConstants.SearchFields.FtpUser) || null;
    const isValidRaw = getGridFilterValue(gridState, FileTransferConstants.SearchFields.IsValid);
    const isValid = isValidRaw !== '' ? true : null;

    const page = gridState.page ?? 1;
    const pageSize = gridState.pageSize ?? 50;

    const result = await FxferBffClient.getFtpServers({
      isValid,
      ftpServerName,
      nameMatch: ftpServerName ? 'contains' : null,
      page,
      pageSize
    });

    const items = Array.isArray(result.items) ? result.items : [];

    const mapped = items.map((row) => ({
      ftpServerId: row.ftpServerId,
      ftpServerName: row.ftpServerName,
      ftpDescription: row.ftpDescription,
      ftpServerIp: row.ftpServerIp,
      ftpTransferMode: row.ftpTransferMode,
      ftpUser: row.ftpUser,
      ftpPort: row.ftpPort,
      ftpPassword: '',
      ftpPasswordConfirmation: '',
      ftpProxyAddress: row.ftpProxyAddress ?? null,
      ftpProxyPort: row.ftpProxyPort ?? null,
      useSshAuth: row.useSshAuth,
      isValid: row.isValid,
      operatorId: row.operatorId,
      editor: {}
    }));

    // Legacy BuildRequest also filters by FtpUser; BFF v1 door does not accept it.
    const filtered = ftpUser ? mapped.filter((m) => m.ftpUser === ftpUser) : mapped;

    return {
      data: filtered,
      total: typeof result.total === 'number' ? result.total : 0
    };
  }

  public static async saveCredentials(
    model: FtpServerModel,
    baseConfiguration?: BaseTransferConfigurationModel
  ): Promise<FtpServerModel> {
    if (!model) {
      throw new FxferBffError('Bad Request', { status: 400, title: 'Bad Request', correlation_id: getCorrelationId() });
    }

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

    const action = isNullOrEmpty(model.ftpServerId)
      ? FileTransferConstants.WebApi.Actions.AddFTPServerCredentials
      : FileTransferConstants.WebApi.Actions.EditFTPServerCredentials;

    let updated: FtpServerModel = model;

    if (action === FileTransferConstants.WebApi.Actions.AddFTPServerCredentials) {
      const created = await FxferBffClient.addFtpServer(toUpsertRequest(model));
      updated = applySavedResponse(model, created);
      return ftpServerCredentialsSave(action, updated);
    }

    if (model.isValid === false) {
      const invalidated = (await FxferBffClient.invalidateFtpServer(model.ftpServerId, {
        isValid: false,
        operatorId: model.operatorId
      })) as FtpServer | null;

      if (invalidated === null || invalidated === undefined) {
        return formatInvalidResponse('Invalid FTP server update response.');
      }

      updated = applySavedResponse(model, invalidated);
      return ftpServerCredentialsSave(action, updated);
    }

    const saved = (await FxferBffClient.updateFtpServer(model.ftpServerId, toUpsertRequest(model))) as FtpServer | null;
    if (saved === null || saved === undefined) {
      return formatInvalidResponse('Invalid FTP server update response.');
    }

    updated = applySavedResponse(model, saved);

    return ftpServerCredentialsSave(action, updated);
  }
}


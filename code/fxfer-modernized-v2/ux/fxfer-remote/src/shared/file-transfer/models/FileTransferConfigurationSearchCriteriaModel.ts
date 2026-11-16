import type { ListItemModel } from './ListItemModel';

export class FileTransferConfigurationSearchCriteriaModel {
  configId = 0;
  configName: string | null = null;
  ownerName: string | null = null;
  ownerModule: ListItemModel;
  protocol: ListItemModel;

  constructor() {
    this.ownerModule = {};
    this.protocol = {};
  }
}


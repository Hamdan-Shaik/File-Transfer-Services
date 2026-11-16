import type { ListItemModel } from './ListItemModel';

export class DestinationConfigurationModel {
  active = false;
  contactName: string | null = null;
  contactEmail: string | null = null;
  contactPhone: string | null = null;
  compressAtDestination = false;
  destinationAddress: string | null = null;
  destinationId: string | null = null;
  destinationIsArchive = false;
  destinationLineNumber = 0;
  destinationProtocol: ListItemModel;
  ftpServer: ListItemModel;
  destinationType: ListItemModel;
  isAddedUpdated = false;
  overwriteAtDestination = false;
  renameAtDestination = false;
  renamePattern: ListItemModel;
  status: string | null = null;
  contractName: ListItemModel;
  fteMetadataId: string | null = null;
  importJobName: string | null = null;

  constructor() {
    this.ftpServer = {};
    this.destinationProtocol = {};
    this.renamePattern = {};
    this.destinationType = {};
    this.contractName = {};
  }
}


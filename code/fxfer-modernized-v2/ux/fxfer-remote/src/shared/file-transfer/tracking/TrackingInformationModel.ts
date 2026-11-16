import type { ListItemModel } from '../models';

export class TrackingInformationModel {
  checkSumCode: string | null = null;
  direction: ListItemModel;
  fileId: string | null = null;
  fileName: string | null = null;
  fileStatus: ListItemModel;
  createDate: Date | null = null;
  originatingServerName: string | null = null;
  size: number | null = null;
  sourceLocation: string | null = null;
  tradingPartnerId: string | null = null;
  transportType: ListItemModel;

  constructor() {
    this.direction = {};
    this.fileStatus = {};
    this.transportType = {};
  }
}

